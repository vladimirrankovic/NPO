using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JARE;
using JARE.metaheuristics.singleObjective.geneticAlgorithm;
using JARE.problems.singleObjective;
using SignalFilteringGUI.Util.ServiceLayer;

namespace SignalFilteringGUI
{
    public partial class MainForm : Form
    {
        public BackgroundWorker workerThread = null;
        public JARE.problems.singleObjective.SignalFiltering SignalFilteringProblem;
        public PortfolioOptimizationGA SingleObjectiveAlgorithm;
        StreamWriter sw;
        SignalFilteringGUI.Util.OptimizationParameters Parameters = new SignalFilteringGUI.Util.OptimizationParameters();


        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // disable all settings controls except "Stop" button
            loadParameters("OptimizationParameters.csv");
            EnableControls(false);

            workerThread = new BackgroundWorker();
            workerThread.DoWork += new DoWorkEventHandler(Optimization);
            workerThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerThreadCompleted);
            workerThread.WorkerSupportsCancellation = true;

            try
            {
                //workerThread.Start();
                workerThread.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                stopApplication();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
                stopApplication();
        }
        void workerThreadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (sw != null) sw.Dispose();
            EnableControls(true);
        }

        private void stopApplication()
        {
            // stop worker thread
            if (workerThread != null)
            {
                workerThread.CancelAsync();
                OperationCanceled(true);
                EnableControls(true);
                //workerThread = null;
            }
        }
        public delegate void OperationCanceledHandler(bool Cancel);
        public event OperationCanceledHandler OperationCanceled;
        // Delegates to enable async calls for setting controls properties
        private delegate void EnableCallback(bool enable);
        // Delegates to enable async calls for setting controls properties
        private delegate void SetTextCallback(System.Windows.Forms.Control control, string text);

        // Enable/disale controls (safe for threading)
        private void EnableControls(bool enable)
        {
            if (InvokeRequired)
            {
                EnableCallback d = new EnableCallback(EnableControls);
                Invoke(d, new object[] { enable });
            }
            else
            {
                buttonStart.Enabled = enable;
                buttonStop.Enabled = !enable;
            }
        }

        // Worker thread
        void Optimization(object sender, DoWorkEventArgs e)
        //void Optimization()
        {
            try
            {
                JARE.Base.SolutionSet population = new JARE.Base.SolutionSet();
                ExecuteAlgorithm(ref population);
                string workingDir = Directory.GetCurrentDirectory();
                sw = new StreamWriter("SolutionSet.csv");
                SignalFilteringProblem.PrintResults(sw, population);
                sw.Dispose();
                // enable settings controls
                EnableControls(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw ex;
            }

        }
        void ExecuteAlgorithm(ref JARE.Base.SolutionSet s)
        {
            s = SingleObjectiveOptimization();//, Parameters.PopulationSize, Parameters.Generations, Parameters.MutationProbability, Parameters.MutationPerturbation, Parameters.CrossoverProbability);
        }

        JARE.Base.SolutionSet SingleObjectiveOptimization()
        {
            //Kreiranje optimalnih resenja po funkciji cilja pojedinacno
            //////////////////////////////
            JARE.Base.Operator crossover;
            JARE.Base.Operator mutation;
            JARE.Base.Operator selection;
            //int numberOfVariables = GetNumberOfVariables();


            //string workingDir = Directory.GetCurrentDirectory();
            //string fullPath = ((FileStream)(sw.BaseStream)).Name;
            //string PrintDirectory = Path.GetDirectoryName(fullPath);
            //SignalFilteringProblem.m_parameters.setParameter("PrintDirectory", PrintDirectory);

            SignalFilteringProblem = new JARE.problems.singleObjective.SignalFiltering();

            SingleObjectiveAlgorithm = new PortfolioOptimizationGA(SignalFilteringProblem);
            

            SingleObjectiveAlgorithm.setInputParameter("populationSize", Parameters.PopulationSize);
            SingleObjectiveAlgorithm.setInputParameter("maxGenerations", Parameters.Generations);
            SingleObjectiveAlgorithm.setInputParameter("plateauTolerance", Parameters.PlateauTolerance);//VAZNO. Hardkodovano.
            SingleObjectiveAlgorithm.setInputParameter("maxPlateauGenerations", Parameters.MaxPlateauGenerations);

            SingleObjectiveAlgorithm.setInputParameter("onlyOptimalSolutions", true);
            SingleObjectiveAlgorithm.setInputParameter("PrintReportForGeneration", false);


            // crosover
            crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("UniformCrossover");
            crossover.setParameter("probability", Parameters.CrossoverProbability);
            crossover.setParameter("distributionIndex", 20.0);

            // mutation
            mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("UniformMutation");
            //_mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("SwapMutation");
            mutation.setParameter("probability", Parameters.MutationProbability);
            mutation.setParameter("perturbationIndex", Parameters.MutationPerturbation);

            // selection
            selection = JARE.Base.operators.selection.SelectionFactory.getSelectionOperator("BinaryTournament");
            selection.setParameter("size", Parameters.PopulationSize - 1);

            // set algorithm operators
            SingleObjectiveAlgorithm.addOperator("crossover", crossover);
            SingleObjectiveAlgorithm.addOperator("mutation", mutation);
            SingleObjectiveAlgorithm.addOperator("selection", selection);
            //_algorithm.addOperator("selectionPopulation", selection);

            // init algorithm
            //((PortfolioOptimizationGA)_algorithm).Init();
            SingleObjectiveAlgorithm.IterationCounterChanged += new PortfolioOptimizationGA.IterationCounterChangedHandler(GenerationCounterChanged);
            SingleObjectiveAlgorithm.TimeSeriesOnChartChanged += new PortfolioOptimizationGA.TimeSeriesOnChartChangedHandler(UpdateChart);
            OperationCanceled += new OperationCanceledHandler(SingleObjectiveAlgorithm.OperationCanceled);

            // execute algorithm
            JARE.Base.SolutionSet solutionSet = SingleObjectiveAlgorithm.execute();
            JARE.Base.Solution optimalSolution = solutionSet.getSolution(solutionSet.size() - 1);

            JARE.util.TimeSeries FittedTS = SignalFilteringProblem.getCalculatedTimeSeries(optimalSolution);
            FittedTS.toCSV("FittedTimeSeries.csv");

            //return optimalSolution;
            return solutionSet;
        }
        void GenerationCounterChanged(string currentGeneration)
        {
            SetText(currentGenerationBox, currentGeneration);
        }
        // Thread safe updating of control's text property
        private void SetText(System.Windows.Forms.Control control, string text)
        {
            if (control.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { control, text });
            }
            else
            {
                control.Text = text;
            }
        }
        private void loadParameters(string fileName)
        {
            try
            {
                // open selected file
                ParametersService service = new ParametersService();
                service.LoadParametersFromFile(Parameters, fileName);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                //UpdateControls();
            }
        }

        void UpdateChart(JARE.Base.Solution bestSolution)
        {
            UpdateChart(chart1, bestSolution);
        }

        // Thread safe updating of control's text property
        private void UpdateChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, JARE.Base.Solution bestSolution)
        {
            if (chart.InvokeRequired)
            {
                UpdateChartPoints d = new UpdateChartPoints(UpdateChart);
                Invoke(d, new object[] { chart, bestSolution });
            }
            else
            {
                JARE.util.TimeSeries FittedTS = SignalFilteringProblem.getCalculatedTimeSeries(bestSolution);
                chart.Series["ObservedTimeSeries"].Points.Clear();
                chart.Series["FittedTimeSeries"].Points.Clear();

                for (int i = 0; i < SignalFilteringProblem.ObservedSignal.Count(); i++)
                {
                    chart1.Series["ObservedTimeSeries"].Points.AddXY(SignalFilteringProblem.ObservedSignal.GetPointTime(i), SignalFilteringProblem.ObservedSignal.GetPointValue(i));
                    chart1.Series["FittedTimeSeries"].Points.AddXY(FittedTS.GetPointTime(i), FittedTS.GetPointValue(i));
                }
                chart.Update();
            }
        }

        // Delegates to enable async calls for setting controls properties
        private delegate void UpdateChartPoints(System.Windows.Forms.DataVisualization.Charting.Chart chart, JARE.Base.Solution bestSolution);

    }
}

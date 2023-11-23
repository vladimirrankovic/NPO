
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;
using JARE;
using System.Linq;

using TimeSeries = JARE.problems.Finance.DataTypes.TimeSeries;
using JARE.problems.Finance.DataTypes;
using Problem = JARE.Base.Problem;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using PortfolioOptimization = JARE.problems.Finance.PortfolioOptimization;
using PortfolioRebalance = JARE.problems.Finance.PortfolioRebalance;
using Backtesting = JARE.problems.Backtesting;
using PortfolioOptimizationGA = JARE.metaheuristics.singleObjective.geneticAlgorithm.PortfolioOptimizationGA;
using PortfolioOptimizationGAMultiThread = JARE.metaheuristics.singleObjective.geneticAlgorithm.PortfolioOptimizationGAMultiThread;
using PortfolioOptimizationGAViaBinder = JARE.metaheuristics.singleObjective.geneticAlgorithm.PortfolioOptimizationGAViaBinder;
using JARE.metaheuristics.spea2;
using JARE.metaheuristics.nsgaII;
using Solution = JARE.Base.Solution;
using SolutionSet = JARE.Base.SolutionSet;
using PortfolioGUI.DataTypes;
using PortfolioGUI.ServiceLayer;

namespace PortfolioGUI
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupDataBox;
        private System.Windows.Forms.Button loadDataButton1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
        StreamWriter sw;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.GroupBox groupSettingsBox;
        private System.Windows.Forms.Label labelPopulationSize;
        private System.Windows.Forms.TextBox populationSizeBox;
        private System.Windows.Forms.Label labelSelectionMethod;
        private System.Windows.Forms.ComboBox selectionBox;
        private System.Windows.Forms.Label labelIterations;
        private System.Windows.Forms.TextBox generationsNumberBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox groupExecutionBox;
        private System.Windows.Forms.Label labelCurrentIterations;
        private System.Windows.Forms.TextBox currentRebalanceDayBox;
        private System.Windows.Forms.TextBox solutionBox;

        PortfolioGUI.DataTypes.Parameters Parameters = new Parameters();

        private int selectionMethod = 0;

        private System.Collections.Generic.List<double[]> initialSolutionsList = new System.Collections.Generic.List<double[]>();
        private double[] referentSolution = null;
        private System.Collections.Generic.List<EvaluationCriteria> evaluationCriteriaList = new System.Collections.Generic.List<EvaluationCriteria>();

        AssetSet AssetTimeSeriesSet = null;
        AssetSet RebalanceAssetTimeSeriesSet = null;
        AssetSet CreditExposureTimeSeriesSet = null;
        AssetSet stressTimeSeriesSet = null;
        BondSet bondSet = null;
        System.Type _RETURN_BASED_CRITERIA = System.Type.GetType("JARE.problems.Finance.DataTypes.EvaluationCriteriaReturnBased");
        System.Type _CREDIT_BASED_CRITERIA = System.Type.GetType("JARE.problems.Finance.DataTypes.EvaluationCriteriaCreditExposureBased");
        System.Type _PARAMETER_BASED_CRITERIA = System.Type.GetType("JARE.problems.Finance.DataTypes.EvaluationCriteriaParameterBased");
        System.Type _TIMESERIES_BASED_CRITERIA = System.Type.GetType("JARE.problems.Finance.DataTypes.EvaluationCriteriaTimeSeriesBased");


        PortfolioOptimization problem; // The problem to solve
        Algorithm algorithm; // The algorithm to use
        Operator crossover; // Crossover operator
        Operator mutation; // Mutation operator
        Operator selection; // Selection operator

        TimeSeries replicationSerie = null;
        
        //private Thread workerThread = null;
        public BackgroundWorker workerThread = null;
        private ComboBox fitnessRebalCriteriaBox;
        private Label labelRebalCriteria;
        private GroupBox groupEvalSettingsBox;
        private TextBox ReturnPeriodBox;
        private TextBox evaluationPeriodBox;
        private Label labelEvalPeriod;
        private Label labelReturnPeriod;
        private DateTimePicker dateTimeRebalStartDate;
        private Label RebalStartDate;
        private TextBox interestRateBox;
        private Label labelInterestRate;
        private TextBox varThresholdBox;
        private Label labelVarTreshold;
        private TextBox parametersBox;
        private Label labelRebalPeriod;
        private TextBox textRebalPeriodBox;
        private Label labelTimeSeries;
        private Label labelRebalBuffer;
        private TextBox textRebalBufferBox;
        private TextBox mutationPerturbationBox;
        private Label label2;
        private TextBox mutationProbabilityBox;
        private Label label1;
        private Label label6;
        private TextBox lambdaBox;
        private TextBox currentGenerationBox;
        private Label label7;
        private Label label9;
        private TextBox textCriteriaChangeBox;
        private Button buttonLoadReplicationSerie;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button buttonClearOptimalSolutions;
        private Button buttonLoadOptimalSolutions;
        private Label label15;
        private Label label12;
        private DateTimePicker dateTimeOptimStartDate;
        private ComboBox fitnessCriteria2Box;
        private Label label16;
        private ComboBox fitnessCriteria1Box;
        private Label label3;
        private RadioButton radioOptimization;
        private RadioButton radioRebalancing;
        private Label label4;
        private Label label5;
        private Button buttonLoadPortfolio;
        private Button buttonClearOptimalPortfolio;
        private GroupBox groupBox3;
        private Button saveParametersButton;
        private Button loadParametersButton;
        private Label label8;
        private Button loadDataButton2;
        private Label label10;
        private TextBox cardinalityBox;
        private ComboBox decisionVariableBox;
        private Label label11;
        private ComboBox fitnessCriteria3Box;
        private GroupBox groupBox4;
        private Label label14;
        private Button buttonLoadRebalanceData;
        private RadioButton radioBacktesting;
        private Label label17;
        private TextBox crossoverProbabilityBox;
        private GroupBox groupBox5;
        private TextBox textBoxCRChange;
        private Label label19;
        private TextBox textBoxBactestingPeriod;
        private Label label18;
        private Label label20;
        private ComboBox TurnOverLimitTypeComboBox;
        private RadioButton radioButtonDistributed;
        private RadioButton radioButtonSingleThread;
        private Label label21;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private TextBox textBoxMOMaxGenerations;
        private Label label23;
        private TextBox textBoxMOTolerance;
        private Label label22;
        private GroupBox groupBox8;
        private Button buttonLoadStressTimeSeies;
        private Label label24;
        private ComboBox comboBoxCRType;
        private Button buttonLoadReferentSolution;
        private RadioButton radioButtonRelativeOptimization;
        private RadioButton radioButtonGlobalOptimization;
        private Button buttonResultsFile;
        private Label labelXColumn;
        private Label labelYColumn;
        private Label labelResultsFile;
        private TextBox textBoxXColumn;
        private TextBox textBoxYColumn;
        private Button buttonCalculateHypervolume;
        private GroupBox groupBoxHypervolume;
        private Button buttonDefaultSettings;
        private GroupBox groupBox9;
        private ListBox listBoxOutput;
        private CheckBox checkBoxScheduledRebalancing;
        private TextBox textBoxMaxRisk;
        private Label label25;
        private TextBox UpperBoundBox;
        private Label label27;
        private TextBox LowerBoundBox;
        private Label label26;
        private GroupBox groupBox10;
        private ComboBox bondsFitnessCriteriaComboBox2;
        private ComboBox bondsFitnessCriteriaComboBox1;
        private Label label28;
        private Button buttonLoadBonds;
        private Label label29;
        private CheckBox checkBoxPrintOnlyOptimalSolutions;
        private CheckBox checkBoxPrintReportForGeneration;
        private Label label13;

        // Constructor
        public MainForm()
        {
            // Required for Windows Form Designer support
            InitializeComponent();


            UpdateControls();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupDataBox = new System.Windows.Forms.GroupBox();
            this.labelTimeSeries = new System.Windows.Forms.Label();
            this.loadDataButton1 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fitnessCriteria1Box = new System.Windows.Forms.ComboBox();
            this.fitnessCriteria2Box = new System.Windows.Forms.ComboBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupSettingsBox = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.crossoverProbabilityBox = new System.Windows.Forms.TextBox();
            this.mutationPerturbationBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mutationProbabilityBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.generationsNumberBox = new System.Windows.Forms.TextBox();
            this.labelIterations = new System.Windows.Forms.Label();
            this.selectionBox = new System.Windows.Forms.ComboBox();
            this.labelSelectionMethod = new System.Windows.Forms.Label();
            this.populationSizeBox = new System.Windows.Forms.TextBox();
            this.labelPopulationSize = new System.Windows.Forms.Label();
            this.fitnessRebalCriteriaBox = new System.Windows.Forms.ComboBox();
            this.labelRebalCriteria = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.groupExecutionBox = new System.Windows.Forms.GroupBox();
            this.checkBoxPrintOnlyOptimalSolutions = new System.Windows.Forms.CheckBox();
            this.checkBoxPrintReportForGeneration = new System.Windows.Forms.CheckBox();
            this.radioButtonDistributed = new System.Windows.Forms.RadioButton();
            this.radioButtonSingleThread = new System.Windows.Forms.RadioButton();
            this.currentGenerationBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.currentRebalanceDayBox = new System.Windows.Forms.TextBox();
            this.labelCurrentIterations = new System.Windows.Forms.Label();
            this.parametersBox = new System.Windows.Forms.TextBox();
            this.solutionBox = new System.Windows.Forms.TextBox();
            this.groupEvalSettingsBox = new System.Windows.Forms.GroupBox();
            this.textBoxMaxRisk = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.checkBoxScheduledRebalancing = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.TurnOverLimitTypeComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.comboBoxCRType = new System.Windows.Forms.ComboBox();
            this.textBoxCRChange = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxBactestingPeriod = new System.Windows.Forms.TextBox();
            this.buttonClearOptimalPortfolio = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.buttonLoadRebalanceData = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonLoadPortfolio = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonLoadReplicationSerie = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textCriteriaChangeBox = new System.Windows.Forms.TextBox();
            this.labelRebalBuffer = new System.Windows.Forms.Label();
            this.textRebalBufferBox = new System.Windows.Forms.TextBox();
            this.labelRebalPeriod = new System.Windows.Forms.Label();
            this.textRebalPeriodBox = new System.Windows.Forms.TextBox();
            this.RebalStartDate = new System.Windows.Forms.Label();
            this.dateTimeRebalStartDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.lambdaBox = new System.Windows.Forms.TextBox();
            this.labelVarTreshold = new System.Windows.Forms.Label();
            this.varThresholdBox = new System.Windows.Forms.TextBox();
            this.labelInterestRate = new System.Windows.Forms.Label();
            this.interestRateBox = new System.Windows.Forms.TextBox();
            this.labelReturnPeriod = new System.Windows.Forms.Label();
            this.labelEvalPeriod = new System.Windows.Forms.Label();
            this.ReturnPeriodBox = new System.Windows.Forms.TextBox();
            this.evaluationPeriodBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UpperBoundBox = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.LowerBoundBox = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.decisionVariableBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cardinalityBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.bondsFitnessCriteriaComboBox2 = new System.Windows.Forms.ComboBox();
            this.bondsFitnessCriteriaComboBox1 = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.buttonLoadBonds = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.buttonLoadReferentSolution = new System.Windows.Forms.Button();
            this.radioButtonRelativeOptimization = new System.Windows.Forms.RadioButton();
            this.radioButtonGlobalOptimization = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.fitnessCriteria3Box = new System.Windows.Forms.ComboBox();
            this.loadDataButton2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonClearOptimalSolutions = new System.Windows.Forms.Button();
            this.buttonLoadOptimalSolutions = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.dateTimeOptimStartDate = new System.Windows.Forms.DateTimePicker();
            this.radioOptimization = new System.Windows.Forms.RadioButton();
            this.radioRebalancing = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonDefaultSettings = new System.Windows.Forms.Button();
            this.saveParametersButton = new System.Windows.Forms.Button();
            this.loadParametersButton = new System.Windows.Forms.Button();
            this.radioBacktesting = new System.Windows.Forms.RadioButton();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.textBoxMOMaxGenerations = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.textBoxMOTolerance = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.buttonLoadStressTimeSeies = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.buttonResultsFile = new System.Windows.Forms.Button();
            this.labelXColumn = new System.Windows.Forms.Label();
            this.labelYColumn = new System.Windows.Forms.Label();
            this.labelResultsFile = new System.Windows.Forms.Label();
            this.textBoxXColumn = new System.Windows.Forms.TextBox();
            this.textBoxYColumn = new System.Windows.Forms.TextBox();
            this.buttonCalculateHypervolume = new System.Windows.Forms.Button();
            this.groupBoxHypervolume = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.groupDataBox.SuspendLayout();
            this.groupSettingsBox.SuspendLayout();
            this.groupExecutionBox.SuspendLayout();
            this.groupEvalSettingsBox.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBoxHypervolume.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupDataBox
            // 
            this.groupDataBox.Controls.Add(this.labelTimeSeries);
            this.groupDataBox.Controls.Add(this.loadDataButton1);
            this.groupDataBox.Controls.Add(this.label16);
            this.groupDataBox.Controls.Add(this.label3);
            this.groupDataBox.Controls.Add(this.fitnessCriteria1Box);
            this.groupDataBox.Controls.Add(this.fitnessCriteria2Box);
            this.groupDataBox.Location = new System.Drawing.Point(626, 18);
            this.groupDataBox.Name = "groupDataBox";
            this.groupDataBox.Size = new System.Drawing.Size(761, 92);
            this.groupDataBox.TabIndex = 0;
            this.groupDataBox.TabStop = false;
            this.groupDataBox.Text = "Assets";
            // 
            // labelTimeSeries
            // 
            this.labelTimeSeries.Location = new System.Drawing.Point(59, 23);
            this.labelTimeSeries.Name = "labelTimeSeries";
            this.labelTimeSeries.Size = new System.Drawing.Size(125, 21);
            this.labelTimeSeries.TabIndex = 28;
            this.labelTimeSeries.Text = "Time Series:";
            // 
            // loadDataButton1
            // 
            this.loadDataButton1.Location = new System.Drawing.Point(186, 19);
            this.loadDataButton1.Name = "loadDataButton1";
            this.loadDataButton1.Size = new System.Drawing.Size(112, 31);
            this.loadDataButton1.TabIndex = 1;
            this.loadDataButton1.Text = "&Load";
            this.loadDataButton1.Click += new System.EventHandler(this.loadDataButton_Click);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(318, 60);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(144, 23);
            this.label16.TabIndex = 31;
            this.label16.Text = "Fitness criteria 2:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(318, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 23);
            this.label3.TabIndex = 29;
            this.label3.Text = "Fitness criteria 1:";
            // 
            // fitnessCriteria1Box
            // 
            this.fitnessCriteria1Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fitnessCriteria1Box.Items.AddRange(new object[] {
            "None",
            "Max Return",
            "Min STDEVP",
            "Max Sharp Index",
            "Max Sortino",
            "Min VaR",
            "Min cVaR",
            "Min ExpWeightedVaR",
            "Min ExpWeightedcVaR",
            "Max Skew",
            "Min Kurtosis",
            "Min VaRGarch",
            "Max Sharp-VaR",
            "Max Sharp-cVaR",
            "Min Repl Stdev",
            "Min CR VaR",
            "Min CR GARCHVaR",
            "Min DrawDown"});
            this.fitnessCriteria1Box.Location = new System.Drawing.Point(472, 18);
            this.fitnessCriteria1Box.Name = "fitnessCriteria1Box";
            this.fitnessCriteria1Box.Size = new System.Drawing.Size(256, 28);
            this.fitnessCriteria1Box.TabIndex = 30;
            // 
            // fitnessCriteria2Box
            // 
            this.fitnessCriteria2Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fitnessCriteria2Box.Items.AddRange(new object[] {
            "None",
            "Max Return",
            "Min STDEVP",
            "Max Sharp Index",
            "Max Sortino",
            "Min VaR",
            "Min cVaR",
            "Min ExpWeightedVaR",
            "Min ExpWeightedcVaR",
            "Max Skew",
            "Min Kurtosis",
            "Min VaRGarch",
            "Max Sharp-VaR",
            "Max Sharp-cVaR",
            "Min Repl Stdev",
            "Min CR VaR",
            "Min CR GARCHVaR",
            "Min DrawDown"});
            this.fitnessCriteria2Box.Location = new System.Drawing.Point(472, 53);
            this.fitnessCriteria2Box.Name = "fitnessCriteria2Box";
            this.fitnessCriteria2Box.Size = new System.Drawing.Size(256, 28);
            this.fitnessCriteria2Box.TabIndex = 32;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // groupSettingsBox
            // 
            this.groupSettingsBox.Controls.Add(this.label17);
            this.groupSettingsBox.Controls.Add(this.crossoverProbabilityBox);
            this.groupSettingsBox.Controls.Add(this.mutationPerturbationBox);
            this.groupSettingsBox.Controls.Add(this.label2);
            this.groupSettingsBox.Controls.Add(this.mutationProbabilityBox);
            this.groupSettingsBox.Controls.Add(this.label1);
            this.groupSettingsBox.Controls.Add(this.generationsNumberBox);
            this.groupSettingsBox.Controls.Add(this.labelIterations);
            this.groupSettingsBox.Controls.Add(this.selectionBox);
            this.groupSettingsBox.Controls.Add(this.labelSelectionMethod);
            this.groupSettingsBox.Controls.Add(this.populationSizeBox);
            this.groupSettingsBox.Controls.Add(this.labelPopulationSize);
            this.groupSettingsBox.Location = new System.Drawing.Point(387, 7);
            this.groupSettingsBox.Name = "groupSettingsBox";
            this.groupSettingsBox.Size = new System.Drawing.Size(834, 114);
            this.groupSettingsBox.TabIndex = 2;
            this.groupSettingsBox.TabStop = false;
            this.groupSettingsBox.Text = "GA Settings";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(512, 73);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(184, 23);
            this.label17.TabIndex = 15;
            this.label17.Text = "Crossover probability:";
            // 
            // crossoverProbabilityBox
            // 
            this.crossoverProbabilityBox.Location = new System.Drawing.Point(706, 67);
            this.crossoverProbabilityBox.Name = "crossoverProbabilityBox";
            this.crossoverProbabilityBox.Size = new System.Drawing.Size(56, 26);
            this.crossoverProbabilityBox.TabIndex = 14;
            // 
            // mutationPerturbationBox
            // 
            this.mutationPerturbationBox.Location = new System.Drawing.Point(440, 67);
            this.mutationPerturbationBox.Name = "mutationPerturbationBox";
            this.mutationPerturbationBox.Size = new System.Drawing.Size(56, 26);
            this.mutationPerturbationBox.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(250, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "Mutation perturbation:";
            // 
            // mutationProbabilityBox
            // 
            this.mutationProbabilityBox.Location = new System.Drawing.Point(186, 66);
            this.mutationProbabilityBox.Name = "mutationProbabilityBox";
            this.mutationProbabilityBox.Size = new System.Drawing.Size(56, 26);
            this.mutationProbabilityBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Mutation probability:";
            // 
            // generationsNumberBox
            // 
            this.generationsNumberBox.Location = new System.Drawing.Point(366, 31);
            this.generationsNumberBox.Name = "generationsNumberBox";
            this.generationsNumberBox.Size = new System.Drawing.Size(130, 26);
            this.generationsNumberBox.TabIndex = 9;
            // 
            // labelIterations
            // 
            this.labelIterations.Location = new System.Drawing.Point(250, 34);
            this.labelIterations.Name = "labelIterations";
            this.labelIterations.Size = new System.Drawing.Size(110, 23);
            this.labelIterations.TabIndex = 8;
            this.labelIterations.Text = "Generations:";
            // 
            // selectionBox
            // 
            this.selectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectionBox.Location = new System.Drawing.Point(680, 28);
            this.selectionBox.Name = "selectionBox";
            this.selectionBox.Size = new System.Drawing.Size(123, 28);
            this.selectionBox.TabIndex = 3;
            // 
            // labelSelectionMethod
            // 
            this.labelSelectionMethod.Location = new System.Drawing.Point(512, 34);
            this.labelSelectionMethod.Name = "labelSelectionMethod";
            this.labelSelectionMethod.Size = new System.Drawing.Size(152, 23);
            this.labelSelectionMethod.TabIndex = 2;
            this.labelSelectionMethod.Text = "Selection method:";
            // 
            // populationSizeBox
            // 
            this.populationSizeBox.Location = new System.Drawing.Point(162, 31);
            this.populationSizeBox.Name = "populationSizeBox";
            this.populationSizeBox.Size = new System.Drawing.Size(80, 26);
            this.populationSizeBox.TabIndex = 1;
            // 
            // labelPopulationSize
            // 
            this.labelPopulationSize.Location = new System.Drawing.Point(11, 35);
            this.labelPopulationSize.Name = "labelPopulationSize";
            this.labelPopulationSize.Size = new System.Drawing.Size(141, 23);
            this.labelPopulationSize.TabIndex = 0;
            this.labelPopulationSize.Text = "Population size:";
            // 
            // fitnessRebalCriteriaBox
            // 
            this.fitnessRebalCriteriaBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fitnessRebalCriteriaBox.Items.AddRange(new object[] {
            "None(1/N)",
            "Max Return",
            "Min STDEVP",
            "Max Sharp Index",
            "Max Sortino",
            "Min VaR",
            "Min cVaR",
            "Min ExpWeightedVaR",
            "Min ExpWeightedcVaR",
            "Max Skew",
            "Min Kurtosis",
            "Min VaRGarch",
            "Max Sharp-VaR",
            "Max Sharp-cVaR",
            "Min Repl Stdev",
            "Min CR VaR (N/A)",
            "Min CR GARCHVaR (N/A)",
            "Min DrawDown"});
            this.fitnessRebalCriteriaBox.Location = new System.Drawing.Point(554, 41);
            this.fitnessRebalCriteriaBox.Name = "fitnessRebalCriteriaBox";
            this.fitnessRebalCriteriaBox.Size = new System.Drawing.Size(363, 28);
            this.fitnessRebalCriteriaBox.TabIndex = 12;
            this.fitnessRebalCriteriaBox.SelectedValueChanged += new System.EventHandler(this.fitnessRebalCriteriaBox_SelectedValueChanged);
            // 
            // labelRebalCriteria
            // 
            this.labelRebalCriteria.Location = new System.Drawing.Point(358, 45);
            this.labelRebalCriteria.Name = "labelRebalCriteria";
            this.labelRebalCriteria.Size = new System.Drawing.Size(178, 27);
            this.labelRebalCriteria.TabIndex = 11;
            this.labelRebalCriteria.Text = "Optimization criteria:";
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(678, 28);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(120, 33);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(808, 28);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(120, 33);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // groupExecutionBox
            // 
            this.groupExecutionBox.Controls.Add(this.checkBoxPrintOnlyOptimalSolutions);
            this.groupExecutionBox.Controls.Add(this.checkBoxPrintReportForGeneration);
            this.groupExecutionBox.Controls.Add(this.radioButtonDistributed);
            this.groupExecutionBox.Controls.Add(this.radioButtonSingleThread);
            this.groupExecutionBox.Controls.Add(this.currentGenerationBox);
            this.groupExecutionBox.Controls.Add(this.label7);
            this.groupExecutionBox.Controls.Add(this.currentRebalanceDayBox);
            this.groupExecutionBox.Controls.Add(this.stopButton);
            this.groupExecutionBox.Controls.Add(this.startButton);
            this.groupExecutionBox.Controls.Add(this.labelCurrentIterations);
            this.groupExecutionBox.Location = new System.Drawing.Point(19, 881);
            this.groupExecutionBox.Name = "groupExecutionBox";
            this.groupExecutionBox.Size = new System.Drawing.Size(1554, 119);
            this.groupExecutionBox.TabIndex = 5;
            this.groupExecutionBox.TabStop = false;
            this.groupExecutionBox.Text = "Execution";
            // 
            // checkBoxPrintOnlyOptimalSolutions
            // 
            this.checkBoxPrintOnlyOptimalSolutions.AutoSize = true;
            this.checkBoxPrintOnlyOptimalSolutions.Checked = true;
            this.checkBoxPrintOnlyOptimalSolutions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPrintOnlyOptimalSolutions.Location = new System.Drawing.Point(171, 72);
            this.checkBoxPrintOnlyOptimalSolutions.Name = "checkBoxPrintOnlyOptimalSolutions";
            this.checkBoxPrintOnlyOptimalSolutions.Size = new System.Drawing.Size(221, 24);
            this.checkBoxPrintOnlyOptimalSolutions.TabIndex = 10;
            this.checkBoxPrintOnlyOptimalSolutions.Text = "Print only optimal solutions";
            this.checkBoxPrintOnlyOptimalSolutions.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrintReportForGeneration
            // 
            this.checkBoxPrintReportForGeneration.AutoSize = true;
            this.checkBoxPrintReportForGeneration.Location = new System.Drawing.Point(171, 37);
            this.checkBoxPrintReportForGeneration.Name = "checkBoxPrintReportForGeneration";
            this.checkBoxPrintReportForGeneration.Size = new System.Drawing.Size(216, 24);
            this.checkBoxPrintReportForGeneration.TabIndex = 9;
            this.checkBoxPrintReportForGeneration.Text = "Print report for generation";
            this.checkBoxPrintReportForGeneration.UseVisualStyleBackColor = true;
            // 
            // radioButtonDistributed
            // 
            this.radioButtonDistributed.AutoSize = true;
            this.radioButtonDistributed.Location = new System.Drawing.Point(10, 70);
            this.radioButtonDistributed.Name = "radioButtonDistributed";
            this.radioButtonDistributed.Size = new System.Drawing.Size(111, 24);
            this.radioButtonDistributed.TabIndex = 8;
            this.radioButtonDistributed.Text = "Distributed";
            this.radioButtonDistributed.UseVisualStyleBackColor = true;
            // 
            // radioButtonSingleThread
            // 
            this.radioButtonSingleThread.AutoSize = true;
            this.radioButtonSingleThread.Checked = true;
            this.radioButtonSingleThread.Location = new System.Drawing.Point(10, 37);
            this.radioButtonSingleThread.Name = "radioButtonSingleThread";
            this.radioButtonSingleThread.Size = new System.Drawing.Size(128, 24);
            this.radioButtonSingleThread.TabIndex = 7;
            this.radioButtonSingleThread.TabStop = true;
            this.radioButtonSingleThread.Text = "SingleThread";
            this.radioButtonSingleThread.UseVisualStyleBackColor = true;
            // 
            // currentGenerationBox
            // 
            this.currentGenerationBox.Location = new System.Drawing.Point(1429, 26);
            this.currentGenerationBox.Name = "currentGenerationBox";
            this.currentGenerationBox.ReadOnly = true;
            this.currentGenerationBox.Size = new System.Drawing.Size(99, 26);
            this.currentGenerationBox.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(1309, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 23);
            this.label7.TabIndex = 5;
            this.label7.Text = "Generation:";
            // 
            // currentRebalanceDayBox
            // 
            this.currentRebalanceDayBox.Location = new System.Drawing.Point(1176, 28);
            this.currentRebalanceDayBox.Name = "currentRebalanceDayBox";
            this.currentRebalanceDayBox.ReadOnly = true;
            this.currentRebalanceDayBox.Size = new System.Drawing.Size(99, 26);
            this.currentRebalanceDayBox.TabIndex = 1;
            // 
            // labelCurrentIterations
            // 
            this.labelCurrentIterations.Location = new System.Drawing.Point(1027, 32);
            this.labelCurrentIterations.Name = "labelCurrentIterations";
            this.labelCurrentIterations.Size = new System.Drawing.Size(139, 24);
            this.labelCurrentIterations.TabIndex = 0;
            this.labelCurrentIterations.Text = "Rebalance day:";
            // 
            // parametersBox
            // 
            this.parametersBox.Location = new System.Drawing.Point(0, 0);
            this.parametersBox.Name = "parametersBox";
            this.parametersBox.Size = new System.Drawing.Size(100, 26);
            this.parametersBox.TabIndex = 0;
            // 
            // solutionBox
            // 
            this.solutionBox.Location = new System.Drawing.Point(10, 20);
            this.solutionBox.Name = "solutionBox";
            this.solutionBox.ReadOnly = true;
            this.solutionBox.Size = new System.Drawing.Size(767, 26);
            this.solutionBox.TabIndex = 0;
            // 
            // groupEvalSettingsBox
            // 
            this.groupEvalSettingsBox.Controls.Add(this.textBoxMaxRisk);
            this.groupEvalSettingsBox.Controls.Add(this.label25);
            this.groupEvalSettingsBox.Controls.Add(this.checkBoxScheduledRebalancing);
            this.groupEvalSettingsBox.Controls.Add(this.label21);
            this.groupEvalSettingsBox.Controls.Add(this.label20);
            this.groupEvalSettingsBox.Controls.Add(this.TurnOverLimitTypeComboBox);
            this.groupEvalSettingsBox.Controls.Add(this.groupBox5);
            this.groupEvalSettingsBox.Controls.Add(this.label14);
            this.groupEvalSettingsBox.Controls.Add(this.textBoxBactestingPeriod);
            this.groupEvalSettingsBox.Controls.Add(this.buttonClearOptimalPortfolio);
            this.groupEvalSettingsBox.Controls.Add(this.label18);
            this.groupEvalSettingsBox.Controls.Add(this.buttonLoadRebalanceData);
            this.groupEvalSettingsBox.Controls.Add(this.label5);
            this.groupEvalSettingsBox.Controls.Add(this.buttonLoadPortfolio);
            this.groupEvalSettingsBox.Controls.Add(this.label4);
            this.groupEvalSettingsBox.Controls.Add(this.buttonLoadReplicationSerie);
            this.groupEvalSettingsBox.Controls.Add(this.label9);
            this.groupEvalSettingsBox.Controls.Add(this.textCriteriaChangeBox);
            this.groupEvalSettingsBox.Controls.Add(this.labelRebalBuffer);
            this.groupEvalSettingsBox.Controls.Add(this.textRebalBufferBox);
            this.groupEvalSettingsBox.Controls.Add(this.labelRebalPeriod);
            this.groupEvalSettingsBox.Controls.Add(this.textRebalPeriodBox);
            this.groupEvalSettingsBox.Controls.Add(this.RebalStartDate);
            this.groupEvalSettingsBox.Controls.Add(this.dateTimeRebalStartDate);
            this.groupEvalSettingsBox.Controls.Add(this.fitnessRebalCriteriaBox);
            this.groupEvalSettingsBox.Controls.Add(this.labelRebalCriteria);
            this.groupEvalSettingsBox.Location = new System.Drawing.Point(176, 576);
            this.groupEvalSettingsBox.Name = "groupEvalSettingsBox";
            this.groupEvalSettingsBox.Size = new System.Drawing.Size(1397, 304);
            this.groupEvalSettingsBox.TabIndex = 13;
            this.groupEvalSettingsBox.TabStop = false;
            this.groupEvalSettingsBox.Text = "Rebalancing/Backtesting";
            // 
            // textBoxMaxRisk
            // 
            this.textBoxMaxRisk.Enabled = false;
            this.textBoxMaxRisk.Location = new System.Drawing.Point(1331, 170);
            this.textBoxMaxRisk.Name = "textBoxMaxRisk";
            this.textBoxMaxRisk.Size = new System.Drawing.Size(56, 26);
            this.textBoxMaxRisk.TabIndex = 46;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(1218, 172);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(98, 20);
            this.label25.TabIndex = 45;
            this.label25.Text = "Max risk (%):";
            // 
            // checkBoxScheduledRebalancing
            // 
            this.checkBoxScheduledRebalancing.AutoSize = true;
            this.checkBoxScheduledRebalancing.Location = new System.Drawing.Point(821, 250);
            this.checkBoxScheduledRebalancing.Name = "checkBoxScheduledRebalancing";
            this.checkBoxScheduledRebalancing.Size = new System.Drawing.Size(204, 24);
            this.checkBoxScheduledRebalancing.TabIndex = 44;
            this.checkBoxScheduledRebalancing.Text = "Scheduled Rebalancing";
            this.checkBoxScheduledRebalancing.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(1019, 79);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(352, 53);
            this.label21.TabIndex = 43;
            this.label21.Text = "*Rebalancing: starting date of rebalancing procedure";
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(530, 175);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(160, 19);
            this.label20.TabIndex = 42;
            this.label20.Text = "Turnover limit type:";
            // 
            // TurnOverLimitTypeComboBox
            // 
            this.TurnOverLimitTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TurnOverLimitTypeComboBox.Items.AddRange(new object[] {
            "TurnOver Greater Than",
            "TurnOver Less Than",
            "Cumulative TurnOver Less Than",
            "TurnOver Within Limit (Ver2)"});
            this.TurnOverLimitTypeComboBox.Location = new System.Drawing.Point(690, 168);
            this.TurnOverLimitTypeComboBox.Name = "TurnOverLimitTypeComboBox";
            this.TurnOverLimitTypeComboBox.Size = new System.Drawing.Size(286, 28);
            this.TurnOverLimitTypeComboBox.TabIndex = 41;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.comboBoxCRType);
            this.groupBox5.Controls.Add(this.textBoxCRChange);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Location = new System.Drawing.Point(14, 219);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(496, 73);
            this.groupBox5.TabIndex = 39;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Capital Requirements";
            // 
            // comboBoxCRType
            // 
            this.comboBoxCRType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCRType.Items.AddRange(new object[] {
            "None",
            "CR Historical VaR",
            "CR GARCH VaR"});
            this.comboBoxCRType.Location = new System.Drawing.Point(13, 29);
            this.comboBoxCRType.Name = "comboBoxCRType";
            this.comboBoxCRType.Size = new System.Drawing.Size(256, 28);
            this.comboBoxCRType.TabIndex = 42;
            // 
            // textBoxCRChange
            // 
            this.textBoxCRChange.Location = new System.Drawing.Point(424, 29);
            this.textBoxCRChange.Name = "textBoxCRChange";
            this.textBoxCRChange.Size = new System.Drawing.Size(56, 26);
            this.textBoxCRChange.TabIndex = 40;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(285, 34);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(121, 20);
            this.label19.TabIndex = 41;
            this.label19.Text = "CR change (%):";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 45);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(125, 21);
            this.label14.TabIndex = 34;
            this.label14.Text = "Time Series:";
            // 
            // textBoxBactestingPeriod
            // 
            this.textBoxBactestingPeriod.Location = new System.Drawing.Point(757, 101);
            this.textBoxBactestingPeriod.Name = "textBoxBactestingPeriod";
            this.textBoxBactestingPeriod.Size = new System.Drawing.Size(85, 26);
            this.textBoxBactestingPeriod.TabIndex = 40;
            // 
            // buttonClearOptimalPortfolio
            // 
            this.buttonClearOptimalPortfolio.Location = new System.Drawing.Point(438, 98);
            this.buttonClearOptimalPortfolio.Name = "buttonClearOptimalPortfolio";
            this.buttonClearOptimalPortfolio.Size = new System.Drawing.Size(104, 34);
            this.buttonClearOptimalPortfolio.TabIndex = 38;
            this.buttonClearOptimalPortfolio.Text = "Clear";
            this.buttonClearOptimalPortfolio.UseVisualStyleBackColor = true;
            this.buttonClearOptimalPortfolio.Click += new System.EventHandler(this.buttonLClearOptimalPortfolio_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(590, 105);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(145, 20);
            this.label18.TabIndex = 28;
            this.label18.Text = "Backtesting period:";
            // 
            // buttonLoadRebalanceData
            // 
            this.buttonLoadRebalanceData.Location = new System.Drawing.Point(174, 38);
            this.buttonLoadRebalanceData.Name = "buttonLoadRebalanceData";
            this.buttonLoadRebalanceData.Size = new System.Drawing.Size(104, 34);
            this.buttonLoadRebalanceData.TabIndex = 33;
            this.buttonLoadRebalanceData.Text = "&Load";
            this.buttonLoadRebalanceData.Click += new System.EventHandler(this.buttonLoadRebalanceData_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(286, 27);
            this.label5.TabIndex = 38;
            this.label5.Text = "Rebalancing/Backtesting portfolio:";
            // 
            // buttonLoadPortfolio
            // 
            this.buttonLoadPortfolio.Location = new System.Drawing.Point(306, 98);
            this.buttonLoadPortfolio.Name = "buttonLoadPortfolio";
            this.buttonLoadPortfolio.Size = new System.Drawing.Size(104, 34);
            this.buttonLoadPortfolio.TabIndex = 38;
            this.buttonLoadPortfolio.Text = "&Load";
            this.buttonLoadPortfolio.Click += new System.EventHandler(this.buttonLoadPortfolio_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(530, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 23);
            this.label4.TabIndex = 37;
            this.label4.Text = "Replication series:";
            // 
            // buttonLoadReplicationSerie
            // 
            this.buttonLoadReplicationSerie.Location = new System.Drawing.Point(704, 244);
            this.buttonLoadReplicationSerie.Name = "buttonLoadReplicationSerie";
            this.buttonLoadReplicationSerie.Size = new System.Drawing.Size(104, 34);
            this.buttonLoadReplicationSerie.TabIndex = 29;
            this.buttonLoadReplicationSerie.Text = "&Load";
            this.buttonLoadReplicationSerie.Click += new System.EventHandler(this.buttonLoadReplicationSerie_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(989, 174);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(148, 20);
            this.label9.TabIndex = 36;
            this.label9.Text = "Criteria change (%):";
            // 
            // textCriteriaChangeBox
            // 
            this.textCriteriaChangeBox.Location = new System.Drawing.Point(1157, 170);
            this.textCriteriaChangeBox.Name = "textCriteriaChangeBox";
            this.textCriteriaChangeBox.Size = new System.Drawing.Size(56, 26);
            this.textCriteriaChangeBox.TabIndex = 35;
            // 
            // labelRebalBuffer
            // 
            this.labelRebalBuffer.Location = new System.Drawing.Point(301, 174);
            this.labelRebalBuffer.Name = "labelRebalBuffer";
            this.labelRebalBuffer.Size = new System.Drawing.Size(144, 19);
            this.labelRebalBuffer.TabIndex = 29;
            this.labelRebalBuffer.Text = "Turnover limit (%):";
            // 
            // textRebalBufferBox
            // 
            this.textRebalBufferBox.Location = new System.Drawing.Point(454, 170);
            this.textRebalBufferBox.Name = "textRebalBufferBox";
            this.textRebalBufferBox.Size = new System.Drawing.Size(56, 26);
            this.textRebalBufferBox.TabIndex = 28;
            // 
            // labelRebalPeriod
            // 
            this.labelRebalPeriod.AutoSize = true;
            this.labelRebalPeriod.Location = new System.Drawing.Point(10, 175);
            this.labelRebalPeriod.Name = "labelRebalPeriod";
            this.labelRebalPeriod.Size = new System.Drawing.Size(164, 20);
            this.labelRebalPeriod.TabIndex = 27;
            this.labelRebalPeriod.Text = "Rebalancing duration:";
            // 
            // textRebalPeriodBox
            // 
            this.textRebalPeriodBox.Location = new System.Drawing.Point(194, 170);
            this.textRebalPeriodBox.Name = "textRebalPeriodBox";
            this.textRebalPeriodBox.Size = new System.Drawing.Size(84, 26);
            this.textRebalPeriodBox.TabIndex = 26;
            // 
            // RebalStartDate
            // 
            this.RebalStartDate.Location = new System.Drawing.Point(1019, 48);
            this.RebalStartDate.Name = "RebalStartDate";
            this.RebalStartDate.Size = new System.Drawing.Size(56, 24);
            this.RebalStartDate.TabIndex = 17;
            this.RebalStartDate.Text = "Date:";
            // 
            // dateTimeRebalStartDate
            // 
            this.dateTimeRebalStartDate.Location = new System.Drawing.Point(1080, 45);
            this.dateTimeRebalStartDate.Name = "dateTimeRebalStartDate";
            this.dateTimeRebalStartDate.Size = new System.Drawing.Size(307, 26);
            this.dateTimeRebalStartDate.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1053, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 20);
            this.label6.TabIndex = 34;
            this.label6.Text = "Lambda:";
            // 
            // lambdaBox
            // 
            this.lambdaBox.Location = new System.Drawing.Point(1133, 63);
            this.lambdaBox.Name = "lambdaBox";
            this.lambdaBox.Size = new System.Drawing.Size(61, 26);
            this.lambdaBox.TabIndex = 33;
            // 
            // labelVarTreshold
            // 
            this.labelVarTreshold.Location = new System.Drawing.Point(824, 69);
            this.labelVarTreshold.Name = "labelVarTreshold";
            this.labelVarTreshold.Size = new System.Drawing.Size(162, 23);
            this.labelVarTreshold.TabIndex = 21;
            this.labelVarTreshold.Text = "VaR threshold (%):";
            // 
            // varThresholdBox
            // 
            this.varThresholdBox.Location = new System.Drawing.Point(986, 64);
            this.varThresholdBox.Name = "varThresholdBox";
            this.varThresholdBox.Size = new System.Drawing.Size(56, 26);
            this.varThresholdBox.TabIndex = 20;
            // 
            // labelInterestRate
            // 
            this.labelInterestRate.Location = new System.Drawing.Point(598, 67);
            this.labelInterestRate.Name = "labelInterestRate";
            this.labelInterestRate.Size = new System.Drawing.Size(156, 24);
            this.labelInterestRate.TabIndex = 19;
            this.labelInterestRate.Text = "Interest rate (%):";
            // 
            // interestRateBox
            // 
            this.interestRateBox.Location = new System.Drawing.Point(758, 64);
            this.interestRateBox.Name = "interestRateBox";
            this.interestRateBox.Size = new System.Drawing.Size(56, 26);
            this.interestRateBox.TabIndex = 18;
            // 
            // labelReturnPeriod
            // 
            this.labelReturnPeriod.AutoSize = true;
            this.labelReturnPeriod.Location = new System.Drawing.Point(915, 31);
            this.labelReturnPeriod.Name = "labelReturnPeriod";
            this.labelReturnPeriod.Size = new System.Drawing.Size(186, 20);
            this.labelReturnPeriod.TabIndex = 16;
            this.labelReturnPeriod.Text = "Return evaluation period:";
            // 
            // labelEvalPeriod
            // 
            this.labelEvalPeriod.AutoSize = true;
            this.labelEvalPeriod.Location = new System.Drawing.Point(630, 28);
            this.labelEvalPeriod.Name = "labelEvalPeriod";
            this.labelEvalPeriod.Size = new System.Drawing.Size(168, 20);
            this.labelEvalPeriod.TabIndex = 15;
            this.labelEvalPeriod.Text = "Risk evaluation period:";
            // 
            // ReturnPeriodBox
            // 
            this.ReturnPeriodBox.Location = new System.Drawing.Point(1126, 26);
            this.ReturnPeriodBox.Name = "ReturnPeriodBox";
            this.ReturnPeriodBox.Size = new System.Drawing.Size(68, 26);
            this.ReturnPeriodBox.TabIndex = 14;
            // 
            // evaluationPeriodBox
            // 
            this.evaluationPeriodBox.Location = new System.Drawing.Point(829, 25);
            this.evaluationPeriodBox.Name = "evaluationPeriodBox";
            this.evaluationPeriodBox.Size = new System.Drawing.Size(67, 26);
            this.evaluationPeriodBox.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UpperBoundBox);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.LowerBoundBox);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.decisionVariableBox);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cardinalityBox);
            this.groupBox1.Controls.Add(this.labelEvalPeriod);
            this.groupBox1.Controls.Add(this.evaluationPeriodBox);
            this.groupBox1.Controls.Add(this.labelReturnPeriod);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.ReturnPeriodBox);
            this.groupBox1.Controls.Add(this.lambdaBox);
            this.groupBox1.Controls.Add(this.labelVarTreshold);
            this.groupBox1.Controls.Add(this.interestRateBox);
            this.groupBox1.Controls.Add(this.labelInterestRate);
            this.groupBox1.Controls.Add(this.varThresholdBox);
            this.groupBox1.Location = new System.Drawing.Point(16, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1205, 148);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optimization parameters";
            // 
            // UpperBoundBox
            // 
            this.UpperBoundBox.Location = new System.Drawing.Point(371, 102);
            this.UpperBoundBox.Name = "UpperBoundBox";
            this.UpperBoundBox.Size = new System.Drawing.Size(61, 26);
            this.UpperBoundBox.TabIndex = 43;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(258, 107);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(108, 20);
            this.label27.TabIndex = 42;
            this.label27.Text = "Upper Bound:";
            // 
            // LowerBoundBox
            // 
            this.LowerBoundBox.Location = new System.Drawing.Point(184, 102);
            this.LowerBoundBox.Name = "LowerBoundBox";
            this.LowerBoundBox.Size = new System.Drawing.Size(61, 26);
            this.LowerBoundBox.TabIndex = 41;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(66, 107);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(107, 20);
            this.label26.TabIndex = 40;
            this.label26.Text = "Lower Bound:";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(85, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 23);
            this.label11.TabIndex = 39;
            this.label11.Text = "Variable type:";
            // 
            // decisionVariableBox
            // 
            this.decisionVariableBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.decisionVariableBox.Items.AddRange(new object[] {
            "Buy-and-Hold (Realized return)",
            "Active trading (Expected return)"});
            this.decisionVariableBox.Location = new System.Drawing.Point(219, 25);
            this.decisionVariableBox.Name = "decisionVariableBox";
            this.decisionVariableBox.Size = new System.Drawing.Size(399, 28);
            this.decisionVariableBox.TabIndex = 39;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(86, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 20);
            this.label10.TabIndex = 36;
            this.label10.Text = "Cardinality:";
            // 
            // cardinalityBox
            // 
            this.cardinalityBox.Location = new System.Drawing.Point(184, 64);
            this.cardinalityBox.Name = "cardinalityBox";
            this.cardinalityBox.Size = new System.Drawing.Size(61, 26);
            this.cardinalityBox.TabIndex = 35;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox10);
            this.groupBox2.Controls.Add(this.buttonLoadReferentSolution);
            this.groupBox2.Controls.Add(this.radioButtonRelativeOptimization);
            this.groupBox2.Controls.Add(this.radioButtonGlobalOptimization);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.buttonClearOptimalSolutions);
            this.groupBox2.Controls.Add(this.buttonLoadOptimalSolutions);
            this.groupBox2.Controls.Add(this.groupDataBox);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.dateTimeOptimStartDate);
            this.groupBox2.Location = new System.Drawing.Point(176, 281);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1397, 292);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optimization";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.bondsFitnessCriteriaComboBox2);
            this.groupBox10.Controls.Add(this.bondsFitnessCriteriaComboBox1);
            this.groupBox10.Controls.Add(this.label28);
            this.groupBox10.Controls.Add(this.buttonLoadBonds);
            this.groupBox10.Controls.Add(this.label29);
            this.groupBox10.Location = new System.Drawing.Point(626, 184);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(760, 95);
            this.groupBox10.TabIndex = 45;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Bonds";
            // 
            // bondsFitnessCriteriaComboBox2
            // 
            this.bondsFitnessCriteriaComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bondsFitnessCriteriaComboBox2.Items.AddRange(new object[] {
            "None",
            "Max YieldToMaturity",
            "Min SCR"});
            this.bondsFitnessCriteriaComboBox2.Location = new System.Drawing.Point(472, 56);
            this.bondsFitnessCriteriaComboBox2.Name = "bondsFitnessCriteriaComboBox2";
            this.bondsFitnessCriteriaComboBox2.Size = new System.Drawing.Size(256, 28);
            this.bondsFitnessCriteriaComboBox2.TabIndex = 35;
            // 
            // bondsFitnessCriteriaComboBox1
            // 
            this.bondsFitnessCriteriaComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bondsFitnessCriteriaComboBox1.Items.AddRange(new object[] {
            "None",
            "Max YieldToMaturity",
            "Min SCR"});
            this.bondsFitnessCriteriaComboBox1.Location = new System.Drawing.Point(472, 20);
            this.bondsFitnessCriteriaComboBox1.Name = "bondsFitnessCriteriaComboBox1";
            this.bondsFitnessCriteriaComboBox1.Size = new System.Drawing.Size(256, 28);
            this.bondsFitnessCriteriaComboBox1.TabIndex = 33;
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(314, 58);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(144, 24);
            this.label28.TabIndex = 34;
            this.label28.Text = "Fitness criteria 2:";
            // 
            // buttonLoadBonds
            // 
            this.buttonLoadBonds.Location = new System.Drawing.Point(186, 18);
            this.buttonLoadBonds.Name = "buttonLoadBonds";
            this.buttonLoadBonds.Size = new System.Drawing.Size(112, 30);
            this.buttonLoadBonds.TabIndex = 33;
            this.buttonLoadBonds.Text = "&Load";
            this.buttonLoadBonds.Click += new System.EventHandler(this.buttonLoadBonds_Click);
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(314, 23);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(144, 24);
            this.label29.TabIndex = 33;
            this.label29.Text = "Fitness criteria 1:";
            // 
            // buttonLoadReferentSolution
            // 
            this.buttonLoadReferentSolution.Location = new System.Drawing.Point(362, 178);
            this.buttonLoadReferentSolution.Name = "buttonLoadReferentSolution";
            this.buttonLoadReferentSolution.Size = new System.Drawing.Size(112, 34);
            this.buttonLoadReferentSolution.TabIndex = 44;
            this.buttonLoadReferentSolution.Text = "&Load";
            this.buttonLoadReferentSolution.Click += new System.EventHandler(this.buttonLoadReferentSolution_Click);
            // 
            // radioButtonRelativeOptimization
            // 
            this.radioButtonRelativeOptimization.AutoSize = true;
            this.radioButtonRelativeOptimization.Location = new System.Drawing.Point(362, 126);
            this.radioButtonRelativeOptimization.Name = "radioButtonRelativeOptimization";
            this.radioButtonRelativeOptimization.Size = new System.Drawing.Size(211, 24);
            this.radioButtonRelativeOptimization.TabIndex = 43;
            this.radioButtonRelativeOptimization.Text = "Relative to given portfolio";
            this.radioButtonRelativeOptimization.UseVisualStyleBackColor = true;
            this.radioButtonRelativeOptimization.Click += new System.EventHandler(this.radioButtonRelativeOptimization_Click);
            // 
            // radioButtonGlobalOptimization
            // 
            this.radioButtonGlobalOptimization.AutoSize = true;
            this.radioButtonGlobalOptimization.Checked = true;
            this.radioButtonGlobalOptimization.Location = new System.Drawing.Point(362, 92);
            this.radioButtonGlobalOptimization.Name = "radioButtonGlobalOptimization";
            this.radioButtonGlobalOptimization.Size = new System.Drawing.Size(169, 24);
            this.radioButtonGlobalOptimization.TabIndex = 42;
            this.radioButtonGlobalOptimization.TabStop = true;
            this.radioButtonGlobalOptimization.Text = "Global optimization";
            this.radioButtonGlobalOptimization.UseVisualStyleBackColor = true;
            this.radioButtonGlobalOptimization.Click += new System.EventHandler(this.radioButtonGlobalOptimization_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.fitnessCriteria3Box);
            this.groupBox4.Controls.Add(this.loadDataButton2);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(626, 115);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(761, 65);
            this.groupBox4.TabIndex = 41;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Credit Exposure";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(318, 35);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 23);
            this.label13.TabIndex = 39;
            this.label13.Text = "Fitness criteria 3:";
            // 
            // fitnessCriteria3Box
            // 
            this.fitnessCriteria3Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fitnessCriteria3Box.Items.AddRange(new object[] {
            "None",
            "Min Average",
            "Min STDEVP"});
            this.fitnessCriteria3Box.Location = new System.Drawing.Point(472, 26);
            this.fitnessCriteria3Box.Name = "fitnessCriteria3Box";
            this.fitnessCriteria3Box.Size = new System.Drawing.Size(256, 28);
            this.fitnessCriteria3Box.TabIndex = 40;
            // 
            // loadDataButton2
            // 
            this.loadDataButton2.Location = new System.Drawing.Point(187, 29);
            this.loadDataButton2.Name = "loadDataButton2";
            this.loadDataButton2.Size = new System.Drawing.Size(112, 31);
            this.loadDataButton2.TabIndex = 29;
            this.loadDataButton2.Text = "&Load";
            this.loadDataButton2.Click += new System.EventHandler(this.loadDataButton2_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(59, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 27);
            this.label8.TabIndex = 30;
            this.label8.Text = "Time series:";
            // 
            // buttonClearOptimalSolutions
            // 
            this.buttonClearOptimalSolutions.Location = new System.Drawing.Point(146, 178);
            this.buttonClearOptimalSolutions.Name = "buttonClearOptimalSolutions";
            this.buttonClearOptimalSolutions.Size = new System.Drawing.Size(112, 34);
            this.buttonClearOptimalSolutions.TabIndex = 37;
            this.buttonClearOptimalSolutions.Text = "Clear";
            this.buttonClearOptimalSolutions.UseVisualStyleBackColor = true;
            this.buttonClearOptimalSolutions.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonLoadOptimalSolutions
            // 
            this.buttonLoadOptimalSolutions.Location = new System.Drawing.Point(24, 178);
            this.buttonLoadOptimalSolutions.Name = "buttonLoadOptimalSolutions";
            this.buttonLoadOptimalSolutions.Size = new System.Drawing.Size(112, 34);
            this.buttonLoadOptimalSolutions.TabIndex = 35;
            this.buttonLoadOptimalSolutions.Text = "&Load";
            this.buttonLoadOptimalSolutions.Click += new System.EventHandler(this.buttonLoadOptimalSolutions_Click);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(27, 140);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(155, 27);
            this.label15.TabIndex = 36;
            this.label15.Text = "Initial solutions:";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(22, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(192, 29);
            this.label12.TabIndex = 34;
            this.label12.Text = "Optimization Date:";
            // 
            // dateTimeOptimStartDate
            // 
            this.dateTimeOptimStartDate.Location = new System.Drawing.Point(24, 88);
            this.dateTimeOptimStartDate.Name = "dateTimeOptimStartDate";
            this.dateTimeOptimStartDate.Size = new System.Drawing.Size(320, 26);
            this.dateTimeOptimStartDate.TabIndex = 33;
            // 
            // radioOptimization
            // 
            this.radioOptimization.AutoSize = true;
            this.radioOptimization.Checked = true;
            this.radioOptimization.Location = new System.Drawing.Point(16, 376);
            this.radioOptimization.Name = "radioOptimization";
            this.radioOptimization.Size = new System.Drawing.Size(122, 24);
            this.radioOptimization.TabIndex = 16;
            this.radioOptimization.TabStop = true;
            this.radioOptimization.Text = "Optimization";
            this.radioOptimization.UseVisualStyleBackColor = true;
            this.radioOptimization.CheckedChanged += new System.EventHandler(this.radioOptimization_CheckedChanged);
            // 
            // radioRebalancing
            // 
            this.radioRebalancing.AutoSize = true;
            this.radioRebalancing.Location = new System.Drawing.Point(16, 738);
            this.radioRebalancing.Name = "radioRebalancing";
            this.radioRebalancing.Size = new System.Drawing.Size(123, 24);
            this.radioRebalancing.TabIndex = 17;
            this.radioRebalancing.Text = "Rebalancing";
            this.radioRebalancing.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonDefaultSettings);
            this.groupBox3.Controls.Add(this.saveParametersButton);
            this.groupBox3.Controls.Add(this.loadParametersButton);
            this.groupBox3.Location = new System.Drawing.Point(16, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(362, 114);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Calculation parameters";
            // 
            // buttonDefaultSettings
            // 
            this.buttonDefaultSettings.Location = new System.Drawing.Point(13, 51);
            this.buttonDefaultSettings.Name = "buttonDefaultSettings";
            this.buttonDefaultSettings.Size = new System.Drawing.Size(104, 29);
            this.buttonDefaultSettings.TabIndex = 30;
            this.buttonDefaultSettings.Text = "&Default";
            this.buttonDefaultSettings.Click += new System.EventHandler(this.buttonDefaultSettings_Click);
            // 
            // saveParametersButton
            // 
            this.saveParametersButton.Location = new System.Drawing.Point(240, 51);
            this.saveParametersButton.Name = "saveParametersButton";
            this.saveParametersButton.Size = new System.Drawing.Size(104, 29);
            this.saveParametersButton.TabIndex = 29;
            this.saveParametersButton.Text = "&Save";
            this.saveParametersButton.Click += new System.EventHandler(this.saveParametersButton_Click);
            // 
            // loadParametersButton
            // 
            this.loadParametersButton.Location = new System.Drawing.Point(126, 51);
            this.loadParametersButton.Name = "loadParametersButton";
            this.loadParametersButton.Size = new System.Drawing.Size(104, 29);
            this.loadParametersButton.TabIndex = 29;
            this.loadParametersButton.Text = "&Load";
            this.loadParametersButton.Click += new System.EventHandler(this.loadParametersButton_Click);
            // 
            // radioBacktesting
            // 
            this.radioBacktesting.AutoSize = true;
            this.radioBacktesting.Location = new System.Drawing.Point(16, 621);
            this.radioBacktesting.Name = "radioBacktesting";
            this.radioBacktesting.Size = new System.Drawing.Size(118, 24);
            this.radioBacktesting.TabIndex = 19;
            this.radioBacktesting.Text = "Backtesting";
            this.radioBacktesting.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.AxisX.LabelStyle.Format = "e2";
            chartArea1.AxisY.LabelStyle.Format = "e2";
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(10, 19);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Front";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(691, 525);
            this.chart1.TabIndex = 20;
            this.chart1.Text = "chart1";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chart1);
            this.groupBox6.Location = new System.Drawing.Point(1582, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(711, 553);
            this.groupBox6.TabIndex = 21;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Chart";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.textBoxMOMaxGenerations);
            this.groupBox7.Controls.Add(this.label23);
            this.groupBox7.Controls.Add(this.textBoxMOTolerance);
            this.groupBox7.Controls.Add(this.label22);
            this.groupBox7.Location = new System.Drawing.Point(1230, 7);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(341, 114);
            this.groupBox7.TabIndex = 22;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Convergence parameters";
            // 
            // textBoxMOMaxGenerations
            // 
            this.textBoxMOMaxGenerations.Location = new System.Drawing.Point(168, 31);
            this.textBoxMOMaxGenerations.Name = "textBoxMOMaxGenerations";
            this.textBoxMOMaxGenerations.Size = new System.Drawing.Size(56, 26);
            this.textBoxMOMaxGenerations.TabIndex = 4;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(14, 34);
            this.label23.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(148, 23);
            this.label23.TabIndex = 3;
            this.label23.Text = "Max generations:";
            // 
            // textBoxMOTolerance
            // 
            this.textBoxMOTolerance.Location = new System.Drawing.Point(122, 66);
            this.textBoxMOTolerance.Name = "textBoxMOTolerance";
            this.textBoxMOTolerance.Size = new System.Drawing.Size(102, 26);
            this.textBoxMOTolerance.TabIndex = 2;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(14, 72);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(96, 23);
            this.label22.TabIndex = 1;
            this.label22.Text = "Tolerance:";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.buttonLoadStressTimeSeies);
            this.groupBox8.Controls.Add(this.label24);
            this.groupBox8.Location = new System.Drawing.Point(1230, 124);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(340, 100);
            this.groupBox8.TabIndex = 23;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Capital Requirements";
            // 
            // buttonLoadStressTimeSeies
            // 
            this.buttonLoadStressTimeSeies.Location = new System.Drawing.Point(168, 32);
            this.buttonLoadStressTimeSeies.Name = "buttonLoadStressTimeSeies";
            this.buttonLoadStressTimeSeies.Size = new System.Drawing.Size(112, 31);
            this.buttonLoadStressTimeSeies.TabIndex = 17;
            this.buttonLoadStressTimeSeies.Text = "&Load";
            this.buttonLoadStressTimeSeies.Click += new System.EventHandler(this.buttonLoadStressTimeSeries_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(13, 38);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(139, 20);
            this.label24.TabIndex = 16;
            this.label24.Text = "Stress time series:";
            // 
            // buttonResultsFile
            // 
            this.buttonResultsFile.Location = new System.Drawing.Point(182, 25);
            this.buttonResultsFile.Name = "buttonResultsFile";
            this.buttonResultsFile.Size = new System.Drawing.Size(112, 31);
            this.buttonResultsFile.TabIndex = 24;
            this.buttonResultsFile.Text = "&Chose";
            this.buttonResultsFile.Click += new System.EventHandler(this.buttonResultsFile_Click);
            // 
            // labelXColumn
            // 
            this.labelXColumn.Location = new System.Drawing.Point(14, 66);
            this.labelXColumn.Name = "labelXColumn";
            this.labelXColumn.Size = new System.Drawing.Size(140, 23);
            this.labelXColumn.TabIndex = 41;
            this.labelXColumn.Text = "X column:";
            // 
            // labelYColumn
            // 
            this.labelYColumn.Location = new System.Drawing.Point(14, 99);
            this.labelYColumn.Name = "labelYColumn";
            this.labelYColumn.Size = new System.Drawing.Size(140, 24);
            this.labelYColumn.TabIndex = 42;
            this.labelYColumn.Text = "Y column:";
            // 
            // labelResultsFile
            // 
            this.labelResultsFile.Location = new System.Drawing.Point(10, 31);
            this.labelResultsFile.Name = "labelResultsFile";
            this.labelResultsFile.Size = new System.Drawing.Size(144, 23);
            this.labelResultsFile.TabIndex = 43;
            this.labelResultsFile.Text = "Results file:";
            // 
            // textBoxXColumn
            // 
            this.textBoxXColumn.Location = new System.Drawing.Point(182, 61);
            this.textBoxXColumn.Name = "textBoxXColumn";
            this.textBoxXColumn.Size = new System.Drawing.Size(112, 26);
            this.textBoxXColumn.TabIndex = 44;
            // 
            // textBoxYColumn
            // 
            this.textBoxYColumn.Location = new System.Drawing.Point(182, 95);
            this.textBoxYColumn.Name = "textBoxYColumn";
            this.textBoxYColumn.Size = new System.Drawing.Size(112, 26);
            this.textBoxYColumn.TabIndex = 45;
            // 
            // buttonCalculateHypervolume
            // 
            this.buttonCalculateHypervolume.Location = new System.Drawing.Point(182, 129);
            this.buttonCalculateHypervolume.Name = "buttonCalculateHypervolume";
            this.buttonCalculateHypervolume.Size = new System.Drawing.Size(112, 30);
            this.buttonCalculateHypervolume.TabIndex = 47;
            this.buttonCalculateHypervolume.Text = "&Calculate";
            this.buttonCalculateHypervolume.Click += new System.EventHandler(this.buttonCalculateHypervolume_Click);
            // 
            // groupBoxHypervolume
            // 
            this.groupBoxHypervolume.Controls.Add(this.textBoxYColumn);
            this.groupBoxHypervolume.Controls.Add(this.buttonCalculateHypervolume);
            this.groupBoxHypervolume.Controls.Add(this.buttonResultsFile);
            this.groupBoxHypervolume.Controls.Add(this.labelXColumn);
            this.groupBoxHypervolume.Controls.Add(this.textBoxXColumn);
            this.groupBoxHypervolume.Controls.Add(this.labelYColumn);
            this.groupBoxHypervolume.Controls.Add(this.labelResultsFile);
            this.groupBoxHypervolume.Location = new System.Drawing.Point(1971, 576);
            this.groupBoxHypervolume.Name = "groupBoxHypervolume";
            this.groupBoxHypervolume.Size = new System.Drawing.Size(322, 193);
            this.groupBoxHypervolume.TabIndex = 48;
            this.groupBoxHypervolume.TabStop = false;
            this.groupBoxHypervolume.Text = "Hypervolume";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.listBoxOutput);
            this.groupBox9.Location = new System.Drawing.Point(1592, 576);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(370, 193);
            this.groupBox9.TabIndex = 49;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Output";
            // 
            // listBoxOutput
            // 
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.ItemHeight = 20;
            this.listBoxOutput.Location = new System.Drawing.Point(10, 28);
            this.listBoxOutput.Name = "listBoxOutput";
            this.listBoxOutput.Size = new System.Drawing.Size(350, 64);
            this.listBoxOutput.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1284, 701);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBoxHypervolume);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.radioBacktesting);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.radioRebalancing);
            this.Controls.Add(this.radioOptimization);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupEvalSettingsBox);
            this.Controls.Add(this.groupExecutionBox);
            this.Controls.Add(this.groupSettingsBox);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Portfolio $olutions";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.groupDataBox.ResumeLayout(false);
            this.groupSettingsBox.ResumeLayout(false);
            this.groupSettingsBox.PerformLayout();
            this.groupExecutionBox.ResumeLayout(false);
            this.groupExecutionBox.PerformLayout();
            this.groupEvalSettingsBox.ResumeLayout(false);
            this.groupEvalSettingsBox.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBoxHypervolume.ResumeLayout(false);
            this.groupBoxHypervolume.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        // Delegates to enable async calls for setting controls properties
        private delegate void SetTextCallback(System.Windows.Forms.Control control, string text);

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

        // Delegates to enable async calls for setting controls properties
        private delegate void UpdateChartPoints(System.Windows.Forms.DataVisualization.Charting.Chart chart, SolutionSet efficientFrontier);

        // Thread safe updating of control's text property
        private void UpdateChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, SolutionSet efficientFrontier)
        {
            if (chart.InvokeRequired)
            {
                UpdateChartPoints d = new UpdateChartPoints(UpdateChart);
                Invoke(d, new object[] { chart, efficientFrontier });
            }
            else
            {
                chart.Series["Front"].Points.Clear();
                for (int i = 0; i < efficientFrontier.size(); i++) chart1.Series["Front"].Points.AddXY(efficientFrontier.getSolution(i).getObjective(1), -efficientFrontier.getSolution(i).getObjective(0));
                chart.Update();
            }
        }


        // On main form closing
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // check if worker thread is running
            if (workerThread != null)
            {
                Dispose(true);
                workerThread.CancelAsync();
                workerThread = null;               
            }
            if (sw != null) sw.Dispose();
        }

        // Update settings controls
        private void UpdateControls()
        {
            try
            {
                //GA Settings
                //selectionBox.SelectedIndex = selectionMethod;
                populationSizeBox.Text = Parameters.PopulationSize.ToString();
                generationsNumberBox.Text = Parameters.Generations.ToString();
                mutationProbabilityBox.Text = Parameters.MutationProbability.ToString();
                mutationPerturbationBox.Text = Parameters.MutationPerturbation.ToString();
                crossoverProbabilityBox.Text = Parameters.CrossoverProbability.ToString();


                //Optimization parameters
                decisionVariableBox.SelectedIndex = (int)Parameters.DecisionVarType;
                cardinalityBox.Text = Parameters.Cardinality.ToString();
                LowerBoundBox.Text = Parameters.LowerBound.ToString();
                UpperBoundBox.Text = Parameters.UpperBound.ToString();
                evaluationPeriodBox.Text = Parameters.EvaluationPeriod.ToString();
                ReturnPeriodBox.Text = Parameters.ReturnPeriod.ToString();
                interestRateBox.Text = Parameters.InterestRate.ToString();
                varThresholdBox.Text = Parameters.VarTreshold.ToString();
                lambdaBox.Text = Parameters.Lambda.ToString();

                //Optimization
                dateTimeOptimStartDate.Value = Parameters.OptimizationDate;
                fitnessCriteria1Box.SelectedIndex = (int)Parameters.OptimizationEvaluationCriteria1.CriteriaType;
                fitnessCriteria2Box.SelectedIndex = (int)Parameters.OptimizationEvaluationCriteria2.CriteriaType;
                fitnessCriteria3Box.SelectedIndex = (int)Parameters.OptimizationEvaluationCriteria3.CriteriaType;
                bondsFitnessCriteriaComboBox1.SelectedIndex = (int)Parameters.OptimizationEvaluationCriteria4.CriteriaType;
                bondsFitnessCriteriaComboBox2.SelectedIndex = (int)Parameters.OptimizationEvaluationCriteria5.CriteriaType;

                //Rebalancing
                fitnessRebalCriteriaBox.SelectedIndex = (int)Parameters.RebalanceEvaluationCriteria.CriteriaType;
                dateTimeRebalStartDate.Value = Parameters.RebalancingStartDate;
                textRebalPeriodBox.Text = Parameters.RebalancingDuration.ToString();
                textRebalBufferBox.Text = Parameters.RebalancingBuffer.ToString();
                textCriteriaChangeBox.Text = Parameters.CriteriaChange.ToString();
                textBoxBactestingPeriod.Text = Parameters.BacktestingPeriod.ToString();
                textBoxCRChange.Text = Parameters.CRChange.ToString();
                TurnOverLimitTypeComboBox.SelectedIndex = (int)Parameters.TOLimitType;
                comboBoxCRType.SelectedIndex = (int)Parameters.CRType;
                checkBoxScheduledRebalancing.Checked = Parameters.ScheduledRebalancing;
                textBoxMaxRisk.Text = Parameters.MaxAllowedRisk.ToString();

                //Multiobjective settings
                textBoxMOMaxGenerations.Text = Parameters.MaxPlateauGenerations.ToString();
                textBoxMOTolerance.Text = Parameters.PlateauTolerance.ToString();

                //Hypervolume
                int XColumn = 12;
                textBoxXColumn.Text = XColumn.ToString();
                int YColumn = 1;
                textBoxYColumn.Text = YColumn.ToString();
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
            }
            EnableControls();
        }

        // Update settings controls
        private void UpdateParameters()
        {
            try
            {
                Parameters.PopulationSize = int.Parse(populationSizeBox.Text);
                Parameters.Generations = int.Parse(generationsNumberBox.Text);
                Parameters.DecisionVarType = (DecisionVariableType)decisionVariableBox.SelectedIndex;
                Parameters.Cardinality = int.Parse(cardinalityBox.Text);
                Parameters.LowerBound = double.Parse(LowerBoundBox.Text);
                Parameters.UpperBound = double.Parse(UpperBoundBox.Text);
                Parameters.EvaluationPeriod = int.Parse(evaluationPeriodBox.Text);
                Parameters.ReturnPeriod = int.Parse(ReturnPeriodBox.Text);
                Parameters.InterestRate = double.Parse(interestRateBox.Text);
                Parameters.VarTreshold = double.Parse(varThresholdBox.Text);
                Parameters.Lambda = double.Parse(lambdaBox.Text);
                Parameters.OptimizationDate = this.dateTimeOptimStartDate.Value;
                Parameters.RebalancingStartDate = this.dateTimeRebalStartDate.Value;
                Parameters.RebalancingDuration = int.Parse(textRebalPeriodBox.Text);
                Parameters.CriteriaChange = double.Parse(textCriteriaChangeBox.Text);
                Parameters.RebalancingBuffer = double.Parse(textRebalBufferBox.Text);
                Parameters.MutationPerturbation = double.Parse(mutationPerturbationBox.Text);
                Parameters.MutationProbability = double.Parse(mutationProbabilityBox.Text);
                Parameters.OptimizationEvaluationCriteria1.CriteriaType = (EvaluationCriteriaReturnBased.criteriaType)fitnessCriteria1Box.SelectedIndex;
                Parameters.OptimizationEvaluationCriteria2.CriteriaType = (EvaluationCriteriaReturnBased.criteriaType)fitnessCriteria2Box.SelectedIndex;
                Parameters.OptimizationEvaluationCriteria3.CriteriaType = (EvaluationCriteriaCreditExposureBased.criteriaType)fitnessCriteria3Box.SelectedIndex;
                Parameters.OptimizationEvaluationCriteria4.CriteriaType = (EvaluationCriteriaParameterBased.criteriaType)bondsFitnessCriteriaComboBox1.SelectedIndex;
                Parameters.OptimizationEvaluationCriteria5.CriteriaType = (EvaluationCriteriaParameterBased.criteriaType)bondsFitnessCriteriaComboBox2.SelectedIndex;
                Parameters.RebalanceEvaluationCriteria.CriteriaType = (EvaluationCriteriaReturnBased.criteriaType)fitnessRebalCriteriaBox.SelectedIndex;
                Parameters.CrossoverProbability = double.Parse(crossoverProbabilityBox.Text);
                Parameters.BacktestingPeriod = int.Parse(textBoxBactestingPeriod.Text);
                Parameters.CRChange = double.Parse(textBoxCRChange.Text);
                Parameters.CRType = (PortfolioRebalance.CapitalRequirementsType)comboBoxCRType.SelectedIndex;
                Parameters.TOLimitType = (PortfolioRebalance.TurnOverLimitType)TurnOverLimitTypeComboBox.SelectedIndex;
                Parameters.PlateauTolerance = double.Parse(textBoxMOTolerance.Text);
                Parameters.MaxPlateauGenerations = int.Parse(textBoxMOMaxGenerations.Text);
                Parameters.ScheduledRebalancing = checkBoxScheduledRebalancing.Checked;
                Parameters.MaxAllowedRisk = double.Parse(textBoxMaxRisk.Text);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
            }
        }

        // Load data
        private void loadDataButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!ImportTimeSeries("AssetTimeSeriesSet")) return;
                Parameters.OptimizationEvaluationCriteria1.TimeSeriesSetID = AssetTimeSeriesSet.ID;
                Parameters.OptimizationEvaluationCriteria2.TimeSeriesSetID = AssetTimeSeriesSet.ID;
            }
            catch (Exception)
            {
                return;
            }
            Parameters.PopulationSize = AssetTimeSeriesSet.Count * 5;//preporuka
            Parameters.Cardinality = AssetTimeSeriesSet.Count;
            populationSizeBox.Text = Parameters.PopulationSize.ToString();
            cardinalityBox.Text = Parameters.Cardinality.ToString();
            startButton.Enabled = true;
        }

        private bool ImportTimeSeries(string name)
        {
            System.Collections.Generic.List<Asset> Assets = new System.Collections.Generic.List<Asset>();
            if (ImportTimeSeries(Assets))
            {
                if(name == "AssetTimeSeriesSet") AssetTimeSeriesSet = new AssetSet(1, Assets);
                if (name == "CreditExposureTimeSeriesSet") CreditExposureTimeSeriesSet = new AssetSet(1, Assets);
                if (name == "RebalanceAssetTimeSeriesSet") RebalanceAssetTimeSeriesSet = new AssetSet(1, Assets);

                return true;
            }
            else return false;
        }

        private bool ImportTimeSeries(System.Collections.Generic.List<Asset> Assets)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK) return ImportTimeSeriesBasic(Assets, openFileDialog.FileName);
            else return false;
        }

        private bool ImportTimeSeriesBasic(System.Collections.Generic.List<Asset> Assets, string fileName)
        {
            StreamReader reader = null;
            int fundsCount = 5;

            try
            {
                // open selected file
                reader = File.OpenText(fileName);
                string str = null;

                if ((str = reader.ReadLine()) != null)
                {
                    string[] strs = str.Split(';');
                    if (strs.Length == 1) strs = str.Split(',');
                    fundsCount = strs.Length - 1;
                    for (int i = 0; i < fundsCount; i++)
                    {
                        TimeSeries current = new TimeSeries();
                        string name = strs[i + 1];
                        Asset currentAsset = new Asset(name, current);
                        Assets.Add(currentAsset);
                    }

                }
                // read the data
                while ((str = reader.ReadLine()) != null)
                {
                    string[] strs = str.Split(';');
                    if (strs.Length == 1) strs = str.Split(',');
                    // parse X
                    DateTime date = DateTime.Parse(strs[0]);

                    for (int i = 0; i < fundsCount; i++)
                    {
                        DayValue dayValue = new DayValue();
                        dayValue.Day = date;
                        dayValue.Value = double.Parse(strs[i + 1]);
                        Assets[i].timeSeries.DataSeries.Add(dayValue);
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // close file
                if (reader != null)
                    reader.Close();
            }
            return true;
        }

        // Delegates to enable async calls for setting controls properties
        private delegate void EnableCallback(bool enable);

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
                populationSizeBox.Enabled = enable;
                generationsNumberBox.Enabled = enable;
                selectionBox.Enabled = enable;

                loadParametersButton.Enabled = enable;
                saveParametersButton.Enabled = enable;

                cardinalityBox.Enabled = enable;
                decisionVariableBox.Enabled = enable;
                evaluationPeriodBox.Enabled = enable;
                varThresholdBox.Enabled = enable;
                ReturnPeriodBox.Enabled = enable;
                lambdaBox.Enabled = enable;
                mutationProbabilityBox.Enabled = enable;
                mutationPerturbationBox.Enabled = enable;
                interestRateBox.Enabled = enable;
                radioOptimization.Enabled = enable;
                radioRebalancing.Enabled = enable;
                textBoxMOMaxGenerations.Enabled = enable;
                textBoxMOTolerance.Enabled = enable;
                UpperBoundBox.Enabled = enable;
                LowerBoundBox.Enabled = enable;
                crossoverProbabilityBox.Enabled = enable;
                
                if (radioRebalancing.Checked || radioBacktesting.Checked)
                {
                    buttonLoadRebalanceData.Enabled = enable;
                    fitnessRebalCriteriaBox.Enabled = enable;
                    dateTimeRebalStartDate.Enabled = enable;
                    textRebalPeriodBox.Enabled = enable;
                    textRebalBufferBox.Enabled = enable;
                    buttonLoadReplicationSerie.Enabled = enable;
                    buttonLoadPortfolio.Enabled = enable;
                    buttonClearOptimalPortfolio.Enabled = enable;
                    textBoxBactestingPeriod.Enabled = enable;
                    textBoxCRChange.Enabled = enable;
                    TurnOverLimitTypeComboBox.Enabled = enable;
                    checkBoxScheduledRebalancing.Enabled = enable;
                }
                if (radioOptimization.Checked)
                {
                    loadDataButton1.Enabled = enable;
                    loadDataButton2.Enabled = enable;
                    fitnessCriteria1Box.Enabled = enable;
                    fitnessCriteria2Box.Enabled = enable;
                    fitnessCriteria3Box.Enabled = enable;
                    bondsFitnessCriteriaComboBox1.Enabled = enable;
                    bondsFitnessCriteriaComboBox2.Enabled = enable;

                    dateTimeOptimStartDate.Enabled = enable;
                    buttonLoadOptimalSolutions.Enabled = enable;
                    buttonClearOptimalSolutions.Enabled = enable;                    
                }


                startButton.Enabled = enable;
                stopButton.Enabled = !enable;
            }
        }

        void RebalanceDayChanged(int currentRebalanceDay)
        {
            SetText(currentRebalanceDayBox, currentRebalanceDay.ToString());
        }

        void GenerationCounterChanged(string currentGeneration)
        {
            SetText(currentGenerationBox, currentGeneration);
        }

        void UpdateChart(SolutionSet eficientFrontier)
        {
            UpdateChart(chart1, eficientFrontier);
        }

        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {
            solutionBox.Text = string.Empty;
            parametersBox.Text = string.Empty;
            selectionMethod = selectionBox.SelectedIndex;

            UpdateParameters();
            
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            saveFileDialog.Title = "Save data file";

            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
                {
                    sw = new StreamWriter(saveFileDialog.FileName, false);
                }
                else return;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            workerThread = new BackgroundWorker();
            //if (radioRebalancing.Checked) workerThread = new Thread(new ThreadStart(ExecuteRebalancing));
            //else if (radioBacktesting.Checked) workerThread = new Thread(new ThreadStart(ExecuteBacktesting));
            //else if (radioOptimization.Checked) workerThread = new Thread(new ThreadStart(Optimization));
            if (radioRebalancing.Checked)
            {
                workerThread.DoWork += new DoWorkEventHandler(ExecuteRebalancing);
            }
            else if (radioBacktesting.Checked)
            {
                workerThread.DoWork += new DoWorkEventHandler(ExecuteBacktesting);
            }
            else if (radioOptimization.Checked)
            {
                workerThread.DoWork += new DoWorkEventHandler(Optimization);
            }
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
        void workerThreadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (sw != null) sw.Dispose();
            EnableControls(true);
        }
        
        public delegate void OperationCanceledHandler(bool Cancel);
        public event OperationCanceledHandler OperationCanceled;

        // On button "Stop"
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            stopApplication();
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

 
        // Worker thread
        void ExecuteRebalancing(object sender, DoWorkEventArgs e)
        {
            int numberOfVariables = GetNumberOfVariables();
            PortfolioRebalance PR = new PortfolioRebalance(Parameters.RebalanceEvaluationCriteria, Parameters.RebalancingStartDate, 
                                                           Parameters.RebalancingDuration, Parameters.RebalancingBuffer, 
                                                           numberOfVariables, Parameters.Cardinality, Parameters.LowerBound, Parameters.UpperBound, 
                                                           Parameters.CRType, Parameters.TOLimitType, Parameters.ScheduledRebalancing);

            PR.m_parameters.setParameter("RebalanceAssetTimeSeriesSet", RebalanceAssetTimeSeriesSet);
            PR.m_parameters.setParameter("populationSize", Parameters.PopulationSize);
            PR.m_parameters.setParameter("maxGenerations", Parameters.Generations);
            PR.m_parameters.setParameter("crossoverProbability", Parameters.CrossoverProbability);
            PR.m_parameters.setParameter("mutationProbability", Parameters.MutationProbability);
            PR.m_parameters.setParameter("mutationPerturbation", Parameters.MutationPerturbation);
            PR.m_parameters.setParameter("varThreshold", Parameters.VarTreshold * 0.01);
            PR.m_parameters.setParameter("targetRate", Parameters.InterestRate);
            PR.m_parameters.setParameter("lambda", Parameters.Lambda);
            PR.m_parameters.setParameter("criteriaChange", Parameters.CriteriaChange);
            if (Parameters.RebalanceEvaluationCriteria.CriteriaType == EvaluationCriteriaReturnBased.criteriaType.ReplicationStDev) PR.m_parameters.setParameter("replicationSerie", replicationSerie);
            PR.m_parameters.setParameter("backtestingPeriod", Parameters.BacktestingPeriod);
            PR.m_parameters.setParameter("CRChange", Parameters.CRChange);
            PR.m_parameters.setParameter("maxAllowedRisk", Parameters.MaxAllowedRisk);
            PR.m_parameters.setParameter("plateauTolerance", Parameters.PlateauTolerance);//VAZNO. Hardkodovano.
            PR.m_parameters.setParameter("maxPlateauGenerations", Parameters.MaxPlateauGenerations);
            PR.m_parameters.setParameter("optimizationDate", Parameters.OptimizationDate);
            PR.m_parameters.setParameter("returnEvaluationPeriod", Parameters.ReturnPeriod);
            PR.m_parameters.setParameter("evaluationPeriod", Parameters.EvaluationPeriod);

            string workingDir = Directory.GetCurrentDirectory();
            PR.m_parameters.setParameter("RScriptPath", workingDir);
            string fullPath = ((FileStream)(sw.BaseStream)).Name;
            string PrintDirectory = Path.GetDirectoryName(fullPath);
            PR.m_parameters.setParameter("PrintDirectory", PrintDirectory);
            PR.Init();

            PR.RebalanceDayChanged += new PortfolioRebalance.RebalanceDayChangedHandler(RebalanceDayChanged);
            PR.GenerationChanged += new PortfolioRebalance.GenerationChangedHandler(GenerationCounterChanged);
            OperationCanceled += new OperationCanceledHandler(PR.OperationCanceled);
            OperationCanceled += new OperationCanceledHandler(PR.algorithm.OperationCanceled);

            try
            {
                if (initialSolutionsList != null && initialSolutionsList.Count > 0)
                {
                    for (int i = 0; i < initialSolutionsList.Count; i++)
                    {
                        string firstLine = "Solution - " + (i+1).ToString();
                        sw.WriteLine(firstLine);
                        PR.m_parameters.setParameter("activePortfolio", initialSolutionsList[i], true);
                        PR.Execute(sw);
                        if (workerThread.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        //FileStream fs = (FileStream)sw.BaseStream;
                        //string fullPath = fs.Name;
                        //fullPath.Replace(".csv", (i + 1).ToString() + ".csv");
                        //sw.Dispose();
                        //if ((i+1) < optimalSolutionsList.Count) sw = new StreamWriter(fullPath);
                    }
                }
                else PR.Execute(sw);
                //sw.Dispose();

                //// enable settings controls
                //EnableControls(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                
        }

        // Worker thread
        void ExecuteBacktesting(object sender, DoWorkEventArgs e)
        {
            Backtesting BT = new Backtesting(Parameters.RebalanceEvaluationCriteria, Parameters.RebalancingStartDate, Parameters.BacktestingPeriod);

            BT.m_parameters.setParameter("RebalanceAssetTimeSeriesSet", RebalanceAssetTimeSeriesSet);
            BT.m_parameters.setParameter("returnEvaluationPeriod", Parameters.ReturnPeriod);
            BT.m_parameters.setParameter("evaluationPeriod", Parameters.EvaluationPeriod);
            BT.m_parameters.setParameter("varThreshold", Parameters.VarTreshold * 0.01);
            BT.m_parameters.setParameter("targetRate", Parameters.InterestRate);
            BT.m_parameters.setParameter("lambda", Parameters.Lambda);
            string workingDir = Directory.GetCurrentDirectory();
            BT.m_parameters.setParameter("RScriptPath", workingDir);
            string fullPath = ((FileStream)(sw.BaseStream)).Name;
            string PrintDirectory = Path.GetDirectoryName(fullPath);
            BT.m_parameters.setParameter("PrintDirectory", PrintDirectory);
            if (stressTimeSeriesSet != null) BT.m_parameters.setParameter("stressTimeSeriesSet", stressTimeSeriesSet);

            BT.RebalanceDayChanged += new Backtesting.RebalanceDayChangedHandler(RebalanceDayChanged);
            BT.GenerationChanged += new Backtesting.GenerationChangedHandler(GenerationCounterChanged);
            OperationCanceled += new OperationCanceledHandler(BT.OperationCanceled);

            try
            {
                if (initialSolutionsList != null && initialSolutionsList.Count > 0)
                {
                    for (int i = 0; i < initialSolutionsList.Count; i++)
                    {
                        string firstLine = "Solution - " + (i + 1).ToString();
                        sw.WriteLine(firstLine);
                        BT.m_parameters.setParameter("activePortfolio", initialSolutionsList[i], true);
                        BT.Execute(sw);
                        if (workerThread.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                else BT.Execute(sw);
                //sw.Dispose();

                //// enable settings controls
                //EnableControls(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonLoadReplicationSerie_Click(object sender, EventArgs e)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);
                    string str = null;
                    replicationSerie = new TimeSeries();

                    if ((str = reader.ReadLine()) != null)
                    {
                        string[] strs = str.Split(';');
                        strs = str.Split(',');
                        replicationSerie.name = strs[1];
                    }

                    // read the data
                    while ((str = reader.ReadLine()) != null)
                    {
                        string[] strs = str.Split(';');
                        strs = str.Split(',');
                        // parse X
                        DateTime date = DateTime.Parse(strs[0]);

                        DayValue dayValue = new DayValue();
                        dayValue.Day = date;
                        dayValue.Value = double.Parse(strs[1]);
                        replicationSerie.DataSeries.Add(dayValue);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }
                //if (timeSeriesSets.GetAt(0).Count > 0) startButton.Enabled = true;
            }
        }

        private void fitnessRebalCriteriaBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((string)(fitnessRebalCriteriaBox.SelectedItem) == "Min Repl Stdev" && replicationSerie == null) startButton.Enabled = false;
        }

        private void buttonLoadOptimalSolutions_Click(object sender, EventArgs e)
        {
            importInputSolutions(initialSolutionsList);
        }

        private void importInputSolutions(System.Collections.Generic.List<double[]> solutionList)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);
                    string str = null;

                    solutionList.Clear();
                    // read the data
                    while ((str = reader.ReadLine()) != null)
                    {
                        string[] strs = str.Split(';');
                        if (strs.Length == 1) strs = str.Split(',');
                        double[] ponder = new double[strs.Length];
                        for (int i = 0; i < strs.Length; i++)
                        {
                            ponder[i] = double.Parse(strs[i]);
                        }
                        solutionList.Add(ponder);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }
            }
        }
        private void importInputSolutions(out double[] solution)
        {
            solution = new double[0];

            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);
                    string str = null;

                    // read the data
                    while ((str = reader.ReadLine()) != null)
                    {
                        string[] strs = str.Split(';');
                        if (strs.Length == 1) strs = str.Split(',');
                        solution = new double[strs.Length];
                        for (int i = 0; i < strs.Length; i++)
                        {
                            solution[i] = double.Parse(strs[i]);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }
            }
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            initialSolutionsList.Clear();
        }

        private void radioOptimization_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void EnableControls()
        {
            //Optimization
            fitnessCriteria1Box.Enabled = radioOptimization.Checked;
            fitnessCriteria2Box.Enabled = radioOptimization.Checked;
            loadDataButton1.Enabled = radioOptimization.Checked;
            loadDataButton2.Enabled = radioOptimization.Checked;
            fitnessCriteria3Box.Enabled = radioOptimization.Checked;
            dateTimeOptimStartDate.Enabled = radioOptimization.Checked;
            buttonLoadOptimalSolutions.Enabled = radioOptimization.Checked;
            buttonClearOptimalSolutions.Enabled = radioOptimization.Checked;
            radioButtonGlobalOptimization.Enabled = radioOptimization.Checked;
            radioButtonRelativeOptimization.Enabled = radioOptimization.Checked;
            buttonLoadReferentSolution.Enabled = (radioOptimization.Checked && radioButtonRelativeOptimization.Checked);
            bondsFitnessCriteriaComboBox1.Enabled = radioOptimization.Checked;
            bondsFitnessCriteriaComboBox2.Enabled = radioOptimization.Checked;

            //Rebalancing
            bool Checked = radioRebalancing.Checked ? true : radioBacktesting.Checked ? true : false;
            buttonLoadRebalanceData.Enabled = Checked;
            fitnessRebalCriteriaBox.Enabled = Checked;
            dateTimeRebalStartDate.Enabled = Checked;
            textRebalBufferBox.Enabled = (Checked || radioButtonRelativeOptimization.Checked);
            textRebalPeriodBox.Enabled = Checked;
            buttonLoadReplicationSerie.Enabled = Checked;
            textCriteriaChangeBox.Enabled = Checked;
            buttonLoadPortfolio.Enabled = Checked;
            buttonClearOptimalPortfolio.Enabled = Checked;
            textBoxCRChange.Enabled = Checked;
            textBoxBactestingPeriod.Enabled = Checked;
            TurnOverLimitTypeComboBox.Enabled = Checked;
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
                problem.m_parameters.setParameter("RScriptPath", workingDir, true);
                problem.executionType = JARE.problems.Finance.ExecutionType.singleThread;
                problem.PrintResults(sw, population);
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
            generateCriteriaList();

            //Multiobjective
            if (evaluationCriteriaList.Count > 1)
            {
                InitOptimization();
                s = algorithm.execute();
            }
            else
            {
                //JARE.Base.Solution optimalSolutionForSingleObjective = SingleObjectiveOptimization(optimizationEvaluationCriteria[0], Parameters.PopulationSize, Parameters.Generations, Parameters.MutationProbability, Parameters.MutationPerturbation);
                s = SingleObjectiveOptimization(evaluationCriteriaList[0]);//, Parameters.PopulationSize, Parameters.Generations, Parameters.MutationProbability, Parameters.MutationPerturbation, Parameters.CrossoverProbability);
                // s = new JARE.Base.SolutionSet(1);
                // s.add(optimalSolutionForSingleObjective);
            }
        }

        void generateCriteriaList()
        {
            evaluationCriteriaList.Clear();
            if (Parameters.OptimizationEvaluationCriteria1.CriteriaType != EvaluationCriteriaReturnBased.criteriaType.none)
            {
                evaluationCriteriaList.Add(Parameters.OptimizationEvaluationCriteria1);
            }
            if (Parameters.OptimizationEvaluationCriteria2.CriteriaType != EvaluationCriteriaReturnBased.criteriaType.none)
            {
                evaluationCriteriaList.Add(Parameters.OptimizationEvaluationCriteria2);
            }
            if (Parameters.OptimizationEvaluationCriteria3.CriteriaType != EvaluationCriteriaCreditExposureBased.criteriaType.none)
            {
                evaluationCriteriaList.Add(Parameters.OptimizationEvaluationCriteria3);
            }
            if (Parameters.OptimizationEvaluationCriteria4.CriteriaType != EvaluationCriteriaParameterBased.criteriaType.none)
            {
                evaluationCriteriaList.Add(Parameters.OptimizationEvaluationCriteria4);
            }
            if (Parameters.OptimizationEvaluationCriteria5.CriteriaType != EvaluationCriteriaParameterBased.criteriaType.none)
            {
                evaluationCriteriaList.Add(Parameters.OptimizationEvaluationCriteria5);
            }
        }

        void InitOptimization()
        {
            JARE.problems.Finance.ExecutionType executionType;
            if (radioButtonSingleThread.Checked) executionType = JARE.problems.Finance.ExecutionType.singleThread;
            else executionType = JARE.problems.Finance.ExecutionType.viaBinder;
            int numberOfVariables = GetNumberOfVariables();

            //MO
            problem = (JARE.problems.PortfolioOptimizationMO)new JARE.problems.PortfolioOptimizationMO(evaluationCriteriaList, Parameters.DecisionVarType, 
                numberOfVariables, Parameters.Cardinality, Parameters.LowerBound, Parameters.UpperBound, executionType);

            bool evaluationCriteriaTimeSeriesBased = false;
            bool evaluationCriteriaParameterBased = false;

            foreach (EvaluationCriteria EC in evaluationCriteriaList)
            {
                //if (EC.GetType() == PortfolioOptimization.CREDIT_BASED_CRITERIA || EC.GetType() == PortfolioOptimization.RETURN_BASED_CRITERIA) evaluationCriteriaTimeSeriesBased = true;
                if (EC.GetType().BaseType == PortfolioOptimization.TIMESERIES_BASED_CRITERIA) evaluationCriteriaTimeSeriesBased = true;
                if (EC.GetType() == PortfolioOptimization.PARAMETER_BASED_CRITERIA) evaluationCriteriaParameterBased = true;
            }

            if (evaluationCriteriaTimeSeriesBased)
            {
                problem.m_parameters.setParameter("optimizationDate", Parameters.OptimizationDate);
                problem.m_parameters.setParameter("returnEvaluationPeriod", Parameters.ReturnPeriod);
                problem.m_parameters.setParameter("evaluationPeriod", Parameters.EvaluationPeriod);
                problem.m_parameters.setParameter("varThreshold", Parameters.VarTreshold * 0.01);
                problem.m_parameters.setParameter("targetRate", Parameters.InterestRate * 0.01);
                problem.m_parameters.setParameter("lambda", Parameters.Lambda);
                problem.m_parameters.setParameter("AssetTimeSeriesSet", AssetTimeSeriesSet);

                //Add additional time series for credit exposure calculation
                foreach (EvaluationCriteria EC in evaluationCriteriaList)
                {
                    if (EC.GetType() == PortfolioOptimization.CREDIT_BASED_CRITERIA)
                    {
                        problem.m_parameters.setParameter("CreditExposureTimeSeriesSet", CreditExposureTimeSeriesSet);
                        break;
                    }
                }

                int temp;
                if (((JARE.problems.PortfolioOptimizationMO)problem).IsEvaluationCriteria(EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR, out temp) ||
                   ((JARE.problems.PortfolioOptimizationMO)problem).IsEvaluationCriteria(EvaluationCriteriaReturnBased.criteriaType.CR_VaR, out temp))
                {
                    problem.m_parameters.setParameter("backtestingPeriod", Parameters.BacktestingPeriod);
                    if (stressTimeSeriesSet != null) problem.m_parameters.setParameter("stressTimeSeriesSet", stressTimeSeriesSet);
                    //else throw new Exception("Time Series for stress backtesting is not defined!");
                }
            }
            else if (evaluationCriteriaParameterBased)
            {
                problem.m_parameters.setParameter("BondSet", bondSet);
            }

            //
            for (int i = 0; i < initialSolutionsList.Count; i++)
            {
                ((JARE.problems.PortfolioOptimizationMO)problem).addOptimalSolution(i.ToString(), initialSolutionsList[i]);
            }
            //End of Adding optimal solution for each criteria

            string workingDir = Directory.GetCurrentDirectory();


            //////////////////////////////////////////////////////////////////////////////////////
            if (radioButtonRelativeOptimization.Checked)
            {
                if (referentSolution == null) throw new Exception("Referent solution is not definied!");
                Solution ReferentSolution = new Solution(problem);
                for (int j = 0; j < referentSolution.Length; j++) ReferentSolution.DecisionVariables[j].setValue((double)referentSolution[j]);

                ((JARE.Base.solutionType.RealWeightsSolutionType)(problem.SolutionType)).ReferentSolution = ReferentSolution;
                ((JARE.Base.solutionType.RealWeightsSolutionType)problem.SolutionType).AbsoluteDeviationFromReferentSolution = Parameters.RebalancingBuffer * 0.01;
            }

            string fullPath = ((FileStream)(sw.BaseStream)).Name;
            string PrintDirectory = Path.GetDirectoryName(fullPath);
            problem.m_parameters.setParameter("PrintDirectory", PrintDirectory);
            //////////////////////////////////////////////////////////////////////////////////////
            // algorithm
            if (radioButtonSingleThread.Checked)
            {
                problem.m_parameters.setParameter("RScriptPath", workingDir);
                problem.Init();
                //algorithm = new NSGAIIF(problem, Directory.GetCurrentDirectory());
                algorithm = new NSGAIIF(problem, PrintDirectory);
            }
            else
            {
                workingDir += "\\ViaBinder";
                problem.m_parameters.setParameter("RScriptPath", workingDir);
                problem.Init();
                string script = workingDir + "\\R.bat";
                string dir = workingDir;
                string varDirectory = Path.GetTempPath();
                try
                {
                    varDirectory = Path.GetTempPath() + "weights";
                    System.IO.Directory.CreateDirectory(varDirectory);
                }
                catch (Exception ex)
                {
                }
                //string varFile = varDirectory + "\\weights";
                string varFile = workingDir + "\\ClientR.properties";
                int procTimeout = 600000;
                string binderLogFilesDir = Directory.GetCurrentDirectory();
                algorithm = new NSGAIIFViaBinder(problem, script, dir, varFile, binderLogFilesDir, procTimeout);
            }
           
            /* Algorithm parameters*/
            algorithm.setInputParameter("populationSize", Parameters.PopulationSize);
            algorithm.setInputParameter("archiveSize", Parameters.PopulationSize);
            algorithm.setInputParameter("maxEvaluations", Parameters.Generations * Parameters.PopulationSize);
            algorithm.setInputParameter("PlateauTolerance", Parameters.PlateauTolerance);//VAZNO. Hardkodovano.
            algorithm.setInputParameter("MaxPlateauGenerations", Parameters.MaxPlateauGenerations);

            algorithm.setInputParameter("onlyOptimalSolutions", checkBoxPrintOnlyOptimalSolutions.Checked);
            algorithm.setInputParameter("PrintReportForGeneration", checkBoxPrintReportForGeneration.Checked);

            //FileStream fs = (FileStream)sw.BaseStream;
            //int lastIndexofBackslash = fs.Name.LastIndexOf("\\");
            //string generationsPrintingPath = fs.Name.Remove(lastIndexofBackslash);
            //problem.m_parameters.setParameter("generationsPrintingPath", generationsPrintingPath);


            // Mutation and Crossover for Real codification
            if (Parameters.Cardinality == numberOfVariables)
            {
                if (radioButtonRelativeOptimization.Checked)
                {
                    //crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WholeArithmeticCrossover");
                    //mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("ReplacementMutation");
                    crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WeightsUniformCrossover");
                    mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("WeightsUniformMutation");
                }
                else
                {
                    //crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WeightsSinglePointCrossover");
                    crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WeightsUniformCrossover");
                    mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("WeightsUniformMutation");
                    //crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WholeArithmeticCrossover");
                    //mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("ReplacementMutation");
                }
            }
            else if (Parameters.Cardinality != numberOfVariables)
            {
                crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("CCWeightsCrossover");
                mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("SwapMutation");//Swap mutation satisfies cardinality constraint
            }
            crossover.setParameter("probability", Parameters.CrossoverProbability);
            crossover.setParameter("distributionIndex", 20.0);

            mutation.setParameter("probability", Parameters.MutationProbability);
            mutation.setParameter("perturbationIndex", Parameters.MutationPerturbation);

            /* Selection Operator */
            selection = JARE.Base.operators.selection.SelectionFactory.getSelectionOperator("BinaryTournament");

            /* Add the operators to the algorithm*/
            algorithm.addOperator("crossover", crossover);
            algorithm.addOperator("mutation", mutation);
            algorithm.addOperator("selection", selection);

            //((SPEA2F)algorithm).IterationCounterChanged += new SPEA2F.IterationCounterChangedHandler(GenerationCounterChanged);
            ((NSGAIIF)algorithm).IterationCounterChanged += new NSGAIIF.IterationCounterChangedHandler(GenerationCounterChanged);
            ((NSGAIIF)algorithm).EfficientFrontierOnChartChanged += new NSGAIIF.EfficientFrontierOnChartChangedHandler(UpdateChart);
            OperationCanceled += new OperationCanceledHandler(((NSGAIIF)algorithm).OperationCanceled);
        }

        JARE.Base.SolutionSet SingleObjectiveOptimization(EvaluationCriteria EC)
        {
            //Kreiranje optimalnih resenja po funkciji cilja pojedinacno
            //////////////////////////////
            PortfolioOptimizationGA algorithm;
            Operator crossover;
            Operator mutation;
            Operator selection;
            JARE.problems.Finance.ExecutionType executionType;
            if (radioButtonSingleThread.Checked) executionType = JARE.problems.Finance.ExecutionType.singleThread;
            else executionType = JARE.problems.Finance.ExecutionType.viaBinder;
            int numberOfVariables = GetNumberOfVariables();

            // problem
            //TimeSeriesSet tss = timeSeriesSets.GetAt(0);
            problem = new JARE.problems.Finance.PortfolioOptimizationSO(EC, Parameters.DecisionVarType, numberOfVariables, Parameters.Cardinality, Parameters.LowerBound, Parameters.UpperBound, executionType);

            if(EC.GetType() == PortfolioOptimization.RETURN_BASED_CRITERIA || EC.GetType() == PortfolioOptimization.CREDIT_BASED_CRITERIA)
            {
                problem.m_parameters.setParameter("optimizationDate", Parameters.OptimizationDate);
                problem.m_parameters.setParameter("returnEvaluationPeriod", Parameters.ReturnPeriod);
                problem.m_parameters.setParameter("evaluationPeriod", Parameters.EvaluationPeriod);
                problem.m_parameters.setParameter("varThreshold", Parameters.VarTreshold*0.01);
                problem.m_parameters.setParameter("targetRate", Parameters.InterestRate);
                problem.m_parameters.setParameter("lambda", Parameters.Lambda);
                problem.m_parameters.setParameter("AssetTimeSeriesSet", AssetTimeSeriesSet);
                if (EC.GetType() == PortfolioOptimization.CREDIT_BASED_CRITERIA) problem.m_parameters.setParameter("CreditExposureTimeSeriesSet", CreditExposureTimeSeriesSet);
            
                if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR ||
                    ((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.CR_VaR)
                {
                    problem.m_parameters.setParameter("backtestingPeriod", Parameters.BacktestingPeriod);
                    if (stressTimeSeriesSet != null) problem.m_parameters.setParameter("stressTimeSeriesSet", stressTimeSeriesSet);
                    else throw new Exception("Time Series for stress backtesting is not defined!");
                }
            }
            string workingDir = Directory.GetCurrentDirectory();
            string fullPath = ((FileStream)(sw.BaseStream)).Name;
            string PrintDirectory = Path.GetDirectoryName(fullPath);
            problem.m_parameters.setParameter("PrintDirectory", PrintDirectory);
    
            // algorithm
            if(radioButtonSingleThread.Checked)
            {
                problem.m_parameters.setParameter("RScriptPath", workingDir);
                problem.Init();
                algorithm = new PortfolioOptimizationGA(problem);
            }
            else
            {
                workingDir += "\\ViaBinder";
                problem.m_parameters.setParameter("RScriptPath", workingDir);
                problem.Init();
                algorithm = new PortfolioOptimizationGAViaBinder(problem);
                string script = workingDir + "\\R.bat";
                algorithm.setInputParameter("binderScript", script);
                string dir = workingDir;
                algorithm.setInputParameter("binderDir", dir);

                string varDirectory = Path.GetTempPath();
                try
                {
                    varDirectory = Path.GetTempPath() + "weights";
                    System.IO.Directory.CreateDirectory(varDirectory);
                }
                catch (Exception ex)
                {
                }

                string varFile = varDirectory + "\\weights";
                algorithm.setInputParameter("binderVarFile", varFile);
                int procTimeout = 600000;
                algorithm.setInputParameter("binderProcTimeout", procTimeout);
                string binderLogFilesDir = Directory.GetCurrentDirectory();
                algorithm.setInputParameter("binderLogFilesDir", binderLogFilesDir);
            }
            algorithm.setInputParameter("populationSize", Parameters.PopulationSize);
            algorithm.setInputParameter("maxGenerations", Parameters.Generations);
            algorithm.setInputParameter("plateauTolerance", Parameters.PlateauTolerance);//VAZNO. Hardkodovano.
            algorithm.setInputParameter("maxPlateauGenerations", Parameters.MaxPlateauGenerations);

            algorithm.setInputParameter("onlyOptimalSolutions", checkBoxPrintOnlyOptimalSolutions.Checked);
            algorithm.setInputParameter("PrintReportForGeneration", checkBoxPrintReportForGeneration.Checked);


            // crosover
            //_crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WeightsSinglePointCrossover");
            crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WeightsUniformCrossover");
            crossover.setParameter("probability", Parameters.CrossoverProbability);
            crossover.setParameter("distributionIndex", 20.0);

            // mutation
            mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("WeightsUniformMutation");
            //_mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("SwapMutation");
            mutation.setParameter("probability", Parameters.MutationProbability);
            mutation.setParameter("perturbationIndex", Parameters.MutationPerturbation);

            // selection
            //_selection = JARE.Base.operators.selection.SelectionFactory.getSelectionOperator("BestSolutionSelection");
            //_selection.setParameter("comparator", new JARE.Base.operators.comparator.FitnessComparator());
            //Operator selectionPopulation = JARE.Base.operators.selection.SelectionFactory.getSelectionOperator("RouletteWheelSelection2");
            selection = JARE.Base.operators.selection.SelectionFactory.getSelectionOperator("BinaryTournament");
            selection.setParameter("size", Parameters.PopulationSize - 1);

            // set algorithm operators
            algorithm.addOperator("crossover", crossover);
            algorithm.addOperator("mutation", mutation);
            algorithm.addOperator("selection", selection);
            //_algorithm.addOperator("selectionPopulation", selection);

            // init algorithm
            //((PortfolioOptimizationGA)_algorithm).Init();
            algorithm.IterationCounterChanged += new PortfolioOptimizationGA.IterationCounterChangedHandler(GenerationCounterChanged);
            OperationCanceled += new OperationCanceledHandler(algorithm.OperationCanceled);

            // execute algorithm
            JARE.Base.SolutionSet solutionSet = algorithm.execute();
            JARE.Base.Solution optimalSolution = solutionSet.getSolution(solutionSet.size() - 1);
            //return optimalSolution;
            return solutionSet;
        }

        private int GetNumberOfVariables()
        {
            int numberOfVariables = 0;
            
            if (AssetTimeSeriesSet != null) numberOfVariables = AssetTimeSeriesSet.Count;
            else if(CreditExposureTimeSeriesSet != null) numberOfVariables = CreditExposureTimeSeriesSet.Count;
            else if(RebalanceAssetTimeSeriesSet != null) numberOfVariables = RebalanceAssetTimeSeriesSet.Count;
            else if(bondSet != null) numberOfVariables = bondSet.Count;

            if (numberOfVariables == 0) throw new Exception("Number of variables must be greater than 0!");

            return numberOfVariables;
        }

        private void buttonLoadPortfolio_Click(object sender, EventArgs e)
        {
            importInputSolutions(initialSolutionsList);
        }

        private void buttonLClearOptimalPortfolio_Click(object sender, EventArgs e)
        {
            buttonClear_Click(sender, e);
        }

        private void loadParametersButton_Click(object sender, EventArgs e)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK) loadParameters(openFileDialog.FileName);
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
                UpdateControls();
            }
        }

        private void saveParametersButton_Click(object sender, EventArgs e)
        {
            UpdateParameters();
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            saveFileDialog.Title = "Save data file";
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
                {
                    ParametersService service = new ParametersService();
                    service.SaveParametersToFile(Parameters, saveFileDialog.FileName);
                }
                else return;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void loadDataButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ImportTimeSeries("CreditExposureTimeSeriesSet")) return;
                Parameters.OptimizationEvaluationCriteria3.TimeSeriesSetID = CreditExposureTimeSeriesSet.ID;
            }
            catch (Exception)
            {
                return;
            }
        }

        private void buttonLoadRebalanceData_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ImportTimeSeries("RebalanceAssetTimeSeriesSet")) return;
                Parameters.RebalanceEvaluationCriteria.TimeSeriesSetID = AssetTimeSeriesSet.ID;
            }
            catch (Exception)
            {
                return;
            }
            Parameters.PopulationSize = AssetTimeSeriesSet.Count * 5;//preporuka
            Parameters.Cardinality = AssetTimeSeriesSet.Count;
            populationSizeBox.Text = Parameters.PopulationSize.ToString();
            cardinalityBox.Text = Parameters.Cardinality.ToString();
            startButton.Enabled = true;
        }

        private void buttonLoadStressTimeSeries_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<Asset> Assets = new System.Collections.Generic.List<Asset>();
            if (ImportTimeSeries(Assets)) stressTimeSeriesSet = new AssetSet(1, Assets);
        }

        private void buttonLoadReferentSolution_Click(object sender, EventArgs e)
        {
            importInputSolutions(out referentSolution);
        }

        private void radioButtonGlobalOptimization_Click(object sender, EventArgs e)
        {
            //buttonLoadReferentSolution.Enabled = radioButtonRelativeOptimization.Checked;
            EnableControls();
        }

        private void radioButtonRelativeOptimization_Click(object sender, EventArgs e)
        {
            //buttonLoadReferentSolution.Enabled = radioButtonRelativeOptimization.Checked;
            EnableControls();
        }

        string resultsFileName;
        private void buttonResultsFile_Click(object sender, EventArgs e)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                resultsFileName = openFileDialog.FileName;
            }
        }

        private void buttonCalculateHypervolume_Click(object sender, EventArgs e)
        {
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            saveFileDialog.Title = "Save data file";

            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
                {
                    sw = new StreamWriter(saveFileDialog.FileName, false);
                }
                else return;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StreamReader reader = null;
            bool Break = false;
            string[] split = resultsFileName.Split('-');
            string[] split2 = split[1].Split('.');

            int i = int.Parse(split2[0]);
            int XColumnIndex = int.Parse(textBoxXColumn.Text) - 1;//11;
            int YColumnIndex = int.Parse(textBoxYColumn.Text) - 1; //0;
            System.Collections.Generic.List<double[]> XYValues = new System.Collections.Generic.List<double[]>();

            while (!Break)
            {
                string resultsFile = split[0] + "-" + i.ToString() + ".csv";
                try
                {
                    // open selected file
                    reader = File.OpenText(resultsFile);
                    string str = null;
                    XYValues.Clear();

                    if ((str = reader.ReadLine()) != null)
                    {
                    }
                    // read the data
                    while ((str = reader.ReadLine()) != null)
                    {
                        string[] strs = str.Split(';');
                        if (strs.Length == 1) strs = str.Split(',');
                        double[] xyvalue = new double[2] {double.Parse(strs[XColumnIndex]), double.Parse(strs[YColumnIndex])};
                        XYValues.Add(xyvalue);

                    }
                    //////////////////////////////////
                    XYValues.Sort(Compare);
                    double[] ReferencePoint = new double[2] { (double)XYValues[XYValues.Count - 1].GetValue(0), (double)XYValues[0].GetValue(1) };
                    //double[] ReferencePoint = new double[2] { (double)XYValues[XYValues.Count - 1].GetValue(0), 0.0 };
                    double Hypervolume = 0.0;
                    for (int j = 0; j < XYValues.Count-1; j++)
                    {
                        double Y1 = (double) XYValues[j].GetValue(1);
                        double Y2 = (double) XYValues[j + 1].GetValue(1);
                        double X1 = (double) XYValues[j].GetValue(0);
                        double X2 = (double) XYValues[j + 1].GetValue(0);
                        //Area below pareto optimal set
                        //Hypervolume += (X2 - X1) * (Y1 + 0.5 * (Y2 - Y1) - ReferencePoint[1]);
                        //By definition
                        Hypervolume += (X2 - X1) * (Y1 - ReferencePoint[1]);
                    }
                    string line = i.ToString() + "," + Hypervolume.ToString();
                    sw.WriteLine(line);
                    ///////////////////////////////////

                }
                catch (Exception)
                {
                    MessageBox.Show("Finished", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (sw != null)
                        sw.Close();
                    return;
                }
                finally
                {
                    i++;
                    // close file
                    if (reader != null)
                        reader.Close();
                }
            }
            return;
        }

        private int Compare(double[] x, double[] y)
        {
            if (x[1] == y[1]) return 0;
            else if (x[1] > y[1]) return 1;
            else return -1;
        }

        private void buttonDefaultSettings_Click(object sender, EventArgs e)
        {
            string workingDir = Directory.GetCurrentDirectory();
            string assetsFileName = workingDir + "\\S&P 100 Components.csv";

            System.Collections.Generic.List<Asset> Assets = new System.Collections.Generic.List<Asset>();
            int timeSeriesID = 1;
            if (ImportTimeSeriesBasic(Assets, assetsFileName))
            {
                AssetTimeSeriesSet = new AssetSet(timeSeriesID, Assets);
                Parameters.PopulationSize = AssetTimeSeriesSet.Count * 5;//preporuka
                Parameters.Cardinality = AssetTimeSeriesSet.Count;
                populationSizeBox.Text = Parameters.PopulationSize.ToString();
                cardinalityBox.Text = Parameters.Cardinality.ToString();
                startButton.Enabled = true;
            }
            else return;

            Parameters.OptimizationEvaluationCriteria1.TimeSeriesSetID = timeSeriesID;
            Parameters.OptimizationEvaluationCriteria2.TimeSeriesSetID = timeSeriesID;

            //Import Stress time series
            //System.Collections.Generic.List<TimeSeries> stressAssets = new System.Collections.Generic.List<TimeSeries>();
            //string stressAssetsFileName = workingDir + "\\Min Vol Historical Stress Time Series.csv";
            //if (ImportTimeSeriesBasic(stressAssets, stressAssetsFileName)) stressTimeSeriesSet = new TimeSeriesSet(1, stressAssets, false, "");

            string parametersFileName = workingDir + "\\default params.csv";
            loadParameters(parametersFileName);
            radioButtonSingleThread.Checked = false;
            radioButtonDistributed.Checked = true;
        }

        private void buttonLoadBonds_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.List<Bond> BondList = new System.Collections.Generic.List<Bond>();

                // show file selection dialog
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if(!ImportBonds(BondList, openFileDialog.FileName)) return;
                    bondSet = new BondSet(1, BondList);
                }

            }
            catch (Exception)
            {
                return;
            }
            
            Parameters.PopulationSize = bondSet.Count * 5;//preporuka
            Parameters.Cardinality = bondSet.Count;
            populationSizeBox.Text = Parameters.PopulationSize.ToString();
            cardinalityBox.Text = Parameters.Cardinality.ToString();
            startButton.Enabled = true;
        }

        private bool ImportBonds(System.Collections.Generic.List<Bond> BondList, string fileName)
        {

            StreamReader reader = null;
            int parameterCount = 5;

            try
            {
                Dictionary<int, string> ParameterIndex = new Dictionary<int, string>();
                
                // open selected file
                reader = File.OpenText(fileName);
                string str = null;

                if ((str = reader.ReadLine()) != null)
                {
                    string[] strs = str.Split(';');
                    if (strs.Length == 1) strs = str.Split(',');
                    parameterCount = strs.Length;
                    for (int i = 0; i < parameterCount; i++)
                    {
                        ParameterIndex.Add(i, strs[i]);
                    }

                }
                // read the data
                while ((str = reader.ReadLine()) != null)
                {
                    string[] strs = str.Split(';');
                    if (strs.Length == 1) strs = str.Split(',');

                    Bond bond = new Bond();

                    for (int i = 0; i < parameterCount; i++)
                    {
                        if (ParameterIndex.ElementAt(i).Value.ToLower().Equals("isin")) bond.name = strs[i];
                        if (ParameterIndex.ElementAt(i).Value.ToLower().Equals("ticker")) bond.ticker = strs[i];
                        if (ParameterIndex.ElementAt(i).Value.ToLower().Equals("price")) bond.price = double.Parse(strs[i]);
                        if (ParameterIndex.ElementAt(i).Value.ToLower().Equals("stressedprice")) bond.stressedPrice = double.Parse(strs[i]);
                        if (ParameterIndex.ElementAt(i).Value.ToLower().Equals("rating")) bond.rating = strs[i];
                        if (ParameterIndex.ElementAt(i).Value.ToLower().Equals("duration")) bond.duration = double.Parse(strs[i]);
                        if (ParameterIndex.ElementAt(i).Value.ToLower().Equals("yieldtomaturity")) bond.yieldToMaturity = double.Parse(strs[i]); 
                    }
                    
                    BondList.Add(bond);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // close file
                if (reader != null)
                    reader.Close();
            }
            return true;
        }
    }
}
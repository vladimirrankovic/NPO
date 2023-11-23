using System;
using System.Collections.Generic;
using System.Text;
using JARE.Base;
using JARE.Base.operators.comparator;
using Algorithm = JARE.Base.Algorithm;
using RouletteWheelSelection = JARE.Base.operators.selection.RouletteWheelSelection;
using XReal = JARE.util.wrapper.XReal;
using JARE.util;
using JARE.problems.Finance;

namespace JARE.metaheuristics.singleObjective.geneticAlgorithm
{
    // Genetski algoritam u kojem broj generacija oznacava broj generacija u kojima nije pronadjena bolja jedinka

    public class PortfolioOptimizationGA : Algorithm
    {
        public enum TerminationCondition
        {
            MaxGenerations = 0,
            GenerationsWithoutImprovement = 1
        }

        bool Canceled = false;

        // random number generator
        protected int populationSize;
        protected int maxGenerations;
        protected double plateauTolerance;
        protected int maxPlateauGenerations;
        protected int newIndividualsCount;
        protected double crossoverRate;
        protected double mutationRate;

        protected SolutionSet population;
        protected SolutionSet resultPopulation;
        protected SolutionSet offspringPopulation;

        protected Operator mutationOperator;
        protected Operator crossoverOperator;
        protected Operator selectionOperator;
        protected Operator selectionBestSolution;

        protected Problem m_problem;
        public TerminationCondition terminationCondition;
        Solution bestSolution;

        //public delegate void IterationCounterChangedHandler(int iterationCount);
        public delegate void IterationCounterChangedHandler(string iterationCount);
        public event IterationCounterChangedHandler IterationCounterChanged;

        //public delegate void IterationCounterChangedHandler(int iterationCount);
        public delegate void TimeSeriesOnChartChangedHandler(Solution bestSolution);
        public event TimeSeriesOnChartChangedHandler TimeSeriesOnChartChanged;

        /// <summary> 
        /// Constructor
        /// Create a new GGA instance.
        /// </summary>
        /// <param name="problem">Problem to solve.
        /// </param>

        public PortfolioOptimizationGA(Problem problem)
		{
			this.m_problem = problem;
		} // GGA
		
		/// <summary> Execute the GGA algorithm</summary>
		/// <throws>  SMException  </throws>

        public virtual void Init()
        {
            // Read the params
            populationSize = ((System.Int32)this.getInputParameter("populationSize"));
            maxGenerations = ((System.Int32)this.getInputParameter("maxGenerations"));
            plateauTolerance = ((System.Double)this.getInputParameter("plateauTolerance"));
            maxPlateauGenerations = ((System.Int32)this.getInputParameter("maxPlateauGenerations"));
            if (this.m_inputParameters.ContainsKey("newIndividualsCount")) newIndividualsCount = ((int)this.getInputParameter("newIndividualsCount"));
            else newIndividualsCount = 0;

            // Initialize the variables
            population = new SolutionSet(populationSize*5);
            resultPopulation = new SolutionSet(populationSize*5);

            // Read the operators
            mutationOperator = this.m_operators["mutation"];
            crossoverOperator = this.m_operators["crossover"];
            selectionOperator = this.m_operators["selection"];
            selectionBestSolution = JARE.Base.operators.selection.SelectionFactory.getSelectionOperator("BestSolutionSelection");
            selectionBestSolution.setParameter("comparator", new JARE.Base.operators.comparator.FitnessComparator());
            crossoverRate = (double)crossoverOperator.getParameter("probability");

            // Create the initial population
            population = m_problem.createInitialPopulation(populationSize);
            Evaluate(population);

            int bestSolutionIndex = (int)selectionBestSolution.execute(population);
            bestSolution = new Solution((Solution)population.getSolution(bestSolutionIndex));
        }

        void Main(int iterationCount)
        {
            throw new NotImplementedException();
        }

        public virtual void Crossover()
        {
            int bestSolutionCount = 1;
            int reproductionCount = populationSize - bestSolutionCount - newIndividualsCount;
            while (resultPopulation.size() <= (reproductionCount - 2))
            {
                // Crossover
                Solution[] offspring = CrossoverBasic();

                // add two new offsprings to the population
                resultPopulation.add(offspring[0]);
                resultPopulation.add(offspring[1]);      
            }
            if (resultPopulation.size() < reproductionCount)
            {
                Solution[] offspring = CrossoverBasic();
                resultPopulation.add(offspring[0]);
            }
        }

        public virtual Solution[] CrossoverBasic()
        {
            Solution[] parents = new Solution[2];

            parents[0] = (Solution)selectionOperator.execute(population);
            parents[1] = (Solution)selectionOperator.execute(population);

            // Crossover
            Solution[] offspring = (Solution[])crossoverOperator.execute(parents);
            return offspring;
        }

        public virtual void Mutate()
        {
            double probability = (double)mutationOperator.getParameter("probability");
            if (probability > 0.0)
            {
                // mutate
                for (int i = 0; i < populationSize; i++)
                {
                    Solution s = population.getSolution(i);
                    s = (Solution)mutationOperator.execute(s);
                }
            }
        }

        private void InsertNewIndividuals()
        {
            if (newIndividualsCount > 0)
            {
                for (int i = 0; i < newIndividualsCount; i++)
                {
                    Solution newSolution = new Solution(m_problem);

                    // add new individual to the population
                    resultPopulation.add(newSolution);
                }
            }
        }

        public virtual void Evaluate(SolutionSet population)
        {
            for (int i = 0; i < population.size(); i++)
            {
                if (Canceled) break;
                m_problem.evaluate(population.getSolution(i));
            } //for   
        }

        public override SolutionSet execute()
        {
            Init();
            double previousFitnessValue = 1.0e+10;
            double currentFitnessValue = 0.0;
            double tolerance = System.Math.Pow(10, -5);

            int j = 0;
            int totalIterations = 0;

            while (totalIterations < maxGenerations)
            {
                if (Canceled) break;

                resultPopulation.clear();

                Crossover();
                Mutate();
                InsertNewIndividuals();

                //Evaluate result population
                Evaluate(resultPopulation);
                
                population.clear();
                for (int i = 0; i < resultPopulation.size(); i++)
                {
                    population.add(resultPopulation.getSolution(i));
                }
                population.add(bestSolution);

                int bestSolutionIndex = (int)selectionBestSolution.execute(population);
                bestSolution = (Solution)population.getSolution(bestSolutionIndex);

                currentFitnessValue = bestSolution.Fitness;
                if (Math.Abs(currentFitnessValue/previousFitnessValue-1.0) > plateauTolerance)
                {
                    j = 0;
                    previousFitnessValue = currentFitnessValue;
                    OnIterationCounterChanged(bestSolution);
                }
                else j++;

                // raise event
                //OnIterationCounterChanged(j);
                totalIterations++;
                OnIterationCounterChanged(j.ToString()+"-"+totalIterations.ToString());
                
                if (j == maxPlateauGenerations) break;
            }

            return population;
        }

        public void OnIterationCounterChanged(string iterationCount)
        {
            if(IterationCounterChanged != null) IterationCounterChanged(iterationCount);
        }
        public void OnIterationCounterChanged(Solution bestSolution)
        {
            TimeSeriesOnChartChanged(bestSolution);
        }
        public void OperationCanceled(bool Canceled)
        {
            this.Canceled = Canceled;
        }
        //public IterationCounterChangedHandler MainForm { get; set; }
    }
}

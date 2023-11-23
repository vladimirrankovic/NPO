/// <summary> NSGAII.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0  
/// </version>
using System;
using JARE.Base;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using Hypervolume = JARE.qualityIndicator.Hypervolume;
using JARE.util;
using System.IO;

namespace JARE.metaheuristics.nsgaII
{
	
	/// <summary> This class implements the NSGA-II algorithm. </summary>
	[Serializable]
	public class NSGAIIF:Algorithm
	{	
		/// <summary> stores the problem  to solve</summary>
		protected Problem m_problem;

        protected System.IO.StreamWriter sw;
        protected System.IO.StreamWriter swHyper;

        /// <summary>
        /// evalutions counter
        /// </summary>
  		public int evaluations = 0;
        /// <summary>
        /// maximal number of evaluations
        /// </summary>
        public int maxEvaluations;
        /// <summary>
        /// generation counter
        /// </summary>
        protected int generations;
        /// <summary>
        /// number of evalutions needed to achieve desired performance
        /// </summary>
        public int requiredEvaluations = 0;
        /// <summary>
        /// maximal number of generations without improvement in chosen quality metric
        /// </summary>
        public int maxGenerationsWithoutImprovement;
        /// <summary>
        /// number of generations without improvement in chosen quality metric
        /// </summary>
        public int generationsWithoutImprovement = 0;
  
        /// <summary>
        /// flag if calculation is canceled
        /// </summary>
        public bool Canceled = false;

        protected DateTime startTime;
        protected DateTime begginingOfGenerationEvaluation;
        protected double t;
        //Slavisa
        protected String infoFromConsole = "";

        /// <summary>
        /// Wether to 
        /// </summary>
        bool onlyOptimalSolutions = true;
        /// <summary>
        /// 
        /// </summary>
        bool PrintReportForGeneration = false;


        /// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
        public NSGAIIF(Problem problem, string LogFilesDir)
		{
			this.m_problem = problem;
            sw = new System.IO.StreamWriter(String.Format("{0}\\times_{1}_{2}_{3}_{4}_{5}.txt", LogFilesDir, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute));
            swHyper = new System.IO.StreamWriter(String.Format("{0}\\Hyper_{1}_{2}_{3}_{4}_{5}.txt", LogFilesDir, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute));
        } // NSGAII
		
		/// <summary> Runs the NSGA-II algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize;
            double previousQualityValue = 0;

            try
            {
                onlyOptimalSolutions = (bool)getInputParameter("onlyOptimalSolutions");
            }
            catch (Exception ex) { }
            try
            {
                PrintReportForGeneration = (bool)getInputParameter("PrintReportForGeneration");
            }
            catch (Exception ex) { }
            
			
			QualityIndicator indicators; // QualityIndicator object
			
            SolutionSet population = new SolutionSet(); //Slavisa dodao
			SolutionSet offspringPopulation;
			SolutionSet union;
			
			Operator mutationOperator;
			Operator crossoverOperator;
			Operator selectionOperator;
			
			Distance distance = new Distance();
			
			//Read the parameters
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			indicators = (QualityIndicator) getInputParameter("indicators");
			
			
			//Read the operators
			mutationOperator = m_operators["mutation"];
			crossoverOperator = m_operators["crossover"];
			selectionOperator = m_operators["selection"];
			
            //Init execution parameters
            initExecution();

            // Create the initial solutionSet
            Console.WriteLine("Creating initial population...");
            population = createInitialPopulation(populationSize);
            Console.WriteLine("Initial population created.");
          
            evaluatePopulation(population);

			while (evaluations < maxEvaluations)
			{
                if (Canceled) break;
                
                generations++;

				// Create the offSpring solutionSet
				offspringPopulation = new SolutionSet(populationSize);
				Solution[] parents = new Solution[2];
				for (int i = 0; i < (populationSize / 2); i++)
				{
					parents[0] = (Solution) selectionOperator.execute(population);
					parents[1] = (Solution) selectionOperator.execute(population);
					Solution[] offSpring = (Solution[]) crossoverOperator.execute(parents);
					mutationOperator.execute(offSpring[0]);
					mutationOperator.execute(offSpring[1]);
					offspringPopulation.add(offSpring[0]);
					offspringPopulation.add(offSpring[1]);
				} // for
                
                evaluatePopulation(offspringPopulation);
                if (Canceled) break;
                
                // Create the solutionSet union of solutionSet and offSpring
				union = ((SolutionSet) population).union(offspringPopulation);
				
				// Ranking the union
                Console.WriteLine("Ranking " + generations.ToString() + ". generation...");
				Ranking ranking = new Ranking(union);
				
				int remain = populationSize;
				int index = 0;
				SolutionSet front = null;
				population.clear();
				
				// Obtain the next front
				front = ranking.getSubfront(index);
				
				while ((remain > 0) && (remain >= front.size()))
				{
					//Assign crowding distance to individuals
					distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
					//Add the individuals of this front
					for (int k = 0; k < front.size(); k++)
					{
						population.add(front.getSolution(k));
					} // for
					
					//Decrement remain
					remain = remain - front.size();
					
					//Obtain the next front
					index++;
					if (remain > 0)
					{
						front = ranking.getSubfront(index);
					} // if        
				} // while
				
				// Remain is less than front(index).size, insert only the best one
				if (remain > 0)
				{
					// front contains individuals to insert                        
					distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
                    front.sort(new JARE.Base.operators.comparator.CrowdingComparator());
					for (int k = 0; k < remain; k++)
					{
						population.add(front.getSolution(k));
					} // for
					
					remain = 0;
				} // if			
                
                //Calculate required evaluations
                determineRequiredEvaluations(population, indicators);

                Ranking pRanking = new Ranking(population);

                SolutionSet s1 = pRanking.getSubfront(0);
                
                if(PrintReportForGeneration) printReportForGeneration(population, s1, generations);
                
                OnEfficientFrontierChanged(s1);

                if(isQualityTerminationConditionSatisfied(s1, ref previousQualityValue)) break;

			} // while            

			// Return as output parameter the required evaluations
			setOutputParameter("evaluations", requiredEvaluations);			

			// Return the first non-dominated front
			Ranking ranking2 = new Ranking(population);

            //Finish execution
            closeExecution();

            if(onlyOptimalSolutions) return ranking2.getSubfront(0);
            else return population;
        } // execute


        protected void evaluatePopulation(SolutionSet population)
        {
            //Slavisa
            string info = "";
            infoFromConsole = "Evaluating " + generations.ToString() + ". generation... \n";
            m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
            m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
            Console.WriteLine(infoFromConsole);
            infoFromConsole = "";

            begginingOfGenerationEvaluation = DateTime.Now;

            //
            evaluatePopulationBasic(population);

            t = (DateTime.Now - begginingOfGenerationEvaluation).TotalSeconds;
            info = String.Format(generations.ToString() + ". generation evaluated in {0} s. Total time: {1} s",
            t, (DateTime.Now - startTime).TotalSeconds);
            if(sw.BaseStream != null) sw.WriteLine(t);
            infoFromConsole = info;
            m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
            m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
            Console.WriteLine(info);

            infoFromConsole = String.Format("Begining time: {0}. Estimated end time: {1}", startTime, DateTime.Now.AddSeconds(t * (maxEvaluations / population.size() - generations)));
            m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
            m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
            Console.WriteLine("Begining time: {0}. Estimated end time: {1}", startTime, DateTime.Now.AddSeconds(t * (maxEvaluations / population.size() - generations)));

            OnIterationCounterChanged(generations.ToString());
        }
        public virtual void evaluatePopulationBasic(SolutionSet population)
        {
            for (int i = 0; i < population.size(); i++)
            {
                if (Canceled) break;
                Solution solution = population.getSolution(i);
                m_problem.evaluate(solution);
                m_problem.evaluateConstraints(solution);
                evaluations++;
            } //for
        }

        public virtual SolutionSet createInitialPopulation(int populationSize)
        {
            return m_problem.createInitialPopulation(populationSize);
        }

        protected virtual void initExecution()
        {
            //Ser the start of execution
            startTime = DateTime.Now;

            generations = 1;
            evaluations = 0;
            requiredEvaluations = 0;
        }

        protected virtual void closeExecution()
        {
            sw.Close();
            if (swHyper != null) swHyper.Close();
        }

        protected void determineRequiredEvaluations(SolutionSet population, QualityIndicator indicators)
        {
            // This piece of code shows how to use the indicator object into the code
            // of NSGA-II. In particular, it finds the number of evaluations required
            // by the algorithm to obtain a Pareto front with a hypervolume higher
            // than the hypervolume of the true Pareto front.
            if ((indicators != null) && (requiredEvaluations == 0))
            {
                double HV = indicators.getHypervolume(population);
                if (HV >= (0.98 * indicators.TrueParetoFrontHypervolume))
                {
                    requiredEvaluations = evaluations;
                } // if
            } // if
        }

        protected void printReportForGeneration(SolutionSet population, SolutionSet bestFront, int noGeneration)
        {
            //Slavisa
            infoFromConsole = "Writing report for " + generations.ToString() + ". generation... \n";
            m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
            m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
            Console.WriteLine("Writing report for " + generations.ToString() + ". generation...");
            m_problem.PrintGenerationReport(population, bestFront, generations);
            m_problem.CreateAlgorithmInfo(new AlgorithmInfo(bestFront, "", m_problem.m_numberOfVariables));
            //sw.Flush();
        }

        protected bool isQualityTerminationConditionSatisfied(SolutionSet population, ref double previousQualityValue)
        {
            int start = 2;
            if (generations < start) return false;
            double HypervolumeTolerance = (double)getInputParameter("PlateauTolerance");
            int maxGenerationsWithoutImprovement = (int)getInputParameter("MaxPlateauGenerations");
            if (HypervolumeTolerance > 0.0)
            {
                //double currentHypervolume = Hypervolume.getHypervolume(population);
                double currentHypervolume = new Hypervolume().hypervolume(population.writeObjectivesToMatrix(), m_problem.m_numberOfObjectives);
                if (generations == start)
                {
                    previousQualityValue = currentHypervolume;
                }
                else
                {
                    if (swHyper != null) swHyper.WriteLine(currentHypervolume.ToString());

                    double HypervolumeIncreasement = (currentHypervolume - previousQualityValue) / previousQualityValue;
                    Console.WriteLine("HypervolumeIncreasement: " + HypervolumeIncreasement.ToString());
                    if (HypervolumeIncreasement < HypervolumeTolerance)
                    {
                        if (++generationsWithoutImprovement == maxGenerationsWithoutImprovement) return true;
                    }
                    else generationsWithoutImprovement = 0;
                    Console.WriteLine("generationsWithoutImprovement: " + generationsWithoutImprovement.ToString());
                    previousQualityValue = currentHypervolume;
                }
            }
            
            return false;
        }
        //
        public delegate void IterationCounterChangedHandler(string iterationCount);
        public event IterationCounterChangedHandler IterationCounterChanged;

        public void OnIterationCounterChanged(string iterationCount)
        {
            if (IterationCounterChanged != null) IterationCounterChanged(iterationCount);
        }
        public virtual void OperationCanceled(bool Canceled)
        {
            this.Canceled = Canceled;
            closeExecution();
        }

        public delegate void EfficientFrontierOnChartChangedHandler(SolutionSet efficientFrontier);
        public event EfficientFrontierOnChartChangedHandler EfficientFrontierOnChartChanged;

        public void OnEfficientFrontierChanged(SolutionSet efficientFrontier)
        {
            EfficientFrontierOnChartChanged(efficientFrontier);
        }

	} // NSGA-II
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.Base;
using JARE.Base.operators.comparator;
using Algorithm = JARE.Base.Algorithm;
using JARE.util;

namespace JARE.metaheuristics.singleObjective.geneticAlgorithm
{
    public class GAviaBinder : Algorithm
    {
        private Problem m_problem;
        DateTime beggining = DateTime.Now;
        DateTime begginingOfGenerationEvaluation;
        double t;
        EvaluationPool.EvaluationPool epw;


        //VISNJA: UPISIVANJE NAJBOLJIH IZ SVAKE POPULACIJE U FAJL
        System.IO.StreamWriter sw;
		
		/// <summary> 
		/// Constructor
		/// Create a new GGA instance.
		/// </summary>
		/// <param name="problem">Problem to solve.
		/// </param>
        public GAviaBinder(Problem problem, string script, string dir, string varFile, string LogFilesDir,int procTimeout)
		{
			this.m_problem = problem;
            EvaluationPool.IWBEvaluation p;
            p = problem as EvaluationPool.IWBEvaluation;

            if (p != null)
            {
                epw = new JARE.EvaluationPool.EvaluationPoolForBinderTPL(p, script, dir, varFile, procTimeout);
            }
            else
            {
                throw new Exception("This problem can not be solved via Binder");
            }

            sw = new System.IO.StreamWriter(String.Format("{0}\\times_{1}_{2}_{3}_{4}_{5}.txt", LogFilesDir, DateTime.Now.Year,
            DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute));
		} // GGA

        public GAviaBinder(Problem problem, string script, string dir, string varFile, string LogFilesDir, int numOfClients, int procTimeout)
        {
            this.m_problem = problem;
            EvaluationPool.IWBEvaluation p;
            p = problem as EvaluationPool.IWBEvaluation;

            if (p != null)
            {
                epw = new JARE.EvaluationPool.EvaluationPoolForBinder(p, script, dir, varFile, numOfClients, procTimeout);
            }
            else
            {
                throw new Exception("This problem can not be solved via Binder");
            }

            sw = new System.IO.StreamWriter(String.Format("{0}\\times_{1}_{2}_{3}_{4}_{5}.txt", LogFilesDir, DateTime.Now.Year,
            DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute));
        } // GGA
		
		/// <summary> Execute the GGA algorithm</summary>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize;
			int maxEvaluations;
			int evaluations = 0;
            int evaluationsInOnePopulation = 0;

			
			SolutionSet population;
			SolutionSet offspringPopulation;
			
			Operator mutationOperator;
			Operator crossoverOperator;
			Operator selectionOperator;

            System.Collections.Generic.IComparer<JARE.Base.Solution> comparator;
			comparator = new ObjectiveComparator(0); // Single objective comparator
			
			// Read the params
			populationSize = ((System.Int32) this.getInputParameter("populationSize"));
			maxEvaluations = ((System.Int32) this.getInputParameter("maxEvaluations"));
			
			// Initialize the variables
			population = new SolutionSet(populationSize);
			offspringPopulation = new SolutionSet(populationSize);
			
			evaluations = 0;
			
			// Read the operators
			mutationOperator = this.m_operators["mutation"];
			crossoverOperator = this.m_operators["crossover"];
            selectionOperator = this.m_operators["selection"];

            double totalTime = 0;
            double currentTime;
            DateTime totalStartTime = DateTime.Now;
            DateTime t0 = totalStartTime;
            DateTime NOW;


            // Create the initial population
			Solution newIndividual;
         
            // prvo sve doda u populaciju, pa onda evaluira
			for (int i = 0; i < populationSize; i++)
			{
                newIndividual = new Solution(m_problem);
				population.add(newIndividual);
			} //for 
            Console.WriteLine("Population size: " + population.size());
            epw.Start();
            
            sw.WriteLine("Population size: " + population.size());
            Console.WriteLine("Evaluating initial population...");
            begginingOfGenerationEvaluation = DateTime.Now;

            for (int i = 0; i < populationSize; i++)
            {
                epw.SendSolution(population.m_solutionsList[i]);
                evaluations++;
                evaluationsInOnePopulation++;
            } //for 

            epw.Wait();
            t = (DateTime.Now - begginingOfGenerationEvaluation).TotalSeconds;
            sw.WriteLine(t);
            Console.WriteLine(t);

			// Sort population
			population.sort(comparator);

            NOW = DateTime.Now;
            currentTime = ((TimeSpan)(NOW - t0)).TotalSeconds;
            totalTime += currentTime;

            int generations = 0;

            DateTime GAbeginning;
            double GAtotal;

			while (evaluations < maxEvaluations)
			{
                GAtotal = 0;
                GAbeginning = DateTime.Now;
                generations++;

                // Create the offSpring solutionSet  
                Console.WriteLine("Creating offspring of " + generations.ToString() + ". generation...");

				// Copy the best two individuals to the offspring population
				offspringPopulation.add(new Solution(population.getSolution(0)));
				offspringPopulation.add(new Solution(population.getSolution(1)));
				
				// Reproductive cycle
				for (int i = 0; i < (populationSize / 2 - 1); i++)
				{
					// Selection
					Solution[] parents = new Solution[2];
					
					parents[0] = (Solution) selectionOperator.execute(population);
					parents[1] = (Solution) selectionOperator.execute(population);
					
					// Crossover
					Solution[] offspring = (Solution[]) crossoverOperator.execute(parents);
					
					// Mutation
					mutationOperator.execute(offspring[0]);
					mutationOperator.execute(offspring[1]);
					// Replacement: the two new individuals are inserted in the offspring
					// population
					offspringPopulation.add(offspring[0]);
					offspringPopulation.add(offspring[1]);
				} // for

                GAtotal = (DateTime.Now - GAbeginning).TotalSeconds;
                Console.WriteLine("Evaluating offspring of " + generations.ToString() + ". generation...");

                begginingOfGenerationEvaluation = DateTime.Now;
                
                for (int i = 0; i < offspringPopulation.size(); i++)
                {
                    if (evaluations < maxEvaluations)
                    {
                        epw.SendSolution(offspringPopulation.m_solutionsList[i]);
                        evaluations++;
                        evaluationsInOnePopulation++;
                    } // if                            
                } // for

                epw.Wait();

                GAbeginning = DateTime.Now;

                t = (DateTime.Now - begginingOfGenerationEvaluation).TotalSeconds;
                Console.WriteLine("Offspring of " + generations.ToString() + ". generation evaluated in {0} s. Total time: {1} s -------------- {2}",
                t, (DateTime.Now - beggining).TotalSeconds, evaluationsInOnePopulation);

                Console.WriteLine("Begining time: {0}. Estimated end time: {1}", beggining, DateTime.Now.AddSeconds(t * (maxEvaluations / populationSize - generations - 1)));
                
                sw.WriteLine(t);
            				
				// The offspring population becomes the new current population
				population.clear();
				for (int i = 0; i < populationSize; i++)
				{
					population.add(offspringPopulation.getSolution(i));
				}
				offspringPopulation.clear();
				population.sort(comparator);

                Console.WriteLine("Writing report for " + generations.ToString() + ". generation...");
                m_problem.PrintGenerationReport(population, population.getSolution(0), generations);
                evaluationsInOnePopulation = 0;
                
                GAtotal += (DateTime.Now - GAbeginning).TotalSeconds;
                sw.WriteLine("GA time: " + GAtotal);
                sw.Flush();
			} // while

            epw.Stop();

			// Return a population with the best individual
			SolutionSet resultPopulation = new SolutionSet(1);
			resultPopulation.add(population.getSolution(0));
			
			System.Console.WriteLine("Evaluations: " + evaluations);

            Console.WriteLine("Completed in {0} s", (DateTime.Now - beggining).TotalSeconds);
            sw.Close();
			return resultPopulation;
		} // execute
    }
}

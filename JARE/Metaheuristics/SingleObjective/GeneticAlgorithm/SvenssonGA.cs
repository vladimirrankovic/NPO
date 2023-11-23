using System;
using System.Collections.Generic;
using System.Text;
using JARE.Base;
using JARE.Base.operators.comparator;
using Algorithm = JARE.Base.Algorithm;
using RouletteWheelSelection = JARE.Base.operators.selection.RouletteWheelSelection;
using JARE.util;

namespace JARE.metaheuristics.singleObjective.geneticAlgorithm
{
    // klasa koja je ista kao gGA samo sto sortira solution-e na osnovu fitness-a

    public class SvenssonGA : Algorithm
    {
        // random number generator
        private static Random rand = new Random();
        protected int populationSize;
        protected int maxEvaluations;
        //protected int evaluations;
        protected double crossoverRate;
        protected double mutationRate;

        protected SolutionSet population;
        protected SolutionSet offspringPopulation;

        protected  Operator mutationOperator;
        protected Operator crossoverOperator;
        //protected Operator selectionOperator;
        protected RouletteWheelSelection selectionMethod;

        //protected System.Collections.Generic.IComparer<JARE.Base.Solution> comparator;

        private Problem m_problem;
		
		/// <summary> 
		/// Constructor
		/// Create a new GGA instance.
		/// </summary>
		/// <param name="problem">Problem to solve.
		/// </param>
		
        public SvenssonGA(Problem problem)
		{
			this.m_problem = problem;
		} // GGA
		
		/// <summary> Execute the GGA algorithm</summary>
		/// <throws>  SMException  </throws>

        public void Init()
        {
            selectionMethod = new RouletteWheelSelection();
            //this.comparator = new ObjectiveComparator(0); // Single objective comparator
            // Read the params
            populationSize = ((System.Int32)this.getInputParameter("populationSize"));
            maxEvaluations = ((System.Int32)this.getInputParameter("maxEvaluations"));

            // Initialize the variables
            population = new SolutionSet(500);
            //offspringPopulation = new SolutionSet(populationSize);

            crossoverRate = 0.75;
            mutationRate = 0.1;

            //evaluations = 0;

            // Read the operators
            mutationOperator = this.m_operators["mutation"];
            crossoverOperator = this.m_operators["crossover"];
            //selectionOperator = this.m_operators["selection"];

            // Create the initial population
            Solution newIndividual;
            for (int i = 0; i < populationSize; i++)
            {
                newIndividual = new Solution(m_problem);
                m_problem.evaluate(newIndividual);
                //evaluations++;
                population.add(newIndividual);
            } //for       


        }
        public virtual void Crossover()
        {
            // crossover
            for (int i = 1; i < populationSize; i += 2)
            {
                // generate next random number and check if we need to do crossover
                if (rand.NextDouble() <= crossoverRate)
                {
                    Solution[] parents = new Solution[2];

                    parents[0] = (Solution)population.getSolution(i - 1);
                    parents[1] = (Solution)population.getSolution(i);

                    // Crossover
                    Solution[] offspring = (Solution[])crossoverOperator.execute(parents);

                    // Evaluation of the new individual
                    m_problem.evaluate(offspring[0]);
                    m_problem.evaluate(offspring[1]);

                    // add two new offsprings to the population
                    population.add(offspring[0]);
                    population.add(offspring[1]);
                }
            }
        }

        public virtual void Mutate()
        {
            // mutate
            for (int i = 0; i < populationSize; i++)
            {
                // generate next random number and check if we need to do mutation
                if (rand.NextDouble() <= mutationRate)
                {
                    Solution s = population.getSolution(i);
                    mutationOperator.execute(s);
                   
                    // calculate fitness of the mutant
                    m_problem.evaluate(s);

                    // add mutant to the population
                    population.add(s);
                }
            }
        }

        public override SolutionSet execute()
		{
			
            //while (evaluations < maxEvaluations)
            //{
            //    if ((evaluations % 1000) == 0)
            //    {
            //        System.Console.Out.WriteLine(evaluations + ": " + population.getSolution(0).getObjective(0));
            //    } //
				
				// Copy the best two individuals to the offspring population
				//offspringPopulation.add(new Solution(population.getSolution(0)));
				//offspringPopulation.add(new Solution(population.getSolution(1)));
            Crossover();
            Mutate();
            //////selectionMethod.ApplySelection(population, populationSize);

            //    // Reproductive cycle
            //    for (int i = 0; i < (populationSize / 2 - 1); i++)
            //    {
            //        // Selection
            //        Solution[] parents = new Solution[2];
					
            //        parents[0] = (Solution) selectionOperator.execute(population);
            //        parents[1] = (Solution) selectionOperator.execute(population);
					
            //        // Crossover
            //        Solution[] offspring = (Solution[]) crossoverOperator.execute(parents);
					
            //        // Mutation
            //        mutationOperator.execute(offspring[0]);
            //        mutationOperator.execute(offspring[1]);
					
            //        // Evaluation of the new individual
            //        m_problem.evaluate(offspring[0]);
            //        m_problem.evaluate(offspring[1]);
					
            //        evaluations += 2;
					
            //        // Replacement: the two new individuals are inserted in the offspring
            //        //                population
            //        offspringPopulation.add(offspring[0]);
            //        offspringPopulation.add(offspring[1]);
            //    } // for
				
            //    // The offspring population becomes the new current population
            //    population.clear();
            //    for (int i = 0; i < populationSize; i++)
            //    {
            //        population.add(offspringPopulation.getSolution(i));
            //    }
            //    offspringPopulation.clear();
            //    population.sort(comparator);
            ////} // while
			
			// Return a population with the best individual
			SolutionSet resultPopulation = new SolutionSet(1);
			resultPopulation.add(population.getSolution(0));

            //System.Console.Out.WriteLine("Evaluations: " + evaluations);
			return resultPopulation;
		} // execute
    }
}

/// <summary> gGA.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.comparator;
using Algorithm = JARE.Base.Algorithm;
using JARE.util;
namespace JARE.metaheuristics.singleObjective.geneticAlgorithm
{
	
	/// <summary> Class implementing a generational genetic algorithm</summary>
	[Serializable]
	public class gGA : Algorithm
	{
		private Problem m_problem;
		
		/// <summary> 
		/// Constructor
		/// Create a new GGA instance.
		/// </summary>
		/// <param name="problem">Problem to solve.
		/// </param>
		public gGA(Problem problem)
		{
			this.m_problem = problem;
		} // GGA
		
		/// <summary> Execute the GGA algorithm</summary>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize;
			int maxEvaluations;
			int evaluations;
			
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
			
			// Create the initial population
			Solution newIndividual;
			for (int i = 0; i < populationSize; i++)
			{
				newIndividual = new Solution(m_problem);
				m_problem.evaluate(newIndividual);
				evaluations++;
				population.add(newIndividual);
			} //for       
			
			// Sort population
			population.sort(comparator);
			while (evaluations < maxEvaluations)
			{
				if ((evaluations % 1000) == 0)
				{
					System.Console.Out.WriteLine(evaluations + ": " + population.getSolution(0).getObjective(0));
				} //
				
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
					
					// Evaluation of the new individual
					m_problem.evaluate(offspring[0]);
					m_problem.evaluate(offspring[1]);
					
					evaluations += 2;
					
					// Replacement: the two new individuals are inserted in the offspring
					// population
					offspringPopulation.add(offspring[0]);
					offspringPopulation.add(offspring[1]);
				} // for
				
				// The offspring population becomes the new current population
				population.clear();
				for (int i = 0; i < populationSize; i++)
				{
					population.add(offspringPopulation.getSolution(i));
				}
				offspringPopulation.clear();
				population.sort(comparator);
			} // while
			
			// Return a population with the best individual
			SolutionSet resultPopulation = new SolutionSet(1);
			resultPopulation.add(population.getSolution(0));
			
			System.Console.WriteLine("Evaluations: " + evaluations);
            
			return resultPopulation;
		} // execute
	} // SSGA
}
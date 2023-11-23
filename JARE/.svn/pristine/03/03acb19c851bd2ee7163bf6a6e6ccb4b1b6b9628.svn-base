/// <summary> Elitist.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.comparator;
using Algorithm = JARE.Base.Algorithm;
using JARE.util;
namespace JARE.metaheuristics.singleObjective.evolutionStrategy
{
	
	/// <summary> Class implementing a (mu + lambda) ES. Lambda must be divisible by mu</summary>
	[Serializable]
	public class ElitistES:Algorithm
	{
		private Problem m_problem;
		private int m_mu;
		private int m_lambda;
		
		/// <summary> Constructor
		/// Create a new ElitistES instance.
		/// </summary>
		/// <param name="problem">Problem to solve.
		/// </param>
		/// <mu>  Mu </mu>
		/// <lambda>  Lambda </lambda>
		public ElitistES(Problem problem, int mu, int lambda)
		{
			m_problem = problem;
			m_mu = mu;
			m_lambda = lambda;
		} // ElitistES
		
		/// <summary> Execute the ElitistES algorithm</summary>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int maxEvaluations;
			int evaluations;
			
			SolutionSet population;
			SolutionSet offspringPopulation;
			
			Operator mutationOperator;
            System.Collections.Generic.IComparer<JARE.Base.Solution> comparator;
			
			comparator = new ObjectiveComparator(0); // Single objective comparator
			
			// Read the params
			maxEvaluations = ((System.Int32) this.getInputParameter("maxEvaluations"));
			
			// Initialize the variables
			population = new SolutionSet(m_mu);
			offspringPopulation = new SolutionSet(m_mu + m_lambda);
			
			evaluations = 0;
			
			// Read the operators
			mutationOperator = this.m_operators["mutation"];
			
			System.Console.Out.WriteLine("(" + m_mu + " + " + m_lambda + ")ES");
			
			// Create the parent population of mu solutions
			Solution newIndividual;
			for (int i = 0; i < m_mu; i++)
			{
				newIndividual = new Solution(m_problem);
				m_problem.evaluate(newIndividual);
				evaluations++;
				population.add(newIndividual);
			} //for       
			
			// Main loop
			int offsprings;
			offsprings = m_lambda / m_mu;
			while (evaluations < maxEvaluations)
			{
				// STEP 1. Generate the mu+lambda population
				for (int i = 0; i < m_mu; i++)
				{
					for (int j = 0; j < offsprings; j++)
					{
						Solution offspring = new Solution(population.getSolution(i));
						mutationOperator.execute(offspring);
						m_problem.evaluate(offspring);
						offspringPopulation.add(offspring);
						evaluations++;
					} // for
				} // for
				
				// STEP 2. Add the mu individuals to the offspring population
				for (int i = 0; i < m_mu; i++)
				{
					offspringPopulation.add(population.getSolution(i));
				} // for
				population.clear();
				
				// STEP 3. Sort the mu+lambda population
				offspringPopulation.sort(comparator);
				
				// STEP 4. Create the new mu population
				for (int i = 0; i < m_mu; i++)
					population.add(offspringPopulation.getSolution(i));
				
				System.Console.Out.WriteLine("Evaluation: " + evaluations + " Fitness: " + population.getSolution(0).getObjective(0));
				
				// STEP 6. Delete the mu+lambda population
				offspringPopulation.clear();
			} // while
			
			// Return a population with the best individual
			SolutionSet resultPopulation = new SolutionSet(1);
			resultPopulation.add(population.getSolution(0));
			
			return resultPopulation;
		} // execute
	} // SSGA
}
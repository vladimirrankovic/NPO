/// <summary> NonElitist.java</summary>
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
	
	/// <summary> Class implementing a (mu,lambda) ES. Lambda must be divisible by mu.</summary>
	[Serializable]
	public class NonElitistES:Algorithm
	{
		private Problem m_problem;
		private int m_mu;
		private int m_lambda;
		
		/// <summary> Constructor
		/// Create a new NonElitistES instance.
		/// </summary>
		/// <param name="problem">Problem to solve.
		/// </param>
		/// <mu>  Mu </mu>
		/// <lambda>  Lambda </lambda>
		public NonElitistES(Problem problem, int mu, int lambda)
		{
			m_problem = problem;
			m_mu = mu;
			m_lambda = lambda;
		} // NonElitistES
		
		/// <summary> Execute the NonElitistES algorithm</summary>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int maxEvaluations;
			int evaluations;
			
			Solution bestIndividual;
			
			SolutionSet population;
			SolutionSet offspringPopulation;
			
			Operator mutationOperator;
            System.Collections.Generic.IComparer<JARE.Base.Solution> comparator;
			
			comparator = new ObjectiveComparator(0); // Single objective comparator
			
			// Read the params
			maxEvaluations = ((System.Int32) this.getInputParameter("maxEvaluations"));
			
			// Initialize the variables
			population = new SolutionSet(m_mu + 1);
			offspringPopulation = new SolutionSet(m_lambda);
			
			evaluations = 0;
			
			// Read the operators
			mutationOperator = this.m_operators["mutation"];
			
			System.Console.Out.WriteLine("(" + m_mu + " , " + m_lambda + ")ES");
			
			// Create the parent population of mu solutions
			Solution newIndividual;
			newIndividual = new Solution(m_problem);
			m_problem.evaluate(newIndividual);
			evaluations++;
			population.add(newIndividual);
			bestIndividual = new Solution(newIndividual);
			
			for (int i = 1; i < m_mu; i++)
			{
				System.Console.Out.WriteLine(i);
				newIndividual = new Solution(m_problem);
				m_problem.evaluate(newIndividual);
				evaluations++;
				population.add(newIndividual);
				
				if (comparator.Compare(bestIndividual, newIndividual) > 0)
					bestIndividual = new Solution(newIndividual);
			} //for       
			
			// Main loop
			int offsprings;
			offsprings = m_lambda / m_mu;
			while (evaluations < maxEvaluations)
			{
				// STEP 1. Generate the lambda population
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
				
				// STEP 2. Sort the lambda population
				offspringPopulation.sort(comparator);
				
				// STEP 3. Update the best individual 
				if (comparator.Compare(bestIndividual, offspringPopulation.getSolution(0)) > 0)
					bestIndividual = new Solution(offspringPopulation.getSolution(0));
				
				// STEP 4. Create the new mu population
				population.clear();
				for (int i = 0; i < m_mu; i++)
					population.add(offspringPopulation.getSolution(i));
				
				System.Console.Out.WriteLine("Evaluation: " + evaluations + " Current best fitness: " + population.getSolution(0).getObjective(0) + " Global best fitness: " + bestIndividual.getObjective(0));
				
				// STEP 5. Delete the lambda population
				offspringPopulation.clear();
			} // while
			
			// Return a population with the best individual
			SolutionSet resultPopulation = new SolutionSet(1);
			resultPopulation.add(population.getSolution(0));
			
			return resultPopulation;
		} // execute
	} // SSGA
}
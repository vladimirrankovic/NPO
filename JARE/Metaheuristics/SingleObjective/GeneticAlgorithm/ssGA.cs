/// <summary> ssGA.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.comparator;
using BestSolutionSelection = JARE.Base.operators.selection.BestSolutionSelection;
using WorstSolutionSelection = JARE.Base.operators.selection.WorstSolutionSelection;
using Permutation = JARE.Base.variable.Permutation;
using Algorithm = JARE.Base.Algorithm;
using JARE.util;
namespace JARE.metaheuristics.singleObjective.geneticAlgorithm
{
	
	/// <summary> Class implementing a steady state genetic algorithm</summary>
	[Serializable]
	public class ssGA:Algorithm
	{
		private Problem m_problem;
		
		/// <summary> 
		/// Constructor
		/// Create a new SSGA instance.
		/// </summary>
		/// <param name="problem">Problem to solve
		/// 
		/// </param>
		public ssGA(Problem problem)
		{
			this.m_problem = problem;
		} // SSGA
		
		/// <summary> Execute the SSGA algorithm</summary>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize;
			int maxEvaluations;
			int evaluations;
			
			SolutionSet population;
			Operator mutationOperator;
			Operator crossoverOperator;
			Operator selectionOperator;

            System.Collections.Generic.IComparer<JARE.Base.Solution> comparator;
			
			comparator = new ObjectiveComparator(0); // Single objective comparator
			
			Operator findWorstSolution;
			findWorstSolution = new WorstSolutionSelection(comparator);
			
			// Read the parameters
			populationSize = ((System.Int32) this.getInputParameter("populationSize"));
			maxEvaluations = ((System.Int32) this.getInputParameter("maxEvaluations"));
			
			// Initialize the variables
			population = new SolutionSet(populationSize);
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
			
			// main loop
			while (evaluations < maxEvaluations)
			{
				Solution[] parents = new Solution[2];
				
				// Selection
				parents[0] = (Solution) selectionOperator.execute(population);
				parents[1] = (Solution) selectionOperator.execute(population);
				
				// Crossover
				Solution[] offspring = (Solution[]) crossoverOperator.execute(parents);
				
				// Mutation
				mutationOperator.execute(offspring[0]);
				
				// Evaluation of the new individual
				m_problem.evaluate(offspring[0]);
				
				evaluations++;
				
				// Replacement: replace the last individual is the new one is better
				int worstIndividual = (System.Int32) findWorstSolution.execute(population);
				
				if (comparator.Compare(population.getSolution(worstIndividual), offspring[0]) > 0)
				{
					population.remove(worstIndividual);
					population.add(offspring[0]);
				} // if
			} // while
			
			// Return a population with the best individual
			population.sort(comparator);
			
			SolutionSet resultPopulation = new SolutionSet(1);
			resultPopulation.add(population.getSolution(0));
			
			System.Console.Out.WriteLine("Evaluations: " + evaluations);
			
			return resultPopulation;
		} // execute
	} // SSGA
}
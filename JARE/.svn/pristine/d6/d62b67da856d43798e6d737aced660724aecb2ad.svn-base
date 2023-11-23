/// <summary> DE.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0  
/// </version>
using System;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using Problem = JARE.Base.Problem;
using Solution = JARE.Base.Solution;
using SolutionSet = JARE.Base.SolutionSet;
using ObjectiveComparator = JARE.Base.operators.comparator.ObjectiveComparator;
using Distance = JARE.util.Distance;
using SMException = JARE.util.SMException;
using Ranking = JARE.util.Ranking;
namespace JARE.metaheuristics.singleObjective.differentialEvolution
{
	
	/// <summary> This class implements a differential evolution algorithm. </summary>
	[Serializable]
	public class DE:Algorithm
	{
		/// <summary> stores the problem  to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public DE(Problem problem)
		{
			this.m_problem = problem;
		} // gDE
		
		/// <summary> Runs of the DE algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize;
			int maxEvaluations;
			int evaluations;
			
			SolutionSet population;
			SolutionSet offspringPopulation;
			
			Operator selectionOperator;
			Operator crossoverOperator;

            System.Collections.Generic.IComparer<JARE.Base.Solution> comparator;
			comparator = new ObjectiveComparator(0); // Single objective comparator
			
			// Differential evolution parameters
			int r1;
			int r2;
			int r3;
			int jrand;
			
			Solution[] parent;
			
			//Read the parameters
			populationSize = ((System.Int32) this.getInputParameter("populationSize"));
			maxEvaluations = ((System.Int32) this.getInputParameter("maxEvaluations"));
			
			selectionOperator = m_operators["selection"];
			crossoverOperator = m_operators["crossover"];
			
			//Initialize the variables
			population = new SolutionSet(populationSize);
			evaluations = 0;
			
			// Create the initial solutionSet
			Solution newSolution;
			for (int i = 0; i < populationSize; i++)
			{
				newSolution = new Solution(m_problem);
				m_problem.evaluate(newSolution);
				m_problem.evaluateConstraints(newSolution);
				evaluations++;
				population.add(newSolution);
			} //for       
			
			// Generations ...
			population.sort(comparator);
			while (evaluations < maxEvaluations)
			{
				
				// Create the offSpring solutionSet      
				offspringPopulation = new SolutionSet(populationSize);
				
				//offspringPopulation.add(new Solution(population.get(0))) ;	
				
				for (int i = 0; i < populationSize; i++)
				{
					// Obtain parents. Two parameters are required: the population and the 
					//                 index of the current individual
					parent = (Solution[]) selectionOperator.execute(new System.Object[]{population, i});
					
					Solution child;
					
					// Crossover. Two parameters are required: the current individual and the 
					//            array of parents
					child = (Solution) crossoverOperator.execute(new System.Object[]{population.getSolution(i), parent});
					
					m_problem.evaluate(child);
					
					evaluations++;
					
					if (comparator.Compare(population.getSolution(i), child) < 0)
						offspringPopulation.add(new Solution(population.getSolution(i)));
					else
						offspringPopulation.add(child);
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
			
			System.Console.Out.WriteLine("Evaluations: " + evaluations);
			return resultPopulation;
		} // execute
	} // gDE
}
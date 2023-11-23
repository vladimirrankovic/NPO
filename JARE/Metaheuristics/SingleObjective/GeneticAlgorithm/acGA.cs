/// <summary> acGA.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.comparator;
using BestSolutionSelection = JARE.Base.operators.selection.BestSolutionSelection;
using JARE.util;
namespace JARE.metaheuristics.singleObjective.geneticAlgorithm
{
	
	/// <summary> Class implementing an asynchronous cellular genetic algorithm</summary>
	[Serializable]
	public class acGA:Algorithm
	{
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public acGA(Problem problem)
		{
			m_problem = problem;
		} // sMOCell1
		
		
		/// <summary> Runs of the acGA algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that contains the best found solution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize, maxEvaluations, evaluations;
			Operator mutationOperator = null;
			Operator crossoverOperator = null;
			Operator selectionOperator = null;
			
			SolutionSet[] neighbors;
			SolutionSet population;
			Neighborhood neighborhood;

            System.Collections.Generic.IComparer<JARE.Base.Solution> comparator;
			comparator = new ObjectiveComparator(0); // Single objective comparator
			
			Operator findBestSolution;
			findBestSolution = new BestSolutionSelection(comparator);
			
			//Read the params
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			
			//Read the operators
			mutationOperator = m_operators["mutation"];
			crossoverOperator = m_operators["crossover"];
			selectionOperator = m_operators["selection"];
			
			//Initialize the variables    
			evaluations = 0;
			neighborhood = new Neighborhood(populationSize);
			neighbors = new SolutionSet[populationSize];
			
			population = new SolutionSet(populationSize);
			//Create the initial population
			for (int i = 0; i < populationSize; i++)
			{
				Solution solution = new Solution(m_problem);
				m_problem.evaluate(solution);
				population.add(solution);
				solution.Location = i;
				evaluations++;
			}
			
			bool solutionFound = false;
			while ((evaluations < maxEvaluations) && !solutionFound)
			{
				for (int ind = 0; ind < population.size(); ind++)
				{
					Solution individual = new Solution(population.getSolution(ind));
					
					Solution[] parents = new Solution[2];
					Solution[] offSpring = null;
					
					neighbors[ind] = neighborhood.getEightNeighbors(population, ind);
					neighbors[ind].add(individual);
					
					//parents
					parents[0] = (Solution) selectionOperator.execute(neighbors[ind]);
					parents[1] = (Solution) selectionOperator.execute(neighbors[ind]);
					
					//Create a new solution, using genetic operators mutation and crossover
					if (crossoverOperator != null)
						offSpring = (Solution[]) crossoverOperator.execute(parents);
					else
					{
						offSpring = new Solution[1];
						offSpring[0] = new Solution(parents[0]);
					}
					mutationOperator.execute(offSpring[0]);
					
					//->Evaluate offspring and constraints
					m_problem.evaluate(offSpring[0]);
					//m_problem.evaluateConstraints(offSpring[0]);
					evaluations++;
					
					if (comparator.Compare(individual, offSpring[0]) > 0)
						population.replace(ind, offSpring[0]);
					
					if ((evaluations % 1000) == 0)
					{
						int bestSolution = (System.Int32) findBestSolution.execute(population);
						System.Console.Out.WriteLine("Evals: " + evaluations + "\t Fitness: " + population.getSolution(bestSolution).getObjective(0));
					} // if
				} // for                     
			} // while
			
			population.sort(comparator);
			SolutionSet resultPopulation = new SolutionSet(1);
			resultPopulation.add(population.getSolution(0));
			
			System.Console.Out.WriteLine("Evaluations: " + evaluations);
			return resultPopulation;
		} // execute        
	} // acGA
}
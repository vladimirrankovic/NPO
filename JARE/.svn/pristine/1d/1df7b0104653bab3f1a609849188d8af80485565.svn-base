/// <summary>a * GDE3.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0  
/// </version>
using System;
using JARE.Base;
using JARE.util;
namespace JARE.metaheuristics.gde3
{
	
	/// <summary> This class implements the GDE3 algorithm. </summary>
	[Serializable]
	public class GDE3:Algorithm
	{
		
		/// <summary> stores the problem  to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public GDE3(Problem problem)
		{
			this.m_problem = problem;
		} // GDE3
		
		/// <summary> Runs of the GDE3 algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize;
			int maxIterations;
			int evaluations;
			int iterations;
			
			SolutionSet population;
			SolutionSet offspringPopulation;
			SolutionSet union;
			
			Distance distance;
			System.Collections.IComparer dominance;
			
			Operator selectionOperator;
			Operator crossoverOperator;
			
			distance = new Distance();
            dominance = new JARE.Base.operators.comparator.DominanceComparator();
			
			// Differential evolution parameters
			int r1;
			int r2;
			int r3;
			int jrand;
			
			Solution[] parent;
			
			//Read the parameters
			populationSize = ((System.Int32) this.getInputParameter("populationSize"));
			maxIterations = ((System.Int32) this.getInputParameter("maxIterations"));
			
			selectionOperator = m_operators["selection"];
			crossoverOperator = m_operators["crossover"];
			
			//Initialize the variables
			population = new SolutionSet(populationSize);
			evaluations = 0;
			iterations = 0;
			
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
			while (iterations < maxIterations)
			{
				// Create the offSpring solutionSet      
				offspringPopulation = new SolutionSet(populationSize * 2);
				
				for (int i = 0; i < (populationSize); i++)
				{
					// Obtain parents. Two parameters are required: the population and the 
					//                 index of the current individual
					parent = (Solution[]) selectionOperator.execute(new System.Object[]{population, i});
					
					Solution child;
					// Crossover. Two parameters are required: the current individual and the 
					//            array of parents
					child = (Solution) crossoverOperator.execute(new System.Object[]{population.getSolution(i), parent});
					
					m_problem.evaluate(child);
					m_problem.evaluateConstraints(child);
					evaluations++;
					
					// Dominance test
					int result;
					result = dominance.Compare(population.getSolution(i), child);
					if (result == - 1)
					{
						// Solution i dominates child
						offspringPopulation.add(population.getSolution(i));
					}
					// if
					else if (result == 1)
					{
						// child dominates
						offspringPopulation.add(child);
					}
					// else if
					else
					{
						// the two solutions are non-dominated
						offspringPopulation.add(child);
						offspringPopulation.add(population.getSolution(i));
					} // else
				} // for           
				
				// Ranking the offspring population
				Ranking ranking = new Ranking(offspringPopulation);
				
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
				
				// remain is less than front(index).size, insert only the best one
				if (remain > 0)
				{
					// front contains individuals to insert                        
					while (front.size() > remain)
					{
						distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
                        front.sort(new JARE.Base.operators.comparator.CrowdingComparator());
						front.remove(front.size() - 1);
					}
					for (int k = 0; k < front.size(); k++)
					{
						population.add(front.getSolution(k));
					}
					
					remain = 0;
				} // if                   
				
				iterations++;
			} // while
			
			// Return the first non-dominated front
			Ranking ranking2 = new Ranking(population);
			return ranking2.getSubfront(0);
		} // execute
	} // GDE3
}
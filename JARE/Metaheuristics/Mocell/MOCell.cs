/// <summary> MOCell4.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
using JARE.Base;
using CrowdingArchive = JARE.util.archive.CrowdingArchive;
using JARE.Base.operators.comparator;
using JARE.util;
namespace JARE.metaheuristics.mocell
{
	
	/// <summary> This class represents an asynchronous version of MOCell algorithm, combining
	/// aMOCell2 and aMOCell3.
	/// </summary>
	[Serializable]
	public class MOCell:Algorithm
	{
		
		//->fields
		private Problem m_problem; //The problem to solve        
		
		public MOCell(Problem problem)
		{
			m_problem = problem;
		}
		
		/// <summary>Execute the algorithm </summary>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			//Init the parameters
			int populationSize, archiveSize, maxEvaluations, evaluations;
			Operator mutationOperator, crossoverOperator, selectionOperator;
			SolutionSet currentPopulation;
			CrowdingArchive archive;
			SolutionSet[] neighbors;
			Neighborhood neighborhood;
			System.Collections.IComparer dominance = new DominanceComparator();
            System.Collections.Generic.IComparer<JARE.Base.Solution> crowdingComparator = new CrowdingComparator();
			Distance distance = new Distance();
			
			// Read the parameters
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			archiveSize = ((System.Int32) getInputParameter("archiveSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			
			// Read the operators
			mutationOperator = m_operators["mutation"];
			crossoverOperator = m_operators["crossover"];
			selectionOperator = m_operators["selection"];
			
			// Initialize the variables    
			currentPopulation = new SolutionSet(populationSize);
			archive = new CrowdingArchive(archiveSize, m_problem.NumberOfObjectives);
			evaluations = 0;
			neighborhood = new Neighborhood(populationSize);
			neighbors = new SolutionSet[populationSize];
			
			// Create the initial population
			for (int i = 0; i < populationSize; i++)
			{
				Solution individual = new Solution(m_problem);
				m_problem.evaluate(individual);
				m_problem.evaluateConstraints(individual);
				currentPopulation.add(individual);
				individual.Location = i;
				evaluations++;
			}
			
			// Main loop
			while (evaluations < maxEvaluations)
			{
				for (int ind = 0; ind < currentPopulation.size(); ind++)
				{
					Solution individual = new Solution(currentPopulation.getSolution(ind));
					
					Solution[] parents = new Solution[2];
					Solution[] offSpring;
					
					//neighbors[ind] = neighborhood.getFourNeighbors(currentPopulation,ind);
					neighbors[ind] = neighborhood.getEightNeighbors(currentPopulation, ind);
					neighbors[ind].add(individual);
					
					// parents
					parents[0] = (Solution) selectionOperator.execute(neighbors[ind]);
					if (archive.size() > 0)
					{
						parents[1] = (Solution) selectionOperator.execute(archive);
					}
					else
					{
						parents[1] = (Solution) selectionOperator.execute(neighbors[ind]);
					}
					
					// Create a new individual, using genetic operators mutation and crossover
					offSpring = (Solution[]) crossoverOperator.execute(parents);
					mutationOperator.execute(offSpring[0]);
					
					// Evaluate individual an his constraints
					m_problem.evaluate(offSpring[0]);
					m_problem.evaluateConstraints(offSpring[0]);
					evaluations++;
					
					int flag = dominance.Compare(individual, offSpring[0]);
					
					if (flag == 1)
					{
						//The new individual dominates
						offSpring[0].Location = individual.Location;
						currentPopulation.replace(offSpring[0].Location, offSpring[0]);
						archive.add(new Solution(offSpring[0]));
					}
					else if (flag == 0)
					{
						//The new individual is non-dominated               
						neighbors[ind].add(offSpring[0]);
						offSpring[0].Location = - 1;
						Ranking rank = new Ranking(neighbors[ind]);
						for (int j = 0; j < rank.getNumberOfSubfronts(); j++)
						{
							distance.crowdingDistanceAssignment(rank.getSubfront(j), m_problem.NumberOfObjectives);
						}
						neighbors[ind].sort(crowdingComparator);
						Solution worst = neighbors[ind].getSolution(neighbors[ind].size() - 1);
						
						if (worst.Location == - 1)
						{
							//The worst is the offspring
							archive.add(new Solution(offSpring[0]));
						}
						else
						{
							offSpring[0].Location = worst.Location;
							currentPopulation.replace(offSpring[0].Location, offSpring[0]);
							archive.add(new Solution(offSpring[0]));
						}
					}
				}
			}
			return archive;
		} // while       
	} // MOCell
}
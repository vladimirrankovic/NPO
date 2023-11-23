/// <summary> sMOCell2.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using CrowdingArchive = JARE.util.archive.CrowdingArchive;
using JARE.Base.operators.comparator;
using JARE.util;
namespace JARE.metaheuristics.mocell
{
	
	/// <summary> This class represents a synchronous version of MOCell algorithm, which 
	/// applies an archive feedback through parent selection. 
	/// </summary>
	[Serializable]
	public class sMOCell2:Algorithm
	{
		
		/// <summary> Stores the problem to solve </summary>
		private Problem m_problem;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public sMOCell2(Problem problem)
		{
			m_problem = problem;
		} //sMOCell2
		
		/// <summary> Runs of the sMOCell2 algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize, archiveSize, maxEvaluations, evaluations, feedBack;
			Operator mutationOperator, crossoverOperator, selectionOperator;
			SolutionSet currentSolutionSet, newSolutionSet;
			CrowdingArchive archive;
			SolutionSet[] neighbors;
			Neighborhood neighborhood;
            System.Collections.IComparer dominance = new DominanceComparator(); 
               System.Collections.Generic.IComparer<JARE.Base.Solution> crowding = new CrowdingComparator();
			Distance distance = new Distance();
			
			//Read the params
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			archiveSize = ((System.Int32) getInputParameter("archiveSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			
			//Read the operators
			mutationOperator = m_operators["mutation"];
			crossoverOperator = m_operators["crossover"];
			selectionOperator = m_operators["selection"];
			
			//Initialize the variables    
			currentSolutionSet = new SolutionSet(populationSize);
			newSolutionSet = new SolutionSet(populationSize);
			archive = new CrowdingArchive(archiveSize, m_problem.NumberOfObjectives);
			evaluations = 0;
			neighborhood = new Neighborhood(populationSize);
			neighbors = new SolutionSet[populationSize];
			
			//Create the initial population
			for (int i = 0; i < populationSize; i++)
			{
				Solution solution = new Solution(m_problem);
				m_problem.evaluate(solution);
				m_problem.evaluateConstraints(solution);
				currentSolutionSet.add(solution);
				solution.Location = i;
				evaluations++;
			}
			//
			int iterations = 0;
			
			while (evaluations < maxEvaluations)
			{
				newSolutionSet = new SolutionSet(populationSize);
				for (int ind = 0; ind < currentSolutionSet.size(); ind++)
				{
					Solution individual = new Solution(currentSolutionSet.getSolution(ind));
					
					Solution[] parents = new Solution[2];
					Solution[] offSpring;
					
					//neighbors[ind] = neighborhood.getFourNeighbors(currentSolutionSet,ind);
					neighbors[ind] = neighborhood.getEightNeighbors(currentSolutionSet, ind);
					neighbors[ind].add(individual);
					
					//parents
					parents[0] = (Solution) selectionOperator.execute(neighbors[ind]);
					if (archive.size() > 0)
					{
						parents[1] = (Solution) selectionOperator.execute(archive);
					}
					else
					{
						parents[1] = (Solution) selectionOperator.execute(neighbors[ind]);
					}
					
					//Create a new solution, using genetic operators mutation and crossover
					offSpring = (Solution[]) crossoverOperator.execute(parents);
					mutationOperator.execute(offSpring[0]);
					
					//->Evaluate solution an his constraints
					m_problem.evaluate(offSpring[0]);
					m_problem.evaluateConstraints(offSpring[0]);
					evaluations++;
					//<-Individual evaluated
					
					int flag = dominance.Compare(individual, offSpring[0]);
					
					if (flag == - 1)
					{
						newSolutionSet.add(new Solution(currentSolutionSet.getSolution(ind)));
					}
					
					if (flag == 1)
					{
						//The new indivudlas dominate
						offSpring[0].Location = individual.Location;
						//currentSolutionSet.reemplace(offSpring[0].getLocation(),offSpring[0]);
						newSolutionSet.add(offSpring[0]);
						archive.add(new Solution(offSpring[0]));
					}
					else if (flag == 0)
					{
						//The individuals are non-dominates
						neighbors[ind].add(offSpring[0]);
						//(new Spea2Fitness(neighbors[ind])).fitnessAssign();                   
						//neighbors[ind].sort(new FitnessAndCrowdingDistanceComparator()); //Create a new comparator;
						Ranking rank = new Ranking(neighbors[ind]);
						for (int j = 0; j < rank.getNumberOfSubfronts(); j++)
						{
							distance.crowdingDistanceAssignment(rank.getSubfront(j), m_problem.NumberOfObjectives);
						}
						bool deleteMutant = true;
						
						int compareResult = crowding.Compare(individual, offSpring[0]);
						if (compareResult == 1)
						{
							//The offSpring[0] is better
							deleteMutant = false;
						}
						
						if (!deleteMutant)
						{
							offSpring[0].Location = individual.Location;
							//currentSolutionSet.reemplace(offSpring[0].getLocation(),offSpring[0]);
							newSolutionSet.add(offSpring[0]);
							archive.add(new Solution(offSpring[0]));
						}
						else
						{
							newSolutionSet.add(new Solution(currentSolutionSet.getSolution(ind)));
							archive.add(new Solution(offSpring[0]));
						}
					}
				}
				
				
				currentSolutionSet = newSolutionSet;
			}
			return archive;
		} // execute       
	} // sMOCell2
}
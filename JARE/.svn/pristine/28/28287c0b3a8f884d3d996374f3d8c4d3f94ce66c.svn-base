/// <summary> aMOCell1.java</summary>
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
	
	/// <summary> This class represents the asynchronous version of the MOCell algorithm</summary>
	[Serializable]
	public class aMOCell1:Algorithm
	{
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public aMOCell1(Problem problem)
		{
			m_problem = problem;
		} //aMOCell1       
		
		/// <summary> Runs of the aMOCell1 algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize, archiveSize, maxEvaluations, evaluations, feedBack;
			Operator mutationOperator, crossoverOperator, selectionOperator;
			SolutionSet currentSolutionSet;
			CrowdingArchive archive;
			SolutionSet[] neighbors;
			Neighborhood neighborhood;
            System.Collections.IComparer dominance = new DominanceComparator(); 
              System.Collections.Generic.IComparer<JARE.Base.Solution>  crowding = new CrowdingComparator();
			Distance distance = new Distance();
			
			//Read the params
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			archiveSize = ((System.Int32) getInputParameter("archiveSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			feedBack = ((System.Int32) getInputParameter("feedBack"));
			
			//Read the operators
			mutationOperator = m_operators["mutation"];
			crossoverOperator = m_operators["crossover"];
			selectionOperator = m_operators["selection"];
			
			//Init the variables
			//init the population and the archive
			currentSolutionSet = new SolutionSet(populationSize);
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
			
			
			while (evaluations < maxEvaluations)
			{
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
					parents[1] = (Solution) selectionOperator.execute(neighbors[ind]);
					
					//Create a new solution, using genetic operators mutation and crossover
					offSpring = (Solution[]) crossoverOperator.execute(parents);
					mutationOperator.execute(offSpring[0]);
					
					//Evaluate solution and constraints
					m_problem.evaluate(offSpring[0]);
					m_problem.evaluateConstraints(offSpring[0]);
					evaluations++;
					
					// Check dominance
					int flag = dominance.Compare(individual, offSpring[0]);
					if (flag == 1)
					{
						// offSpring[0] dominates
						offSpring[0].Location = individual.Location;
						currentSolutionSet.replace(offSpring[0].Location, offSpring[0]);
						archive.add(new Solution(offSpring[0]));
					}
					else if (flag == 0)
					{
						//Both two are non-dominates               
						neighbors[ind].add(offSpring[0]);
						//(new Spea2Fitness(neighbors[ind])).fitnessAssign();                   
						//neighbors[ind].sort(new FitnessAndCrowdingDistanceComparator());
						Ranking rank = new Ranking(neighbors[ind]);
						for (int j = 0; j < rank.getNumberOfSubfronts(); j++)
						{
							(distance).crowdingDistanceAssignment(rank.getSubfront(j), m_problem.NumberOfObjectives);
						}
						
						bool deleteMutant = true;
						int compareResult = crowding.Compare(individual, offSpring[0]);
						if (compareResult == 1)
						//The offSpring[0] is better
							deleteMutant = false;
						
						if (!deleteMutant)
						{
							offSpring[0].Location = individual.Location;
							currentSolutionSet.replace(offSpring[0].Location, offSpring[0]);
							archive.add(new Solution(offSpring[0]));
						}
						else
						{
							archive.add(new Solution(offSpring[0]));
						}
					}
				}
				
				//Stores a portion of the archive into the population
				(distance).crowdingDistanceAssignment(archive, m_problem.NumberOfObjectives);
				for (int j = 0; j < feedBack; j++)
				{
					if (archive.size() > j)
					{
						int r = PseudoRandom.randInt(0, currentSolutionSet.size() - 1);
						if (r < currentSolutionSet.size())
						{
							Solution individual = archive.getSolution(j);
							individual.Location = r;
							currentSolutionSet.replace(r, new Solution(individual));
						}
					}
				}
			}
			return archive;
		} // execute        
	} // aMOCell1
}
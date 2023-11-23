/// <summary> CellDE.java</summary>
/// <author>  Juan J. Durillo, Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.comparator;
using JARE.util;
namespace JARE.metaheuristics.cellde
{
	
	/// <summary> This class represents the original asynchronous MOCell algorithm
	/// hybridized with Diferential evolutions (GDE3), called CellDE. It uses an 
	/// archive based on spea2 fitness to store non-dominated solutions.
	/// </summary>
	[Serializable]
	public class CellDE:Algorithm
	{
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public CellDE(Problem problem)
		{
			m_problem = problem;
		} // CellDE
		
		
		/// <summary> Runs of the CellDE algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		/// <throws>  ClassNotFoundException  </throws>
		public override SolutionSet execute()
		{
			int populationSize, archiveSize, maxEvaluations, evaluations, feedBack;
			Operator crossoverOperator, selectionOperator;
			SolutionSet currentSolutionSet;
			SolutionSet archive;
			SolutionSet[] neighbors;
			Neighborhood neighborhood;
            System.Collections.IComparer dominance = new DominanceComparator();
            System.Collections.Generic.IComparer<JARE.Base.Solution> crowding = new CrowdingComparator();
			Distance distance = new Distance();
			
			//Read the params
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			archiveSize = ((System.Int32) getInputParameter("archiveSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			feedBack = ((System.Int32) getInputParameter("feedBack"));
			
			//Read the operators
			crossoverOperator = m_operators["crossover"];
			selectionOperator = m_operators["selection"];
			
			//Initialize the variables    
			currentSolutionSet = new SolutionSet(populationSize);
			archive = new JARE.util.archive.StrengthRawFitnessArchive(archiveSize);
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
					
					Solution[] parents = new Solution[3];
					Solution offSpring;
					
					neighbors[ind] = neighborhood.getEightNeighbors(currentSolutionSet, ind);
					
					//parents
					parents[0] = (Solution) selectionOperator.execute(neighbors[ind]);
					parents[1] = (Solution) selectionOperator.execute(neighbors[ind]);
					parents[2] = individual;
					
					//Create a new solution, using genetic operators mutation and crossover
					offSpring = (Solution) crossoverOperator.execute(new System.Object[]{individual, parents});
					
					//->Evaluate offspring and constraints
					m_problem.evaluate(offSpring);
					m_problem.evaluateConstraints(offSpring);
					evaluations++;
					
					int flag = dominance.Compare(individual, offSpring);
					
					if (flag == 1)
					{
						//The offSpring dominates
						offSpring.Location = individual.Location;
						//currentSolutionSet.reemplace(offSpring[0].getLocation(),offSpring[0]);
						currentSolutionSet.replace(ind, new Solution(offSpring));
						//newSolutionSet.add(offSpring);
						archive.add(new Solution(offSpring));
					}
					else if (flag == 0)
					{
						//Both two are non-dominates
						neighbors[ind].add(offSpring);
						//(new Spea2Fitness(neighbors[ind])).fitnessAssign();                   
						//neighbors[ind].sort(new FitnessAndCrowdingDistanceComparator()); //Create a new comparator;
						Ranking rank = new Ranking(neighbors[ind]);
						for (int j = 0; j < rank.getNumberOfSubfronts(); j++)
						{
							distance.crowdingDistanceAssignment(rank.getSubfront(j), m_problem.NumberOfObjectives);
						}
						
						bool deleteMutant = true;
						int compareResult = crowding.Compare(individual, offSpring);
						if (compareResult == 1)
						{
							//The offSpring[0] is better
							deleteMutant = false;
						}
						
						if (!deleteMutant)
						{
							offSpring.Location = individual.Location;
							//currentSolutionSet.reemplace(offSpring[0].getLocation(),offSpring[0]);
							//newSolutionSet.add(offSpring);
							currentSolutionSet.replace(offSpring.Location, offSpring);
							archive.add(new Solution(offSpring));
						}
						else
						{
							//newSolutionSet.add(new Solution(currentSolutionSet.get(ind)));
							archive.add(new Solution(offSpring));
						}
					}
				}
				
				//Store a portion of the archive into the population
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
	} // CellDE
}
/// <summary> SPEA2.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.util;
namespace JARE.metaheuristics.spea2
{
	
	/// <summary> This class representing the SPEA2 algorithm</summary>
	[Serializable]
	public class SPEA2:Algorithm
	{
		
		/// <summary> Defines the number of tournaments for creating the mating pool</summary>
		public const int TOURNAMENTS_ROUNDS = 1;
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor.
		/// Create a new SPEA2 instance
		/// </summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public SPEA2(Problem problem)
		{
			this.m_problem = problem;
		} // Spea2
		
		/// <summary> Runs of the Spea2 algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int populationSize, archiveSize, maxEvaluations, evaluations;
			Operator crossoverOperator, mutationOperator, selectionOperator;
			SolutionSet solutionSet, archive, offSpringSolutionSet;
			
			//Read the params
			populationSize = ((System.Int32) getInputParameter("populationSize"));
			archiveSize = ((System.Int32) getInputParameter("archiveSize"));
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			
			//Read the operators
			crossoverOperator = m_operators["crossover"];
			mutationOperator = m_operators["mutation"];
			selectionOperator = m_operators["selection"];
			
			//Initialize the variables
			solutionSet = new SolutionSet(populationSize);
			archive = new SolutionSet(archiveSize);
			evaluations = 0;
			
			//-> Create the initial solutionSet
			Solution newSolution;
			for (int i = 0; i < populationSize; i++)
			{
				newSolution = new Solution(m_problem);
				m_problem.evaluate(newSolution);
				m_problem.evaluateConstraints(newSolution);
				evaluations++;
				solutionSet.add(newSolution);
			}
			
			while (evaluations < maxEvaluations)
			{
				SolutionSet union = ((SolutionSet) solutionSet).union(archive);
				Spea2Fitness spea = new Spea2Fitness(union);
				spea.fitnessAssign();
				archive = spea.environmentalSelection(archiveSize);
				// Create a new offspringPopulation
				offSpringSolutionSet = new SolutionSet(populationSize);
				Solution[] parents = new Solution[2];
				while (offSpringSolutionSet.size() < populationSize)
				{
					int j = 0;
					do 
					{
						j++;
						parents[0] = (Solution) selectionOperator.execute(archive);
					}
					while (j < SPEA2.TOURNAMENTS_ROUNDS); // do-while                    
					int k = 0;
					do 
					{
						k++;
						parents[1] = (Solution) selectionOperator.execute(archive);
					}
					while (k < SPEA2.TOURNAMENTS_ROUNDS); // do-while
					
					//make the crossover 
					Solution[] offSpring = (Solution[]) crossoverOperator.execute(parents);
					mutationOperator.execute(offSpring[0]);
					m_problem.evaluate(offSpring[0]);
					m_problem.evaluateConstraints(offSpring[0]);
					offSpringSolutionSet.add(offSpring[0]);
					evaluations++;
				} // while
				// End Create a offSpring solutionSet
				solutionSet = offSpringSolutionSet;
			} // while
			
			Ranking ranking = new Ranking(archive);
			return ranking.getSubfront(0);
		} // execute    
	} // Spea2
}
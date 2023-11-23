/// <summary> Paes.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using AdaptiveGridArchive = JARE.util.archive.AdaptiveGridArchive;
using JARE.Base.operators.comparator;
using SMException = JARE.util.SMException;
namespace JARE.metaheuristics.paes
{
	
	/// <summary> This class implements the NSGA-II algorithm. </summary>
	[Serializable]
	public class PAES:Algorithm
	{
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Create a new PAES instance for resolve a problem</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public PAES(Problem problem)
		{
			m_problem = problem;
		} // Paes
		
		/// <summary> Tests two solutions to determine which one becomes be the guide of PAES
		/// algorithm
		/// </summary>
		/// <param name="solution">The actual guide of PAES
		/// </param>
		/// <param name="mutatedSolution">A candidate guide
		/// </param>
		public virtual Solution test(Solution solution, Solution mutatedSolution, AdaptiveGridArchive archive)
		{
			
			int originalLocation = archive.Grid.location(solution);
			int mutatedLocation = archive.Grid.location(mutatedSolution);
			
			if (originalLocation == - 1)
			{
				return new Solution(mutatedSolution);
			}
			
			if (mutatedLocation == - 1)
			{
				return new Solution(solution);
			}
			
			if (archive.Grid.getLocationDensity(mutatedLocation) < archive.Grid.getLocationDensity(originalLocation))
			{
				return new Solution(mutatedSolution);
			}
			
			return new Solution(solution);
		} // test
		
		/// <summary> Runs of the Paes algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int bisections, archiveSize, maxEvaluations, evaluations;
			AdaptiveGridArchive archive;
			Operator mutationOperator;
			System.Collections.IComparer dominance;
			
			//Read the params
			bisections = ((System.Int32) this.getInputParameter("biSections"));
			archiveSize = ((System.Int32) this.getInputParameter("archiveSize"));
			maxEvaluations = ((System.Int32) this.getInputParameter("maxEvaluations"));
			
			//Read the operators        
			mutationOperator = this.m_operators["mutation"];
			
			//Initialize the variables                
			evaluations = 0;
			archive = new AdaptiveGridArchive(archiveSize, bisections, m_problem.NumberOfObjectives);
			dominance = new DominanceComparator();
			
			//-> Create the initial solution and evaluate it and his constraints
			Solution solution = new Solution(m_problem);
			m_problem.evaluate(solution);
			m_problem.evaluateConstraints(solution);
			evaluations++;
			
			// Add it to the archive
			archive.add(new Solution(solution));
			
			//Iterations....
			do 
			{
				// Create the mutate one
				Solution mutatedIndividual = new Solution(solution);
				mutationOperator.execute(mutatedIndividual);
				
				m_problem.evaluate(mutatedIndividual);
				m_problem.evaluateConstraints(mutatedIndividual);
				evaluations++;
				//<-
				
				// Check dominance
				int flag = dominance.Compare(solution, mutatedIndividual);
				
				if (flag == 1)
				{
					//If mutate solution dominate                  
					solution = new Solution(mutatedIndividual);
					archive.add(mutatedIndividual);
				}
				else if (flag == 0)
				{
					//If none dominate the other                               
					if (archive.add(mutatedIndividual))
					{
						solution = test(solution, mutatedIndividual, archive);
					}
				}
			}
			while (evaluations < maxEvaluations);
			
			//Return the  population of non-dominated solution
			return archive;
		} // execute  
	} // PAES
}
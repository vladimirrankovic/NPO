/// <summary> PESA2.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
using JARE.Base;
using AdaptiveGridArchive = JARE.util.archive.AdaptiveGridArchive;
using PESA2Selection = JARE.Base.operators.selection.PESA2Selection;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.metaheuristics.pesa2
{
	
	/// <summary> This class implements the PESA2 algorithm. </summary>
	[Serializable]
	public class PESA2:Algorithm
	{
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor
		/// Creates a new instance of PESA2
		/// </summary>
		public PESA2(Problem problem)
		{
			m_problem = problem;
		} // PESA2
		
		/// <summary> Runs of the PESA2 algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
		/// as a result of the algorithm execution  
		/// </returns>
		/// <throws>  SMException  </throws>
		public override SolutionSet execute()
		{
			int archiveSize, bisections, maxEvaluations, evaluations, populationSize;
			AdaptiveGridArchive archive;
			SolutionSet solutionSet;
			Operator crossover, mutation, selection;
			
			// Read parameters
			populationSize = ((System.Int32) (m_inputParameters["populationSize"]));
			archiveSize = ((System.Int32) (m_inputParameters["archiveSize"]));
			bisections = ((System.Int32) (m_inputParameters["bisections"]));
			maxEvaluations = ((System.Int32) (m_inputParameters["maxEvaluations"]));
			
			// Get the operators
			crossover = m_operators["crossover"];
			mutation = m_operators["mutation"];
			
			// Initialize the variables
			evaluations = 0;
			archive = new AdaptiveGridArchive(archiveSize, bisections, m_problem.NumberOfObjectives);
			solutionSet = new SolutionSet(populationSize);
			selection = new PESA2Selection();
			
			//-> Create the initial individual and evaluate it and his constraints
			for (int i = 0; i < populationSize; i++)
			{
				Solution solution = new Solution(m_problem);
				m_problem.evaluate(solution);
				m_problem.evaluateConstraints(solution);
				evaluations++;
				solutionSet.add(solution);
			}
			//<-                
			
			// Incorporate non-dominated solution to the archive
			for (int i = 0; i < solutionSet.size(); i++)
			{
				archive.add(solutionSet.getSolution(i)); // Only non dominated are accepted by 
				// the archive
			}
			
			// Clear the init solutionSet
			solutionSet.clear();
			
			//Iterations....
			Solution[] parents = new Solution[2];
			do 
			{
				//-> Create the offSpring solutionSet                    
				while (solutionSet.size() < populationSize)
				{
					parents[0] = (Solution) selection.execute(archive);
					parents[1] = (Solution) selection.execute(archive);
					
					Solution[] offSpring = (Solution[]) crossover.execute(parents);
					mutation.execute(offSpring[0]);
					m_problem.evaluate(offSpring[0]);
					m_problem.evaluateConstraints(offSpring[0]);
					evaluations++;
					solutionSet.add(offSpring[0]);
				}
				
				for (int i = 0; i < solutionSet.size(); i++)
					archive.add(solutionSet.getSolution(i));
				
				// Clear the solutionSet
				solutionSet.clear();
			}
			while (evaluations < maxEvaluations);
			//Return the  solutionSet of non-dominated individual
			return archive;
		} // execute      
	} // PESA2
}
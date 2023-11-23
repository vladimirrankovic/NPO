/// <summary> RandomSearch.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.util;
namespace JARE.metaheuristics.randomSearch
{
	
	/// <summary> This class implements the NSGA-II algorithm.</summary>
	[Serializable]
	public class RandomSearch:Algorithm
	{
		
		/// <summary> stores the problem  to solve</summary>
		private Problem m_problem;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public RandomSearch(Problem problem)
		{
			this.m_problem = problem;
		} // RandomSearch
		
		/// <summary> Runs the RandomSearch algorithm.</summary>
		/// <returns> a <code>SolutionSet</code> that is a set of solutions
		/// as a result of the algorithm execution
		/// </returns>
		/// <throws>  SMException </throws>
		public override SolutionSet execute()
		{
			int maxEvaluations;
			int evaluations;
			
			maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
			
			//Initialize the variables
			evaluations = 0;
			
			NonDominatedSolutionList ndl = new NonDominatedSolutionList();
			
			// Create the initial solutionSet
			Solution newSolution;
			for (int i = 0; i < maxEvaluations; i++)
			{
				newSolution = new Solution(m_problem);
				m_problem.evaluate(newSolution);
				m_problem.evaluateConstraints(newSolution);
				evaluations++;
				ndl.add(newSolution);
			} //for
			
			return ndl;
		} // execute
	} // RandomSearch
}
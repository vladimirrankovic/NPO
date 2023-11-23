/// <summary> SolutionType
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// This is an abstract class representing solution types, which define the 
/// types of the variables constituting a solution
/// </version>
using System;
namespace JARE.Base
{
	
	
	public abstract class SolutionType
	{
		public Problem m_problem; /// <summary>Problem to be solved </summary>
		
		/// <summary> Constructor</summary>
		/// <param name="problem">The problem to solve
		/// </param>
		
        public SolutionType(Problem problem)
		{
			this.m_problem = problem;
		} 
		
		/// <summary> Abstract method to create the variables of the solution</summary>
		/// <param name="decisionVariables">
		/// </param>
		
        public abstract Variable[] createVariables();
		
		/// <summary> Copies the decision variables</summary>
		/// <param name="decisionVariables">
		/// </param>
		/// <returns> An array of variables
		/// </returns>
		
        public virtual Variable[] copyVariables(Variable[] vars)
		{
			Variable[] variables;
			
			variables = new Variable[vars.Length];
			for (int var = 0; var < vars.Length; var++)
			{
				variables[var] = vars[var].deepCopy();
			} 
			
			return variables;
		} 
	} 
}
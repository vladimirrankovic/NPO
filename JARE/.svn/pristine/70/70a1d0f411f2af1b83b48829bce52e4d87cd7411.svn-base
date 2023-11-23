/// <summary> PermutationType
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Class representing the solution type of solutions composed of Permuation
/// variables 
/// </version>
using System;
using Problem = JARE.Base.Problem;
using SolutionType = JARE.Base.SolutionType;
using Variable = JARE.Base.Variable;
using Int = JARE.Base.variable.Int;
using Permutation = JARE.Base.variable.Permutation;
using Real = JARE.Base.variable.Real;
using Configuration = JARE.util.Configuration;
namespace JARE.Base.solutionType
{
	
	public class PermutationSolutionType:SolutionType
	{
		
		/// <summary> Constructor</summary>
		/// <param name="problem">
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public PermutationSolutionType(Problem problem):base(problem)
		{
			problem.m_variableType = new System.Type[problem.NumberOfVariables];
			problem.SolutionType = this;
			
			// Initializing the types of the variables
			for (int i = 0; i < problem.NumberOfVariables; i++)
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                problem.m_variableType[i] = System.Type.GetType("JARE.Base.variable.Permutation");
			}
		} // PermutationSolution
		
		/// <summary> Creates the variables of the solution</summary>
		/// <param name="decisionVariables">
		/// </param>
		public override Variable[] createVariables()
		{
			Variable[] variables = new Variable[m_problem.NumberOfVariables];
			
			for (int var = 0; var < m_problem.NumberOfVariables; var++)
				variables[var] = new Permutation(m_problem.getLength(var));
			
			return variables;
		} // createVariables
	} // PermutationSolution
}
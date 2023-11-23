/// <summary> IntSolutionType
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Class representing the solution type of solutions composed of Int variables 
/// </version>
using System;
using Problem = JARE.Base.Problem;
using SolutionType = JARE.Base.SolutionType;
using Variable = JARE.Base.Variable;
using Int = JARE.Base.variable.Int;
namespace JARE.Base.solutionType
{
	
	public class IntSolutionType:SolutionType
	{
		
		/// <summary> Constructor</summary>
		/// <param name="problem">
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public IntSolutionType(Problem problem):base(problem)
		{
			problem.m_variableType = new System.Type[problem.NumberOfVariables];
			problem.SolutionType = this;
			
			// Initializing the types of the variables
			for (int i = 0; i < problem.NumberOfVariables; i++)
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                problem.m_variableType[i] = System.Type.GetType("JARE.Base.variable.Int");
			} // for    
		} // Constructor
		
		/// <summary> Creates the variables of the solution</summary>
		/// <param name="decisionVariables">
		/// </param>
		public override Variable[] createVariables()
		{
			Variable[] variables = new Variable[m_problem.NumberOfVariables];
			
			for (int var = 0; var < m_problem.NumberOfVariables; var++)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				variables[var] = new Int((int) m_problem.getLowerLimit(var), (int) m_problem.getUpperLimit(var));
			}
			
			return variables;
		} // createVariables
	} // IntSolutionType
}
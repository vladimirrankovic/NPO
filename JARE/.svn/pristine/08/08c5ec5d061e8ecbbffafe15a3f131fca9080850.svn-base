/// <summary> RealSolutionType
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Class representing the solution type of solutions composed of Binary 
/// variables
/// </version>
using System;
using Problem = JARE.Base.Problem;
using SolutionType = JARE.Base.SolutionType;
using Variable = JARE.Base.Variable;
using Real = JARE.Base.variable.Real;
namespace JARE.Base.solutionType
{
	
	public class RealSolutionType:SolutionType
	{
		
		/// <summary> Constructor</summary>
		/// <param name="problem">
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public RealSolutionType(Problem problem):base(problem)
		{
			problem.m_variableType = new System.Type[problem.NumberOfVariables];
			problem.SolutionType = this;
			
			// Initializing the types of the variables
			for (int i = 0; i < problem.NumberOfVariables; i++)
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                problem.m_variableType[i] = System.Type.GetType("JARE.Base.variable.Real");
			} // for    
		} // Constructor
		
		/// <summary> Creates the variables of the solution</summary>
		/// <param name="decisionVariables">
		/// </param>
		public override Variable[] createVariables()
		{
			Variable[] variables = new Variable[m_problem.NumberOfVariables];
			
			for (int var = 0; var < m_problem.NumberOfVariables; var++)
				variables[var] = new Real(m_problem.getLowerLimit(var), m_problem.getUpperLimit(var));
			
			return variables;
		} // createVariables
	} // RealSolutionType
}
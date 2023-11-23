/// <summary> ArrayRealSolutionType
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Class representing the solution type of solutions composed of an ArrayReal 
/// variable 
/// </version>
using System;
using Problem = JARE.Base.Problem;
using SolutionType = JARE.Base.SolutionType;
using Variable = JARE.Base.Variable;
using ArrayReal = JARE.Base.variable.ArrayReal;
using Real = JARE.Base.variable.Real;
namespace JARE.Base.solutionType
{
	
	public class ArrayRealSolutionType : SolutionType
	{
		
		/// <summary> Constructor</summary>
		/// <param name="problem">
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public ArrayRealSolutionType(Problem problem):base(problem)
		{
			problem.m_variableType = new System.Type[1];
			problem.SolutionType = this;
			
			// Initializing the types of the variables
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
            problem.m_variableType[0] = System.Type.GetType("JARE.Base.variable.ArrayReal");
		}
		
		/// <summary> Creates the variables of the solution</summary>
		/// <param name="decisionVariables">
		/// </param>
		public override Variable[] createVariables()
		{
			Variable[] variables = new Variable[1];
			
			variables[0] = new ArrayReal(m_problem.NumberOfVariables, m_problem);
			return variables;
		} // createVariables
		
		/// <summary> Copy the variables</summary>
		/// <param name="decisionVariables">
		/// </param>
		/// <returns> An array of variables
		/// </returns>
		public override Variable[] copyVariables(Variable[] vars)
		{
			Variable[] variables;
			
			variables = new Variable[1];
			variables[0] = vars[0].deepCopy();
			
			return variables;
		} // copyVariables
	} // ArrayRealSolutionType
}
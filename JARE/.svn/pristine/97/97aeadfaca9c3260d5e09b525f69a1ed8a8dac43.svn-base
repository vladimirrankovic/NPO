/// <summary> BinaryRealSolutionType
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Class representing the solution type of solutions composed of BinaryReal 
/// variables
/// </version>
using System;
using Problem = JARE.Base.Problem;
using SolutionType = JARE.Base.SolutionType;
using Variable = JARE.Base.Variable;
using BinaryReal = JARE.Base.variable.BinaryReal;
namespace JARE.Base.solutionType
{
	
	public class BinaryRealSolutionType:SolutionType
	{
		
		/// <summary> Constructor</summary>
		/// <param name="problem">
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public BinaryRealSolutionType(Problem problem):base(problem)
		{
			problem.m_variableType = new System.Type[problem.NumberOfVariables];
			problem.SolutionType = this;
			
			// Initializing the types of the variables
			for (int i = 0; i < problem.NumberOfVariables; i++)
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                problem.m_variableType[i] = System.Type.GetType("JARE.Base.variable.BinaryReal");
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
				if (m_problem.getPrecision() == null)
				{
					int[] precision = new int[m_problem.NumberOfVariables];
                    for (int i = 0; i < m_problem.NumberOfVariables; i++)
                        precision[i] = JARE.Base.variable.BinaryReal.DEFAULT_PRECISION; 
					m_problem.setPrecision(precision);
				} // if
				variables[var] = new BinaryReal(m_problem.getPrecision(var), m_problem.getLowerLimit(var), m_problem.getUpperLimit(var));
			} // for 
			return variables;
		} // createVariables
	} // BinaryRealSolutionType
}
/// <summary> IntRealSolutionType
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Class representing the solution type of solutions composed of IntReal 
/// variables
/// </version>
using System;
using Problem = JARE.Base.Problem;
using SolutionType = JARE.Base.SolutionType;
using Variable = JARE.Base.Variable;
using Int = JARE.Base.variable.Int;
using Real = JARE.Base.variable.Real;
using Configuration = JARE.util.Configuration;
namespace JARE.Base.solutionType
{
	
	public class IntRealSolutionType:SolutionType
	{
		
		private int m_intVariables;
		private int m_realVariables;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">
		/// </param>
		/// <param name="intVariables">Number of integer variables
		/// </param>
		/// <param name="realVariables">Number of real variables
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public IntRealSolutionType(Problem problem, int intVariables, int realVariables):base(problem)
		{
			problem.m_variableType = new System.Type[problem.NumberOfVariables];
			m_intVariables = intVariables;
			m_realVariables = realVariables;
			
			problem.SolutionType = this;
			
			// Initializing the types of the variables
			for (int i = 0; i < m_intVariables; i++)
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                problem.m_variableType[i] = System.Type.GetType("JARE.Base.variable.Int");
			}
			
			for (int i = m_intVariables; i < (m_intVariables + m_realVariables); i++)
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                problem.m_variableType[i] = System.Type.GetType("JARE.Base.variable.Real");
			} // for    
		} // Constructor
		
		/// <summary> Creates the variables of the solution</summary>
		/// <param name="decisionVariables">
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
		public override Variable[] createVariables()
		{
			Variable[] variables = new Variable[m_problem.NumberOfVariables];
			
			for (int var = 0; var < m_problem.NumberOfVariables; var++)
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                if (m_problem.m_variableType[var] == System.Type.GetType("JARE.Base.variable.Int"))
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					variables[var] = new Int((int) m_problem.getLowerLimit(var), (int) m_problem.getUpperLimit(var));
				}
				else
				{
					//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    if (m_problem.m_variableType[var] == System.Type.GetType("JARE.Base.variable.Real"))
						variables[var] = new Real(m_problem.getLowerLimit(var), m_problem.getUpperLimit(var));
					else
					{
						Configuration.m_logger.WriteLog("DecisionVariables.DecisionVariables: " + "error creating a Solution of type IntReal");
					}
				}
			} // else
			return variables;
		} // createVariables
	} // IntRealSolutionType
}
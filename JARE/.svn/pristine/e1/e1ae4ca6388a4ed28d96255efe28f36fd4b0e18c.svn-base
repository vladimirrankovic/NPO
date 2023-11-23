/// <summary> IntRealProblem.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using ArrayRealSolutionType = JARE.Base.solutionType.ArrayRealSolutionType;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using IntRealSolutionType = JARE.Base.solutionType.IntRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
namespace JARE.problems
{
	
	/// <summary> Class representing a problem having N integer and M real variables.
	/// This is not a true problem; it is only intended as an example
	/// </summary>
	[Serializable]
	public class IntRealProblem:Problem
	{
		
		internal int m_intVariables;
		internal int m_realVariables;
		
		/// <summary> Constructor.
		/// Creates a default instance of the IntRealProblem problem.
		/// </summary>
		public IntRealProblem(System.String solutionType):this(solutionType, 3, 3)
		{
		} // IntRealProblem
		
		/// <summary> Constructor.
		/// Creates a new instance of the IntRealProblem problem.
		/// </summary>
		/// <param name="intVariables">Number of integer variables of the problem 
		/// </param>
		/// <param name="realVariables">Number of real variables of the problem 
		/// </param>
		public IntRealProblem(System.String solutionType, int intVariables, int realVariables)
		{
			m_intVariables = intVariables;
			m_realVariables = realVariables;
			
			m_numberOfVariables = m_intVariables + m_realVariables;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "IntRealProblem";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			
			for (int i = 0; i < intVariables; i++)
			{
				m_lowerLimit[i] = - 5;
				m_upperLimit[i] = 5;
			} // for
			
			for (int i = intVariables; i < (intVariables + realVariables); i++)
			{
				m_lowerLimit[i] = - 5.0;
				m_upperLimit[i] = 5.0;
			} // for
			
			if (String.CompareOrdinal(solutionType, "IntReal") == 0)
				m_solutionType = new IntRealSolutionType(this, intVariables, realVariables);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} // IntRealProblem
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] variable = solution.DecisionVariables;
			
			double[] fx = new double[2]; // function values     
			
			fx[0] = 0.0;
			for (int var = 0; var < m_intVariables; var++)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				fx[0] += (int) variable[var].getValue();
			} // for
			
			fx[1] = 0.0;
			for (int var = m_intVariables; var < m_numberOfVariables; var++)
			{
				fx[0] += variable[var].getValue();
			} // for
			
			solution.setObjective(0, fx[0]);
			solution.setObjective(1, fx[1]);
		} // evaluate
	} // IntRealProblem
}
/// <summary> CEC2009_UF8.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
namespace JARE.problems.cec2009Competition
{
	
	/// <summary> Class representing problem CEC2009_UF8</summary>
	[Serializable]
	public class CEC2009_UF8:Problem
	{
		
		/// <summary> Constructor.
		/// Creates a default instance of problem CEC2009_UF8 (30 decision variables)
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public CEC2009_UF8(System.String solutionType):this(solutionType, 30)
		{ // 30 variables by default
		} // CEC2009_UF8
		
		/// <summary> Creates a new instance of problem CEC2009_UF8.</summary>
		/// <param name="numberOfVariables">Number of variables.
		/// </param>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public CEC2009_UF8(System.String solutionType, System.Int32 numberOfVariables)
		{
			m_numberOfVariables = numberOfVariables;
			m_numberOfObjectives = 3;
			m_numberOfConstraints = 0;
			m_problemName = "CEC2009_UF8";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			
			// Establishes upper and lower limits for the variables
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = 0.0;
				m_upperLimit[var] = 1.0;
			} // for
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} // CEC2009_UF7
		
		/// <summary> Evaluates a solution.</summary>
		/// <param name="solution">The solution to evaluate.
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] decisionVariables = solution.DecisionVariables;
			
			double[] x = new double[m_numberOfVariables];
			for (int i = 0; i < m_numberOfVariables; i++)
				x[i] = decisionVariables[i].getValue();
			
			int count1, count2, count3;
			double sum1, sum2, sum3, yj;
			sum1 = sum2 = sum3 = 0.0;
			count1 = count2 = count3 = 0;
			
			for (int j = 3; j <= m_numberOfVariables; j++)
			{
				yj = x[j - 1] - 2.0 * x[1] * System.Math.Sin(2.0 * System.Math.PI * x[0] + j * System.Math.PI / m_numberOfVariables);
				if (j % 3 == 1)
				{
					sum1 += yj * yj;
					count1++;
				}
				else if (j % 3 == 2)
				{
					sum2 += yj * yj;
					count2++;
				}
				else
				{
					sum3 += yj * yj;
					count3++;
				}
			}
			
			solution.setObjective(0, System.Math.Cos(0.5 * System.Math.PI * x[0]) * System.Math.Cos(0.5 * System.Math.PI * x[1]) + 2.0 * sum1 / (double) count1);
			solution.setObjective(1, System.Math.Cos(0.5 * System.Math.PI * x[0]) * System.Math.Sin(0.5 * System.Math.PI * x[1]) + 2.0 * sum2 / (double) count2);
			solution.setObjective(2, System.Math.Sin(0.5 * System.Math.PI * x[0]) + 2.0 * sum3 / (double) count3);
		} // evaluate
	} // CEC2009_UF8
}
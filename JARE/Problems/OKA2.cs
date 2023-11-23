/// <summary> OKA2.java
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
namespace JARE.problems
{
	
	/// <summary> Class representing problem Kursawe</summary>
	[Serializable]
	public class OKA2:Problem
	{
		
		
		/// <summary> Constructor.
		/// Creates a new instance of the OKA2 problem.
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public OKA2(System.String solutionType)
		{
			m_numberOfVariables = 3;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "OKA2";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			
			m_lowerLimit[0] = - System.Math.PI;
			m_upperLimit[0] = System.Math.PI;
			for (int i = 1; i < m_numberOfVariables; i++)
			{
				m_lowerLimit[i] = - 5.0;
				m_upperLimit[i] = 5.0;
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
		} // OKA2
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] decisionVariables = solution.DecisionVariables;
			
			double[] fx = new double[m_numberOfObjectives]; // 2 functions
			double[] x = new double[m_numberOfVariables]; // 3 variables
			
			for (int i = 0; i < m_numberOfVariables; i++)
				x[i] = decisionVariables[i].getValue();
			
			fx[0] = x[0];
			
			fx[1] = 1 - System.Math.Pow((x[0] + System.Math.PI), 2) / (4 * System.Math.Pow(System.Math.PI, 2)) + System.Math.Pow(System.Math.Abs(x[1] - 5 * System.Math.Cos(x[0])), 1.0 / 3.0) + System.Math.Pow(System.Math.Abs(x[2] - 5 * System.Math.Sin(x[0])), 1.0 / 3.0);
			
			solution.setObjective(0, fx[0]);
			solution.setObjective(1, fx[1]);
		} // evaluate
	} // OKA2
}
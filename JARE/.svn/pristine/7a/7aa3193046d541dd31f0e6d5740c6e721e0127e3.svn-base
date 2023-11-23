/// <summary> OKA1.java
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
	public class OKA1:Problem
	{
		
		
		/// <summary> Constructor.
		/// Creates a new instance of the OKA2 problem.
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public OKA1(System.String solutionType)
		{
			m_numberOfVariables = 2;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "OKA1";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			
			m_lowerLimit[0] = 6 * System.Math.Sin(System.Math.PI / 12.0);
			m_upperLimit[0] = 6 * System.Math.Sin(System.Math.PI / 12.0) + 2 * System.Math.PI * System.Math.Cos(System.Math.PI / 12.0);
			m_lowerLimit[1] = (- 2) * System.Math.PI * System.Math.Sin(System.Math.PI / 12.0);
			m_upperLimit[1] = 6 * System.Math.Cos(System.Math.PI / 12.0);
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} // OKA1
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] decisionVariables = solution.DecisionVariables;
			
			double[] fx = new double[m_numberOfObjectives]; // 2 functions
			double[] x = new double[m_numberOfVariables]; // 2 variables
			
			for (int i = 0; i < m_numberOfVariables; i++)
				x[i] = decisionVariables[i].getValue();
			
			double x0 = System.Math.Cos(System.Math.PI / 12.0) * x[0] - System.Math.Sin(System.Math.PI / 12.0) * x[1];
			double x1 = System.Math.Sin(System.Math.PI / 12.0) * x[0] + System.Math.Cos(System.Math.PI / 12.0) * x[1];
			
			fx[0] = x0;
			fx[1] = System.Math.Sqrt(2 * System.Math.PI) - System.Math.Sqrt(System.Math.Abs(x0)) + 2 * System.Math.Pow(System.Math.Abs(x1 - 3 * System.Math.Cos(x0) - 3), 1.0 / 3.0);
			
			solution.setObjective(0, fx[0]);
			solution.setObjective(1, fx[1]);
		} // evaluate
	} // OKA1
}
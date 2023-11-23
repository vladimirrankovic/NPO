/// <summary> Viennet3.java
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
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
namespace JARE.problems
{
	
	/// <summary> Class representing problem Viennet3</summary>
	[Serializable]
	public class Viennet3:Problem
	{
		
		/// <summary> Constructor.
		/// Creates a default instance of the Viennet3 problem.
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public Viennet3(System.String solutionType)
		{
			m_numberOfVariables = 2;
			m_numberOfObjectives = 3;
			m_numberOfConstraints = 0;
			m_problemName = "Viennet3";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = - 3.0;
				m_upperLimit[var] = 3.0;
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
		} //Viennet3
		
		
		/// <summary> Evaluates a solution.</summary>
		/// <param name="solution">The solution to evaluate.
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			double[] x = new double[m_numberOfVariables];
			double[] f = new double[m_numberOfObjectives];
			
			for (int i = 0; i < m_numberOfVariables; i++)
				x[i] = solution.DecisionVariables[i].getValue();
			
			f[0] = 0.5 * (x[0] * x[0] + x[1] * x[1]) + System.Math.Sin(x[0] * x[0] + x[1] * x[1]);
			
			// Second function
			double value1 = 3.0 * x[0] - 2.0 * x[1] + 4.0;
			double value2 = x[0] - x[1] + 1.0;
			f[1] = (value1 * value1) / 8.0 + (value2 * value2) / 27.0 + 15.0;
			
			// Third function
			f[2] = 1.0 / (x[0] * x[0] + x[1] * x[1] + 1) - 1.1 * System.Math.Exp(- (x[0] * x[0]) - (x[1] * x[1]));
			
			
			for (int i = 0; i < m_numberOfObjectives; i++)
				solution.setObjective(i, f[i]);
		} // evaluate
	} // Viennet3
}
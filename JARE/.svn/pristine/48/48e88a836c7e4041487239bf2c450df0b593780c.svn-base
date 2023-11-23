/// <summary> Fonseca.java
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
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using ArrayReal = JARE.Base.variable.ArrayReal;
using SMException = JARE.util.SMException;
using XReal = JARE.util.wrapper.XReal;
namespace JARE.problems
{
	
	/// <summary> Class representing problem Fonseca</summary>
	[Serializable]
	public class Fonseca:Problem
	{
		
		/// <summary> Constructor
		/// Creates a default instance of the Fonseca problem
		/// </summary>
		/// <param name="solutionType">The solution type must "Real", "BinaryReal, 
		/// ArrayReal, or ArrayRealC".
		/// </param>
		public Fonseca(System.String solutionType)
		{
			m_numberOfVariables = 3;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "Fonseca";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = - 4.0;
				m_upperLimit[var] = 4.0;
			} // for
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "ArrayReal") == 0)
				m_solutionType = new ArrayRealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} //Fonseca
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			XReal x = new XReal(solution);
			
			double[] f = new double[m_numberOfObjectives];
			double sum1 = 0.0;
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				sum1 += System.Math.Pow(x.getValue(var) - (1.0 / System.Math.Sqrt((double) m_numberOfVariables)), 2.0);
			}
			double exp1 = System.Math.Exp((- 1.0) * sum1);
			f[0] = 1 - exp1;
			
			double sum2 = 0.0;
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				sum2 += System.Math.Pow(x.getValue(var) + (1.0 / System.Math.Sqrt((double) m_numberOfVariables)), 2.0);
			}
			double exp2 = System.Math.Exp((- 1.0) * sum2);
			f[1] = 1 - exp2;
			
			solution.setObjective(0, f[0]);
			solution.setObjective(1, f[1]);
		} // evaluate
	} // Fonseca
}
/// <summary> DTLZ5.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juanjo Durillo
/// </author>
/// <version>  1.0
/// Created on 16 de octubre de 2006, 17:30
/// </version>
using System;
using JARE.Base;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
namespace JARE.problems.DTLZ
{
	
	/// <summary> Class representing problem DTLZ5</summary>
	[Serializable]
	public class DTLZ5:Problem
	{
		
		/// <summary> Creates a default DTLZ5 problem instance (12 variables and 3 objectives)</summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		public DTLZ5(System.String solutionType):this(solutionType, 12, 3)
		{
		} // DTLZ5
		
		/// <summary> Creates a new DTLZ5 problem instance</summary>
		/// <param name="numberOfVariables">Number of variables
		/// </param>
		/// <param name="numberOfObjectives">Number of objective functions
		/// </param>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public DTLZ5(System.String solutionType, System.Int32 numberOfVariables, System.Int32 numberOfObjectives)
		{
			m_numberOfVariables = numberOfVariables;
			m_numberOfObjectives = numberOfObjectives;
			m_numberOfConstraints = 0;
			m_problemName = "DTLZ5";
			
			m_lowerLimit = new double[m_numberOfVariables];
			m_upperLimit = new double[m_numberOfVariables];
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = 0.0;
				m_upperLimit[var] = 1.0;
			}
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} // DTLZ5
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] gen = solution.DecisionVariables;
			
			double[] x = new double[m_numberOfVariables];
			double[] f = new double[m_numberOfObjectives];
			double[] theta = new double[m_numberOfObjectives - 1];
			double g = 0.0;
			int k = m_numberOfVariables - m_numberOfObjectives + 1;
			
			for (int i = 0; i < m_numberOfVariables; i++)
				x[i] = gen[i].getValue();
			
			for (int i = m_numberOfVariables - k; i < m_numberOfVariables; i++)
				g += (x[i] - 0.5) * (x[i] - 0.5);
			
			double t = System.Math.PI / (4.0 * (1.0 + g));
			
			theta[0] = x[0] * System.Math.PI / 2.0;
			for (int i = 1; i < (m_numberOfObjectives - 1); i++)
				theta[i] = t * (1.0 + 2.0 * g * x[i]);
			
			for (int i = 0; i < m_numberOfObjectives; i++)
				f[i] = 1.0 + g;
			
			for (int i = 0; i < m_numberOfObjectives; i++)
			{
				for (int j = 0; j < m_numberOfObjectives - (i + 1); j++)
					f[i] *= System.Math.Cos(theta[j]);
				if (i != 0)
				{
					int aux = m_numberOfObjectives - (i + 1);
					f[i] *= System.Math.Sin(theta[aux]);
				} // if
			} //for
			
			for (int i = 0; i < m_numberOfObjectives; i++)
				solution.setObjective(i, f[i]);
		} // evaluate
	}
}
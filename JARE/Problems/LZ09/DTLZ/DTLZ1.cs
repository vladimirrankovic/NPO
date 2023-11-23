/// <summary> DTLZ1.java
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
	
	/// <summary> Class representing problem DTLZ1 </summary>
	[Serializable]
	public class DTLZ1:Problem
	{
		/// <summary> Creates a default DTLZ1 problem (7 variables and 3 objectives)</summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		public DTLZ1(System.String solutionType):this(solutionType, 7, 3)
		{
		} // DTLZ1   
		
		/// <summary> Creates a DTLZ1 problem instance</summary>
		/// <param name="numberOfVariables">Number of variables
		/// </param>
		/// <param name="numberOfObjectives">Number of objective functions
		/// </param>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public DTLZ1(System.String solutionType, System.Int32 numberOfVariables, System.Int32 numberOfObjectives)
		{
			m_numberOfVariables = numberOfVariables;
			m_numberOfObjectives = numberOfObjectives;
			m_numberOfConstraints = 0;
			m_problemName = "DTLZ1";
			
			m_lowerLimit = new double[m_numberOfVariables];
			m_upperLimit = new double[m_numberOfVariables];
			for (int var = 0; var < numberOfVariables; var++)
			{
				m_lowerLimit[var] = 0.0;
				m_upperLimit[var] = 1.0;
			} //for
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		}
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] gen = solution.DecisionVariables;
			
			double[] x = new double[m_numberOfVariables];
			double[] f = new double[m_numberOfObjectives];
			int k = m_numberOfVariables - m_numberOfObjectives + 1;
			
			for (int i = 0; i < m_numberOfVariables; i++)
				x[i] = gen[i].getValue();
			
			double g = 0.0;
			for (int i = m_numberOfVariables - k; i < m_numberOfVariables; i++)
				g += (x[i] - 0.5) * (x[i] - 0.5) - System.Math.Cos(20.0 * System.Math.PI * (x[i] - 0.5));
			
			g = 100 * (k + g);
			for (int i = 0; i < m_numberOfObjectives; i++)
				f[i] = (1.0 + g) * 0.5;
			
			for (int i = 0; i < m_numberOfObjectives; i++)
			{
				for (int j = 0; j < m_numberOfObjectives - (i + 1); j++)
					f[i] *= x[j];
				if (i != 0)
				{
					int aux = m_numberOfObjectives - (i + 1);
					f[i] *= (1 - x[aux]);
				} //if
			} //for
			
			for (int i = 0; i < m_numberOfObjectives; i++)
				solution.setObjective(i, f[i]);
		} // evaluate   
	}
}
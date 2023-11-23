/// <summary> ZDT6.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juanjo Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using ArrayRealSolutionType = JARE.Base.solutionType.ArrayRealSolutionType;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
using XReal = JARE.util.wrapper.XReal;
namespace JARE.problems.ZDT
{
	
	/// <summary> Class representing problem ZDT6</summary>
	[Serializable]
	public class ZDT6:Problem
	{
		
		/// <summary> Creates a default instance of problem ZDT6 (10 decision variables)</summary>
		/// <param name="solutionType">The solution type must "Real", "BinaryReal, and "ArrayReal". 
		/// </param>
		public ZDT6(System.String solutionType):this(solutionType, 10)
		{ // 10 variables by default
		} // ZDT6
		
		/// <summary> Creates a instance of problem ZDT6</summary>
		/// <param name="numberOfVariables">Number of variables
		/// </param>
		/// <param name="solutionType">The solution type must "Real", "BinaryReal, and "ArrayReal". 
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public ZDT6(System.String solutionType, System.Int32 numberOfVariables)
		{
			m_numberOfVariables = numberOfVariables;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "ZDT6";
			
			m_lowerLimit = new double[m_numberOfVariables];
			m_upperLimit = new double[m_numberOfVariables];
			
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = 0.0;
				m_upperLimit[var] = 1.0;
			} //for
			
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
		} //ZDT6
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			XReal x = new XReal(solution);
			
			double x1 = x.getValue(0);
			double[] f = new double[m_numberOfObjectives];
			f[0] = 1.0 - System.Math.Exp((- 4.0) * x1) * System.Math.Pow(System.Math.Sin(6.0 * System.Math.PI * x1), 6.0);
			double g = this.evalG(x);
			double h = this.evalH(f[0], g);
			f[1] = h * g;
			
			solution.setObjective(0, f[0]);
			solution.setObjective(1, f[1]);
		} //evaluate
		
		/// <summary> Returns the value of the ZDT6 function G.</summary>
		/// <param name="decisionVariables">The decision variables of the solution to 
		/// evaluate.
		/// </param>
		/// <throws>  SMException  </throws>
		public virtual double evalG(XReal x)
		{
			double g = 0.0;
			for (int var = 1; var < this.m_numberOfVariables; var++)
				g += x.getValue(var);
			g = g / (m_numberOfVariables - 1);
			g = System.Math.Pow(g, 0.25);
			g = 9.0 * g;
			g = 1.0 + g;
			return g;
		} // evalG
		
		/// <summary> Returns the value of the ZDT6 function H.</summary>
		/// <param name="f">First argument of the function H.
		/// </param>
		/// <param name="g">Second argument of the function H.
		/// </param>
		public virtual double evalH(double f, double g)
		{
			return 1.0 - System.Math.Pow((f / g), 2.0);
		} // evalH       
	} //ZDT6
}
/// <summary> ZDT4.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// Created on 16 de octubre de 2006, 17:30
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
	
	/// <summary> Class representing problem ZDT4</summary>
	[Serializable]
	public class ZDT4:Problem
	{
		
		/// <summary> Constructor.
		/// Creates a default instance of problem ZDT4 (10 decision variables)
		/// </summary>
		/// <param name="solutionType">The solution type must "Real", "BinaryReal, and "ArrayReal". 
		/// </param>
		public ZDT4(System.String solutionType):this(solutionType, 10)
		{ // 10 variables by default
		} // ZDT4
		
		/// <summary> Creates a instance of problem ZDT4.</summary>
		/// <param name="numberOfVariables">Number of variables.
		/// </param>
		/// <param name="solutionType">The solution type must "Real", "BinaryReal, and "ArrayReal". 
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public ZDT4(System.String solutionType, System.Int32 numberOfVariables)
		{
			m_numberOfVariables = numberOfVariables;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "ZDT4";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			
			m_lowerLimit[0] = 0.0;
			m_upperLimit[0] = 1.0;
			for (int var = 1; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = - 5.0;
				m_upperLimit[var] = 5.0;
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
		} //ZDT4
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			XReal x = new XReal(solution);
			
			double[] f = new double[m_numberOfObjectives];
			f[0] = x.getValue(0);
			double g = this.evalG(x);
			double h = this.evalH(f[0], g);
			f[1] = h * g;
			
			solution.setObjective(0, f[0]);
			solution.setObjective(1, f[1]);
		} //evaluate
		
		/// <summary> Returns the value of the ZDT4 function G.</summary>
		/// <param name="decisionVariables">The decision variables of the solution to 
		/// evaluate.
		/// </param>
		/// <throws>  SMException  </throws>
		public virtual double evalG(XReal x)
		{
			double g = 0.0;
			for (int var = 1; var < m_numberOfVariables; var++)
				g += System.Math.Pow(x.getValue(var), 2.0) + (- 10.0) * System.Math.Cos(4.0 * System.Math.PI * x.getValue(var));
			
			double constante = 1.0 + 10.0 * (m_numberOfVariables - 1);
			return g + constante;
		} // evalG
		
		/// <summary> Returns the value of the ZDT4 function H.</summary>
		/// <param name="f">First argument of the function H.
		/// </param>
		/// <param name="g">Second argument of the function H.
		/// </param>
		public virtual double evalH(double f, double g)
		{
			return 1.0 - System.Math.Sqrt(f / g);
		} // evalH      
	} // ZDT4
}
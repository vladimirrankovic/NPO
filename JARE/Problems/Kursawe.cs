/// <summary> Kursawe.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  2.0
/// </version>
using System;
using JARE.Base;
using ArrayRealSolutionType = JARE.Base.solutionType.ArrayRealSolutionType;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
using XReal = JARE.util.wrapper.XReal;
namespace JARE.problems
{
	
	/// <summary> Class representing problem Kursawe</summary>
	[Serializable]
	public class Kursawe:Problem
	{
		
		/// <summary> Constructor.
		/// Creates a default instance of the Kursawe problem.
		/// </summary>
		/// <param name="solutionType">The solution type must "Real", "BinaryReal, and "ArrayReal". 
		/// </param>
		public Kursawe(System.String solutionType):this(solutionType, 3)
		{
		} // Kursawe
		
		/// <summary> Constructor.
		/// Creates a new instance of the Kursawe problem.
		/// </summary>
		/// <param name="numberOfVariables">Number of variables of the problem 
		/// </param>
		/// <param name="solutionType">The solution type must "Real", "BinaryReal, and "ArrayReal". 
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public Kursawe(System.String solutionType, System.Int32 numberOfVariables)
		{
			m_numberOfVariables = numberOfVariables;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "Kursawe";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			
			for (int i = 0; i < m_numberOfVariables; i++)
			{
				m_lowerLimit[i] = - 5.0;
				m_upperLimit[i] = 5.0;
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
		} // Kursawe
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			XReal x = new XReal(solution);
			
			double aux, xi, xj; // auxiliar variables
			double[] fx = new double[2]; // function values     
			
			fx[0] = 0.0;
			for (int var = 0; var < m_numberOfVariables - 1; var++)
			{
				xi = x.getValue(var) * x.getValue(var);
				xj = x.getValue(var + 1) * x.getValue(var + 1);
				aux = (- 0.2) * System.Math.Sqrt(xi + xj);
				fx[0] += (- 10.0) * System.Math.Exp(aux);
			} // for
			
			fx[1] = 0.0;
			
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				fx[1] += System.Math.Pow(System.Math.Abs(x.getValue(var)), 0.8) + 5.0 * System.Math.Sin(System.Math.Pow(x.getValue(var), 3.0));
			} // for
			
			solution.setObjective(0, fx[0]);
			solution.setObjective(1, fx[1]);
		} // evaluate
	} // Kursawe
}
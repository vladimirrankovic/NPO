/// <summary> Viennet2.java
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
	
	/// <summary> Class representing problem Viennet2</summary>
	[Serializable]
	public class Viennet2:Problem
	{
		
		/// <summary> Constructor.
		/// Creates a default instance of the Viennet2 problem
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public Viennet2(System.String solutionType)
		{
			m_numberOfVariables = 2;
			m_numberOfObjectives = 3;
			m_numberOfConstraints = 0;
			m_problemName = "Viennet4";
			
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
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} //Viennet2
		
		
		/// <summary> Evaluates a solution</summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			double[] x = new double[m_numberOfVariables];
			double[] f = new double[m_numberOfObjectives];
			
			for (int i = 0; i < m_numberOfVariables; i++)
				x[i] = solution.DecisionVariables[i].getValue();
			
			// First function
			f[0] = (x[0] - 2) * (x[0] - 2) / 2.0 + (x[1] + 1) * (x[1] + 1) / 13.0 + 3.0;
			
			// Second function
			f[1] = (x[0] + x[1] - 3.0) * (x[0] + x[1] - 3.0) / 36.0 + (- x[0] + x[1] + 2.0) * (- x[0] + x[1] + 2.0) / 8.0 - 17.0;
			
			// Third function
			f[2] = (x[0] + 2 * x[1] - 1) * (x[0] + 2 * x[1] - 1) / 175.0 + (2 * x[1] - x[0]) * (2 * x[1] - x[0]) / 17.0 - 13.0;
			
			for (int i = 0; i < m_numberOfObjectives; i++)
				solution.setObjective(i, f[i]);
		} // evaluate 
	} // Viennet2
}
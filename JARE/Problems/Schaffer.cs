/// <summary> Schaffer.java
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
	
	/// <summary> Class representing problem Schaffer</summary>
	[Serializable]
	public class Schaffer:Problem
	{
		
		/// <summary> Constructor.
		/// Creates a default instance of problem Schaffer
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".s 
		/// </param>
		public Schaffer(System.String solutionType)
		{
			m_numberOfVariables = 1;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "Schaffer";
			
			m_lowerLimit = new double[m_numberOfVariables];
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit[0] = - 50000;
			m_upperLimit[0] = 50000;
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} //Schaffer
		
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] variable = solution.DecisionVariables;
			
			double[] f = new double[m_numberOfObjectives];
			f[0] = variable[0].getValue() * variable[0].getValue();
			
			f[1] = (variable[0].getValue() - 2.0) * (variable[0].getValue() - 2.0);
			
			solution.setObjective(0, f[0]);
			solution.setObjective(1, f[1]);
		} //evaluate    
	} //Schaffer
}
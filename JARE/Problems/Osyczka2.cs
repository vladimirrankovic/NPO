/// <summary> Osyczka2.java
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
	
	/// <summary> Class representing problem Oyczka2</summary>
	[Serializable]
	public class Osyczka2:Problem
	{
		/// <summary> Constructor.
		/// Creates a default instance of the Osyczka2 problem.
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		public Osyczka2(System.String solutionType)
		{
			m_numberOfVariables = 6;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 6;
			m_problemName = "Osyczka2";
			
			m_lowerLimit = new double[m_numberOfVariables];
			m_upperLimit = new double[m_numberOfVariables];
			//Fill lower and upper limits
			m_lowerLimit[0] = 0.0;
			m_lowerLimit[1] = 0.0;
			m_lowerLimit[2] = 1.0;
			m_lowerLimit[3] = 0.0;
			m_lowerLimit[4] = 1.0;
			m_lowerLimit[5] = 0.0;
			
			m_upperLimit[0] = 10.0;
			m_upperLimit[1] = 10.0;
			m_upperLimit[2] = 5.0;
			m_upperLimit[3] = 6.0;
			m_upperLimit[4] = 5.0;
			m_upperLimit[5] = 10.0;
			//
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} // Osyczka2
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] decisionVariables = solution.DecisionVariables;
			
			double[] f = new double[m_numberOfObjectives];
			
			double x1, x2, x3, x4, x5, x6;
			x1 = decisionVariables[0].getValue();
			x2 = decisionVariables[1].getValue();
			x3 = decisionVariables[2].getValue();
			x4 = decisionVariables[3].getValue();
			x5 = decisionVariables[4].getValue();
			x6 = decisionVariables[5].getValue();
			f[0] = - (25.0 * (x1 - 2.0) * (x1 - 2.0) + (x2 - 2.0) * (x2 - 2.0) + (x3 - 1.0) * (x3 - 1.0) + (x4 - 4.0) * (x4 - 4.0) + (x5 - 1.0) * (x5 - 1.0));
			
			f[1] = x1 * x1 + x2 * x2 + x3 * x3 + x4 * x4 + x5 * x5 + x6 * x6;
			
			solution.setObjective(0, f[0]);
			solution.setObjective(1, f[1]);
		} // evaluate
		
		/// <summary> Evaluates the constraint overhead of a solution </summary>
		/// <param name="solution">The solution
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluateConstraints(Solution solution)
		{
			double[] constraint = new double[this.NumberOfConstraints];
			Variable[] decisionVariables = solution.DecisionVariables;
			
			double x1, x2, x3, x4, x5, x6;
			x1 = decisionVariables[0].getValue();
			x2 = decisionVariables[1].getValue();
			x3 = decisionVariables[2].getValue();
			x4 = decisionVariables[3].getValue();
			x5 = decisionVariables[4].getValue();
			x6 = decisionVariables[5].getValue();
			
			constraint[0] = (x1 + x2) / 2.0 - 1.0;
			constraint[1] = (6.0 - x1 - x2) / 6.0;
			constraint[2] = (2.0 - x2 + x1) / 2.0;
			constraint[3] = (2.0 - x1 + 3.0 * x2) / 2.0;
			constraint[4] = (4.0 - (x3 - 3.0) * (x3 - 3.0) - x4) / 4.0;
			constraint[5] = ((x5 - 3.0) * (x5 - 3.0) + x6 - 4.0) / 4.0;
			
			double total = 0.0;
			int number = 0;
			for (int i = 0; i < this.NumberOfConstraints; i++)
				if (constraint[i] < 0.0)
				{
					total += constraint[i];
					number++;
				}
			
			solution.OverallConstraintViolation = total;
			solution.NumberOfViolatedConstraint = number;
		} // evaluateConstraints 
	} // Osyczka2
}
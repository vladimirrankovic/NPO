/// <summary> Srinivas.java
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
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
namespace JARE.problems
{
	
	/// <summary> Class representing problem Srinivas</summary>
	[Serializable]
	public class Srinivas:Problem
	{
		
		/// <summary> Constructor.
		/// Creates a default instance of the Srinivas problem
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public Srinivas(System.String solutionType)
		{
			m_numberOfVariables = 2;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 2;
			m_problemName = "Srinivas";
			
			m_lowerLimit = new double[m_numberOfVariables];
			m_upperLimit = new double[m_numberOfVariables];
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = - 20.0;
				m_upperLimit[var] = 20.0;
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
		} //Srinivas
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] variable = solution.DecisionVariables;
			
			double[] f = new double[m_numberOfObjectives];
			
			double x1 = variable[0].getValue();
			double x2 = variable[1].getValue();
			f[0] = 2.0 + (x1 - 2.0) * (x1 - 2.0) + (x2 - 1.0) * (x2 - 1.0);
			f[1] = 9.0 * x1 - (x2 - 1.0) * (x2 - 1.0);
			
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
			
			double x1 = solution.DecisionVariables[0].getValue();
			double x2 = solution.DecisionVariables[1].getValue();
			
			constraint[0] = 1.0 - (x1 * x1 + x2 * x2) / 225.0;
			constraint[1] = (3.0 * x2 - x1) / 10.0 - 1.0;
			
			double total = 0.0;
			int number = 0;
			for (int i = 0; i < this.NumberOfConstraints; i++)
				if (constraint[i] < 0.0)
				{
					number++;
					total += constraint[i];
				}
			
			solution.OverallConstraintViolation = total;
			solution.NumberOfViolatedConstraint = number;
		} // evaluateConstraints
	} // Srinivas
}
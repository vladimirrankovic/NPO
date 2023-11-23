/// <summary> Tanaka.java
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
	
	/// <summary> Class representing problem Tanaka</summary>
	[Serializable]
	public class Tanaka:Problem
	{
		
		/// <summary> Constructor.
		/// Creates a default instance of the problem Tanaka
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		public Tanaka(System.String solutionType)
		{
			m_numberOfVariables = 2;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 2;
			m_problemName = "Tanaka";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = 10e-5;
				m_upperLimit[var] = System.Math.PI;
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
		}
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] variable = solution.DecisionVariables;
			
			double[] f = new double[m_numberOfObjectives];
			f[0] = variable[0].getValue();
			f[1] = variable[1].getValue();
			
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
			
			constraint[0] = (x1 * x1 + x2 * x2 - 1.0 - 0.1 * System.Math.Cos(16.0 * System.Math.Atan(x1 / x2)));
			constraint[1] = (- 2.0) * ((x1 - 0.5) * (x1 - 0.5) + (x2 - 0.5) * (x2 - 0.5) - 0.5);
			
			int number = 0;
			double total = 0.0;
			for (int i = 0; i < this.NumberOfConstraints; i++)
				if (constraint[i] < 0.0)
				{
					number++;
					total += constraint[i];
				}
			
			solution.OverallConstraintViolation = total;
			solution.NumberOfViolatedConstraint = number;
		} // evaluateConstraints   
	} // Tanaka
}
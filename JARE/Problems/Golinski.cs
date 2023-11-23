/// <summary> Golinski.java
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
	
	/// <summary> Class representing problem Golinski.</summary>
	[Serializable]
	public class Golinski:Problem
	{
		
		// defining lowerLimits and upperLimits for the problem
		//UPGRADE_NOTE: Final was removed from the declaration of 'LOWERLIMIT'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly double[] LOWERLIMIT = new double[]{2.6, 0.7, 17.0, 7.3, 7.3, 2.9, 5.0};
		//UPGRADE_NOTE: Final was removed from the declaration of 'UPPERLIMIT'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly double[] UPPERLIMIT = new double[]{3.6, 0.8, 28.0, 8.3, 8.3, 3.9, 5.5};
		
		/// <summary> Constructor.
		/// Creates a defalut instance of the Golinski problem.
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		public Golinski(System.String solutionType)
		{
			m_numberOfVariables = 7;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 11;
			m_problemName = "Golinski";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = LOWERLIMIT[var];
				m_upperLimit[var] = UPPERLIMIT[var];
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
		} //Golinski
		
		/// <summary> Evaluates a solution.</summary>
		/// <param name="solution">The solution to evaluate.
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			double x1, x2, x3, x4, x5, x6, x7;
			x1 = solution.DecisionVariables[0].getValue();
			x2 = solution.DecisionVariables[1].getValue();
			x3 = solution.DecisionVariables[2].getValue();
			x4 = solution.DecisionVariables[3].getValue();
			x5 = solution.DecisionVariables[4].getValue();
			x6 = solution.DecisionVariables[5].getValue();
			x7 = solution.DecisionVariables[6].getValue();
			
			double f1 = 0.7854 * x1 * x2 * x2 * ((10 * x3 * x3) / 3.0 + 14.933 * x3 - 43.0934) - 1.508 * x1 * (x6 * x6 + x7 * x7) + 7.477 * (x6 * x6 * x6 + x7 * x7 * x7) + 0.7854 * (x4 * x6 * x6 + x5 * x7 * x7);
			
			double aux = 745.0 * x4 / (x2 * x3);
			double f2 = System.Math.Sqrt((aux * aux) + 1.69e7) / (0.1 * x6 * x6 * x6);
			
			solution.setObjective(0, f1);
			solution.setObjective(1, f2);
		} // evaluate
		
		/// <summary> Evaluates the constraint overhead of a solution </summary>
		/// <param name="solution">The solution
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluateConstraints(Solution solution)
		{
			double[] constraint = new double[m_numberOfConstraints];
			double x1, x2, x3, x4, x5, x6, x7;
			
			x1 = solution.DecisionVariables[0].getValue();
			x2 = solution.DecisionVariables[1].getValue();
			x3 = solution.DecisionVariables[2].getValue();
			x4 = solution.DecisionVariables[3].getValue();
			x5 = solution.DecisionVariables[4].getValue();
			x6 = solution.DecisionVariables[5].getValue();
			x7 = solution.DecisionVariables[6].getValue();
			
			
			constraint[0] = - ((1.0 / (x1 * x2 * x2 * x3)) - (1.0 / 27.0));
			constraint[1] = - ((1.0 / (x1 * x2 * x2 * x3 * x3)) - (1.0 / 397.5));
			constraint[2] = - ((x4 * x4 * x4) / (x2 * x3 * x3 * x6 * x6 * x6 * x6) - (1.0 / 1.93));
			constraint[3] = - ((x5 * x5 * x5) / (x2 * x3 * x7 * x7 * x7 * x7) - (1.0 / 1.93));
			constraint[4] = - (x2 * x3 - 40.0);
			constraint[5] = - ((x1 / x2) - 12.0);
			constraint[6] = - (5.0 - (x1 / x2));
			constraint[7] = - (1.9 - x4 + 1.5 * x6);
			constraint[8] = - (1.9 - x5 + 1.1 * x7);
			
			double aux = 745.0 * x4 / (x2 * x3);
			double f2 = System.Math.Sqrt((aux * aux) + 1.69e7) / (0.1 * x6 * x6 * x6);
			constraint[9] = - (f2 - 1300);
			double a = 745.0 * x5 / (x2 * x3);
			double b = 1.575e8;
			constraint[10] = - (System.Math.Sqrt(a * a + b) / (0.1 * x7 * x7 * x7) - 1100.0);
			
			double total = 0.0;
			int number = 0;
			for (int i = 0; i < m_numberOfConstraints; i++)
			{
				if (constraint[i] < 0.0)
				{
					total += constraint[i];
					number++;
				}
			}
			
			solution.OverallConstraintViolation = total;
			solution.NumberOfViolatedConstraint = number;
		} // evaluateConstraints
	} // Golinski
}
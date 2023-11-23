/// <summary> ConstrEx.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using ArrayRealSolutionType = JARE.Base.solutionType.ArrayRealSolutionType;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using JARE.Base;
using SMException = JARE.util.SMException;
namespace JARE.problems
{
	
	/// <summary> Class representing problem Constr_Ex</summary>
	[Serializable]
	public class ConstrEx:Problem
	{
		/// <summary> Constructor
		/// Creates a default instance of the Constr_Ex problem
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public ConstrEx(System.String solutionType)
		{
			m_numberOfVariables = 2;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 2;
			m_problemName = "Constr_Ex";
			
			m_lowerLimit = new double[m_numberOfVariables];
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit[0] = 0.1;
			m_lowerLimit[1] = 0.0;
			m_upperLimit[0] = 1.0;
			m_upperLimit[1] = 5.0;
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} // ConstrEx
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] variable = solution.DecisionVariables;
			
			double[] f = new double[m_numberOfObjectives];
			f[0] = variable[0].getValue();
			f[1] = (1.0 + variable[1].getValue()) / variable[0].getValue();
			
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
			
			constraint[0] = (x2 + 9 * x1 - 6.0);
			constraint[1] = (- x2 + 9 * x1 - 1.0);
			
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
	} // ConstrEx
}
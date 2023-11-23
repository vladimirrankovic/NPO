/// <summary> Sphere.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using Problem = JARE.Base.Problem;
using Solution = JARE.Base.Solution;
using Variable = JARE.Base.Variable;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
namespace JARE.problems.singleObjective
{
	
	[Serializable]
	public class Sphere:Problem
	{
		/// <summary> Constructor
		/// Creates a default instance of the Sphere problem
		/// </summary>
		/// <param name="numberOfVariables">Number of variables of the problem 
		/// </param>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public Sphere(System.String solutionType, System.Int32 numberOfVariables)
		{
			m_numberOfVariables = numberOfVariables;
			m_numberOfObjectives = 1;
			m_numberOfConstraints = 0;
			m_problemName = "Sphere";
			
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit = new double[m_numberOfVariables];
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = - 5.12;
				m_upperLimit[var] = 5.12;
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
		} // Sphere
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] decisionVariables = solution.DecisionVariables;
			
			double sum = 0.0;
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				sum += System.Math.Pow(decisionVariables[var].getValue(), 2.0);
			}
			solution.setObjective(0, sum);
		} // evaluate
	} // Sphere
}
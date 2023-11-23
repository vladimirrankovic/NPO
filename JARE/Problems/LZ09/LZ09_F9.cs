/// <summary> LZ09_F9.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// Created on 17 de junio de 2006, 17:30
/// </version>
using System;
using JARE.Base;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
namespace JARE.problems.LZ09
{
	
	/// <summary> Class representing problem LZ09_F9 </summary>
	[Serializable]
	public class LZ09_F9:Problem
	{
		internal LZ09 m_LZ09;
		/// <summary> Creates a default LZ09_F9 problem (30 variables and 2 objectives)</summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		public LZ09_F9(System.String solutionType):this(solutionType, 22, 1, 22)
		{
		} // LZ09_F9
		
		/// <summary> Creates a DTLZ1 problem instance</summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public LZ09_F9(System.String solutionType, System.Int32 ptype, System.Int32 dtype, System.Int32 ltype)
		{
			m_numberOfVariables = 30;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "LZ09_F9";
			
			m_LZ09 = new LZ09(m_numberOfVariables, m_numberOfObjectives, ptype, dtype, ltype);
			
			m_lowerLimit = new double[m_numberOfVariables];
			m_upperLimit = new double[m_numberOfVariables];
			m_lowerLimit[0] = 0.0;
			m_upperLimit[0] = 1.0;
			for (int var = 1; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = - 1.0;
				m_upperLimit[var] = 1.0;
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
		} // LZ09_F9
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
		public override void  evaluate(Solution solution)
		{
			Variable[] gen = solution.DecisionVariables;
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
            //Vector < Double > x = new Vector < Double >(m_numberOfVariables);
            System.Collections.Generic.List<double> x = new System.Collections.Generic.List<double>(m_numberOfVariables);
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
            //Vector < Double > y = new Vector < Double >(m_numberOfObjectives);
            System.Collections.Generic.List<double> y = new System.Collections.Generic.List<double>(m_numberOfObjectives);
			
			for (int i = 0; i < m_numberOfVariables; i++)
			{
				x.Add(gen[i].getValue());
				y.Add(0.0);
			} // for
			
			m_LZ09.objective(x, y);
			
			for (int i = 0; i < m_numberOfObjectives; i++)
				solution.setObjective(i, (double)y[i]);
		} // evaluate
	} // LZ09_F9
}
/// <summary> ZDT5.java
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
using BinarySolutionType = JARE.Base.solutionType.BinarySolutionType;
using JARE.Base.variable;
namespace JARE.problems.ZDT
{
	
	/// <summary> Class representing problem ZDT5</summary>
	[Serializable]
	public class ZDT5:Problem
	{
		
		/// <summary> Creates a default instance of problem ZDT5 (11 decision variables).
		/// This problem allows only "Binary" representations.
		/// </summary>
		public ZDT5():this(11)
		{ // 11 variables by default
		} // ZDT5
		
		/// <summary> Creates a instance of problem ZDT5</summary>
		/// <param name="numberOfVariables">Number of variables.
		/// This problem allows only "Binary" representations.
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public ZDT5(System.Int32 numberOfVariables)
		{
			m_numberOfVariables = numberOfVariables;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "ZDT5";
			
			m_length = new int[m_numberOfVariables];
			m_length[0] = 30;
			for (int var = 1; var < m_numberOfVariables; var++)
			{
				m_length[var] = 5;
			}
			
			m_solutionType = new BinarySolutionType(this);
			
			// All the variables of this problem are Binary
			m_variableType = new System.Type[m_numberOfVariables];
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				m_variableType[var] = System.Type.GetType("JARE.Base.variable.Binary");
			} // for
		} //ZDT5
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		public override void  evaluate(Solution solution)
		{
			double[] f = new double[m_numberOfObjectives];
			f[0] = 1 + u((Binary) solution.DecisionVariables[0]);
			double g = evalG(solution.DecisionVariables);
			double h = evalH(f[0], g);
			f[1] = h * g;
			
			solution.setObjective(0, f[0]);
			solution.setObjective(1, f[1]);
		} //evaluate
		
		/// <summary> Returns the value of the ZDT5 function G.</summary>
		/// <param name="decisionVariables">The decision variables of the solution to 
		/// evaluate.
		/// </param>
		public virtual double evalG(Variable[] decisionVariables)
		{
			double res = 0.0;
			for (int var = 1; var < m_numberOfVariables; var++)
			{
				res += evalV(u((Binary) decisionVariables[var]));
			}
			
			return res;
		} // evalG
		
		/// <summary> Returns the value of the ZDT5 function V.</summary>
		/// <param name="value">The parameter of V function.
		/// </param>
		public virtual double evalV(double value)
		{
			if (value < 5.0)
			{
				return 2.0 + value;
			}
			else
			{
				return 1.0;
			}
		} // evalV
		
		/// <summary> Returns the value of the ZDT5 function H.</summary>
		/// <param name="f">First argument of the function H.
		/// </param>
		/// <param name="g">Second argument of the function H.
		/// </param>
		public virtual double evalH(double f, double g)
		{
			return 1 / f;
		} // evalH
		
		/// <summary> Returns the u value defined in ZDT5 for a variable.</summary>
		/// <param name="The">variable.
		/// </param>
		private double u(Binary variable)
		{
			return variable.cardinality();
		} // u
		
		/// <summary> Returns the precision of the variable var</summary>
		/// <param name="var">The position of the variable
		/// </param>
		public override int getPrecision(int var)
		{
			return m_precision[var];
		} // getPrecision
	} // ZDT5
}
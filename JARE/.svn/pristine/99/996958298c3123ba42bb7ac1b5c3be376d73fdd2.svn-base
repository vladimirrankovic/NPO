/// <summary> OneMax.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using BinarySolutionType = JARE.Base.solutionType.BinarySolutionType;
using Binary = JARE.Base.variable.Binary;
namespace JARE.problems.singleObjective
{
	
	/// <summary> Class representing problem OneMax. The problem consist of maximizing the
	/// number of '1's in a binary string.
	/// </summary>
	[Serializable]
	public class OneMax:Problem
	{
		
		
		/// <summary> Creates a new OneMax problem instance</summary>
		/// <param name="numberOfBits">Length of the problem
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public OneMax(System.Int32 numberOfBits)
		{
			m_numberOfVariables = 1;
			m_numberOfObjectives = 1;
			m_numberOfConstraints = 0;
			m_problemName = "ONEMAX";
			
			m_solutionType = new BinarySolutionType(this);
			
			m_variableType = new System.Type[m_numberOfVariables];
			m_length = new int[m_numberOfVariables];
			
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			m_variableType[0] = System.Type.GetType("JARE.Base.variable.Binary");
			m_length[0] = numberOfBits;
		} // OneMax
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		public override void  evaluate(Solution solution)
		{
			Binary variable;
			int counter;
			
			variable = ((Binary) solution.DecisionVariables[0]);
			
			counter = 0;
			
			for (int i = 0; i < variable.NumberOfBits; i++)
				if (variable.m_bits.Get(i) == true)
					counter++;
			
			// OneMax is a maximization problem: multiply by -1 to minimize
			solution.setObjective(0, (- 1.0) * counter);
		} // evaluate
	} // OneMax
}
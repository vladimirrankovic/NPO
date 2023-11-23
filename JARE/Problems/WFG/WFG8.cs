/// <summary> WFG8.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
using Variable = JARE.Base.Variable;
using SMException = JARE.util.SMException;
namespace JARE.problems.WFG
{
	
	/// <summary> Creates a default WFG8 problem with 
	/// 2 position-related parameters, 
	/// 4 distance-related parameters,
	/// and 2 objectives
	/// </summary>
	/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
	/// </param>
	public class WFG8:WFG
	{
		
		/// <summary> Creates a default WFG8 with 
		/// 2 position-related parameters, 
		/// 4 distance-related parameters,
		/// and 2 objectives
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public WFG8(System.String solutionType):this(solutionType, 2, 4, 2)
		{
		} // WFG8
		
		/// <summary> Creates a WFG8 problem instance</summary>
		/// <param name="k">Number of position parameters
		/// </param>
		/// <param name="l">Number of distance parameters
		/// </param>
		/// <param name="M">Number of objective functions
		/// </param>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public WFG8(System.String solutionType, System.Int32 k, System.Int32 l, System.Int32 M):base(solutionType, k, l, M)
		{
			m_problemName = "WFG8";
			
			m_S = new int[m_M];
			for (int i = 0; i < m_M; i++)
				m_S[i] = 2 * (i + 1);
			
			m_A = new int[m_M - 1];
			for (int i = 0; i < m_M - 1; i++)
				m_A[i] = 1;
		} // WFG8           
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="z">The solution to evaluate
		/// </param>
		/// <returns> double [] with the evaluation results
		/// </returns>
        public override float[] evaluate(float[] z)
		{
			float[] y;
			
			y = normalise(z);
			y = t1(y, m_k);
			y = t2(y, m_k);
			y = t3(y, m_k, m_M);
			
			float[] result = new float[m_M];
			float[] x = calculate_x(y);
			for (int m = 1; m <= m_M; m++)
			{
				result[m - 1] = m_D * x[m_M - 1] + m_S[m - 1] * (new Shapes()).concave(x, m);
			}
			
			return result;
		} // evaluate
		
		
		/// <summary> WFG8 t1 transformation</summary>
		public virtual float[] t1(float[] z, int k)
		{
			float[] result = new float[z.Length];
			float[] w = new float[z.Length];
			
			for (int i = 0; i < w.Length; i++)
			{
				w[i] = (float) 1.0;
			}
			
			for (int i = 0; i < k; i++)
			{
				result[i] = z[i];
			}
			
			for (int i = k; i < z.Length; i++)
			{
				int head = 0;
				int tail = i - 1;
				float[] subZ = subVector(z, head, tail);
				float[] subW = subVector(w, head, tail);
				float aux = (new Transformations()).r_sum(subZ, subW);
				
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				result[i] = (new Transformations()).b_param(z[i], aux, (float) 0.98 / (float) 49.98, (float) 0.02, 50);
			}
			
			return result;
		} // t1
		
		/// <summary> WFG8 t2 transformation</summary>
		public virtual float[] t2(float[] z, int k)
		{
			float[] result = new float[z.Length];
			
			for (int i = 0; i < k; i++)
			{
				result[i] = z[i];
			}
			
			for (int i = k; i < z.Length; i++)
			{
				result[i] = (new Transformations()).s_linear(z[i], (float) 0.35);
			}
			
			return result;
		} // t2
		
		/// <summary> WFG8 t3 transformation</summary>
		public virtual float[] t3(float[] z, int k, int M)
		{
			float[] result = new float[M];
			float[] w = new float[z.Length];
			
			for (int i = 0; i < z.Length; i++)
			{
				w[i] = (float) 1.0;
			}
			
			for (int i = 1; i <= M - 1; i++)
			{
				int head = (i - 1) * k / (M - 1) + 1;
				int tail = i * k / (M - 1);
				float[] subZ = subVector(z, head - 1, tail - 1);
				float[] subW = subVector(w, head - 1, tail - 1);
				
				result[i - 1] = (new Transformations()).r_sum(subZ, subW);
			}
			
			int head2 = k + 1;
			int tail2 = z.Length;
			float[] subZ2 = subVector(z, head2 - 1, tail2 - 1);
			float[] subW2 = subVector(w, head2 - 1, tail2 - 1);
			result[M - 1] = (new Transformations()).r_sum(subZ2, subW2);
			
			return result;
		} // t3
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
        public override void evaluate(Solution solution)
		{
			float[] variables = new float[NumberOfVariables];
			Variable[] dv = solution.DecisionVariables;

            for (int i = 0; i < NumberOfVariables; i++)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				variables[i] = (float) dv[i].getValue();
			}
			
			float[] sol = evaluate(variables);
			
			for (int i = 0; i < sol.Length; i++)
			{
				solution.setObjective(i, sol[i]);
			}
		} // evaluate
	} // WFG8
}
/// <summary> WFG1.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
using Variable = JARE.Base.Variable;
using SMException = JARE.util.SMException;
using SupportClass = JARE.support.SupportClass;
namespace JARE.problems.WFG
{
	
	/// <summary> This class implements the WFG1 problem
	/// Reference: Simon Huband, Luigi Barone, Lyndon While, Phil Hingston
	/// A Scalable Multi-objective Test Problem Toolkit.
	/// Evolutionary Multi-Criterion Optimization: 
	/// Third International Conference, EMO 2005. 
	/// Proceedings, volume 3410 of Lecture Notes in Computer Science
	/// </summary>
	public class WFG1:WFG
	{
		
		/// <summary> Constructor
		/// Creates a default WFG1 instance with 
		/// 2 position-related parameters
		/// 4 distance-related parameters 
		/// and 2 objectives
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public WFG1(System.String solutionType):this(solutionType, 2, 4, 2)
		{
		} // WFG1
		
		/// <summary> Creates a WFG1 problem instance</summary>
		/// <param name="k">Number of position parameters
		/// </param>
		/// <param name="l">Number of distance parameters
		/// </param>
		/// <param name="M">Number of objective functions
		/// </param>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public WFG1(System.String solutionType, System.Int32 k, System.Int32 l, System.Int32 M):base(solutionType, k, l, M)
		{
			m_problemName = "WFG1";
			
			m_S = new int[m_M];
			for (int i = 0; i < m_M; i++)
				m_S[i] = 2 * (i + 1);
			
			m_A = new int[m_M - 1];
			for (int i = 0; i < m_M - 1; i++)
				m_A[i] = 1;
		} // WFG1
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="z">The solution to evaluate
		/// </param>
		/// <returns> a double [] with the evaluation results
		/// </returns>
        public override float[] evaluate(float[] z)
		{
			float[] y;
			
			y = normalise(z);
			y = t1(y, m_k);
			y = t2(y, m_k);
			try
			{
				y = t3(y);
			}
			catch (SMException e)
			{
				// TODO Auto-generated catch block
				SupportClass.WriteStackTrace(e, Console.Error);
			}
			y = t4(y, m_k, m_M);
			
			
			float[] result = new float[m_M];
			float[] x = calculate_x(y);
			for (int m = 1; m <= m_M - 1; m++)
			{
				result[m - 1] = m_D * x[m_M - 1] + m_S[m - 1] * (new Shapes()).convex(x, m);
			}
			
			result[m_M - 1] = m_D * x[m_M - 1] + m_S[m_M - 1] * (new Shapes()).mixed(x, 5, (float) 1.0);
			
			return result;
		} // evaluate
		
		/// <summary> WFG1 t1 transformation</summary>
		public virtual float[] t1(float[] z, int k)
		{
			float[] result = new float[z.Length];
			
			for (int i = 0; i < k; i++)
			{
				result[i] = z[i];
			}
			
			for (int i = k; i < z.Length; i++)
			{
				result[i] = (float) (new Transformations()).s_linear(z[i], (float) 0.35);
			}
			
			return result;
		} // t1
		
		/// <summary> WFG1 t2 transformation</summary>
		public virtual float[] t2(float[] z, int k)
		{
			float[] result = new float[z.Length];
			
			for (int i = 0; i < k; i++)
			{
				result[i] = z[i];
			}
			
			for (int i = k; i < z.Length; i++)
			{
				result[i] = (float) (new Transformations()).b_flat(z[i], (float) 0.8, (float) 0.75, (float) 0.85);
			}
			
			return result;
		} // t2
		
		/// <summary> WFG1 t3 transformation</summary>
		/// <throws>  SMException  </throws>
		public virtual float[] t3(float[] z)
		{
			float[] result = new float[z.Length];
			
			for (int i = 0; i < z.Length; i++)
			{
				result[i] = (float) (new Transformations()).b_poly(z[i], (float) 0.02);
			}
			
			return result;
		} // t3
		
		/// <summary> WFG1 t4 transformation</summary>
		public virtual float[] t4(float[] z, int k, int M)
		{
			float[] result = new float[M];
			float[] w = new float[z.Length];
			
			for (int i = 0; i < z.Length; i++)
			{
				w[i] = (float) 2.0 * (i + 1);
			}
			
			for (int i = 1; i <= M - 1; i++)
			{
				int head = (i - 1) * k / (M - 1) + 1;
				int tail = i * k / (M - 1);
				float[] subZ = subVector(z, head - 1, tail - 1);
				float[] subW = subVector(w, head - 1, tail - 1);
				
				result[i - 1] = (float) (new Transformations()).r_sum(subZ, subW);
			}
			
			int head2 = k + 1 - 1;
			int tail2 = z.Length - 1;
			float[] subZ2 = subVector(z, head2, tail2);
			float[] subW2 = subVector(w, head2, tail2);
			result[M - 1] = (new Transformations()).r_sum(subZ2, subW2);
			
			return result;
		} // t4
		
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
			
			float[] f = evaluate(variables);
			
			for (int i = 0; i < f.Length; i++)
			{
				solution.setObjective(i, f[i]);
			}
		} //evaluate
	} // WFG1
}
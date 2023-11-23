/// <summary> WFG.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
namespace JARE.problems.WFG
{
	
	/// <summary> Implements a reference abstract class for all WFG test problems
	/// Reference: Simon Huband, Luigi Barone, Lyndon While, Phil Hingston
	/// A Scalable Multi-objective Test Problem Toolkit.
	/// Evolutionary Multi-Criterion Optimization: 
	/// Third International Conference, EMO 2005. 
	/// Proceedings, volume 3410 of Lecture Notes in Computer Science
	/// </summary>
	public abstract class WFG:Problem
	{
		
		/// <summary> stores a epsilon default value</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'epsilon '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private float epsilon = (float) 1e-7;
		
		protected internal int m_k; //Var for walking fish group
		protected internal int m_M;
		protected internal int m_l;
		protected internal int[] m_A;
		protected internal int[] m_S;
		protected internal int m_D = 1;
		protected internal System.Random random = new System.Random();
		
		/// <summary> Constructor
		/// Creates a WFG problem
		/// </summary>
		/// <param name="k">position-related parameters
		/// </param>
		/// <param name="l">distance-related parameters
		/// </param>
		/// <param name="M">Number of objectives
		/// </param>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal". 
		/// </param>
		//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
		public WFG(System.String solutionType, System.Int32 k, System.Int32 l, System.Int32 M)
		{
			this.m_k = k;
			this.m_l = l;
			this.m_M = M;
            m_numberOfVariables = this.m_k + this.m_l;
			m_numberOfObjectives = this.m_M;
			m_numberOfConstraints = 0;

            m_lowerLimit = new double[m_numberOfVariables];
            m_upperLimit = new double[m_numberOfVariables];
            for (int var = 0; var < m_numberOfVariables; var++)
			{
                m_lowerLimit[var] = 0;
                m_upperLimit[var] = 2 * (var + 1);
			}
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} // WFG
		
		/// <summary> Gets the x vector (consulte WFG tooltik reference)</summary>
		public virtual float[] calculate_x(float[] t)
		{
			float[] x = new float[m_M];
			
			for (int i = 0; i < m_M - 1; i++)
			{
				x[i] = System.Math.Max(t[m_M - 1], m_A[i]) * (t[i] - (float) 0.5) + (float) 0.5;
			}
			
			x[m_M - 1] = t[m_M - 1];
			
			return x;
		} // calculate_x
		
		/// <summary> Normalizes a vector (consulte WFG toolkit reference)</summary>
		public virtual float[] normalise(float[] z)
		{
			float[] result = new float[z.Length];
			
			for (int i = 0; i < z.Length; i++)
			{
				float bound = (float) 2.0 * (i + 1);
				result[i] = z[i] / bound;
				result[i] = correct_to_01(result[i]);
			}
			
			return result;
		} // normalize    
		
		
		
		public virtual float correct_to_01(float a)
		{
			float min = (float) 0.0;
			float max = (float) 1.0;
			
			float min_epsilon = min - epsilon;
			float max_epsilon = max + epsilon;
			
			if ((a <= min && a >= min_epsilon) || (a >= min && a <= min_epsilon))
			{
				return min;
			}
			else if ((a >= max && a <= max_epsilon) || (a <= max && a >= max_epsilon))
			{
				return max;
			}
			else
			{
				return a;
			}
		} // correct_to_01  
		
		/// <summary> Gets a subvector of a given vector
		/// (Head inclusive and tail inclusive)
		/// </summary>
		/// <param name="z">the vector
		/// </param>
		/// <returns> the subvector
		/// </returns>
		public virtual float[] subVector(float[] z, int head, int tail)
		{
			int size = tail - head + 1;
			float[] result = new float[size];
			
			for (int i = head; i <= tail; i++)
			{
				result[i - head] = z[i];
			}
			
			return result;
		} // subVector
		
		/// <summary> Evaluates a solution </summary>
		/// <param name="variables">The solution to evaluate
		/// </param>
		/// <returns> a double [] with the evaluation results
		/// </returns>
		abstract public float[] evaluate(float[] variables);
		// evaluate
	}
}
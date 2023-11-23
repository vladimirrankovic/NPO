/// <summary> Shapes.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.problems.WFG
{
	
	/// <summary> Class implementing shape functions for WFG benchmark
	/// Reference: Simon Huband, Luigi Barone, Lyndon While, Phil Hingston
	/// A Scalable Multi-objective Test Problem Toolkit.
	/// Evolutionary Multi-Criterion Optimization: 
	/// Third International Conference, EMO 2005. 
	/// Proceedings, volume 3410 of Lecture Notes in Computer Science
	/// </summary>
	public class Shapes
	{
		
		/// <summary> Calculate a linear shape</summary>
		public virtual float linear(float[] x, int m)
		{
			float result = (float) 1.0;
			int M = x.Length;
			
			for (int i = 1; i <= M - m; i++)
				result *= x[i - 1];
			
			if (m != 1)
				result *= (1 - x[M - m]);
			
			return result;
		} // linear
		
		/// <summary> Calculate a convex shape</summary>
		public virtual float convex(float[] x, int m)
		{
			float result = (float) 1.0;
			int M = x.Length;
			
			for (int i = 1; i <= M - m; i++)
				result = (float) (result * (1 - System.Math.Cos(x[i - 1] * System.Math.PI * 0.5)));
			
			if (m != 1)
				result = (float) (result * (1 - System.Math.Sin(x[M - m] * System.Math.PI * 0.5)));
			
			
			return result;
		} // convex
		
		/// <summary> Calculate a concave shape</summary>
		public virtual float concave(float[] x, int m)
		{
			float result = (float) 1.0;
			int M = x.Length;
			
			for (int i = 1; i <= M - m; i++)
				result = (float) (result * System.Math.Sin(x[i - 1] * System.Math.PI * 0.5));
			
			if (m != 1)
				result = (float) (result * System.Math.Cos(x[M - m] * System.Math.PI * 0.5));
			
			return result;
		} // concave
		
		/// <summary> Calculate a mixed shape</summary>
		public virtual float mixed(float[] x, int A, float alpha)
		{
			float tmp;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			tmp = (float) System.Math.Cos((float) 2.0 * A * (float) System.Math.PI * x[0] + (float) System.Math.PI * (float) 0.5);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			tmp = (float) (tmp / (2.0 * (float) A * System.Math.PI));
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) System.Math.Pow(((float) 1.0 - x[0] - tmp), alpha);
		} // mixed
		
		/// <summary>  Calculate a disc shape</summary>
		public virtual float disc(float[] x, int A, float alpha, float beta)
		{
			float tmp;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			tmp = (float) System.Math.Cos((float) A * System.Math.Pow(x[0], beta) * System.Math.PI);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) 1.0 - (float) System.Math.Pow(x[0], alpha) * (float) System.Math.Pow(tmp, 2.0);
		} // disc
	} // Shapes
}
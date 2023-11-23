/*
* Transformations.java
* @author Juan J. Durillo
* @version 1.0
* 
*/
using System;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
namespace JARE.problems.WFG
{
	
	/// <summary> Class implementing the basics transformations for WFG</summary>
	public class Transformations
	{
		
		/// <summary> Stores a default epsilon value </summary>
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
		private static float epsilon = (float) 1.0e-10;
		
		/// <summary> b_poly transformation</summary>
		/// <throws>  SMException  </throws>
		public virtual float b_poly(float y, float alpha)
		{
			if (!(alpha > 0))
			{
				
				Configuration.m_logger.WriteLog("WFG.Transformations.b_poly: Param alpha " + "must be > 0");
				System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.String name = cls.FullName;
				throw new SMException("Exception in " + name + ".b_poly()");
			}
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return correct_to_01((float) System.Math.Pow(y, alpha));
		} //b_poly
		
		
		/// <summary> b_flat transformation</summary>
		public virtual float b_flat(float y, float A, float B, float C)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float tmp1 = System.Math.Min((float) 0, (float) System.Math.Floor(y - B)) * A * (B - y) / B;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			float tmp2 = System.Math.Min((float) 0, (float) System.Math.Floor(C - y)) * (1 - A) * (y - C) / (1 - C);
			
			return correct_to_01(A + tmp1 - tmp2);
		} // b_flat      
		
		/// <summary> s_linear transformation</summary>
		public virtual float s_linear(float y, float A)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return correct_to_01((float) System.Math.Abs(y - A) / (float) System.Math.Abs(System.Math.Floor(A - y) + A));
		} // s_linear
		
		/// <summary> s_decept transformation</summary>
		public virtual float s_decept(float y, float A, float B, float C)
		{
			float tmp, tmp1, tmp2;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			tmp1 = (float) System.Math.Floor(y - A + B) * ((float) 1.0 - C + (A - B) / B) / (A - B);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			tmp2 = (float) System.Math.Floor(A + B - y) * ((float) 1.0 - C + ((float) 1.0 - A - B) / B) / ((float) 1.0 - A - B);
			
			tmp = System.Math.Abs(y - A) - B;
			
			return correct_to_01((float) 1 + tmp * (tmp1 + tmp2 + (float) 1.0 / B));
		} // s_decept
		
		/// <summary> s_multi transformation</summary>
		public virtual float s_multi(float y, int A, int B, float C)
		{
			float tmp1, tmp2;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			tmp1 = ((float) 4.0 * A + (float) 2.0) * (float) System.Math.PI * ((float) 0.5 - (float) System.Math.Abs(y - C) / ((float) 2.0 * ((float) System.Math.Floor(C - y) + C)));
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			tmp2 = (float) 4.0 * B * (float) System.Math.Pow((float) System.Math.Abs(y - C) / ((float) 2.0 * ((float) System.Math.Floor(C - y) + C)), (float) 2.0);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return correct_to_01(((float) 1.0 + (float) System.Math.Cos(tmp1) + tmp2) / (B + (float) 2.0));
		} //s_multi
		
		/// <summary> r_sum transformation</summary>
		public virtual float r_sum(float[] y, float[] w)
		{
			float tmp1 = (float) 0.0, tmp2 = (float) 0.0;
			for (int i = 0; i < y.Length; i++)
			{
				tmp1 += y[i] * w[i];
				tmp2 += w[i];
			}
			
			return correct_to_01(tmp1 / tmp2);
		} // r_sum  
		
		/// <summary> r_nonsep transformation</summary>
		public virtual float r_nonsep(float[] y, int A)
		{
			float tmp, denominator, numerator;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			tmp = (float) System.Math.Ceiling(A / (float) 2.0);
			denominator = y.Length * tmp * ((float) 1.0 + (float) 2.0 * A - (float) 2.0 * tmp) / A;
			numerator = (float) 0.0;
			for (int j = 0; j < y.Length; j++)
			{
				numerator += y[j];
				for (int k = 0; k <= A - 2; k++)
				{
					numerator += System.Math.Abs(y[j] - y[(j + k + 1) % y.Length]);
				}
			}
			
			return correct_to_01(numerator / denominator);
		} // r_nonsep
		
		/// <summary> b_param transformation</summary>
		public virtual float b_param(float y, float u, float A, float B, float C)
		{
			float result, v, exp;
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			v = A - ((float) 1.0 - (float) 2.0 * u) * (float) System.Math.Abs((float) System.Math.Floor((float) 0.5 - u) + A);
			exp = B + (C - B) * v;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			result = (float) System.Math.Pow(y, exp);
			
			return correct_to_01(result);
		} // b_param
		
		
		internal virtual float correct_to_01(float a)
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
	} // Transformations
}
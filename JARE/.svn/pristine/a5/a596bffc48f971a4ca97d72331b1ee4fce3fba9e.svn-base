/// <summary> Statistics.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juan J. Durillo
/// 
/// </author>
/// <version>  1.1
/// </version>
using System;
namespace JARE.experiments.util
{
	
	/*
	* This class provides some methods for computing statitics
	*/
	public class Statistics
	{
		
		/// <summary> Calculates the median of a vector considering the positions indicated by
		/// the parameters first and last
		/// </summary>
		/// <param name="vector">
		/// </param>
		/// <param name="first">index of first position to consider in the vector
		/// </param>
		/// <param name="last">index of last position to consider in the vector
		/// </param>
		/// <returns> The median
		/// </returns>
		public static System.Double calculateMedian(System.Collections.ArrayList vector, int first, int last)
		{
			double median = 0.0;
			
			int size = last - first + 1;
			// System.out.println("size: " + size + "first: " + first + " last:  " + last) ;
			
			if (size % 2 != 0)
			{
				median = (System.Double) vector[first + size / 2];
			}
			else
			{
				median = ((System.Double) vector[first + size / 2 - 1] + (System.Double) vector[first + size / 2]) / 2.0;
			}
			
			return median;
		} // calculatemedian
		
		/// <summary> Calculates the interquartile range (IQR) of a vector of Doubles</summary>
		/// <param name="vector">
		/// </param>
		/// <returns> The IQR
		/// </returns>
		public static System.Double calculateIQR(System.Collections.ArrayList vector)
		{
			double q3 = 0.0;
			double q1 = 0.0;
			
			if (vector.Count > 1)
			{
				// == 1 implies IQR = 0
				if (vector.Count % 2 != 0)
				{
					q3 = calculateMedian(vector, vector.Count / 2 + 1, vector.Count - 1);
					q1 = calculateMedian(vector, 0, vector.Count / 2 - 1);
					//System.out.println("Q1: [" + 0 + ", " + (vector.size()/2 - 1) + "] = " + q1) ;
					//System.out.println("Q3: [" + (vector.size()/2+1) + ", " + (vector.size()-1) + "]= " + q3) ;
				}
				else
				{
					q3 = calculateMedian(vector, vector.Count / 2, vector.Count - 1);
					q1 = calculateMedian(vector, 0, vector.Count / 2 - 1);
					//System.out.println("Q1: [" + 0 + ", " + (vector.size()/2 - 1) + "] = " + q1) ;
					//System.out.println("Q3: [" + (vector.size()/2) + ", " + (vector.size()-1) + "]= " + q3) ;
				} // else
			} // if
			
			return q3 - q1;
		} // calculateIQR
	}
}
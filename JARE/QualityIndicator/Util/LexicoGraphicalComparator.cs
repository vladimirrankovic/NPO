/// <summary> LexicoGraphicalComparator.java
/// 
/// </summary>
/// <author>  Juan J. durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
namespace JARE.qualityIndicator.util
{
	
	
	/// <summary> This class implements the <code>Comparator</code> interface. It is used
	/// to compare points given as <code>double</code>.
	/// The order used is the lexicograhphical.
	/// </summary>
	public class LexicoGraphicalComparator : System.Collections.IComparer
	{
		
		/// <summary> The compare method compare the objects o1 and o2.</summary>
		/// <param name="o1">An object that reference a double[]
		/// </param>
		/// <param name="o2">An object that reference a double[]
		/// </param>
		/// <returns> -1 if o1 < o1, 1 if o1 > o2 or 0 in other case.
		/// </returns>
		public virtual int Compare(System.Object o1, System.Object o2)
		{
			//Cast to double [] o1 and o2.
			double[] pointOne = (double[]) o1;
			double[] pointTwo = (double[]) o2;
			
			//To determine the first i, that pointOne[i] != pointTwo[i];
			int index = 0;
            
			while ((index < pointOne.Length) && (index < pointTwo.Length) && pointOne[index] == pointTwo[index])
			{
				index++;
			}
			if ((index >= pointOne.Length) || (index >= pointTwo.Length))
			{
				return 0;
			}
			else if (pointOne[index] < pointTwo[index])
			{
				return - 1;
			}
			else if (pointOne[index] > pointTwo[index])
			{
				return 1;
			}
			return 0;
		} 
	} 
}
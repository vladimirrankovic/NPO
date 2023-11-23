/// <summary> ValueComparator.java
/// 
/// </summary>
/// <author>  Juan j.durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
namespace JARE.qualityIndicator.util
{
	
	/// <summary> This class implemnents the <code>Comparator</code> interface. It is used
	/// to compare points given as <code>double</code>. The points are compared
	/// taken account the value of a index
	/// </summary>
	public class ValueComparator : System.Collections.IComparer
	{
		
		/// <summary> Stores the value of the index to compare</summary>
		private int m_index;
		
		/// <summary>  Constructor. 
		/// Creates a new instance of ValueComparator 
		/// </summary>
		public ValueComparator(int index)
		{
			m_index = index;
		}
		
		/// <summary> Compares the objects o1 and o2.</summary>
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
			
			if (pointOne[m_index] < pointTwo[m_index])
			{
				return - 1;
			}
			else if (pointOne[m_index] > pointTwo[m_index])
			{
				return 1;
			}
			else
			{
				return 0;
			}
		} // compare
	} // ValueComparator
}
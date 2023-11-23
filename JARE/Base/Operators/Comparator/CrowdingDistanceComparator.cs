/// <summary> CrowdingDistanceComparator.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
namespace JARE.Base.operators.comparator
{
	
	/// <summary> This class implements a <code>Comparator</code> (a method for comparing
	/// <code>Solution</code> objects) based on the crowding distance, as in NSGA-II.
	/// </summary>
    public class CrowdingDistanceComparator : System.Collections.Generic.IComparer<JARE.Base.Solution>
	{
		
		/// <summary> Compares two solutions.</summary>
		/// <param name="o1">Object representing the first <code>Solution</code>.
		/// </param>
		/// <param name="o2">Object representing the second <code>Solution</code>.
		/// </param>
		/// <returns> -1, or 0, or 1 if o1 is less than, equal, or greater than o2,
		/// respectively.
		/// </returns>
		public virtual int Compare(Solution s1, Solution s2)
		{
			if (s1 == null)
				return 1;
			else if (s2 == null)
				return - 1;
			
			double distance1 = s1.CrowdingDistance;
            double distance2 = s2.CrowdingDistance;
			if (distance1 > distance2)
				return - 1;
			if (distance1 < distance2)
				return 1;
			return 0;
		} // compare

    } // CrowdingDistanceComparator
}
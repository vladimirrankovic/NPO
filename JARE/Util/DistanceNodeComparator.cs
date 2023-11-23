/// <summary> DistanceNodeComparator.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.util
{
	
	/// <summary> This class implements a <code>Comparator</code> to compare instances of
	/// <code>DistanceNode</code>.
	/// </summary>
	public class DistanceNodeComparator : System.Collections.Generic.IComparer<DistanceNode>
	{
		
		/// <summary> Compares two <code>DistanceNode</code>.</summary>
		/// <param name="o1">Object representing a DistanceNode
		/// </param>
		/// <param name="o2">Object representing a DistanceNode
		/// </param>
		/// <returns> -1 if the distance of o1 is smaller than the distance of o2,
		/// 0 if the distance of both are equals, and
		/// 1 if the distance of o1 is bigger than the distance of o2
		/// </returns>
		public virtual int Compare(DistanceNode node1, DistanceNode node2)
		{
            //DistanceNode node1 = (DistanceNode) o1;
            //DistanceNode node2 = (DistanceNode) o2;
			
			double distance1, distance2;
			distance1 = node1.Distance;
			distance2 = node2.Distance;
			
			if (distance1 < distance2)
				return - 1;
			else if (distance1 > distance2)
				return 1;
			else
				return 0;
		} 
	} 
}
/// <summary> BinaryTournamentComparator.java
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
	
	/// <summary> This class implements a <code>Comparator</code> for <code>Solution</code></summary>
    public class BinaryTournamentComparator : System.Collections.Generic.IComparer<JARE.Base.Solution>
	{
		
		/// <summary> stores a dominance comparator</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_dominance '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		
        private static readonly System.Collections.IComparer m_dominance = new DominanceComparator();
		
		/// <summary> Compares two solutions.
		/// A <code>Solution</code> a is less than b for this <code>Comparator</code>.
		/// if the crowding distance of a if greater than the crowding distance of b.
		/// </summary>
		/// <param name="o1">Object representing a <code>Solution</code>.
		/// </param>
		/// <param name="o2">Object representing a <code>Solution</code>.
		/// </param>
		/// <returns> -1, or 0, or 1 if o1 is less than, equals, or greater than o2,
		/// respectively.
		/// </returns>
		public virtual int Compare(Solution s1, Solution s2)
		{
			int flag = m_dominance.Compare(s1, s2);
			if (flag != 0)
			{
				return flag;
			}
			
			double crowding1, crowding2;
			crowding1 = s1.CrowdingDistance;
			crowding2 = s2.CrowdingDistance;
			
			if (crowding1 > crowding2)
			{
				return - 1;
			}
			else if (crowding2 > crowding1)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		} // compare
	} // BinaryTournamentComparator.
}
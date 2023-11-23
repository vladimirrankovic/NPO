/// <summary> FitnessAndCrowdingComparator.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.Base.operators.comparator
{
	
	/// <summary> This class implements a <code>Comparator</code> (a method for comparing
	/// <code>Solution</code> objects) based on the fitness and crowding distance.
	/// </summary>
	public class FitnessAndCrowdingDistanceComparator : System.Collections.Generic.IComparer <Solution>
	{
		
		/// <summary> stores a comparator for check the fitness value of the solutions</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'fitnessComparator_ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"

        private static readonly System.Collections.Generic.IComparer<JARE.Base.Solution> m_fitnessComparator = new FitnessComparator();
		
        /// <summary> stores a comparator for check the crowding distance</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_crowdingDistanceComparator '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"

        private static readonly System.Collections.Generic.IComparer<JARE.Base.Solution> m_crowdingDistanceComparator = new CrowdingDistanceComparator();
		
		/// <summary> Compares two solutions.</summary>
		/// <param name="solution1">Object representing the first <code>Solution</code>.
		/// </param>
		/// <param name="solution2">Object representing the second <code>Solution</code>.
		/// </param>
		/// <returns> -1, or 0, or 1 if solution1 is less than, equal, or greater than 
		/// solution2, respectively.
		/// </returns>
		public virtual int Compare(Solution solution1, Solution solution2)
		{
			
			int flag = m_fitnessComparator.Compare(solution1, solution2);
			if (flag != 0)
			{
				return flag;
			}
			else
			{
				return m_crowdingDistanceComparator.Compare(solution1, solution2);
			}
		} 
	} 
}
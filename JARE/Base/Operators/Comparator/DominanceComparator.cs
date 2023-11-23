/// <summary> DominanceComparator.java
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
	/// <code>Solution</code> objects) based on a constraint violation test + 
	/// dominance checking, as in NSGA-II.
	/// </summary>
	public class DominanceComparator : System.Collections.IComparer
	{
		
		/// <summary> stores a comparator for check the OverallConstraintComparator</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'overallConstraintViolationComparator_ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.Collections.IComparer m_overallConstraintViolationComparator = new OverallConstraintViolationComparator();
		/// <summary> Compares two solutions.</summary>
		/// <param name="object1">Object representing the first <code>Solution</code>.
		/// </param>
		/// <param name="object2">Object representing the second <code>Solution</code>.
		/// </param>
		/// <returns> -1, or 0, or 1 if solution1 dominates solution2, both are 
		/// non-dominated, or solution1  is dominated by solution22, respectively.
		/// </returns>
		public virtual int Compare(System.Object object1, System.Object object2)
		{
			if (object1 == null)
				return 1;
			else if (object2 == null)
				return - 1;
			
			int dominate1; // dominate1 indicates if some objective of solution1 
			// dominates the same objective in solution2. dominate2
			int dominate2; // is the complementary of dominate1.
			Solution solution1 = (Solution) object1;
			Solution solution2 = (Solution) object2;
			
			dominate1 = 0;
			dominate2 = 0;
			
			int flag; //stores the result of the comparation
			
			if (solution1.OverallConstraintViolation != solution2.OverallConstraintViolation && (solution1.OverallConstraintViolation < 0) || (solution2.OverallConstraintViolation < 0))
			{
				return (m_overallConstraintViolationComparator.Compare(solution1, solution2));
			}
			
			// Equal number of violated constraint. Apply a dominance Test
			double value1, value2;
			for (int i = 0; i < solution1.NumberOfObjectives(); i++)
			{
				value1 = solution1.getObjective(i);
				value2 = solution2.getObjective(i);
				if (value1 < value2)
				{
					flag = - 1;
				}
				else if (value1 > value2)
				{
					flag = 1;
				}
				else
				{
					flag = 0;
				}
				
				if (flag == - 1)
				{
					dominate1 = 1;
				}
				
				if (flag == 1)
				{
					dominate2 = 1;
				}
			}
			
			if (dominate1 == dominate2)
			{
				return 0; //No one dominate the other
			}
			if (dominate1 == 1)
			{
				return - 1; // solution1 dominate
			}
			return 1; // solution2 dominate   
		} // compare
	} // DominanceComparator
}
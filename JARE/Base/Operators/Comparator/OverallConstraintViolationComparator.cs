/// <summary> OverallConstraintViolationComparator.java
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
	/// <code>Solution</code> objects) based on the overall constraint violation of
	/// the solucions, as in NSGA-II.
	/// </summary>
	public class OverallConstraintViolationComparator : System.Collections.IComparer
	{
		
		/// <summary> Compares two solutions.</summary>
		/// <param name="o1">Object representing the first <code>Solution</code>.
		/// </param>
		/// <param name="o2">Object representing the second <code>Solution</code>.
		/// </param>
		/// <returns> -1, or 0, or 1 if o1 is less than, equal, or greater than o2,
		/// respectively.
		/// </returns>
		public virtual int Compare(System.Object o1, System.Object o2)
		{
			double overall1, overall2;
			overall1 = ((Solution) o1).OverallConstraintViolation;
			overall2 = ((Solution) o2).OverallConstraintViolation;
			
			if ((overall1 < 0) && (overall2 < 0))
			{
				if (overall1 > overall2)
				{
					return - 1;
				}
				else if (overall2 > overall1)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			else if ((overall1 == 0) && (overall2 < 0))
			{
				return - 1;
			}
			else if ((overall1 < 0) && (overall2 == 0))
			{
				return 1;
			}
			else
			{
				return 0;
			}
		} // compare    
	} // ConstraintComparator
}
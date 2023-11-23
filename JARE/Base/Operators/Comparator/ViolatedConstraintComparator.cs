/// <summary> ViolatedConstraintComparator.java
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
	/// <code>Solution</code> objects) based on the number of violated constraints.
	/// </summary>
	public class ViolatedConstraintComparator : System.Collections.IComparer
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
			Solution solution1 = (Solution) o1;
			Solution solution2 = (Solution) o2;
			
			if (solution1.NumberOfViolatedConstraint < solution2.NumberOfViolatedConstraint)
			{
				return - 1;
			}
			else if (solution2.NumberOfViolatedConstraint < solution1.NumberOfViolatedConstraint)
			{
				return 1;
			}
			
			return 0;
		} // compare 
	} // ViolatedConstraintComparator
}
/// <summary> ObjectiveComparator.java
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
	/// <code>Solution</code> objects) based on a objective values.
	/// </summary>
    public class ObjectiveComparator : System.Collections.Generic.IComparer<JARE.Base.Solution>
	{
		
		/// <summary> Stores the index of the objective to compare</summary>
		private int nObj;
		
		/// <summary> Constructor.</summary>
		/// <param name="nObj">The index of the objective to compare
		/// </param>
		public ObjectiveComparator(int nObj)
		{
			this.nObj = nObj;
		} 
		
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
			
			double objetive1 = s1.getObjective(this.nObj);
            double objetive2 = s2.getObjective(this.nObj);
			if (objetive1 < objetive2)
			{
				return - 1;
			}
			else if (objetive1 > objetive2)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		} 
	} 
}
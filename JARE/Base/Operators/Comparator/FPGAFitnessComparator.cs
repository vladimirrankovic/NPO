/// <summary> FPGAFitnessComparator.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.comparator
{
	
	/// <summary> This class implements a <code>Comparator</code> (a method for comparing
	/// <code>Solution</code> objects) based on the rank used in FPGA.
	/// </summary>
    public class FPGAFitnessComparator : System.Collections.Generic.IComparer<JARE.Base.Solution>
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
			
			
			if (s1.Rank == 0 && s2.Rank > 0)
			{
				return - 1;
			}
			else if (s1.Rank > 0 && s2.Rank == 0)
			{
				return 1;
			}
			else
			{
				if (s1.Fitness > s2.Fitness)
				{
					return - 1;
				}
				else if (s1.Fitness < s2.Fitness)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
		} // compare
	} // FPGAFitnessComparator
}
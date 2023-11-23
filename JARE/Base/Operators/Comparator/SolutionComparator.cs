/// <summary> SolutionComparator.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
using Configuration = JARE.util.Configuration;
using Distance = JARE.util.Distance;
using SMException = JARE.util.SMException;
namespace JARE.Base.operators.comparator
{
	
	/// <summary> This class implements a <code>Comparator</code> (a method for comparing
	/// <code>Solution</code> objects) based on the values of the variables.
	/// </summary>
	public class SolutionComparator : System.Collections.IComparer
	{
		
		/// <summary> Establishes a value of allowed dissimilarity</summary>
		private const double EPSILON = 1e-10;
		
		/// <summary> Compares two solutions.</summary>
		/// <param name="o1">Object representing the first <code>Solution</code>. 
		/// </param>
		/// <param name="o2">Object representing the second <code>Solution</code>.
		/// </param>
		/// <returns> 0, if both solutions are equals with a certain dissimilarity, -1
		/// otherwise.
		/// </returns>
		/// <throws>  SMException  </throws>
		/// <throws>  SMException  </throws>
		public virtual int Compare(System.Object o1, System.Object o2)
		{
			Solution solution1, solution2;
			solution1 = (Solution) o1;
			solution2 = (Solution) o2;
			
			if (solution1.numberOfVariables() != solution2.numberOfVariables())
				return - 1;
			
			try
			{
				if ((new Distance()).distanceBetweenSolutions(solution1, solution2) < EPSILON)
					return 0;
			}
			catch (SMException)
			{
				Configuration.m_logger.WriteLog("SolutionComparator.compare: SMException ");
			}
			
			return - 1;
		} // compare
	} // SolutionComparator
}
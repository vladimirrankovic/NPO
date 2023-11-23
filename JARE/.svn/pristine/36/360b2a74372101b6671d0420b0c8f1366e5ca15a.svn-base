/// <summary> EpsilonObjectiveComparator.java
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
	/// <code>Solution</code> objects) based on epsilon dominance over a given
	/// objective function.
	/// </summary>
	public class EpsilonObjectiveComparator : System.Collections.IComparer
	{
		
		/// <summary> Stores the objective index to compare</summary>
		private int m_objective;
		
		/// <summary> Stores the eta value for epsilon-dominance</summary>
		private double m_eta;
		
		/// <summary> Constructor.</summary>
		/// <param name="nObj">Index of the objective to compare.
		/// </param>
		/// <param name="eta">Value for epsilon-dominance.
		/// </param>
		public EpsilonObjectiveComparator(int nObj, double eta)
		{
			m_objective = nObj;
			m_eta = eta;
		} // EObjectiveComparator
		
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
			if (o1 == null)
				return 1;
			else if (o2 == null)
				return - 1;
			
			double objetive1 = ((Solution) o1).getObjective(m_objective);
			double objetive2 = ((Solution) o2).getObjective(m_objective);
			
			//Objetive implements comparable!!! 
			if (objetive1 / (1 + m_eta) < objetive2)
			{
				return - 1;
			}
			else if (objetive1 / (1 + m_eta) > objetive2)
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
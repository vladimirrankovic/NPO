/// <summary> StrengthRawFitnessArchive.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.comparator;
using Spea2Fitness = JARE.util.Spea2Fitness;
namespace JARE.util.archive
{
	
	/// <summary> This class implemens a bounded archive based on strength raw fitness (as
	/// defined in SPEA2).
	/// </summary>
	[Serializable]
	public class StrengthRawFitnessArchive:Archive
	{
		
		/// <summary> Stores the maximum size of the archive.</summary>
		private int m_maxSize;
		
		/// <summary> Stores a <code>Comparator</code> for dominance checking.</summary>
		private System.Collections.IComparer m_dominance;
		
		/// <summary> Stores a <code>Comparator</code> for fitness checking.</summary>
        private System.Collections.Generic.IComparer<JARE.Base.Solution> m_fitnessComparator;
		
		/// <summary> Stores a <code>Comparator</code> for equality checking (in the objective
		/// space).
		/// </summary>
		private System.Collections.IComparer m_equals;
		
		/// <summary> Constructor.</summary>
		/// <param name="maxSize">The maximum size of the archive.
		/// </param>
		public StrengthRawFitnessArchive(int maxSize):base(maxSize)
		{
			m_maxSize = maxSize;
			m_dominance = new DominanceComparator();
			m_equals = new EqualSolutions();
			m_fitnessComparator = new FitnessComparator();
		} // StrengthRawFitnessArchive
		
		/// <summary> Adds a <code>Solution</code> to the archive. If the <code>Solution</code>
		/// is dominated by any member of the archive then it is discarded. If the 
		/// <code>Solution</code> dominates some members of the archive, these are
		/// removed. If the archive is full and the <code>Solution</code> has to be
		/// inserted, all the solutions are ordered by his strengthRawFitness value and
		/// the one having the worst value is removed.
		/// </summary>
		/// <param name="solution">The <code>Solution</code>
		/// </param>
		/// <returns> true if the <code>Solution</code> has been inserted, false
		/// otherwise.
		/// </returns>
		public override bool add(Solution solution)
		{
			int flag = 0;
			int i = 0;
			Solution aux;
			while (i < m_solutionsList.Count)
			{
				aux = m_solutionsList[i];
				flag = m_dominance.Compare(solution, aux);
				if (flag == 1)
				{
					// The solution to add is dominated
					return false; // Discard the new solution
				}
				else if (flag == - 1)
				{
					// A solution in the archive is dominated
					m_solutionsList.RemoveAt(i); // Remove the dominated solution            
				}
				else
				{
					if (m_equals.Compare(aux, solution) == 0)
					{
						return false;
					}
					i++;
				}
			}
			// Insert the solution in the archive
			m_solutionsList.Add(solution);
			
			if (size() > m_maxSize)
			{
				// The archive is full           
				(new Spea2Fitness(this)).fitnessAssign();
				sort(m_fitnessComparator);
				//Remove the last
				remove(m_maxSize);
			}
			return true;
		} // add
	} //StrengthRawFitnessArchive
}
/// <summary> CrowdingArchive.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.comparator;
using Distance = JARE.util.Distance;
namespace JARE.util.archive
{
	
	/// <summary> This class implements a bounded archive based on crowding distances (as
	/// defined in NSGA-II).
	/// </summary>
	[Serializable]
	public class CrowdingArchive:Archive
	{
		
		/// <summary> Stores the maximum size of the archive.</summary>
		private int m_maxSize;
		
		/// <summary> stores the number of the objectives.</summary>
		private int m_objectives;
		
		/// <summary> Stores a <code>Comparator</code> for dominance checking.</summary>
		private System.Collections.IComparer m_dominance;
		
		/// <summary> Stores a <code>Comparator</code> for equality checking (in the objective
		/// space).
		/// </summary>
		private System.Collections.IComparer m_equals;
		
		/// <summary> Stores a <code>Comparator</code> for checking crowding distances.</summary>
        private System.Collections.Generic.IComparer<JARE.Base.Solution> m_crowdingDistance;
		
		/// <summary> Stores a <code>Distance</code> object, for distances utilities</summary>
		private Distance m_distance;
		
		/// <summary> Constructor. </summary>
		/// <param name="maxSize">The maximum size of the archive.
		/// </param>
		/// <param name="numberOfObjectives">The number of objectives.
		/// </param>
		public CrowdingArchive(int maxSize, int numberOfObjectives):base(maxSize)
		{
			m_maxSize = maxSize;
			m_objectives = numberOfObjectives;
			m_dominance = new DominanceComparator();
			m_equals = new EqualSolutions();
			m_crowdingDistance = new CrowdingDistanceComparator();
			m_distance = new Distance();
		} 
		
		
		/// <summary> Adds a <code>Solution</code> to the archive. If the <code>Solution</code>
		/// is dominated by any member of the archive, then it is discarded. If the 
		/// <code>Solution</code> dominates some members of the archive, these are
		/// removed. If the archive is full and the <code>Solution</code> has to be
		/// inserted, the solutions are sorted by crowding distance and the one having
		/// the minimum crowding distance value.
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
			Solution aux; //Store an solution temporally
         
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
                    m_solutionsList.RemoveAt(i);
                    // Remove it from the population            
				}
				else
				{
					if (m_equals.Compare(aux, solution) == 0)
					{
						// There is an equal solution 
						// in the population
						return false; // Discard the new solution
					} 
					i++;
				}
			}
			// Insert the solution into the archive
			m_solutionsList.Add(solution);
			if (size() > m_maxSize)
			{
				// The archive is full
				m_distance.crowdingDistanceAssignment(this, m_objectives);
				sort(m_crowdingDistance);
				//Remove the last
				remove(m_maxSize);
			}
			return true;
		} 
	} 
}
/// <summary> AdaptiveGridArchive.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0 
/// </version>
using System;
using JARE.Base;
using DominanceComparator = JARE.Base.operators.comparator.DominanceComparator;
using AdaptiveGrid = JARE.util.AdaptiveGrid;
namespace JARE.util.archive
{
	
	/// <summary> This class implements an archive based on an adaptative grid used in PAES</summary>
	[Serializable]
	public class AdaptiveGridArchive : Archive
	{
		/// <summary> Returns the AdaptativeGrid used</summary>
		/// <returns> the AdaptativeGrid
		/// </returns>
		virtual public AdaptiveGrid Grid
		{
			get
			{
				return m_grid;
			}
			// AdaptativeGrid  
			
		}
		
		/// <summary> Stores the adaptive grid</summary>
		private AdaptiveGrid m_grid;
		
		/// <summary> Stores the maximum size of the archive</summary>
		private int m_maxSize;
		
		/// <summary> Stores a <code>Comparator</code> for dominance checking</summary>
		private System.Collections.IComparer m_dominance;
		
		/// <summary> Constructor.
		/// 
		/// </summary>
		/// <param name="maxSize">The maximum size of the archive
		/// </param>
		/// <param name="bisections">The maximum number of bi-divisions for the adaptive
		/// grid.
		/// </param>
		/// <param name="objectives">The number of objectives.
		/// </param>
		public AdaptiveGridArchive(int maxSize, int bisections, int objectives):base(maxSize)
		{
			m_maxSize = maxSize;
			m_dominance = new DominanceComparator();
			m_grid = new AdaptiveGrid(bisections, objectives);
		} // AdaptiveGridArchive
		
		/// <summary> Adds a <code>Solution</code> to the archive. If the <code>Solution</code>
		/// is dominated by any member of the archive then it is discarded. If the 
		/// <code>Solution</code> dominates some members of the archive, these are
		/// removed. If the archive is full and the <code>Solution</code> has to be
		/// inserted, one <code>Solution</code> of the most populated hypercube of the
		/// adaptative grid is removed.
		/// </summary>
		/// <param name="solution">The <code>Solution</code>
		/// </param>
		/// <returns> true if the <code>Solution</code> has been inserted, false
		/// otherwise.
		/// </returns>
		public override bool add(Solution solution)
		{
			//Iterator of individuals over the list
            //Iterator<Solution> iterator = solutionsList_.iterator();

			//while (iterator.hasNext())
            foreach (Solution element in m_solutionsList)
            {
                int flag = m_dominance.Compare(solution, element);
                if (flag == - 1)
				{
					// The Individual to insert dominates other 
					// individuals in the archive
                    m_solutionsList.Remove(element); //Delete it from the archive
					
                    int location = m_grid.location(element);
					if (m_grid.getLocationDensity(location) > 1)
					{
						//The hypercube contains 
						m_grid.removeSolution(location); //more than one individual
					}
					else
					{
						m_grid.updateGrid(this);
					} 
				}
				
				else if (flag == 1)
				{
					// An Individual into the file dominates the 
					// solution to insert
					return false; // The solution will not be inserted
				} 
            }
					
			// At this point, the solution may be inserted
			if (size() == 0)
			{
				//The archive is empty
				m_solutionsList.Add(solution);
				m_grid.updateGrid(this);
				return true;
			} //
			
			if (size() < m_maxSize)
			{
				//The archive is not full              
				m_grid.updateGrid(solution, this); // Update the grid if applicable
                int location;
				location = m_grid.location(solution); // Get the location of the solution
				m_grid.addSolution(location); // Increment the density of the hypercube
				m_solutionsList.Add(solution); // Add the solution to the list
				return true;
			} // if
			
			// At this point, the solution has to be inserted and the archive is full
			m_grid.updateGrid(solution, this);
			int location1 = m_grid.location(solution);
			if (location1 == m_grid.MostPopulated)
			{
				// The solution is in the 
				// most populated hypercube
				return false; // Not inserted
			}
			else
			{
				// Remove an solution from most poblated area
				bool removed = false;
               
                foreach (Solution element in m_solutionsList)
                {
                    if (!removed)
					{
						int location2 = m_grid.location(element);
						if (location2 == m_grid.MostPopulated)
						{
                            m_solutionsList.Remove(element);
							m_grid.removeSolution(location2);
						} 
					} 
                }
				
				// A solution from most populated hypercube has been removed, 
				// insert now the solution
				m_grid.addSolution(location1);
				m_solutionsList.Add(solution);
			}
			return true;
		} // add
	} // AdaptativeGridArchive
}
/// <summary> Neighborhood.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
namespace JARE.util
{
	
	/// <summary> Class representing neighborhoods for a <code>Solution</code> into a
	/// <code>SolutionSet</code>.
	/// </summary>
    /// 

    /// <summary> Enum type for defining the North, South, East, West, North-West, South-West,
    /// North-East, South-East neighbor.
    /// </summary>
    enum Row
    {
        N, S, E, W, NW, SW, NE, SE
    }

	public class Neighborhood
	{
		
		/// <summary> Maximum rate considered</summary>
		private static int MAXRADIO = 2;
		
		/// <summary> Stores the neighborhood.
		/// structure_ [i] represents a neighborhood for a solution.
		/// structure_ [i][j] represents a neighborhood with a ratio.
		/// structure_ [i][j][k] represents a neighbor of a solution.
		/// </summary>
		private int[][][] m_structure;
		
		/// <summary> Stores the size of the solutionSet.</summary>
		private int m_solutionSetSize;
		
		/// <summary> Stores the size for each row</summary>
		private int m_rowSize;
		
		
		/// <summary> Constructor.
		/// Defines a neighborhood of a given size.
		/// </summary>
		/// <param name="solutionSetSize">The size.
		/// </param>
		public Neighborhood(int solutionSetSize)
		{
			m_solutionSetSize = solutionSetSize;
			//Create the structure_ for store the neighborhood
			m_structure = new int[m_solutionSetSize][][];
			for (int i = 0; i < m_solutionSetSize; i++)
			{
				m_structure[i] = new int[MAXRADIO][];
			}
			
			//For each individual, and different rates the individual has a different 
			//number of neighborhoods
			for (int ind = 0; ind < m_solutionSetSize; ind++)
			{
				for (int radio = 0; radio < MAXRADIO; radio++)
				{
					if (radio == 0)
					{
						//neighboors whit rate 1
						m_structure[ind][radio] = new int[8];
					}
					else if (radio == 1)
					{
						//neighboors whit rate 2
						m_structure[ind][radio] = new int[24];
					} 
				} 
			} 
			
			//Calculate the size of a row
			m_rowSize = (int) System.Math.Sqrt((double) m_solutionSetSize);
			
			
			//Calculates the neighbors of a individual 
			for (int ind = 0; ind < m_solutionSetSize; ind++)
			{
				//rate 1
				//North neighbors
				if (ind > m_rowSize - 1)
				{
					m_structure[ind][0][(int)Row.N] = ind - m_rowSize;
				}
				else
				{
					m_structure[ind][0][(int)Row.N] = (ind - m_rowSize + solutionSetSize) % solutionSetSize;
				}
				
				//East neighbors
				if ((ind + 1) % m_rowSize == 0)
					m_structure[ind][0][(int)Row.E] = (ind - (m_rowSize - 1));
				else
                    m_structure[ind][0][(int)Row.E] = (ind + 1);
				
				//Western neigbors
				if (ind % m_rowSize == 0)
				{
                    m_structure[ind][0][(int)Row.W] = (ind + (m_rowSize - 1));
				}
				else
				{
                    m_structure[ind][0][(int)Row.W] = (ind - 1);
				}
				
				//South neigbors
                m_structure[ind][0][(int)Row.S] = (ind + m_rowSize) % solutionSetSize;
			}
			
			for (int ind = 0; ind < m_solutionSetSize; ind++)
			{
                m_structure[ind][0][(int)Row.NE] = m_structure[m_structure[ind][0][(int)Row.N]][0][(int)Row.E];
                m_structure[ind][0][(int)Row.NW] = m_structure[m_structure[ind][0][(int)Row.N]][0][(int)Row.W];
                m_structure[ind][0][(int)Row.SE] = m_structure[m_structure[ind][0][(int)Row.S]][0][(int)Row.E];
                m_structure[ind][0][(int)Row.SW] = m_structure[m_structure[ind][0][(int)Row.S]][0][(int)Row.W];
			}
		} 
		
		/// <summary> Returns a <code>SolutionSet</code> with the North, Sout, East and West
		/// neighbors solutions of ratio 0 of a given location into a given 
		/// <code>SolutionSet</code>.
		/// </summary>
		/// <param name="solutionSet">The <code>SolutionSet</code>.
		/// </param>
		/// <param name="location">The location.
		/// </param>
		/// <returns> a <code>SolutionSet</code> with the neighbors.
		/// </returns>
		public virtual SolutionSet getFourNeighbors(SolutionSet solutionSet, int location)
		{
			//SolutionSet that contains the neighbors (to return)
			SolutionSet neighbors;
			
			//instance the solutionSet to a non dominated li of individuals
			neighbors = new SolutionSet(24);
			
			//Gets the neighboords N, S, E, W
			int index;
			
			//North
            index = m_structure[location][0][(int)Row.N];
			neighbors.add(solutionSet.getSolution(index));
			
			//South
            index = m_structure[location][0][(int)Row.S];
			neighbors.add(solutionSet.getSolution(index));
			
			//East
            index = m_structure[location][0][(int)Row.E];
			neighbors.add(solutionSet.getSolution(index));
			
			//West
            index = m_structure[location][0][(int)Row.W];
			neighbors.add(solutionSet.getSolution(index));
			
			//Return the list of non-dominated individuals
			return neighbors;
		}      
		
		/// <summary> Returns a <code>SolutionSet</code> with the North, Sout, East, West, 
		/// North-West, South-West, North-East and South-East neighbors solutions of
		/// ratio 0 of a given location into a given <code>SolutionSet</code>.
		/// solutions of a given location into a given <code>SolutionSet</code>.
		/// </summary>
		/// <param name="population">The <code>SolutionSet</code>.
		/// </param>
		/// <param name="individual">The individual.
		/// </param>
		/// <returns> a <code>SolutionSet</code> with the neighbors.
		/// </returns>
		public virtual SolutionSet getEightNeighbors(SolutionSet population, int individual)
		{
			//SolutionSet that contains the neighbors (to return)
			SolutionSet neighbors;
			
			//instance the population to a non dominated li of individuals
			neighbors = new SolutionSet(24);
			
			//Gets the neighboords N, S, E, W
			int index;
			
			//N
            index = this.m_structure[individual][0][(int)Row.N];
			neighbors.add(population.getSolution(index));
			
			//S
            index = this.m_structure[individual][0][(int)Row.S];
			neighbors.add(population.getSolution(index));
			
			//E
            index = this.m_structure[individual][0][(int)Row.E];
			neighbors.add(population.getSolution(index));
			
			//W
            index = this.m_structure[individual][0][(int)Row.W];
			neighbors.add(population.getSolution(index));
			
			//NE
            index = this.m_structure[individual][0][(int)Row.NE];
			neighbors.add(population.getSolution(index));
			
			//NW
            index = this.m_structure[individual][0][(int)Row.NW];
			neighbors.add(population.getSolution(index));
			
			//SE
            index = this.m_structure[individual][0][(int)Row.SE];
			neighbors.add(population.getSolution(index));
			
			//SW
            index = this.m_structure[individual][0][(int)Row.SW];
			neighbors.add(population.getSolution(index));
			
			
			//Return the list of non-dominated individuals
			return neighbors;
		} 
	}
}
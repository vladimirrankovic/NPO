/// <summary> AdaptativeGrid.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
namespace JARE.util
{
	
	/// <summary> This class defines an adaptative grid over a SolutionSet as the one used the
	/// algorithm PAES.
	/// </summary>
	public class AdaptiveGrid
	{
		/// <summary> Returns the value of the most populated hypercube.</summary>
		/// <returns> The hypercube with the maximum number of solutions.
		/// </returns>
		virtual public int MostPopulated
		{
			get
			{
				return m_mostPopulated;
			}
		}
		/// <summary> Returns the number of bi-divisions performed in each objective.</summary>
		/// <returns> the number of bi-divisions.
		/// </returns>
		virtual public int Bisections
		{
			get
			{
				return m_bisections;
			}
		}
		
		/// <summary> Number of bi-divisions of the objective space</summary>
		private int m_bisections;
		
		/// <summary> Objectives of the problem</summary>
		private int m_objectives;
		
		/// <summary> Number of solutions into a specific hypercube in the adaptative grid</summary>
		private int[] m_hypercubes;
		
		/// <summary> 
		/// Grid lower bounds
		/// </summary>
		private double[] m_lowerLimits;
		
		/// <summary> Grid upper bounds</summary>
		private double[] m_upperLimits;
		
		/// <summary> Size of hypercube for each dimension</summary>
		private double[] m_divisionSize;
		
		/// <summary> Hypercube with maximum number of solutions</summary>
		private int m_mostPopulated;
		
		/// <summary> Hndicates when an hypercube has solutions</summary>
		private int[] m_occupied;
		
		/// <summary> Constructor.
		/// Creates an instance of AdaptativeGrid.
		/// </summary>
		/// <param name="bisections">Number of bi-divisions of the objective space.
		/// </param>
		/// <param name="objetives">Number of objectives of the problem.
		/// </param>
		public AdaptiveGrid(int bisections, int objetives)
		{
			m_bisections = bisections;
			m_objectives = objetives;
			m_lowerLimits = new double[m_objectives];
			m_upperLimits = new double[m_objectives];
			m_divisionSize = new double[m_objectives];
			m_hypercubes = new int[(int) System.Math.Pow(2.0, m_bisections * m_objectives)];
			
			for (int i = 0; i < m_hypercubes.Length; i++)
				m_hypercubes[i] = 0;
		}
		
		
		/// <summary>  Updates the grid limits considering the solutions contained in a 
		/// <code>SolutionSet</code>.
		/// </summary>
		/// <param name="solutionSet">The <code>SolutionSet</code> considered.
		/// </param>
		private void  updateLimits(SolutionSet solutionSet)
		{
			//Init the lower and upper limits 
			for (int obj = 0; obj < m_objectives; obj++)
			{
				//Set the lower limits to the max double
                m_lowerLimits[obj] = Double.MaxValue;
				//Set the upper limits to the min double
				m_upperLimits[obj] = Double.MinValue;
			} 
			
			//Find the max and min limits of objetives into the population
			for (int ind = 0; ind < solutionSet.size(); ind++)
			{
				Solution tmpIndividual = solutionSet.getSolution(ind);
				for (int obj = 0; obj < m_objectives; obj++)
				{
					if (tmpIndividual.getObjective(obj) < m_lowerLimits[obj])
					{
						m_lowerLimits[obj] = tmpIndividual.getObjective(obj);
					}
					if (tmpIndividual.getObjective(obj) > m_upperLimits[obj])
					{
						m_upperLimits[obj] = tmpIndividual.getObjective(obj);
					}
				} 
			}
		} 
		
		/// <summary> Updates the grid adding solutions contained in a specific 
		/// <code>SolutionSet</code>.
		/// <b>REQUIRE</b> The grid limits must have been previously calculated.
		/// </summary>
		/// <param name="solutionSet">The <code>SolutionSet</code> considered.
		/// </param>
		private void  addSolutionSet(SolutionSet solutionSet)
		{
			//Calculate the location of all individuals and update the grid
			m_mostPopulated = 0;
			int location;
			
			for (int ind = 0; ind < solutionSet.size(); ind++)
			{

                location = solutionSet.getSolution(ind).Location;
				m_hypercubes[location]++;
				if (m_hypercubes[location] > m_hypercubes[m_mostPopulated])
					m_mostPopulated = location;
			}  
			
			//The grid has been updated, so also update ocuppied's hypercubes
			calculateOccupied();
		} 
		
		
		/// <summary> Updates the grid limits and the grid content adding the solutions contained
		/// in a specific <code>SolutionSet</code>.
		/// </summary>
		/// <param name="solutionSet">The <code>SolutionSet</code>.
		/// </param>
		public virtual void updateGrid(SolutionSet solutionSet)
		{
			//Update lower and upper limits
			updateLimits(solutionSet);
			
			//Calculate the division size
			for (int obj = 0; obj < m_objectives; obj++)
			{
				m_divisionSize[obj] = m_upperLimits[obj] - m_lowerLimits[obj];
			} 
			
			//Clean the hypercubes
			for (int i = 0; i < m_hypercubes.Length; i++)
			{
				m_hypercubes[i] = 0;
			}
			
			//Add the population
			addSolutionSet(solutionSet);
		} 
		
		
		/// <summary> Updates the grid limits and the grid content adding a new 
		/// <code>Solution</code>.
		/// If the solution falls out of the grid bounds, the limits and content of the
		/// grid must be re-calculated.
		/// </summary>
		/// <param name="solution"><code>Solution</code> considered to update the grid.
		/// </param>
		/// <param name="solutionSet"><code>SolutionSet</code> used to update the grid.
		/// </param>
		public virtual void  updateGrid(Solution solution, SolutionSet solutionSet)
		{
			
			int location = solution.Location;
			if (location == - 1)
			{
				//Re-build the Adaptative-Grid
				//Update lower and upper limits
				updateLimits(solutionSet);
				
				//Actualize the lower and upper limits whit the individual      
				for (int obj = 0; obj < m_objectives; obj++)
				{
					if (solution.getObjective(obj) < m_lowerLimits[obj])
						m_lowerLimits[obj] = solution.getObjective(obj);
					if (solution.getObjective(obj) > m_upperLimits[obj])
						m_upperLimits[obj] = solution.getObjective(obj);
				}
				
				//Calculate the division size
				for (int obj = 0; obj < m_objectives; obj++)
				{
					m_divisionSize[obj] = m_upperLimits[obj] - m_lowerLimits[obj];
				}
				
				//Clean the hypercube
				for (int i = 0; i < m_hypercubes.Length; i++)
				{
					m_hypercubes[i] = 0;
				}
				
				//add the population
				addSolutionSet(solutionSet);
			}                                      
		} 
		
		
		/// <summary> Calculates the hypercube of a solution.</summary>
		/// <param name="solution">The <code>Solution</code>.
		/// </param>
		public virtual int location(Solution solution)
		{
			//Create a int [] to store the range of each objetive
			int[] position = new int[m_objectives];
			
			//Calculate the position for each objetive
			for (int obj = 0; obj < m_objectives; obj++)
			{
				
				if ((solution.getObjective(obj) > m_upperLimits[obj]) || (solution.getObjective(obj) < m_lowerLimits[obj]))
					return - 1;
				else if (solution.getObjective(obj) == m_lowerLimits[obj])
					position[obj] = 0;
				else if (solution.getObjective(obj) == m_upperLimits[obj])
				{
					position[obj] = ((int) System.Math.Pow(2.0, m_bisections)) - 1;
				}
				else
				{
					double tmpSize = m_divisionSize[obj];
					double val = solution.getObjective(obj);
					double account = m_lowerLimits[obj];
					int ranges = (int) System.Math.Pow(2.0, m_bisections);
					for (int b = 0; b < m_bisections; b++)
					{
						tmpSize /= 2.0;
						ranges /= 2;
						if (val > (account + tmpSize))
						{
							position[obj] += ranges;
							account += tmpSize;
						} 
					} 
				} 
			}
			
			//Calcualate the location into the hypercubes
			int location = 0;
			for (int obj = 0; obj < m_objectives; obj++)
			{
				location = (int) (location + position[obj] * System.Math.Pow(2.0, obj * m_bisections));
			}
			return location;
		} 
		
		/// <summary> Returns the number of solutions into a specific hypercube.</summary>
		/// <param name="location">Number of the hypercube.
		/// </param>
		/// <returns> The number of solutions into a specific hypercube.
		/// </returns>
		public virtual int getLocationDensity(int location)
		{
			return m_hypercubes[location];
		} 
		
		/// <summary> Decreases the number of solutions into a specific hypercube.</summary>
		/// <param name="location">Number of hypercube.
		/// </param>
		public virtual void  removeSolution(int location)
		{
			//Decrease the solutions in the location specified.
			m_hypercubes[location]--;
			
			//Update the most poblated hypercube
			if (location == m_mostPopulated)
				for (int i = 0; i < m_hypercubes.Length; i++)
					if (m_hypercubes[i] > m_hypercubes[m_mostPopulated])
						m_mostPopulated = i;
			
			//If hypercubes[location] now becomes to zero, then update ocupped hypercubes
			if (m_hypercubes[location] == 0)
				this.calculateOccupied();
		} 
		
		/// <summary> Increases the number of solutions into a specific hypercube.</summary>
		/// <param name="location">Number of hypercube.
		/// </param>
		public virtual void  addSolution(int location)
		{
			//Increase the solutions in the location specified.
			m_hypercubes[location]++;
			
			//Update the most poblated hypercube
			if (m_hypercubes[location] > m_hypercubes[m_mostPopulated])
				m_mostPopulated = location;
			
			//if hypercubes[location] becomes to one, then recalculate 
			//the occupied hypercubes
			if (m_hypercubes[location] == 1)
				this.calculateOccupied();
		} 
		
		/// <summary> Retunrns a String representing the grid.</summary>
		/// <returns> The String.
		/// </returns>
		public override string ToString()
		{
			string result = "Grid\n";
			for (int obj = 0; obj < m_objectives; obj++)
			{
				result += ("Objective " + obj + " " + m_lowerLimits[obj] + " " + m_upperLimits[obj] + "\n");
			}               
			return result;
		} 
		
		/// <summary> Returns a random hypercube using a rouleteWheel method.  </summary>
		/// <returns> the number of the selected hypercube.
		/// </returns>
		public virtual int rouletteWheel()
		{
			//Calculate the inverse sum
			double inverseSum = 0.0;
			for (int i = 0; i < m_hypercubes.Length; i++)
			{
				if (m_hypercubes[i] > 0)
				{
					inverseSum += 1.0 / (double) m_hypercubes[i];
				}
			}
			
			//Calculate a random value between 0 and sumaInversa
			double random = PseudoRandom.randDouble(0.0, inverseSum);
			int hypercube = 0;
			double accumulatedSum = 0.0;
			while (hypercube < m_hypercubes.Length)
			{
				if (m_hypercubes[hypercube] > 0)
				{
					accumulatedSum += 1.0 / (double) m_hypercubes[hypercube];
				} 
				
				if (accumulatedSum > random)
				{
					return hypercube;
				} 
				
				hypercube++;
			}
			
			return hypercube;
		} 
		
		/// <summary> Calculates the number of hypercubes having one or more solutions.
		/// return the number of hypercubes with more than zero solutions.
		/// </summary>
		public virtual int calculateOccupied()
		{
			int total = 0;
			for (int i = 0; i < m_hypercubes.Length; i++)
			{
				if (m_hypercubes[i] > 0)
				{
					total++;
				} 
			} 
			
			m_occupied = new int[total];
			int b = 0;
			for (int i = 0; i < m_hypercubes.Length; i++)
			{
				if (m_hypercubes[i] > 0)
				{
					m_occupied[b] = i;
					b++;
				} 
			}        
			
			return total;
		} 
		
		/// <summary> Returns the number of hypercubes with more than zero solutions.</summary>
		/// <returns> the number of hypercubes with more than zero solutions.
		/// </returns>
		public virtual int occupiedHypercubes()
		{
			return m_occupied.Length;
		} 
		
		
		/// <summary> Returns a random hypercube that has more than zero solutions.</summary>
		/// <returns> The hypercube.
		/// </returns>
		public virtual int randomOccupiedHypercube()
		{
			int rand = PseudoRandom.randInt(0, m_occupied.Length - 1);
			return m_occupied[rand];
		} //randomOccupiedHypercube
	} //AdaptativeGrid
}
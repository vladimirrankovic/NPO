/// <summary> NonDominatedSolutionList.java 
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
using SolutionSet = JARE.Base.SolutionSet;
using JARE.Base.operators.comparator;
namespace JARE.util
{
	
	/// <summary> This class implements an unbound list of non-dominated solutions</summary>
	[Serializable]
	public class NonDominatedSolutionList : SolutionSet
	{
		
		/// <summary> Stores a <code>Comparator</code> for dominance checking</summary>
		private System.Collections.IComparer m_dominance = new DominanceComparator();
		
		/// <summary> Stores a <code>Comparator</code> for checking if two solutions are equal</summary>
		private static readonly System.Collections.IComparer m_equal = new SolutionComparator();
		
		/// <summary> Constructor.
		/// The objects of this class are lists of non-dominated solutions according to
		/// a Pareto dominance comparator. 
		/// </summary>
		public NonDominatedSolutionList():base()
		{
		} // NonDominatedList
		
		/// <summary> Constructor.
		/// This constructor creates a list of non-dominated individuals using a
		/// comparator object.
		/// </summary>
		/// <param name="dominance">The comparator for dominance checking.
		/// </param>
		public NonDominatedSolutionList(System.Collections.IComparer dominance):base()
		{
			m_dominance = dominance;
		} // NonDominatedList
		
		/// <summary>Inserts a solution in the list</summary>
		/// <param name="solution">The solution to be inserted.
		/// </param>
		/// <returns> true if the operation success, and false if the solution is 
		/// dominated or if an identical individual exists.
		/// The decision variables can be null if the solution is read from a file; in
		/// that case, the domination tests are omitted
		/// </returns>
		public override bool add(Solution solution)
		{
			
			if (solution.DecisionVariables != null)
			{
                //while (iterator.hasNext())
                foreach (Solution listIndividual in m_solutionsList)
				{
					//Solution listIndividual = iterator.next();
					int flag = m_dominance.Compare(solution, listIndividual);
					
					if (flag == - 1)
					{
						// A solution in the list is dominated by the new one
                        m_solutionsList.Remove(listIndividual);
					}
					else if (flag == 0)
					{
						// Non-dominated solutions
						flag = m_equal.Compare(solution, listIndividual);
						if (flag == 0)
						{
							return false; // The new solution is in the list  
						}
					}
					else if (flag == 1)
					{
						// The new solution is dominated
						return false;
					}
				}
			} 
			
			//At this point, the solution is inserted into the list
			m_solutionsList.Add(solution);
			
			return true;
		}                
	} 
}
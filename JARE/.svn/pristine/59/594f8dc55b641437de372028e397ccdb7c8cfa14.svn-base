/// <summary> BinaryTournament2.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
using Operator = JARE.Base.operators;
using SolutionSet = JARE.Base.SolutionSet;
using DominanceComparator = JARE.Base.operators.comparator.DominanceComparator;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.selection
{
	
	/// <summary> This class implements an opertor for binary selections using the same code
	/// in Deb's NSGA-II implementation
	/// </summary>
	[Serializable]
	public class BinaryTournament2:Selection
	{
		
		/// <summary> m_dominance store the <code>Comparator</code> for check m_dominance</summary>
		private System.Collections.IComparer m_dominance;
		
		/// <summary> m_A stores a permutation of the solutions in the solutionSet used</summary>
		private int[] m_a;
		
		/// <summary>  index_ stores the actual index for selection</summary>
		private int m_index = 0;
		
		/// <summary> Constructor
		/// Creates a new instance of the Binary tournament operator (Deb's
		/// NSGA-II implementation version)
		/// </summary>
		public BinaryTournament2()
		{
			m_dominance = new DominanceComparator();
		} // BinaryTournament2
		
		/// <summary> Performs the operation</summary>
		/// <param name="object">Object representing a SolutionSet
		/// </param>
		/// <returns> the selected solution
		/// </returns>
		public override System.Object execute(System.Object obj)
		{
			SolutionSet population = (SolutionSet) obj;

            if (population.size() % 2 != 0) throw new Exception("Population size must be an even number!");

			if (m_index == 0)
			//Create the permutation
			{
				m_a = (new JARE.util.PermutationUtility()).intPermutation(population.size());
			}
			
			Solution solution1, solution2;
			solution1 = population.getSolution(m_a[m_index]);
			solution2 = population.getSolution(m_a[m_index + 1]);
			
			m_index = (m_index + 2) % population.size();
			
			int flag = m_dominance.Compare(solution1, solution2);
			if (flag == - 1)
				return solution1;
			else if (flag == 1)
				return solution2;
			else if (solution1.CrowdingDistance > solution2.CrowdingDistance)
				return solution1;
			else if (solution2.CrowdingDistance > solution1.CrowdingDistance)
				return solution2;
			else if (PseudoRandom.randDouble() < 0.5)
				return solution1;
			else
				return solution2;
		} // execute
	} // BinaryTournament2
}
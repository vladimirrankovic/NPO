/// <summary> BinaryTournament.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
using Operator = JARE.Base.operators;
using SolutionSet = JARE.Base.SolutionSet;
using JARE.Base.operators.comparator;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.selection
{
	
	/// <summary> This class implements an opertor for binary selections</summary>
	[Serializable]
	public class BinaryTournament:Selection
	{
		
		/// <summary> Stores the <code>Comparator</code> used to compare two
		/// solutions
		/// </summary>
        private System.Collections.Generic.IComparer<JARE.Base.Solution> m_comparator;
		
		/// <summary> Constructor
		/// Creates a new Binary tournament operator using a BinaryTournamentComparator
		/// </summary>
		public BinaryTournament()
		{
			m_comparator = new BinaryTournamentComparator();
		} // BinaryTournament
		
		
		/// <summary> Constructor
		/// Creates a new Binary tournament with a specific <code>Comparator</code>
		/// </summary>
		/// <param name="comparator">The comparator
		/// </param>
        public BinaryTournament(System.Collections.Generic.IComparer<JARE.Base.Solution> comparator)
		{
			m_comparator = comparator;
		} // Constructor
		
		
		/// <summary> Performs the operation</summary>
		/// <param name="object">Object representing a SolutionSet
		/// </param>
		/// <returns> the selected solution
		/// </returns>
		public override System.Object execute(System.Object obj)
		{
			SolutionSet SolutionSet = (SolutionSet) obj;
			Solution solution1, solution2;
			solution1 = SolutionSet.getSolution(PseudoRandom.randInt(0, SolutionSet.size() - 1));
			solution2 = SolutionSet.getSolution(PseudoRandom.randInt(0, SolutionSet.size() - 1));
			
			int flag = m_comparator.Compare(solution1, solution2);
			if (flag == - 1)
				return solution1;
			else if (flag == 1)
				return solution2;
			else if (PseudoRandom.randDouble() < 0.5)
				return solution1;
			else
				return solution2;
		} // execute
	} // BinaryTournament
}
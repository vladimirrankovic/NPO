/// <summary> MutationImprovement.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
using Problem = JARE.Base.Problem;
using Solution = JARE.Base.Solution;
using Operator = JARE.Base.operators;
using SolutionSet = JARE.Base.SolutionSet;
using OverallConstraintViolationComparator = JARE.Base.operators.comparator.OverallConstraintViolationComparator;
using DominanceComparator = JARE.Base.operators.comparator.DominanceComparator;
using SMException = JARE.util.SMException;
namespace JARE.Base.operators.localSearch
{
	
	/// <summary> This class implements an local search operator based in the use of a 
	/// mutation operator. An archive is used to store the non-dominated solutions
	/// found during the search.
	/// </summary>
	[Serializable]
	public class MutationLocalSearch:LocalSearch
	{
		/// <summary> Returns the number of evaluations maded</summary>
		override public int Evaluations
		{
			get
			{
				return m_evaluations;
			}
			// evaluations
			
		}
		
		/// <summary> Stores the problem to solve</summary>
		private Problem m_problem;
		
		/// <summary> Stores a reference to the archive in which the non-dominated solutions are
		/// inserted
		/// </summary>
		private SolutionSet m_archive;
		
		
		/// <summary> Stores comparators for dealing with constraints and dominance checking, 
		/// respectively.
		/// </summary>
		private System.Collections.IComparer m_constraintComparator;
		private System.Collections.IComparer m_dominanceComparator;
		
		/// <summary> Stores the mutation operator </summary>
		private Operator m_mutationOperator;
		
		/// <summary> Stores the number of m_evaluations carried out</summary>
		internal int m_evaluations;
		
		
		/// <summary> Constructor. 
		/// Creates a new local search object.
		/// </summary>
		/// <param name="problem">The problem to solve
		/// </param>
		/// <param name="mutationOperator">The mutation operator 
		/// </param>
		/// <param name="archive">The archive
		/// </param>
		public MutationLocalSearch(Problem problem, Operator mutationOperator, SolutionSet archive)
		{
			m_evaluations = 0;
			m_problem = problem;
			m_archive = archive;
			m_dominanceComparator = new DominanceComparator();
			m_constraintComparator = new OverallConstraintViolationComparator();
		} //Mutation improvement
		
		
		/// <summary> Constructor. 
		/// Creates a new local search object.
		/// </summary>
		/// <param name="problem">The problem to solve
		/// </param>
		/// <param name="mutationOperator">The mutation operator 
		/// </param>
		public MutationLocalSearch(Problem problem, Operator mutationOperator)
		{
			m_evaluations = 0;
			m_problem = problem;
			m_mutationOperator = mutationOperator;
			m_dominanceComparator = new DominanceComparator();
			m_constraintComparator = new OverallConstraintViolationComparator();
		} // MutationLocalSearch
		
		/// <summary> Executes the local search. The maximum number of iterations is given by 
		/// the param "improvementRounds", which is in the parameter list of the 
		/// operator. The archive to store the non-dominated solutions is also in the 
		/// parameter list.
		/// </summary>
		/// <param name="object">Object representing a solution
		/// </param>
		/// <returns> An object containing the new improved solution
		/// </returns>
		/// <throws>  SMException  </throws>
		public override System.Object execute(System.Object obj)
		{
			int i = 0;
			int best = 0;
			m_evaluations = 0;
			Solution solution = (Solution) obj;
            //System.Int32 roundsParam = (System.Int32) getParameter("improvementRounds");
			
            m_archive = (SolutionSet) getParameter("archive");
			int rounds;
			
            //UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            
            if (getParameter("improvementRounds") == null) rounds = 0;
            else rounds = (int) getParameter("improvementRounds");
            
            //if (roundsParam == null)
            //    rounds = 0;
            //else
            //    rounds = roundsParam;
			
			if (rounds <= 0)
				return new Solution(solution);
			
			do 
			{
				i++;
				Solution mutatedSolution = new Solution(solution);
				m_mutationOperator.execute(mutatedSolution);
				
				
				// Evaluate the getNumberOfConstraints
				if (m_problem.NumberOfConstraints > 0)
				{
					m_problem.evaluateConstraints(mutatedSolution);
					best = m_constraintComparator.Compare(mutatedSolution, solution);
					if (best == 0)
					//none of then is better that the other one
					{
						m_problem.evaluate(mutatedSolution);
						m_evaluations++;
						best = m_dominanceComparator.Compare(mutatedSolution, solution);
					}
					else if (best == - 1)
					//mutatedSolution is best
					{
						m_problem.evaluate(mutatedSolution);
						m_evaluations++;
					}
				}
				else
				{
					m_problem.evaluate(mutatedSolution);
					m_evaluations++;
					best = m_dominanceComparator.Compare(mutatedSolution, solution);
				}
				if (best == - 1)
				// This is: Mutated is best
					solution = mutatedSolution;
				else if (best == 1)
				// This is: Original is best
				//delete mutatedSolution
				{
				}
				// This is mutatedSolution and original are non-dominated
				else
				{
					//this.archive_.addIndividual(new Solution(solution));                
					//solution = mutatedSolution;
					if (m_archive != null)
						m_archive.add(mutatedSolution);
				}
			}
			while (i < rounds);
			return new Solution(solution);
		} // execute
	} // MutationLocalSearch
}
/// <summary> RankingAndCrowdingSelection.java</summary>
/// <author>  Juan J. Durillo
/// </author>
using System;
using Operator = JARE.Base.operators;
using Problem = JARE.Base.Problem;
using SolutionSet = JARE.Base.SolutionSet;
using Configuration = JARE.util.Configuration;
using Distance = JARE.util.Distance;
using SMException = JARE.util.SMException;
using Ranking = JARE.util.Ranking;
using JARE.Base.operators.comparator;

namespace JARE.Base.operators.selection
{
	
	/// <summary> This class implements a selection for selecting a number of solutions from
	/// a solutionSet. The solutions are taken by mean of its ranking and 
	/// crowding ditance values.
	/// NOTE: if you use the default constructor, the problem has to be passed as
	/// a parameter before invoking the execute() method -- see lines 67 - 74
	/// </summary>
	[Serializable]
	public class RankingAndCrowdingSelection:Selection
	{
		
		/// <summary> stores the problem to solve </summary>
		private Problem m_problem;
		
		/// <summary> stores a <code>Comparator</code> for crowding comparator checking.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'crowdingComparator_ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly System.Collections.Generic.IComparer<JARE.Base.Solution> m_crowdingComparator = new CrowdingComparator();
		
		
		/// <summary> stores a <code>Distance</code> object for distance utilities.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_distance '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly Distance m_distance = new Distance();
		
		/// <summary> Constructor</summary>
		public RankingAndCrowdingSelection()
		{
			m_problem = null;
		} // RankingAndCrowdingSelection
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to be solved
		/// </param>
		public RankingAndCrowdingSelection(Problem problem)
		{
			m_problem = problem;
		} // RankingAndCrowdingSelection
		
		/// <summary> Performs the operation</summary>
		/// <param name="object">Object representing a SolutionSet.
		/// </param>
		/// <returns> an object representing a <code>SolutionSet<code> with the selected parents
		/// </returns>
		/// <throws>  SMException  </throws>
		public override System.Object execute(System.Object obj)
		{
			SolutionSet population = (SolutionSet) obj;
			int populationSize = (System.Int32) m_parameters["populationSize"];
			SolutionSet result = new SolutionSet(populationSize);
			
			if (m_problem == null)
			{
				m_problem = (Problem) getParameter("problem");
				if (m_problem == null)
				{
					
					Configuration.m_logger.WriteLog("RankingAndCrowdingSelection.execute: " + "problem not specified");
                    //System.Type cls = typeof(System.String);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    string name = this.GetType().FullName; 
                    throw new SMException("Exception in " + name + ".execute()");
				} // if
			} // if
			
			//->Ranking the union
			Ranking ranking = new Ranking(population);
			
			int remain = populationSize;
			int index = 0;
			SolutionSet front = null;
			population.clear();
			
			//-> Obtain the next front
			front = ranking.getSubfront(index);
			
			while ((remain > 0) && (remain >= front.size()))
			{
				//Asign crowding distance to individuals
				m_distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
				//Add the individuals of this front
				for (int k = 0; k < front.size(); k++)
				{
					result.add(front.getSolution(k));
				} // for
				
				//Decrement remaint
				remain = remain - front.size();
				
				//Obtain the next front
				index++;
				if (remain > 0)
				{
					front = ranking.getSubfront(index);
				} // if        
			} // while
			
			//-> remain is less than front(index).size, insert only the best one
			if (remain > 0)
			{
				// front containt individuals to insert                        
				m_distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
     
				front.sort(m_crowdingComparator);
				for (int k = 0; k < remain; k++)
				{
					result.add(front.getSolution(k));
				} // for
				
				remain = 0;
			} // if
			
			return result;
		} // execute    
	} // RankingAndCrowding
}
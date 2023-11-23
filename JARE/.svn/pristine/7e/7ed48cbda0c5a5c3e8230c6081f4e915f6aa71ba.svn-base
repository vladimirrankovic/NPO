/// <summary> FPGAFitness.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.comparator;
namespace JARE.util
{
	
	/// <summary> This class implements facilities for calculating the fitness for the
	/// FPGA algorithm
	/// </summary>
	public class FPGAFitness
	{
		/// <summary> Need the population to assign the fitness, this population may contain
		/// solutions in the population and the archive
		/// </summary>
		private SolutionSet m_solutionSet = null;
		
		/// <summary> problem to solve</summary>
		private Problem m_problem = null;
		
		/// <summary> stores a <code>Comparator</code> for dominance checking</summary>
		private static readonly System.Collections.IComparer m_dominance = new DominanceComparator();
		
		/// <summary> Constructor.
		/// Create a new instance of Spea2Fitness
		/// </summary>
		/// <param name="solutionSet">The solutionSet to assign the fitness
		/// </param>
		/// <param name="problem">The problem to solve
		/// </param>
		public FPGAFitness(SolutionSet solutionSet, Problem problem)
		{
			m_solutionSet = solutionSet;
			m_problem = problem;
			for (int i = 0; i < m_solutionSet.size(); i++)
			{
				m_solutionSet.getSolution(i).Location = i;
			} 
		} 
		
		
		/// <summary> Assign FPGA fitness to the solutions. Similar to the SPEA2 fitness.</summary>
		public virtual void  fitnessAssign()
		{
			double[] strength = new double[m_solutionSet.size()];
			double[] rawFitness = new double[m_solutionSet.size()];
			
			//Ranking  ranking  = new Ranking(m_solutionSet);
			//Distance distance = new Distance();
			//distance.crowdingDistanceAsignament(ranking.getSubfront(0),
			//                                    m_problem.getNumberOfObjectives());  
			
			for (int i = 0; i < m_solutionSet.size(); i++)
			{
				if (m_solutionSet.getSolution(i).Rank == 0)
				{
					m_solutionSet.getSolution(i).Fitness = m_solutionSet.getSolution(i).CrowdingDistance;
					//System.out.println(m_solutionSet.get(i).getCrowdingDistance());
				}
			}
			
			//Calculate the strength value
			// strength(i) = |{j | j <- SolutionSet and i dominate j}|
			for (int i = 0; i < m_solutionSet.size(); i++)
			{
				for (int j = 0; j < m_solutionSet.size(); j++)
				{
					if (m_dominance.Compare(m_solutionSet.getSolution(i), m_solutionSet.getSolution(j)) == - 1)
					{
						strength[i] += 1.0;
					}     
				}
			} 
			
			
			//Calculate the fitness
			//F(i) = sum(strength(j) | i dominate j) - sum(strenth(j) | j dominate i)
			for (int i = 0; i < m_solutionSet.size(); i++)
			{
				double fitness = 0.0;
				for (int j = 0; j < m_solutionSet.size(); j++)
				{
					int flag = m_dominance.Compare(m_solutionSet.getSolution(i), m_solutionSet.getSolution(j));
					if (flag == - 1)
					{
						//i domiante j
						fitness += strength[j];
					}
					else if (flag == 1)
					{
						fitness -= strength[j];
					} 
				} 
			} 
		} 
	} 
}
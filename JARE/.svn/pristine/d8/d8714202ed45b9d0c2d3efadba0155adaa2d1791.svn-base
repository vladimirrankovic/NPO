/// <summary> AbYSS_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// MOCell_Settings class of algorithm AbYSS
/// </version>
using System;
using JARE.metaheuristics.abyss;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using Problem = JARE.Base.Problem;
using CrossoverFactory = JARE.Base.operators.crossover.CrossoverFactory;
using MutationLocalSearch = JARE.Base.operators.localSearch.MutationLocalSearch;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using Settings = JARE.experiments.settings;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	/// <summary> Constructor</summary>
	public class AbYSS_Settings:Settings
	{
		private void  InitBlock()
		{
			m_mutationProbability = 1.0 / m_problem.NumberOfVariables;
		}
		// Default settings
		public int m_populationSize = 20;
		public int m_maxEvaluations = 25000;
		public int m_archiveSize = 100;
		public int m_refSet1Size = 10;
		public int m_refSet2Size = 10;
		//UPGRADE_NOTE: The initialization of  'm_mutationProbability' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public double m_mutationProbability;
		public double m_crossoverProbability = 1.0;
		public double m_distributionIndexForMutation = 20;
		public double m_distributionIndexForCrossover = 20;
		public int m_improvementRounds = 1;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public AbYSS_Settings(Problem problem):base(problem)
		{
			InitBlock();
		} // AbYSS_Settings
		
		/// <summary> Configure the MOCell algorithm with default parameter settings</summary>
		/// <returns> an algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			Operator crossover;
			Operator mutation;
			Operator improvement; // Operator for improvement
			
			QualityIndicator indicators;
			
			// Creating the problem
			algorithm = new AbYSS(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", m_populationSize);
			algorithm.setInputParameter("refSet1Size", m_refSet1Size);
			algorithm.setInputParameter("refSet2Size", m_refSet2Size);
			algorithm.setInputParameter("archiveSize", m_archiveSize);
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			
			// Mutation and Crossover for Real codification 
			crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
			crossover.setParameter("probability", m_crossoverProbability);
			crossover.setParameter("distributionIndex", m_distributionIndexForCrossover);
			
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", m_mutationProbability);
			mutation.setParameter("distributionIndex", m_distributionIndexForMutation);
			
			improvement = new MutationLocalSearch(m_problem, mutation);
			improvement.setParameter("improvementRounds", m_improvementRounds);
			
			// STEP 6. Add the operators to the algorithm
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("improvement", improvement);
			
			// Creating the indicator object
			if (!m_paretoFrontFile.Equals(""))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			return algorithm;
		} // Constructor
	} // AbYSS_Settings
}
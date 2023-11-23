/// <summary> NSGAII_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// NSGAII_Settings class of algorithm NSGAII
/// </version>
using System;
using JARE.metaheuristics.nsgaII;
using JARE.Base;
using Crossover = JARE.Base.operators.crossover.Crossover;
using Mutation = JARE.Base.operators.mutation.Mutation;
using CrossoverFactory = JARE.Base.operators.crossover.CrossoverFactory;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using Selection = JARE.Base.operators.selection.Selection;
using SelectionFactory = JARE.Base.operators.selection.SelectionFactory;
using Settings = JARE.experiments.Settings;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	/// <author>  Antonio J. Nebro
	/// </author>
	public class NSGAII_Settings:Settings
	{
		private void  InitBlock()
		{
			m_mutationProbability = 1.0 / m_problem.NumberOfVariables;
		}
		public int m_populationSize = 100;
		public int m_maxEvaluations = 25000;
		//UPGRADE_NOTE: The initialization of  'm_mutationProbability' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public double m_mutationProbability;
		public double m_crossoverProbability = 0.9;
		public double m_mutationDistributionIndex = 20.0;
		public double m_crossoverDistributionIndex = 20.0;
		
		/// <summary> Constructor</summary>
		/// <throws>  SMException  </throws>
		public NSGAII_Settings(Problem problem):base(problem)
		{
			InitBlock();
		} // NSGAII_Settings
		
		
		/// <summary> Configure NSGAII with user-defined parameter settings</summary>
		/// <returns> A NSGAII algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			Selection selection;
			Crossover crossover;
			Mutation mutation;
			
			QualityIndicator indicators;
			
			// Creating the algorithm. There are two choices: NSGAII and its steady-
			// state variant ssNSGAII
			algorithm = new NSGAII(m_problem);
			//algorithm = new ssNSGAII(m_problem) ;
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", m_populationSize);
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			
			// Mutation and Crossover for Real codification
			crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
			crossover.setParameter("probability", m_crossoverProbability);
			crossover.setParameter("distributionIndex", m_crossoverDistributionIndex);
			
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", m_mutationProbability);
			mutation.setParameter("distributionIndex", m_crossoverDistributionIndex);
			
			// Selection Operator
			selection = (Selection) SelectionFactory.getSelectionOperator("BinaryTournament2");
			
			// Add the operators to the algorithm
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("mutation", mutation);
			algorithm.addOperator("selection", selection);
			
			// Creating the indicator object
			if ((m_paretoFrontFile != null) && (!m_paretoFrontFile.Equals("")))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			
			return algorithm;
		} // configure
	} // NSGAII_Settings
}
/// <summary> NSGAIIBinary_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Example of using a binary representation in NSGAII
/// </version>
using System;
using JARE.metaheuristics.nsgaII;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using Problem = JARE.Base.Problem;
using Solution = JARE.Base.Solution;
using CrossoverFactory = JARE.Base.operators.crossover.CrossoverFactory;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using SelectionFactory = JARE.Base.operators.selection.SelectionFactory;
using Settings = JARE.experiments.Settings;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	/// <summary> </summary>
	/// <author>  Antonio J. Nebro
	/// </author>
	public class NSGAIIBinary_Settings:Settings
	{
		private void  InitBlock()
		{
			m_mutationProbability = 1.0 / m_problem.NumberOfBits;
		}
		
		// Default settings
		internal int m_populationSize = 100;
		internal int m_maxEvaluations = 25000;
		
		//UPGRADE_NOTE: The initialization of  'm_mutationProbability' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		internal double m_mutationProbability;
		internal double m_crossoverProbability = 0.9;
		
		/// <summary> Constructor</summary>
		public NSGAIIBinary_Settings(Problem problem):base(problem)
		{
			InitBlock();
		} // NSGAIIBinary_Settings
		
		/// <summary> Configure NSGAII with user-defined parameter settings</summary>
		/// <returns> A NSGAII algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			Operator selection;
			Operator crossover;
			Operator mutation;
			
			QualityIndicator indicators;
			
			// Creating the problem
			algorithm = new NSGAII(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", m_populationSize);
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			
			
			// Mutation and Crossover Binary codification
			crossover = CrossoverFactory.getCrossoverOperator("SinglePointCrossover");
			crossover.setParameter("probability", 0.9);
			mutation = MutationFactory.getMutationOperator("BitFlipMutation");
			mutation.setParameter("probability", m_mutationProbability);
			
			// Selection Operator 
			selection = SelectionFactory.getSelectionOperator("BinaryTournament2");
			
			// Add the operators to the algorithm
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("mutation", mutation);
			algorithm.addOperator("selection", selection);
			
			// Creating the indicator object
			if (!m_paretoFrontFile.Equals(""))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			return algorithm;
		} // configure
	} // NSGAIIBinary_Settings
}
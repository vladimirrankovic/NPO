/// <summary> SPEA2_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// NSGAII_Settings class of algorithm NSGAII
/// </version>
using System;
using JARE.metaheuristics.spea2;
using JARE.Base;
using CrossoverFactory = JARE.Base.operators.crossover.CrossoverFactory;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using Crossover = JARE.Base.operators.crossover.Crossover;
using Mutation = JARE.Base.operators.mutation.Mutation;
using SelectionFactory = JARE.Base.operators.selection.SelectionFactory;
using Settings = JARE.experiments.Settings;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	public class SPEA2_Settings:Settings
	{
		private void  InitBlock()
		{
			m_mutationProbability = 1.0 / m_problem.NumberOfVariables;
		}
		
		public int m_populationSize = 100;
		public int m_archiveSize = 100;
		public int m_maxEvaluations = 25000;
		//UPGRADE_NOTE: The initialization of  'm_mutationProbability' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public double m_mutationProbability;
		public double m_crossoverProbability = 0.9;
		public double m_distributionIndexForCrossover = 20.0;
		public double m_distributionIndexForMutation = 20.0;
		
		/// <summary> Constructor</summary>
		public SPEA2_Settings(Problem problem):base(problem)
		{
			InitBlock();
		} // SPEA2_Settings
		
		/// <summary> Configure SPEA2 with default parameter settings</summary>
		/// <returns> an algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			Operator crossover; // Crossover operator
			Operator mutation; // Mutation operator
			Operator selection; // Selection operator
			
			QualityIndicator indicators;
			
			// Creating the problem
			algorithm = new SPEA2(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", m_populationSize);
			algorithm.setInputParameter("archiveSize", m_archiveSize);
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			
			// Mutation and crossover for real codification
			crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
			crossover.setParameter("probability", m_crossoverProbability);
			crossover.setParameter("distributionIndex", m_distributionIndexForCrossover);
			
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", m_mutationProbability);
			mutation.setParameter("distributionIndex", m_distributionIndexForMutation);
			
			// Selection operator 
			selection = SelectionFactory.getSelectionOperator("BinaryTournament");
			
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
	} // SPEA2_Settings
}
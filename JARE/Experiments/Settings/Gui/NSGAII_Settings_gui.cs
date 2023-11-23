/// <summary> NSGAII_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// NSGAII_Settings_gui class of algorithm NSGAII
/// </version>
using System;
using Algorithm = SharpMetal.Base.Algorithm;
using Operator = SharpMetal.Base.Operator;
using Crossover = SharpMetal.Base.operators.crossover.Crossover;
using Mutation = SharpMetal.Base.operators.mutation.Mutation;
using CrossoverFactory = SharpMetal.Base.operators.crossover.CrossoverFactory;
using MutationFactory = SharpMetal.Base.operators.mutation.MutationFactory;
using SelectionFactory = SharpMetal.Base.operators.selection.SelectionFactory;
using NSGAII = SharpMetal.metaheuristics.nsgaII.NSGAII;
using QualityIndicator = SharpMetal.qualityIndicator.QualityIndicator;
using SMException = SharpMetal.util.SMException;
namespace SharpMetal.experiments.settings.gui
{
	
	/// <author>  Antonio J. Nebro
	/// </author>
	public class NSGAII_Settings_gui:Settings_gui
	{
		public int m_populationSize;
		public int m_maxEvaluations;
		public double m_mutationProbability;
		public double m_crossoverProbability;
		public Mutation m_mutation;
		public Crossover m_crossover;
		public double m_mutationDistributionIndex;
		public double m_crossoverDistributionIndex;
		
		/// <summary> Constructor</summary>
		/// <throws>  SMException </throws>
		public NSGAII_Settings_gui()
		{
			m_populationSize = 100;
			m_maxEvaluations = 25000;
			m_mutationProbability = 0.0; // This value will be ignored    
			m_crossoverProbability = 0.9;
			m_paretoFrontFile = "";
			m_mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			m_crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
		} // NSGAII_Settings
		
		
		public override Algorithm configure()
		{
			Algorithm algorithm;
			Operator selection;
			
			QualityIndicator indicators;
			
			// Creating the algorithm. There are two choices: NSGAII and its steady-
			// state variant ssNSGAII
			algorithm = new NSGAII(m_problem);
			//algorithm = new ssNSGAII(m_problem) ;
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", m_populationSize);
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			
			// Mutation and Crossover for Real codification
			
			
			
			
			
			
			// Selection Operator
			selection = SelectionFactory.getSelectionOperator("BinaryTournament2");
			
			// Add the operators to the algorithm
			algorithm.addOperator("crossover", m_crossover);
			algorithm.addOperator("mutation", m_mutation);
			algorithm.addOperator("selection", selection);
			
			// Creating the indicator object
			if ((m_paretoFrontFile != null) && (!m_paretoFrontFile.Equals("")))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			
			return algorithm;
		} // configure
	} // NSGAII_Settings_gui
}
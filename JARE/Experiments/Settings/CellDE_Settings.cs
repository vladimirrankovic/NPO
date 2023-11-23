/// <summary> CellDE_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// CellDE_Settings class of algorithm CellDE
/// </version>
using System;
using JARE.metaheuristics.cellde;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using Problem = JARE.Base.Problem;
using CrossoverFactory = JARE.Base.operators.crossover.CrossoverFactory;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using SelectionFactory = JARE.Base.operators.selection.SelectionFactory;
using Settings = JARE.experiments.Settings;
////////using PropUtils = JARE.gui.utils.PropUtils; Vlada: Ova klasa se uopste ne koristi u ovom fajlu
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	/// <summary> </summary>
	/// <author>  Antonio
	/// </author>
	public class CellDE_Settings:Settings
	{
		
		// Default settings
		public double m_CR = 0.5;
		public double m_F = 0.5;
		
		public int m_populationSize = 100;
		public int m_archiveSize = 100;
		public int m_maxEvaluations = 25000;
		public int m_archiveFeedback = 20;
		
		/// <summary> Constructor</summary>
		public CellDE_Settings(Problem problem):base(problem)
		{
		} // CellDE_Settings
		
		/// <summary> Configure the algorithm with the specified parameter settings</summary>
		/// <returns> an algorithm object
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
			algorithm = new CellDE(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", m_populationSize);
			algorithm.setInputParameter("archiveSize", m_archiveSize);
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			algorithm.setInputParameter("feedBack", m_archiveFeedback);
			
			// Crossover operator 
			crossover = CrossoverFactory.getCrossoverOperator("DifferentialEvolutionCrossover");
			crossover.setParameter("CR", m_CR);
			crossover.setParameter("F", m_F);
			
			// Add the operators to the algorithm
			selection = SelectionFactory.getSelectionOperator("BinaryTournament");
			
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("selection", selection);
			
			// Creating the indicator object
			if (!m_paretoFrontFile.Equals(""))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			
			return algorithm;
		} // configure
	} // CellDE_Settings
}
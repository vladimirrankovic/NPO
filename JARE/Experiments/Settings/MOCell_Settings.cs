/// <summary> MOCell_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// MOCell_Settings class of algorithm MOCell
/// </version>
using System;
using JARE.metaheuristics.mocell;
using JARE.Base;
using CrossoverFactory = JARE.Base.operators.crossover.CrossoverFactory;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using Crossover = JARE.Base.operators.crossover.Crossover;
using Mutation = JARE.Base.operators.mutation.Mutation;
using SelectionFactory = JARE.Base.operators.selection.SelectionFactory;
using Settings = JARE.experiments.Settings;
////////using PropUtils = JARE.gui.utils.PropUtils; Vlada: Ova klasa se uopste ne koristi u ovom fajlu
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	
	public class MOCell_Settings:Settings
	{
		private void  InitBlock()
		{
			m_mutationProbability = 1.0 / m_problem.NumberOfVariables;
		}
		
		// Default settings
		public int m_populationSize = 100;
		public int m_maxEvaluations = 25000;
		public int m_archiveSize = 100;
		public int m_feedback = 20;
		//UPGRADE_NOTE: The initialization of  'm_mutationProbability' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public double m_mutationProbability;
		public double m_crossoverProbability = 0.9;
		public double m_distributionIndexForMutation = 20.0;
		public double m_distributionIndexForCrossover = 20.0;
		
		/// <summary> Constructor</summary>
		public MOCell_Settings(Problem problem):base(problem)
		{
			InitBlock();
		} // MOCell_Settings
		
		/// <summary> Configure the MOCell algorithm with default parameter settings</summary>
		/// <returns> an algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			
			Crossover crossover;
			Mutation mutation;
			Operator selection;
			
			QualityIndicator indicators;
			
			// Selecting the algorithm: there are six MOCell variants
			//algorithm = new sMOCell1(m_problem) ;
			//algorithm = new sMOCell2(m_problem) ;
			//algorithm = new aMOCell1(m_problem) ;
			//algorithm = new aMOCell2(m_problem) ;
			//algorithm = new aMOCell3(m_problem) ;
			algorithm = new MOCell(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", m_populationSize);
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			algorithm.setInputParameter("archiveSize", m_archiveSize);
			algorithm.setInputParameter("feedBack", m_feedback);
			
			
			// Mutation and Crossover for Real codification 
			crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
			crossover.setParameter("probability", m_crossoverProbability);
			crossover.setParameter("distributionIndex", m_distributionIndexForCrossover);
			
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", m_mutationProbability);
			mutation.setParameter("distributionIndex", m_distributionIndexForMutation);
			
			// Selection Operator 
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
	} // MOCell_Settings
}
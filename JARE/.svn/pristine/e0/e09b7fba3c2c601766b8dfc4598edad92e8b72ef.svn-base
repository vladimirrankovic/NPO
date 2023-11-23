/// <summary> IBEA_Settings.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// IBEA_Settings class of algorithm IBEA
/// </version>
using System;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using Problem = JARE.Base.Problem;
using FitnessComparator = JARE.Base.operators.comparator.FitnessComparator;
using CrossoverFactory = JARE.Base.operators.crossover.CrossoverFactory;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using BinaryTournament = JARE.Base.operators.selection.BinaryTournament;
using Settings = JARE.experiments.Settings;
using IBEA = JARE.metaheuristics.ibea.IBEA;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	public class IBEA_Settings:Settings
	{
		private void  InitBlock()
		{
			m_mutationProbability = 1.0 / m_problem.NumberOfVariables;
		}
		
		// Default settings
		public int m_populationSize = 100;
		public int m_maxEvaluations = 25000;
		public int m_archiveSize = 100;
		
		//UPGRADE_NOTE: The initialization of  'm_mutationProbability' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public double m_mutationProbability;
		public double m_crossoverProbability = 0.9;
		
		public double m_distributionIndexForMutation = 20;
		public double m_distributionIndexForCrossover = 20;
		
		/// <summary> Constructor</summary>
		public IBEA_Settings(Problem problem):base(problem)
		{
			InitBlock();
		} // IBEA_Settings
		
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
			algorithm = new IBEA(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", m_populationSize);
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			algorithm.setInputParameter("archiveSize", m_archiveSize);
			
			// Mutation and Crossover for Real codification 
			crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
			crossover.setParameter("probability", m_crossoverProbability);
			crossover.setParameter("distributionIndex", m_distributionIndexForCrossover);
			
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", m_mutationProbability);
			mutation.setParameter("distributionIndex", m_distributionIndexForMutation);
			
			// Selection Operator 
			selection = new BinaryTournament(new FitnessComparator());
			
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
	} // IBEA_Settings
}
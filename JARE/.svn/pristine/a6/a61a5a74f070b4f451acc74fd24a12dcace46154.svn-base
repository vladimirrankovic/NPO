/// <summary> PAES_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// PAES_Settings class of algorithm PAES
/// </version>
using System;
using JARE.metaheuristics.paes;
using Algorithm = JARE.Base.Algorithm;
using Mutation = JARE.Base.operators.mutation.Mutation;
using Problem = JARE.Base.Problem;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using Settings = JARE.experiments.Settings;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	public class PAES_Settings:Settings
	{
		private void  InitBlock()
		{
			m_mutationProbability = 1.0 / m_problem.NumberOfVariables;
		}
		
		// Default settings
		public int m_maxEvaluations = 25000;
		public int m_archiveSize = 100;
		public int m_biSections = 5;
		//UPGRADE_NOTE: The initialization of  'm_mutationProbability' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public double m_mutationProbability;
		public double m_distributionIndex = 20.0;
		
		/// <summary> Constructor</summary>
		public PAES_Settings(Problem problem):base(problem)
		{
			InitBlock();
		} // PAES_Settings
		
		/// <summary> Configure the MOCell algorithm with default parameter settings</summary>
		/// <returns> an algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			Mutation mutation;
			
			QualityIndicator indicators;
			
			// Creating the problem
			algorithm = new PAES(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			algorithm.setInputParameter("biSections", m_biSections);
			algorithm.setInputParameter("archiveSize", m_archiveSize);
			
			// Add the operators to the algorithm
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", m_mutationProbability);
			mutation.setParameter("distributionIndex", m_distributionIndex);
			
			algorithm.addOperator("mutation", mutation);
			
			// Creating the indicator object
			if (!m_paretoFrontFile.Equals(""))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			return algorithm;
		} // configure
	} // PAES_Settings
}
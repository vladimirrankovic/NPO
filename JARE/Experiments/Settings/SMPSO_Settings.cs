/// <summary> SMPSO_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// SMPSO_Settings class of algorithm SMPSO
/// </version>
using System;
using JARE.metaheuristics.smpso;
using Algorithm = JARE.Base.Algorithm;
using Problem = JARE.Base.Problem;
using Mutation = JARE.Base.operators.mutation.Mutation;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using Settings = JARE.experiments.Settings;
////////using PropUtils = JARE.gui.utils.PropUtils; Vlada: Ova klasa se uopste ne koristi u ovom fajlu
using ZDT1 = JARE.problems.ZDT.ZDT1;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	/// <summary> </summary>
	/// <author>  Antonio
	/// </author>
	public class SMPSO_Settings:Settings
	{
		private void  InitBlock()
		{
			m_mutationProbability = 1.0 / m_problem.NumberOfVariables;
		}
		
		// Default settings
		public int m_swarmSize = 100;
		public int m_maxIterations = 250;
		public int m_archiveSize = 100;
		public double m_mutationDistributionIndex = 20.0;
		//UPGRADE_NOTE: The initialization of  'm_mutationProbability' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public double m_mutationProbability;
		
		/// <summary> Constructor</summary>
		public SMPSO_Settings(Problem problem):base(problem)
		{
			InitBlock();
		} // SMPSO_Settings
		
		/// <summary> Configure NSGAII with user-defined parameter settings</summary>
		/// <returns> A NSGAII algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			Mutation mutation;
			
			QualityIndicator indicators;
			
			// Creating the problem
			algorithm = new SMPSO(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("swarmSize", m_swarmSize);
			algorithm.setInputParameter("maxIterations", m_maxIterations);
			algorithm.setInputParameter("archiveSize", m_archiveSize);
			
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", m_mutationProbability);
			mutation.setParameter("distributionIndex", m_mutationDistributionIndex);
			
			algorithm.addOperator("mutation", mutation);
			
			// Creating the indicator object
			if (!m_paretoFrontFile.Equals(""))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			
			return algorithm;
		} // Configure
	} // SMPSO_Settings
}
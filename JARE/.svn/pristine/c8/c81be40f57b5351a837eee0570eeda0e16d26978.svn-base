/// <summary> RandomSearch_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Settings class of algorithm RandomSearch
/// </version>
using System;
using RandomSearch = JARE.metaheuristics.randomSearch.RandomSearch;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using Problem = JARE.Base.Problem;
using Settings = JARE.experiments.Settings;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	/// <summary> Constructor</summary>
	public class RandomSearch_Settings:Settings
	{
		// Default settings
		public int m_maxEvaluations = 25000;
		
		/// <summary> Constructor</summary>
		/// <param name="problem">Problem to solve
		/// </param>
		public RandomSearch_Settings(Problem problem):base(problem)
		{
		} // AbYSS_Settings
		
		/// <summary> Configure the MOCell algorithm with default parameter settings</summary>
		/// <returns> an algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			
			QualityIndicator indicators;
			
			// Creating the problem
			algorithm = new RandomSearch(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			
			// Creating the indicator object
			if (!m_paretoFrontFile.Equals(""))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			return algorithm;
		} // Constructor
	} // RandomSearch_Settings
}
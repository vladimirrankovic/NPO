/// <summary> OMOPSO_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// OMOPSO_Settings class of algorithm OMOPSO
/// </version>
using System;
using JARE.metaheuristics.omopso;
using Algorithm = JARE.Base.Algorithm;
using Problem = JARE.Base.Problem;
using Settings = JARE.experiments.Settings;
////////using PropUtils = JARE.gui.utils.PropUtils; Vlada: Ova klasa se uopste ne koristi u ovom fajlu
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	public class OMOPSO_Settings:Settings
	{
		
		// Default settings
		public int m_swarmSize = 100;
		public int m_maxIterations = 250;
		public int m_archiveSize = 100;
		public double m_perturbationIndex = 0.5;
		
		/// <summary> Constructor</summary>
		public OMOPSO_Settings(Problem problem):base(problem)
		{
		} // OMOPSO_Settings
		
		/// <summary> Configure OMOPSO with user-defined parameter settings</summary>
		/// <returns> A OMOPSO algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			
			QualityIndicator indicators;
			
			// Creating the problem
			algorithm = new OMOPSO(m_problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("swarmSize", m_swarmSize);
			algorithm.setInputParameter("maxIterations", m_maxIterations);
			algorithm.setInputParameter("archiveSize", m_archiveSize);
			algorithm.setInputParameter("perturbationIndex", m_perturbationIndex);
			
			// Creating the indicator object
			if (!m_paretoFrontFile.Equals(""))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			return algorithm;
		} // configure
	} // OMOPSO_Settings
}
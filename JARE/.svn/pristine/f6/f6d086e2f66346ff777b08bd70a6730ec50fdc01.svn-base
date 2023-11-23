/// <summary> MOEAD_Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// MOEAD_Settings class of algorithm MOEAD and pMOEAD
/// </version>
using System;
using JARE.metaheuristics.moead;
using Algorithm = JARE.Base.Algorithm;
using Operator = JARE.Base.Operator;
using Problem = JARE.Base.Problem;
using CrossoverFactory = JARE.Base.operators.crossover.CrossoverFactory;
using MutationFactory = JARE.Base.operators.mutation.MutationFactory;
using SelectionFactory = JARE.Base.operators.selection.SelectionFactory;
using Settings = JARE.experiments.Settings;
using ProblemFactory = JARE.problems.ProblemFactory;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using SMException = JARE.util.SMException;
namespace JARE.experiments.settings
{
	
	public class MOEAD_Settings:Settings
	{
		private void  InitBlock()
		{
			m_mutationProbability = 1.0 / m_problem.NumberOfVariables;
		}
		// Default settings
		public double m_CR = 1.0;
		public double m_F = 0.5;
		public int m_populationSize = 600;
		public int m_maxEvaluations = 150000;
		
		//UPGRADE_NOTE: The initialization of  'm_mutationProbability' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public double m_mutationProbability;
		public double m_distributionIndexForMutation = 20;
		
		// Directory with the files containing the weight vectors used in 
		// Q. Zhang,  W. Liu,  and H Li, The Performance of a New Version of MOEA/D 
		// on CEC09 Unconstrained MOP Test Instances Working Report CES-491, School 
		// of CS & EE, University of Essex, 02/2009.
		// http://dces.essex.ac.uk/staff/qzhang/MOEAcompetition/CEC09final/code/ZhangMOEADcode/moead0305.rar
		public System.String m_dataDirectory = "/Users/antonio/Softw/pruebas/data/MOEAD_parameters/Weight";
		
		public int numberOfThreads = 2; // Parameter of pMOEAD
		public System.String moeadVersion = "pMOEAD"; // or "pMOEAD"
		
		/// <summary> Constructor</summary>
		public MOEAD_Settings(Problem problem):base(problem)
		{
			InitBlock();
		} // MOEAD_Settings
		
		/// <summary> Configure the algorithm with the specified parameter settings</summary>
		/// <returns> an algorithm object
		/// </returns>
		/// <throws>  JARE.util.SMException </throws>
		public override Algorithm configure()
		{
			Algorithm algorithm;
			Operator crossover;
			Operator mutation;
			
			QualityIndicator indicators;
			
			// Creating the problem
			if (String.CompareOrdinal(moeadVersion, "MOEAD") == 0)
				algorithm = new MOEAD(m_problem);
			else
			{
				// pMOEAD
				algorithm = new pMOEAD(m_problem);
				algorithm.setInputParameter("numberOfThreads", numberOfThreads);
			} // else
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", m_populationSize);
			algorithm.setInputParameter("maxEvaluations", m_maxEvaluations);
			algorithm.setInputParameter("dataDirectory", m_dataDirectory);
			
			// Crossover operator 
			crossover = CrossoverFactory.getCrossoverOperator("DifferentialEvolutionCrossover");
			crossover.setParameter("CR", m_CR);
			crossover.setParameter("F", m_F);
			
			// Mutation operator
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", m_mutationProbability);
			mutation.setParameter("distributionIndex", m_distributionIndexForMutation);
			
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("mutation", mutation);
			
			// Creating the indicator object
			if (!m_paretoFrontFile.Equals(""))
			{
				indicators = new QualityIndicator(m_problem, m_paretoFrontFile);
				algorithm.setInputParameter("indicators", indicators);
			} // if
			
			return algorithm;
		} // configure
	} // MOEAD_Settings
}
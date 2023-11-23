/// <summary> MOEAD_main.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// This class executes the algorithm described in:
/// H. Li and Q. Zhang, 
/// "Multiobjective Optimization Problems with Complicated Pareto Sets,  MOEA/D 
/// and NSGA-II". IEEE Trans on Evolutionary Computation, vol. 12,  no 2,  
/// pp 284-302, April/2009.  
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.crossover;
using JARE.Base.operators.mutation;
using JARE.Base.operators.selection;
using JARE.problems;
using JARE.problems.DTLZ;
using JARE.problems.ZDT;
using JARE.problems.WFG;
using JARE.problems.LZ09;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using Logger = JARE.util.Logger;
//UPGRADE_TODO: The type 'java.util.logging.FileHandler' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//using FileHandler = java.util.logging.FileHandler;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//using Logger = java.util.logging.Logger;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
namespace JARE.metaheuristics.moead
{
	
	public class MOEAD_main
	{
        public static Logger m_logger; // Logger object
        //public static FileHandler fileHandler_; // FileHandler object
		
		/// <param name="args">Command line arguments. The first (optional) argument specifies 
		/// the problem to solve.
		/// </param>
		/// <throws>  SMException  </throws>
		/// <throws>  IOException  </throws>
		/// <throws>  SecurityException  </throws>
		/// <summary> Usage: three options
		/// - JARE.metaheuristics.moead.MOEAD_main
		/// - JARE.metaheuristics.moead.MOEAD_main problemName
		/// - JARE.metaheuristics.moead.MOEAD_main problemName ParetoFrontFile
		/// </summary>
		/// <throws>  ClassNotFoundException  </throws>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			Problem problem; // The problem to solve
			Algorithm algorithm; // The algorithm to use
			Operator crossover; // Crossover operator
			Operator mutation; // Mutation operator
			
			QualityIndicator indicators; // Object to get quality indicators
			
			// Logger object and file to store log messages
			m_logger = Configuration.m_logger;
            //fileHandler_ = new FileHandler("MOEAD.log");
            //logger_.addHandler(fileHandler_);
			
			indicators = null;
			if (args.Length == 1)
			{
				System.Object[] parameters = new System.Object[]{"Real"};
				problem = (new ProblemFactory()).getProblem(args[0], parameters);
			}
			// if
			else if (args.Length == 2)
			{
				System.Object[] parameters = new System.Object[]{"Real"};
				problem = (new ProblemFactory()).getProblem(args[0], parameters);
				indicators = new QualityIndicator(problem, args[1]);
			}
			// if
			else
			{
				// Default problem
				problem = new Kursawe("Real", 3);
				//problem = new Kursawe("BinaryReal", 3);
				//problem = new Water("Real");
				//problem = new ZDT1("ArrayReal", 100);
				//problem = new ConstrEx("Real");
				//problem = new DTLZ1("Real");
				//problem = new OKA2("Real") ;
			} // else
			
			algorithm = new MOEAD(problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", 600);
			algorithm.setInputParameter("maxEvaluations", 150000);
			
			// Directory with the files containing the weight vectors used in 
			// Q. Zhang,  W. Liu,  and H Li, The Performance of a New Version of MOEA/D 
			// on CEC09 Unconstrained MOP Test Instances Working Report CES-491, School 
			// of CS & EE, University of Essex, 02/2009.
			// http://dces.essex.ac.uk/staff/qzhang/MOEAcompetition/CEC09final/code/ZhangMOEADcode/moead0305.rar
			algorithm.setInputParameter("dataDirectory", "/Users/antonio/Softw/pruebas/data/MOEAD_parameters/Weight");
			
			// Crossover operator 
			crossover = CrossoverFactory.getCrossoverOperator("DifferentialEvolutionCrossover");
			crossover.setParameter("CR", 1.0);
			crossover.setParameter("F", 0.5);
			
			// Mutation operator
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", 1.0 / problem.NumberOfVariables);
			mutation.setParameter("distributionIndex", 20.0);
			
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("mutation", mutation);
			
			// Execute the Algorithm
			long initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			SolutionSet population = algorithm.execute();
			long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
			
			// Result messages 
            //logger_.info("Total execution time: " + estimatedTime + "ms");
            //logger_.info("Objectives values have been writen to file FUN");
            //population.printObjectivesToFile("FUN");
            //logger_.info("Variables values have been writen to file VAR");
            //population.printVariablesToFile("VAR");
            m_logger.WriteLog("Total execution time: " + estimatedTime + "ms");
            m_logger.WriteLog("Objectives values have been writen to file FUN");
            population.printObjectivesToFile("FUN");
            m_logger.WriteLog("Variables values have been writen to file VAR");
            population.printVariablesToFile("VAR");
			
			if (indicators != null)
			{
                //logger_.info("Quality indicators");
                //logger_.info("Hypervolume: " + indicators.getHypervolume(population));
                //logger_.info("EPSILON    : " + indicators.getEpsilon(population));
                //logger_.info("GD         : " + indicators.getGD(population));
                //logger_.info("IGD        : " + indicators.getIGD(population));
                //logger_.info("Spread     : " + indicators.getSpread(population));
                m_logger.WriteLog("Quality indicators");
                m_logger.WriteLog("Hypervolume: " + indicators.getHypervolume(population));
                m_logger.WriteLog("EPSILON    : " + indicators.getEpsilon(population));
                m_logger.WriteLog("GD         : " + indicators.getGD(population));
                m_logger.WriteLog("IGD        : " + indicators.getIGD(population));
                m_logger.WriteLog("Spread     : " + indicators.getSpread(population));
            } // if          
		} //main
	} // MOEAD_main
}
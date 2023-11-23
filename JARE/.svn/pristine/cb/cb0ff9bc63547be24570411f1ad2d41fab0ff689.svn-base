/// <summary> CellDE_main.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// This algorithm is described in:
/// J.J. Durillo, A.J. Nebro, F. Luna, E. Alba "Solving Three-Objective 
/// Optimization Problems Using a new Hybrid Cellular Genetic Algorithm". 
/// PPSN'08. Dortmund. September 2008. 
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.crossover;
using JARE.Base.operators.selection;
using JARE.problems;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
//UPGRADE_TODO: The type 'java.util.logging.FileHandler' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//using FileHandler = java.util.logging.FileHandler;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
namespace JARE.metaheuristics.cellde
{
	
	public class CellDE_main
	{
		public static Logger m_logger; // Logger object
        //public static FileHandler fileHandler_; // FileHandler object
		
		/// <param name="args">Command line arguments.
		/// </param>
		/// <throws>  SMException  </throws>
		/// <throws>  IOException  </throws>
		/// <throws>  SecurityException  </throws>
		/// <summary> Usage: three choices
		/// - JARE.metaheuristics.nsgaII.NSGAII_main
		/// - JARE.metaheuristics.nsgaII.NSGAII_main problemName
		/// - JARE.metaheuristics.nsgaII.NSGAII_main problemName paretoFrontFile
		/// </summary>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			Problem problem; // The problem to solve
			Algorithm algorithm; // The algorithm to use
			Operator selection;
			Operator crossover;
			
			QualityIndicator indicators; // Object to get quality indicators
			
			// Logger object and file to store log messages
            m_logger = Configuration.m_logger;
            ////////fileHandler_ = new FileHandler("MOCell_main.log");
            ////////logger_.addHandler(fileHandler_);
			
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
				//problem = new ZDT4("ArrayReal");
				//problem = new WFG1("Real");
				//problem = new DTLZ1("Real");
				//problem = new OKA2("Real") ;
			} // else
			
			algorithm = new CellDE(problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", 100);
			algorithm.setInputParameter("archiveSize", 100);
			algorithm.setInputParameter("maxEvaluations", 25000);
			algorithm.setInputParameter("feedBack", 20);
			
			// Crossover operator 
			crossover = CrossoverFactory.getCrossoverOperator("DifferentialEvolutionCrossover");
			crossover.setParameter("CR", 0.5);
			crossover.setParameter("F", 0.5);
			
			// Add the operators to the algorithm
			selection = SelectionFactory.getSelectionOperator("BinaryTournament");
			
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("selection", selection);
			
			// Execute the Algorithm 
			long initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			SolutionSet population = algorithm.execute();
			long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
			System.Console.Out.WriteLine("Total execution time: " + estimatedTime);
			
			// Log messages 
            ////////logger_.info("Objectives values have been writen to file FUN");
            ////////population.printObjectivesToFile("FUN");
            ////////logger_.info("Variables values have been writen to file VAR");
            ////////population.printVariablesToFile("VAR");

            m_logger.WriteLog("Objectives values have been writen to file FUN");
            population.printObjectivesToFile("FUN");
            m_logger.WriteLog("Variables values have been writen to file VAR");
            population.printVariablesToFile("VAR");

            if (indicators != null)
			{
                ////////logger_.info("Quality indicators");
                ////////logger_.info("Hypervolume: " + indicators.getHypervolume(population));
                ////////logger_.info("GD         : " + indicators.getGD(population));
                ////////logger_.info("IGD        : " + indicators.getIGD(population));
                ////////logger_.info("Spread     : " + indicators.getSpread(population));
                m_logger.WriteLog("Quality indicators");
                m_logger.WriteLog("Hypervolume: " + indicators.getHypervolume(population));
                m_logger.WriteLog("GD         : " + indicators.getGD(population));
                m_logger.WriteLog("IGD        : " + indicators.getIGD(population));
                m_logger.WriteLog("Spread     : " + indicators.getSpread(population));
            } // if
		} //main
	} // CellDE_main
}
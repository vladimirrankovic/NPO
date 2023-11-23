/// <summary> OMOPSO_main.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// This class execute the OMOPSO algorithm used in:
/// ""
/// </version>
using System;
using JARE.Base;
using JARE.problems;
using JARE.problems.DTLZ;
using JARE.problems.ZDT;
using JARE.problems.WFG;
using JARE.problems.LZ09;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
//UPGRADE_TODO: The type 'java.util.logging.FileHandler' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using FileHandler = java.util.logging.FileHandler;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
namespace JARE.metaheuristics.omopso
{
	
	public class OMOPSO_main
	{
		public static Logger m_logger; // Logger object
        ////////public static FileHandler fileHandler_; // FileHandler object
		
		/// <param name="args">Command line arguments. The first (optional) argument specifies 
		/// the problem to solve.
		/// </param>
		/// <throws>  SMException  </throws>
		/// <throws>  IOException  </throws>
		/// <throws>  SecurityException  </throws>
		/// <summary> Usage: three options
		/// - JARE.metaheuristics.mocell.MOCell_main
		/// - JARE.metaheuristics.mocell.MOCell_main problemName
		/// - JARE.metaheuristics.mocell.MOCell_main problemName ParetoFrontFile
		/// </summary>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			Problem problem; // The problem to solve
			Algorithm algorithm; // The algorithm to use
			
			QualityIndicator indicators; // Object to get quality indicators
			
			// Logger object and file to store log messages
            m_logger = Configuration.m_logger;
            //////////fileHandler_ = new FileHandler("OMOPSO_main.log");
            //////////logger_.addHandler(fileHandler_);
			
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
				//problem = new Water("Real");
				//problem = new ZDT4("Real");
				//problem = new WFG1("Real");
				//problem = new DTLZ1("Real");
				//problem = new OKA2("Real") ;
			}
			
			algorithm = new OMOPSO(problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("swarmSize", 100);
			algorithm.setInputParameter("archiveSize", 100);
			algorithm.setInputParameter("maxIterations", 250);
			algorithm.setInputParameter("perturbationIndex", 0.5);
			
			// Execute the Algorithm 
			long initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			SolutionSet population = algorithm.execute();
			long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
			
			// Print the results
            //////////logger_.info("Total execution time: " + estimatedTime + "ms");
            //////////logger_.info("Variables values have been writen to file VAR");
            //////////population.printVariablesToFile("VAR");
            //////////logger_.info("Objectives values have been writen to file FUN");
            //////////population.printObjectivesToFile("FUN");
            m_logger.WriteLog("Total execution time: " + estimatedTime + "ms");
            m_logger.WriteLog("Variables values have been writen to file VAR");
			population.printVariablesToFile("VAR");
            m_logger.WriteLog("Objectives values have been writen to file FUN");
			population.printObjectivesToFile("FUN");
			
			if (indicators != null)
			{
                ////////logger_.info("Quality indicators");
                ////////logger_.info("Hypervolume: " + indicators.getHypervolume(population));
                ////////logger_.info("GD         : " + indicators.getGD(population));
                ////////logger_.info("IGD        : " + indicators.getIGD(population));
                ////////logger_.info("Spread     : " + indicators.getSpread(population));
                ////////logger_.info("Epsilon    : " + indicators.getEpsilon(population));
                m_logger.WriteLog("Quality indicators");
                m_logger.WriteLog("Hypervolume: " + indicators.getHypervolume(population));
                m_logger.WriteLog("GD         : " + indicators.getGD(population));
                m_logger.WriteLog("IGD        : " + indicators.getIGD(population));
                m_logger.WriteLog("Spread     : " + indicators.getSpread(population));
                m_logger.WriteLog("Epsilon    : " + indicators.getEpsilon(population));
            } // if
		} //main
	} // OMOPSO_main
}
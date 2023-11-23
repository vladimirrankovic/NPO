/// <summary> FastPGA_main.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using FPGAFitnessComparator = JARE.Base.operators.comparator.FPGAFitnessComparator;
using JARE.Base.operators.crossover;
using JARE.Base.operators.mutation;
using JARE.Base.operators.selection;
using JARE.Base.variable;
using JARE.problems;
using JARE.problems.ZDT;
using JARE.problems.WFG;
using JARE.problems.DTLZ;
using JARE.problems.LZ09;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
//UPGRADE_TODO: The type 'java.util.logging.FileHandler' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using FileHandler = java.util.logging.FileHandler;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
namespace JARE.metaheuristics.fastPGA
{
	
	public class FastPGA_main
	{
		public static Logger m_logger; // Logger object
        ////////public static FileHandler fileHandler_; // FileHandler object
		
		/// <param name="args">Command line arguments. The first (optional) argument specifies 
		/// the problem to solve.
		/// </param>
		/// <throws>  SMException  </throws>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			Problem problem; // The problem to solve
			Algorithm algorithm; // The algorithm to use
			Operator crossover; // Crossover operator
			Operator mutation; // Mutation operator
			Operator selection; // Selection operator
			
			QualityIndicator indicators; // Object to get quality indicators
			
			// Logger object and file to store log messages
            m_logger = Configuration.m_logger;
            ////////fileHandler_ = new FileHandler("FastPGA_main.log");
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
				//problem = new ZDT1("ArrayReal", 100);
				//problem = new ConstrEx("Real");
				//problem = new DTLZ1("Real");
				//problem = new OKA2("Real") ;
			} // else
			
			algorithm = new FastPGA(problem);
			
			algorithm.setInputParameter("maxPopSize", 100);
			algorithm.setInputParameter("initialPopulationSize", 100);
			algorithm.setInputParameter("maxEvaluations", 25000);
			algorithm.setInputParameter("a", 20.0);
			algorithm.setInputParameter("b", 1.0);
			algorithm.setInputParameter("c", 20.0);
			algorithm.setInputParameter("d", 0.0);
			
			// Parameter "termination"
			// If the preferred stopping criterium is PPR based, termination must 
			// be set to 0; otherwise, if the algorithm is intended to iterate until 
			// a give number of evaluations is carried out, termination must be set to 
			// that number
			algorithm.setInputParameter("termination", 1);
			
			// Mutation and Crossover for Real codification 
			crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
			crossover.setParameter("probability", 1.0);
			crossover.setParameter("distributionIndex", 20.0);
			
			
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", 1.0 / problem.NumberOfVariables);
			mutation.setParameter("distributionIndex", 20.0);
			
			// Mutation and Crossover for Binary codification
			/*
			crossover = CrossoverFactory.getCrossoverOperator("SinglePointCrossover");                   
			crossover.setParameter("probability",1.0);  
			mutation = MutationFactory.getMutationOperator("BitFlipMutation");                    
			mutation.setParameter("probability",1.0/149.0);    
			*/
			
			selection = new BinaryTournament(new FPGAFitnessComparator());
			
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("mutation", mutation);
			algorithm.addOperator("selection", selection);
			
			long initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			SolutionSet population = algorithm.execute();
			long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
			
			// Result messages 
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

                ////////int evaluations = ((System.Int32)algorithm.getOutputParameter("evaluations"));
                ////////logger_.info("Speed      : " + evaluations + " evaluations");
                m_logger.WriteLog("Quality indicators");
                m_logger.WriteLog("Hypervolume: " + indicators.getHypervolume(population));
                m_logger.WriteLog("GD         : " + indicators.getGD(population));
                m_logger.WriteLog("IGD        : " + indicators.getIGD(population));
                m_logger.WriteLog("Spread     : " + indicators.getSpread(population));
                m_logger.WriteLog("Epsilon    : " + indicators.getEpsilon(population));

                int evaluations = ((System.Int32)algorithm.getOutputParameter("evaluations"));
                m_logger.WriteLog("Speed      : " + evaluations + " evaluations");
            } // if
		} //main
	} // FastPGA_main
}
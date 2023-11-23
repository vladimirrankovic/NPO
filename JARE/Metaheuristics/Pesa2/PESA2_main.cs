/// <summary> PESA2_main.java
/// 
/// </summary>
/// <author>  Juanjo Durillo
/// </author>
/// <version>  1.0
/// 
/// This class executes the PESA2 algorithm
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.crossover;
using JARE.Base.operators.mutation;
using JARE.Base.operators.selection;
using JARE.Base.variable;
using JARE.problems;
using JARE.problems.DTLZ;
using JARE.problems.ZDT;
using JARE.problems.WFG;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
//UPGRADE_TODO: The type 'java.util.logging.FileHandler' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using FileHandler = java.util.logging.FileHandler;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
namespace JARE.metaheuristics.pesa2
{
	
	public class PESA2_main
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
			Operator crossover; // Crossover operator
			Operator mutation; // Mutation operator    
			
			QualityIndicator indicators; // Object to get quality indicators
			
			// Logger object and file to store log messages
            m_logger = Configuration.m_logger;
            //////////fileHandler_ = new FileHandler("PESA2_main.log");
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
				//problem = new ZDT1("ArrayReal", 1000);
				//problem = new ZDT4("BinaryReal");
				//problem = new WFG1("Real");
				//problem = new DTLZ1("Real");
				//problem = new OKA2("Real") ;
			} // else
			
			algorithm = new PESA2(problem);
			
			// Algorithm parameters 
			algorithm.setInputParameter("populationSize", 10);
			algorithm.setInputParameter("archiveSize", 100);
			algorithm.setInputParameter("bisections", 5);
			algorithm.setInputParameter("maxEvaluations", 25000);
			
			// Mutation and Crossover for Real codification 
			crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
			crossover.setParameter("probability", 0.9);
			crossover.setParameter("distributionIndex", 20.0);
			
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", 1.0 / problem.NumberOfVariables);
			mutation.setParameter("distributionIndex", 20.0);
			
			// Mutation and Crossover Binary codification
			/*
			crossover = CrossoverFactory.getCrossoverOperator("SinglePointCrossover");                   
			crossover.setParameter("probability",0.9);                   
			mutation = MutationFactory.getMutationOperator("BitFlipMutation");                    
			mutation.setParameter("probability",1.0/80);
			*/
			
			// Add the operators to the algorithm
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("mutation", mutation);
			
			
			// Execute the Algorithm
			long initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			SolutionSet population = algorithm.execute();
			long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
			System.Console.Out.WriteLine("Total execution time: " + estimatedTime);
			
			// Result messages 
            //////logger_.info("Total execution time: " + estimatedTime);
            //////logger_.info("Objectives values have been writen to file FUN");
            //////population.printObjectivesToFile("FUN");
            //////logger_.info("Variables values have been writen to file VAR");
            //////population.printVariablesToFile("VAR");
            m_logger.WriteLog("Total execution time: " + estimatedTime);
            m_logger.WriteLog("Objectives values have been writen to file FUN");
            population.printObjectivesToFile("FUN");
            m_logger.WriteLog("Variables values have been writen to file VAR");
            population.printVariablesToFile("VAR");
        } //main
	} // PESA2_main
}
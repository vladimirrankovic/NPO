/// <summary> DENSEA_Main.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
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
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
//UPGRADE_TODO: The type 'java.util.logging.FileHandler' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using FileHandler = java.util.logging.FileHandler;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
namespace JARE.metaheuristics.densea
{
	
	public class DENSEA_main
	{
		public static Logger m_logger; // Logger object
        ////////public static FileHandler fileHandler_; // FileHandler object
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			Problem problem; // The problem to solve
			Algorithm algorithm; // The algorithm to use
			Operator crossover; // Crossover operator
			Operator mutation; // Mutation operator
			Operator selection; // Selection operator
			
			// Logger object and file to store log messages
            m_logger = Configuration.m_logger;
            //////fileHandler_ = new FileHandler("Densea.log");
            //////logger_.addHandler(fileHandler_);
			
			problem = new RadioNetworkDesign(149);
			
			algorithm = new DENSEA(problem);
			
			// Algorithm parameters
			algorithm.setInputParameter("populationSize", 100);
			algorithm.setInputParameter("maxEvaluations", 25000);
			
			// Mutation and Crossover Binary codification 
			crossover = CrossoverFactory.getCrossoverOperator("SinglePointCrossover");
			crossover.setParameter("probability", 0.9);
			mutation = MutationFactory.getMutationOperator("BitFlipMutation");
			mutation.setParameter("probability", 1.0 / 149);
			
			// Selection Operator 
			selection = new BinaryTournament();
			
			// Add the operators to the algorithm
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("mutation", mutation);
			algorithm.addOperator("selection", selection);
			
			// Execute the Algorithm
			long initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			SolutionSet population = algorithm.execute();
			long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
			System.Console.Out.WriteLine("Total time of execution: " + estimatedTime);
			
			// Log messages 
            //////////logger_.info("Objectives values have been writen to file FUN");
            //////////population.printObjectivesToFile("FUN");
            //////////logger_.info("Variables values have been writen to file VAR");
            //////////population.printVariablesToFile("VAR");
            m_logger.WriteLog("Objectives values have been writen to file FUN");
            population.printObjectivesToFile("FUN");
            m_logger.WriteLog("Variables values have been writen to file VAR");
            population.printVariablesToFile("VAR");
        } //main
	}
}
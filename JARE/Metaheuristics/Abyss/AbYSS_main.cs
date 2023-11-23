/// <summary> AbYSS_main.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// This class executes the algorithm described in:
/// A.J. Nebro, F. Luna, E. Alba, B. Dorronsoro, J.J. Durillo, A. Beham 
/// "AbYSS: Adapting Scatter Search to Multiobjective Optimization." 
/// IEEE Transactions on Evolutionary Computation. Vol. 12, 
/// No. 4 (August 2008), pp. 439-457
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.crossover;
using JARE.Base.operators.mutation;
using JARE.problems;
using JARE.problems.DTLZ;
using JARE.problems.ZDT;
using JARE.problems.WFG;
using JARE.problems.LZ09;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
//UPGRADE_TODO: The type 'java.util.logging.FileHandler' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
////////using FileHandler = java.util.logging.FileHandler;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using Logger = java.util.logging.Logger;
using Logger = JARE.util.Logger;
using MutationLocalSearch = JARE.Base.operators.localSearch.MutationLocalSearch;
namespace JARE.metaheuristics.abyss
{
	/// <summary> This class is the main program used to configure and run AbYSS, a 
	/// multiobjective scatter search metaheuristics.
	/// Comments: AbYSS is configured to work only with continuous decision 
	/// variables.
	/// </summary>
	public class AbYSS_main
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
			Operator crossover; // Crossover operator
			Operator mutation; // Mutation operator
			Operator improvement; // Operator for improvement
			
			QualityIndicator indicators; // Object to get quality indicators
			
			// Logger object and file to store log messages
            m_logger = Configuration.m_logger;
            ////////fileHandler_ = new FileHandler("AbYSS.log");
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
			
			// STEP 2. Select the algorithm (AbYSS)
			algorithm = new AbYSS(problem);
			
			// STEP 3. Set the input parameters required by the metaheuristic
			algorithm.setInputParameter("populationSize", 20);
			algorithm.setInputParameter("refSet1Size", 10);
			algorithm.setInputParameter("refSet2Size", 10);
			algorithm.setInputParameter("archiveSize", 100);
			algorithm.setInputParameter("maxEvaluations", 25000);
			
			// STEP 4. Specify and configure the crossover operator, used in the
			//         solution combination method of the scatter search
			crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
			crossover.setParameter("probability", 1.0);
			crossover.setParameter("distributionIndex", 20.0);
			
			// STEP 5. Specify and configure the improvement method. We use by default
			//         a polynomial mutation in this method.
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", 1.0 / problem.NumberOfVariables);
			
			improvement = new MutationLocalSearch(problem, mutation);
			improvement.setParameter("improvementRounds", 1);
			
			// STEP 6. Add the operators to the algorithm
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("improvement", improvement);
			
			long initTime;
			long estimatedTime;
			initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			
			// STEP 7. Run the algorithm 
			SolutionSet population = algorithm.execute();
			estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
			
			// STEP 8. Print the results
            ////////logger_.info("Total execution time: " + estimatedTime + "ms");
            ////////logger_.info("Variables values have been writen to file VAR");
            ////////population.printVariablesToFile("VAR");
            ////////logger_.info("Objectives values have been writen to file FUN");
            ////////population.printObjectivesToFile("FUN");

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
	} // AbYSS_main
}
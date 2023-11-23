/// <summary> GA_main.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.operators.crossover;
using JARE.Base.operators.mutation;
using JARE.Base.operators.selection;
using JARE.problems.singleObjective;
using SMException = JARE.util.SMException;
namespace JARE.metaheuristics.singleObjective.geneticAlgorithm
{
	
	/// <summary> This class runs a single-objective genetic algorithm (GA). The GA can be 
	/// a steady-state GA (class SSGA) or a generational GA (class GGA). The OneMax
	/// problem is used to test the algorithms.
	/// </summary>
	public class GA_main
	{
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			Problem problem; // The problem to solve
			Algorithm algorithm; // The algorithm to use
			Operator crossover; // Crossover operator
			Operator mutation; // Mutation operator
			Operator selection; // Selection operator
			
			//int bits ; // Length of bit string in the OneMax problem
			
			//bits = 512 ;
			//problem = new OneMax(bits);
			
			problem = new Sphere("Real", 20);
			//problem = new Easom("Real") ;
			//problem = new Griewank("Real", 10) ;
			
			//algorithm = new gGA(problem) ; // Generational GA
			algorithm = new ssGA(problem); // Steady-state GA
			//algorithm = new scGA(problem) ; // Synchronous cGA
			//algorithm = new acGA(problem) ;   // Asynchronous cGA
			
			/* Algorithm parameters*/
			algorithm.setInputParameter("populationSize", 100);
			algorithm.setInputParameter("maxEvaluations", 10000);
			
			// Mutation and Crossover for Real codification 
			crossover = CrossoverFactory.getCrossoverOperator("SBXCrossover");
			crossover.setParameter("probability", 1.0);
			crossover.setParameter("distributionIndex", 20.0);
			
			mutation = MutationFactory.getMutationOperator("PolynomialMutation");
			mutation.setParameter("probability", 1.0 / problem.NumberOfVariables);
			mutation.setParameter("distributionIndex", 20.0);
			
			/**
			// Mutation and Crossover for Binary codification 
			crossover = CrossoverFactory.getCrossoverOperator("SinglePointCrossover");                   
			crossover.setParameter("probability",0.95);                   
			mutation = MutationFactory.getMutationOperator("BitFlipMutation");                    
			mutation.setParameter("probability",1.0/bits); 
			*/
			
			/* Selection Operator */
			selection = SelectionFactory.getSelectionOperator("BinaryTournament");
			
			/* Add the operators to the algorithm*/
			algorithm.addOperator("crossover", crossover);
			algorithm.addOperator("mutation", mutation);
			algorithm.addOperator("selection", selection);
			
			/* Execute the Algorithm */
			long initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			SolutionSet population = algorithm.execute();
			long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
			System.Console.Out.WriteLine("Total execution time: " + estimatedTime);
			
			/* Log messages */
			System.Console.Out.WriteLine("Objectives values have been writen to file FUN");
			population.printObjectivesToFile("FUN");
			System.Console.Out.WriteLine("Variables values have been writen to file VAR");
			population.printVariablesToFile("VAR");
		} //main
	} // GA_main
}
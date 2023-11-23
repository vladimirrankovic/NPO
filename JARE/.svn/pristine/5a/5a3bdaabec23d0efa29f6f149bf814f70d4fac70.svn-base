/// <summary> ES_main.java
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
namespace JARE.metaheuristics.singleObjective.evolutionStrategy
{
	
	/// <summary> This class runs a single-objective Evolution Strategy (ES). The ES can be 
	/// a (mu+lambda) ES (class ElitistES) or a (mu,lambda) ES (class NonElitistGA). 
	/// The OneMax problem is used to test the algorithms.
	/// </summary>
	public class ES_main
	{
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			Problem problem; // The problem to solve
			Algorithm algorithm; // The algorithm to use
			Operator mutation; // Mutation operator
			
			int bits; // Length of bit string in the OneMax problem
			
			bits = 512;
			problem = new OneMax(bits);
			
			int mu;
			int lambda;
			
			// Requirement: lambda must be divisible by mu
			mu = 1;
			lambda = 10;
			
			algorithm = new ElitistES(problem, mu, lambda);
			//algorithm = new NonElitistES(problem, mu, lambda);
			
			/* Algorithm params*/
			algorithm.setInputParameter("maxEvaluations", 20000);
			
			/* Mutation and Crossover for Real codification */
			mutation = MutationFactory.getMutationOperator("BitFlipMutation");
			mutation.setParameter("probability", 1.0 / bits);
			
			algorithm.addOperator("mutation", mutation);
			
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
	} // SSGA_main
}
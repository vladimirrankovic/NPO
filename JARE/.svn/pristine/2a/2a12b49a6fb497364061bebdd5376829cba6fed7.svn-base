/// <summary> TSPGA_main.java
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
	/// a steady-state GA (class SSGA) or a generational GA (class GGA). The TSP
	/// is used to test the algorithms. The data files accepted as in input are from
	/// TSPLIB.
	/// </summary>
	public class TSPGA_main
	{
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			Problem problem; // The problem to solve
			Algorithm algorithm; // The algorithm to use
			Operator crossover; // Crossover operator
			Operator mutation; // Mutation operator
			Operator selection; // Selection operator
			
			System.String problemName = "eil101.tsp";
			
			problem = new TSP(problemName);
			
			//algorithm = new SSGA(problem);
			algorithm = new gGA(problem);
			
			// Algorithm params
			algorithm.setInputParameter("populationSize", 512);
			algorithm.setInputParameter("maxEvaluations", 200000);
			
			// Mutation and Crossover for Real codification */
			crossover = CrossoverFactory.getCrossoverOperator("TwoPointsCrossover");
			//crossover = CrossoverFactory.getCrossoverOperator("PMXCrossover");
			crossover.setParameter("probability", 0.95);
			mutation = MutationFactory.getMutationOperator("SwapMutation");
			mutation.setParameter("probability", 0.2);
			
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
			System.Console.Out.WriteLine("Total time of execution: " + estimatedTime);
			
			/* Log messages */
			System.Console.Out.WriteLine("Objectives values have been writen to file FUN");
			population.printObjectivesToFile("FUN");
			System.Console.Out.WriteLine("Variables values have been writen to file VAR");
			population.printVariablesToFile("VAR");
		} //main
	} // TSPGA_main
}
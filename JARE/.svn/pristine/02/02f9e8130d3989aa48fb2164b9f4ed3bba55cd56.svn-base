/// <summary> MOCHC_main.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// This class executes the algorithm described in:
/// A.J. Nebro, E. Alba, G. Molina, F. Chicano, F. Luna, J.J. Durillo 
/// "Optimal antenna placement using a new multi-objective chc algorithm". 
/// GECCO '07: Proceedings of the 9th annual conference on Genetic and 
/// evolutionary computation. London, England. July 2007.
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
using SupportClass = JARE.support.SupportClass;
namespace JARE.metaheuristics.mochc
{
	
	public class MOCHC_main
	{
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			try
			{
				Problem problem = new RadioNetworkDesign(149);
				
				Algorithm algorithm = null;
				algorithm = new MOCHC(problem);
				
				algorithm.setInputParameter("initialConvergenceCount", 0.25);
				algorithm.setInputParameter("preservedPopulation", 0.05);
				algorithm.setInputParameter("convergenceValue", 3);
				algorithm.setInputParameter("populationSize", 100);
				algorithm.setInputParameter("maxEvaluations", 60000);
				
				Operator crossoverOperator;
				Operator mutationOperator;
				Operator parentsSelection;
				Operator newGenerationSelection;
				
				// Crossover operator
				crossoverOperator = CrossoverFactory.getCrossoverOperator("HUXCrossover");
				//crossoverOperator = CrossoverFactory.getCrossoverOperator("SinglePointCrossover");
				crossoverOperator.setParameter("probability", 1.0);
				
				//parentsSelection = new RandomSelection();
				//newGenerationSelection = new RankingAndCrowdingSelection(problem);
				parentsSelection = SelectionFactory.getSelectionOperator("RandomSelection");
				newGenerationSelection = SelectionFactory.getSelectionOperator("RankingAndCrowdingSelection");
				newGenerationSelection.setParameter("problem", problem);
				
				// Mutation operator
				mutationOperator = MutationFactory.getMutationOperator("BitFlipMutation");
				mutationOperator.setParameter("probability", 0.35);
				
				algorithm.addOperator("crossover", crossoverOperator);
				algorithm.addOperator("cataclysmicMutation", mutationOperator);
				algorithm.addOperator("parentSelection", parentsSelection);
				algorithm.addOperator("newGenerationSelection", newGenerationSelection);
				
				// Execute the Algorithm 
				long initTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				SolutionSet population = algorithm.execute();
				long estimatedTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - initTime;
				System.Console.Out.WriteLine("Total execution time: " + estimatedTime);
				
				// Print results
				population.printVariablesToFile("VAR");
				population.printObjectivesToFile("FUN");
			}
			//try           
			catch (System.Exception e)
			{
                //VLADA-CONVERT NAPOMENA: POTENCIJALNI PROBLEM
				//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Error.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Error.WriteLine(e);
				SupportClass.WriteStackTrace(e, Console.Error);
			} //catch    
		} //main
	}
}
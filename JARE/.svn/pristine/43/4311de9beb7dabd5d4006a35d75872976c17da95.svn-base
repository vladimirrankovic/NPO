/// <summary> DifferentialEvolutionSelection.java
/// Class representing the selection operator used in differential evolution
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using Operator = JARE.Base.operators;
using Solution = JARE.Base.Solution;
using SolutionSet = JARE.Base.SolutionSet;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.selection
{
	
	[Serializable]
	public class DifferentialEvolutionSelection:Selection
	{
		
		
		/// <summary> Constructor</summary>
		internal DifferentialEvolutionSelection()
		{
		} // Constructor
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing the population and the position (index)
		/// of the current individual
		/// </param>
		/// <returns> An object containing the three selected parents
		/// </returns>
		public override System.Object execute(System.Object obj)
		{
			System.Object[] parameters = (System.Object[]) obj;
			SolutionSet population = (SolutionSet) parameters[0];
			int index = (System.Int32) parameters[1];
			
			Solution[] parents = new Solution[3];
			int r1, r2, r3;
			
			if (population.size() < 4)
				throw new SMException("DifferentialEvolutionSelection: the population has less than four solutions");
			
			do 
			{
				r1 = (int) (PseudoRandom.randInt(0, population.size() - 1));
			}
			while (r1 == index);
			do 
			{
				r2 = (int) (PseudoRandom.randInt(0, population.size() - 1));
			}
			while (r2 == index || r2 == r1);
			do 
			{
				r3 = (int) (PseudoRandom.randInt(0, population.size() - 1));
			}
			while (r3 == index || r3 == r1 || r3 == r2);
			
			parents[0] = population.getSolution(r1);
			parents[1] = population.getSolution(r2);
			parents[2] = population.getSolution(r3);
			
			return parents;
		} // execute
	} // DifferentialEvolutionSelection
}
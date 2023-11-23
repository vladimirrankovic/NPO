/// <summary> RandomSelection.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.selection
{
	
	/// <summary> This class implements a random selection operator used for selecting two
	/// random parents
	/// </summary>
	[Serializable]
	public class RandomSelection:Selection
	{
		
		/// <summary> Performs the operation</summary>
		/// <param name="object">Object representing a SolutionSet.
		/// </param>
		/// <returns> an object representing an array with the selected parents
		/// </returns>
		public override System.Object execute(System.Object obj)
		{
			SolutionSet population = (SolutionSet) obj;
			int pos1, pos2;
			pos1 = PseudoRandom.randInt(0, population.size() - 1);
			pos2 = PseudoRandom.randInt(0, population.size() - 1);
			while ((pos1 == pos2) && (population.size() > 1))
			{
				pos2 = PseudoRandom.randInt(0, population.size() - 1);
			}
			
			Solution[] parents = new Solution[2];
			parents[0] = population.getSolution(pos1);
			parents[1] = population.getSolution(pos2);
			
			return parents;
		} // Execute     
	} // RandomSelection
}
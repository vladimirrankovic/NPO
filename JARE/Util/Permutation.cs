/// <summary> Permutation.java
/// 
/// </summary>
/// <author>  juanjo durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.util
{
	
	
	/// <summary> Class providing int [] permutations</summary>
	public class Permutation
	{
		
		/// <summary> Return a permutation vector between the 0 and (length - 1) </summary>
		public virtual int[] intPermutation(int length)
		{
			int[] aux = new int[length];
			int[] result = new int[length];
			
			// First, create an array from 0 to length - 1. We call them result
			// Also is needed to create an random array of size length
			for (int i = 0; i < length; i++)
			{
				result[i] = i;
				aux[i] = PseudoRandom.randInt(0, length - 1);
			} // for
			
			// Sort the random array with effect in result, and then we obtain a
			// permutation array between 0 and length - 1
			for (int i = 0; i < length; i++)
			{
				for (int j = i + 1; j < length; j++)
				{
					if (aux[i] > aux[j])
					{
						int tmp;
						tmp = aux[i];
						aux[i] = aux[j];
						aux[j] = tmp;
						tmp = result[i];
						result[i] = result[j];
						result[j] = tmp;
					} 
				}
			} 
			return result;
		}
	} 
}
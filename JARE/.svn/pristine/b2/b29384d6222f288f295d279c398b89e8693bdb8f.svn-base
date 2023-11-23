/// <summary> PseudoRandom.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
namespace JARE.util
{
	
	/// <summary> Class representing some randoms facilities</summary>
	public class PseudoRandom
	{
		
		/// <summary> generator used to obtain the random values</summary>
		private static RandomGenerator random = null;
		
		/// <summary> other generator used to obtain the random values</summary>
		private static System.Random rand = null;
		
		/// <summary> Constructor.
		/// Creates a new instance of PseudoRandom.
		/// </summary>
		private PseudoRandom()
		{
			if (random == null)
			{
				//this.random = new java.util.Random((long)seed);
				random = new RandomGenerator();
				rand = new System.Random();
			}
		} 
		
		/// <summary> Returns a random int value using the Java random generator.</summary>
		/// <returns> A random int value.
		/// </returns>
		public static int randInt()
		{
			if (random == null)
			{
				new PseudoRandom();
			}
            return rand.Next();
		} // randInt
		
		/// <summary> Returns a random double value using the PseudoRandom generator.
		/// Returns A random double value.
		/// </summary>
		public static double randDouble()
		{
			if (random == null)
			{
				new PseudoRandom();
			}
			return random.rndreal(0.0, 1.0);
			//return randomJava.nextDouble();
		} // randDouble
		
		/// <summary> Returns a random int value between a minimum bound and maximum bound using
		/// the PseudoRandom generator.
		/// </summary>
		/// <param name="minBound">The minimum bound.
		/// </param>
		/// <param name="maxBound">The maximum bound.
		/// Return A pseudo random int value between minBound and maxBound.
		/// </param>
		public static int randInt(int minBound, int maxBound)
		{
			if (random == null)
			{
				new PseudoRandom();
			}
			return random.rnd(minBound, maxBound);
			//return minBound + randomJava.nextInt(maxBound-minBound+1);
		} // randInt
		
		/// <summary>Returns a random double value between a minimum bound and a maximum bound
		/// using the PseudoRandom generator.
		/// </summary>
		/// <param name="minBound">The minimum bound.
		/// </param>
		/// <param name="maxBound">The maximum bound.
		/// </param>
		/// <returns> A pseudo random double value between minBound and maxBound
		/// </returns>
		public static double randDouble(double minBound, double maxBound)
		{
			if (random == null)
			{
				new PseudoRandom();
			}
			return random.rndreal(minBound, maxBound);
			//return minBound + (maxBound - minBound)*randomJava.nextDouble();
		} // randDouble    
	} // PseudoRandom
}
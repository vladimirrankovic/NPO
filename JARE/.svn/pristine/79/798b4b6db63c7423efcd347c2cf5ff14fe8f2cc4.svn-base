/// <summary> RandomGenerator.java
/// 
/// Created on 25 de septiembre de 2006, 18:24
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
using System;
namespace JARE.util
{
	
	/// <summary> This code has been taken from deb NSGA-II implementation
	/// The code is available to download from
	/// http://www.iitk.ac.in/kangal/codes.shtml
	/// 
	/// </summary>
	
	public class RandomGenerator
	{
		
		/* Definition of random number generation routines */
		internal double seed;
		internal double[] oldrand = new double[55];
		internal int jrand;
		
		/// <summary> Constructor</summary>
		public RandomGenerator()
		{
			this.seed = (new Random((int)System.DateTime.Now.Ticks)).NextDouble();
			this.randomize();
		} // RandomGenerator
		
		/* Get seed number for random and start it up */
		internal virtual void  randomize()
		{
			int j1;
			for (j1 = 0; j1 <= 54; j1++)
			{
				oldrand[j1] = 0.0;
			}
			jrand = 0;
			warmup_random(seed);
			return ;
		} // randomize
		
		/* Get randomize off and running */
		internal virtual void  warmup_random(double seed)
		{
			int j1, ii;
			double new_random, prev_random;
			oldrand[54] = seed;
			new_random = 0.000000001;
			prev_random = seed;
			for (j1 = 1; j1 <= 54; j1++)
			{
				ii = (21 * j1) % 54;
				oldrand[ii] = new_random;
				new_random = prev_random - new_random;
				if (new_random < 0.0)
				{
					new_random += 1.0;
				}
				prev_random = oldrand[ii];
			}
			advance_random();
			advance_random();
			advance_random();
			jrand = 0;
			return ;
		} // warmup_random
		
		/* Create next batch of 55 random numbers */
		internal virtual void  advance_random()
		{
			int j1;
			double new_random;
			for (j1 = 0; j1 < 24; j1++)
			{
				new_random = oldrand[j1] - oldrand[j1 + 31];
				if (new_random < 0.0)
				{
					new_random = new_random + 1.0;
				}
				oldrand[j1] = new_random;
			}
			for (j1 = 24; j1 < 55; j1++)
			{
				new_random = oldrand[j1] - oldrand[j1 - 24];
				if (new_random < 0.0)
				{
					new_random = new_random + 1.0;
				}
				oldrand[j1] = new_random;
			}
		} //advance_ramdom
		
		/* Fetch a single random number between 0.0 and 1.0 */
		internal virtual double randomperc()
		{
			jrand++;
			if (jrand >= 55)
			{
				jrand = 1;
				advance_random();
			}
			return ((double) oldrand[jrand]);
		} //randomPerc
		
		/* Fetch a single random integer between low and high including the bounds */
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'rnd'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual int rnd(int low, int high)
		{
			lock (this)
			{
				int res;
				if (low >= high)
				{
					res = low;
				}
				else
				{
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					res = low + (int) (randomperc() * (high - low + 1));
					if (res > high)
					{
						res = high;
					}
				}
				return (res);
			}
		} // rnd
		
		/* Fetch a single random real number between low and high including the */
		/* bounds */
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'rndreal'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		internal virtual double rndreal(double low, double high)
		{
			lock (this)
			{
				return (low + (high - low) * randomperc());
			}
		} //rndreal
	} // RandomGenerator
}
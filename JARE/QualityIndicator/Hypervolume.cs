/// <summary> Hypervolume.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.qualityIndicator
{
	
	/// <summary> This class implements the hypervolume metric. The code is the a Java version
	/// of the orginal metric implementation by Eckart Zitzler.
	/// It can be used also as a command line program just by typing
	/// $java Hypervolume <solutionFrontFile> <trueFrontFile> <numberOfOjbectives>
	/// Reference: E. Zitzler and L. Thiele
	/// Multiobjective Evolutionary Algorithms: A Comparative Case Study 
	/// and the Strength Pareto Approach,
	/// IEEE Transactions on Evolutionary Computation, vol. 3, no. 4, 
	/// pp. 257-271, 1999.
	/// </summary>
	public class Hypervolume
	{

        internal JARE.qualityIndicator.util.MetricsUtil m_utils;
		
		/// <summary> Constructor
		/// Creates a new instance of MultiDelta
		/// </summary>
		public Hypervolume()
		{
            m_utils = new JARE.qualityIndicator.util.MetricsUtil();
		} // Hypervolume
		
		/* 
		returns true if 'point1' dominates 'points2' with respect to the 
		to the first 'noObjectives' objectives 
		*/
		internal virtual bool dominates(double[] point1, double[] point2, int noObjectives)
		{
			int i;
			int betterInAnyObjective;
			
			betterInAnyObjective = 0;
			for (i = 0; i < noObjectives && point1[i] >= point2[i]; i++)
				if (point1[i] > point2[i])
					betterInAnyObjective = 1;
			
			return ((i >= noObjectives) && (betterInAnyObjective > 0));
		} //Dominates
		
		internal virtual void  swap(double[][] front, int i, int j)
		{
			double[] temp;
			
			temp = front[i];
			front[i] = front[j];
			front[j] = temp;
		} // Swap 
		
		
		/* all nondominated points regarding the first 'noObjectives' dimensions
		are collected; the points referenced by 'front[0..noPoints-1]' are
		considered; 'front' is resorted, such that 'front[0..n-1]' contains
		the nondominated points; n is returned */
		internal virtual int filterNondominatedSet(double[][] front, int noPoints, int noObjectives)
		{
			int i, j;
			int n;
			
			n = noPoints;
			i = 0;
			while (i < n)
			{
				j = i + 1;
				while (j < n)
				{
					if (dominates(front[i], front[j], noObjectives))
					{
						/* remove point 'j' */
						n--;
						swap(front, j, n);
					}
					else if (dominates(front[j], front[i], noObjectives))
					{
						/* remove point 'i'; ensure that the point copied to index 'i'
						is considered in the next outer loop (thus, decrement i) */
						n--;
						swap(front, i, n);
						i--;
						break;
					}
					else
						j++;
				}
				i++;
			}
			return n;
		} // FilterNondominatedSet 
		
		
		/* calculate next value regarding dimension 'objective'; consider
		points referenced in 'front[0..noPoints-1]' */
		internal virtual double surfaceUnchangedTo(double[][] front, int noPoints, int objective)
		{
			int i;
			double minValue, val;
			
			if (noPoints < 1)
				System.Console.Error.WriteLine("run-time error");
			
			minValue = front[0][objective];
			for (i = 1; i < noPoints; i++)
			{
				val = front[i][objective];
				if (val < minValue)
					minValue = val;
			}
			return minValue;
		} // SurfaceUnchangedTo 
		
		/* remove all points which have a value <= 'threshold' regarding the
		dimension 'objective'; the points referenced by
		'front[0..noPoints-1]' are considered; 'front' is resorted, such that
		'front[0..n-1]' contains the remaining points; 'n' is returned */
		internal virtual int reduceNondominatedSet(double[][] front, int noPoints, int objective, double threshold)
		{
			int n;
			int i;
			
			n = noPoints;
			for (i = 0; i < n; i++)
				if (front[i][objective] <= threshold)
				{
					n--;
					swap(front, i, n);
				}
			
			return n;
		} // ReduceNondominatedSet
		
		internal virtual double calculateHypervolume(double[][] front, int noPoints, int noObjectives)
		{
			int n;
			double volume, distance;
			
			volume = 0;
			distance = 0;
			n = noPoints;
			while (n > 0)
			{
				int noNondominatedPoints;
				double tempVolume, tempDistance;
				
				noNondominatedPoints = filterNondominatedSet(front, n, noObjectives - 1);
				tempVolume = 0;
				if (noObjectives < 3)
				{
					if (noNondominatedPoints < 1)
						System.Console.Error.WriteLine("run-time error");
					
					tempVolume = front[0][0];
				}
				else
					tempVolume = calculateHypervolume(front, noNondominatedPoints, noObjectives - 1);
				
				tempDistance = surfaceUnchangedTo(front, n, noObjectives - 1);
				volume += tempVolume * (tempDistance - distance);
				distance = tempDistance;
				n = reduceNondominatedSet(front, n, noObjectives - 1, distance);
			}
			return volume;
		} // CalculateHypervolume

		/* merge two fronts */
		internal virtual double[][] mergeFronts(double[][] front1, int sizeFront1, double[][] front2, int sizeFront2, int noObjectives)
		{
			int i, j;
			int noPoints;
			double[][] frontPtr;
			
			/* allocate memory */
			noPoints = sizeFront1 + sizeFront2;
			frontPtr = new double[noPoints][];
			for (int i2 = 0; i2 < noPoints; i2++)
			{
				frontPtr[i2] = new double[noObjectives];
			}
			/* copy points */
			noPoints = 0;
			for (i = 0; i < sizeFront1; i++)
			{
				for (j = 0; j < noObjectives; j++)
					frontPtr[noPoints][j] = front1[i][j];
				noPoints++;
			}
			for (i = 0; i < sizeFront2; i++)
			{
				for (j = 0; j < noObjectives; j++)
					frontPtr[noPoints][j] = front2[i][j];
				noPoints++;
			}
			
			return frontPtr;
		} // MergeFronts
		
		/// <summary> Returns the hypevolume value of the paretoFront. This method call to the
		/// calculate hipervolume one
		/// </summary>
		/// <param name="paretoFront">The pareto front
		/// </param>
		/// <param name="paretoTrueFront">The true pareto front
		/// </param>
		/// <param name="numberOfObjectives">Number of objectives of the pareto front
		/// </param>
		public virtual double hypervolume(double[][] paretoFront, double[][] paretoTrueFront, int numberOfObjectives)
		{
			
			/**
			* Stores the maximum values of true pareto front.
			*/
			double[] maximumValues;
			
			/**
			* Stores the minimum values of the true pareto front.
			*/
			double[] minimumValues;
			
			/**
			* Stores the normalized front.
			*/
			double[][] normalizedFront;
			
			/**
			* Stores the inverted front. Needed for minimization problems
			*/
			double[][] invertedFront;
			
			// STEP 1. Obtain the maximum and minimum values of the Pareto front
			maximumValues = m_utils.getMaximumValues(paretoTrueFront, numberOfObjectives);
			minimumValues = m_utils.getMinimumValues(paretoTrueFront, numberOfObjectives);
			
			// STEP 2. Get the normalized front
			normalizedFront = m_utils.getNormalizedFront(paretoFront, maximumValues, minimumValues);
			
			// STEP 3. Inverse the pareto front. This is needed because of the original
			//metric by Zitzler is for maximization problems
			invertedFront = m_utils.invertedFront(normalizedFront);
			
			// STEP4. The hypervolumen (control is passed to java version of Zitzler code)
			return this.calculateHypervolume(invertedFront, invertedFront.Length, numberOfObjectives);
		} // hypervolume

        /// <summary> Returns the hypevolume value of the paretoFront. This is the case when true pareto front is unknown. This method call to the
        /// calculate hipervolume one
        /// </summary>
        /// <param name="paretoFront">The pareto front
        /// </param>
        /// <param name="numberOfObjectives">Number of objectives of the pareto front
        /// </param>
        public virtual double hypervolume(double[][] paretoFront, int numberOfObjectives)
        {

            /**
            * Stores the maximum values of pareto front.
            */
            double[] maximumValues;

            /**
            * Stores the minimum values of the pareto front.
            */
            double[] minimumValues;

            /**
            * Stores the normalized front.
            */
            double[][] normalizedFront;

            /**
            * Stores the inverted front. Needed for minimization problems
            */
            double[][] invertedFront;

            // STEP 1. Obtain the maximum and minimum values of the Pareto front
            maximumValues = m_utils.getMaximumValues(paretoFront, numberOfObjectives);
            minimumValues = m_utils.getMinimumValues(paretoFront, numberOfObjectives);

            // STEP 2. Get the normalized front
            normalizedFront = m_utils.getNormalizedFront(paretoFront, maximumValues, minimumValues);

            // STEP 3. Inverse the pareto front. This is needed because of the original
            //metric by Zitzler is for maximization problems
            invertedFront = m_utils.invertedFront(normalizedFront);

            // STEP4. The hypervolumen (control is passed to java version of Zitzler code)
            double normalizedHypervolume = this.calculateHypervolume(invertedFront, invertedFront.Length, numberOfObjectives);//Hypervolume calculated for normalized objective values (range [0,1])

            // STEP 5.  Calculate hypervolume of original pareto front. Due to normalization the true dimension of pareto front is lost. 
            double denormalizationCoef = 1;
            for(int i = 0; i < maximumValues.Length; i++) denormalizationCoef *= (maximumValues[i]-minimumValues[i]);
            double Hypervolume = normalizedHypervolume * denormalizationCoef;

            return Hypervolume;
        } // hypervolume
		
		/// <summary> This class can be invoqued from the command line. Three params are required:
		/// 1) the name of the file containing the front,  
		/// 2) the name of the file containig the true Pareto front
		/// 3) the number of objectives
		/// </summary>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			if (args.Length < 2)
			{
				System.Console.Error.WriteLine("Error using delta. Type: \n java hypervolume " + "<SolutionFrontFile>" + "<TrueFrontFile> + <numberOfObjectives>");
				System.Environment.Exit(1);
			}
			
			//Create a new instance of the metric
			Hypervolume qualityIndicator = new Hypervolume();
			//Read the front from the files
			double[][] solutionFront = qualityIndicator.m_utils.readFront(args[0]);
			double[][] trueFront = qualityIndicator.m_utils.readFront(args[1]);
			
			//Obtain delta value
			double val = qualityIndicator.hypervolume(solutionFront, trueFront, (System.Int32.Parse(args[2])));
			
			System.Console.Out.WriteLine(val);
		} // main
	} // Hypervolume
}
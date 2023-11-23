/// <summary> GeneralizedSpread.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
namespace JARE.qualityIndicator
{
	
	/// <summary> This class implements the generalized spread metric for two or more dimensions.
	/// It can be used also as command line program just by typing. 
	/// $ java GeneralizedSpread <solutionFrontFile> <trueFrontFile> <numberOfObjectives>
	/// It is recommendable to see the metric description.
	/// Reference: A. Zhou, Y. Jin, Q. Zhang, B. Sendhoff, and E. Tsang
	/// Combining model-based and genetics-based offspring generation for 
	/// multi-objective optimization using a convergence criterion, 
	/// 2006 IEEE Congress on Evolutionary Computation, 2006, pp. 3234-3241.
	/// </summary>
	public class GeneralizedSpread
	{

        internal static JARE.qualityIndicator.util.MetricsUtil m_utils; // MetricsUtil provides some 
		// utilities for implementing
		// the metric
		
		/// <summary> Constructor
		/// Creates a new instance of GeneralizedSpread
		/// </summary>
		public GeneralizedSpread()
		{
			m_utils = new JARE.qualityIndicator.util.MetricsUtil();
		} // GeneralizedSpread
		
		
		
		/// <summary>  Calculates the generalized spread metric. Given the 
		/// pareto front, the true pareto front as <code>double []</code>
		/// and the number of objectives, the method return the value for the
		/// metric.
		/// </summary>
		/// <param name="paretoFront">The pareto front.
		/// </param>
		/// <param name="paretoTrueFront">The true pareto front.
		/// </param>
		/// <param name="numberOfObjectives">The number of objectives.
		/// </param>
		/// <returns> the value of the generalized spread metric
		/// 
		/// </returns>
		public virtual double generalizedSpread(double[][] paretoFront, double[][] paretoTrueFront, int numberOfObjectives)
		{
			
			/**
			* Stores the maximum values of true pareto front.
			*/
			double[] maximumValue;
			
			/**
			* Stores the minimum values of the true pareto front.
			*/
			double[] minimumValue;
			
			/**
			* Stores the normalized front.
			*/
			double[][] normalizedFront;
			
			/**
			* Stores the normalized true Pareto front.
			*/
			double[][] normalizedParetoFront;
			
			// STEP 1. Obtain the maximum and minimum values of the Pareto front
			maximumValue = m_utils.getMaximumValues(paretoTrueFront, numberOfObjectives);
			minimumValue = m_utils.getMinimumValues(paretoTrueFront, numberOfObjectives);
			
			normalizedFront = m_utils.getNormalizedFront(paretoFront, maximumValue, minimumValue);
			
			// STEP 2. Get the normalized front and true Pareto fronts
			normalizedParetoFront = m_utils.getNormalizedFront(paretoTrueFront, maximumValue, minimumValue);
			
			// STEP 3. Find extremal values
			double[][] extremValues = new double[numberOfObjectives][];
			for (int i = 0; i < numberOfObjectives; i++)
			{
				extremValues[i] = new double[numberOfObjectives];
			}
			for (int i = 0; i < numberOfObjectives; i++)
			{
				System.Array.Sort(normalizedParetoFront, new JARE.qualityIndicator.util.ValueComparator(i));
				for (int j = 0; j < numberOfObjectives; j++)
				{
					extremValues[i][j] = normalizedParetoFront[normalizedParetoFront.Length - 1][j];
				}
			}
			
			int numberOfPoints = normalizedFront.Length;
			int numberOfTruePoints = normalizedParetoFront.Length;
			
			
			// STEP 4. Sorts the normalized front
            System.Array.Sort(normalizedFront, new JARE.qualityIndicator.util.LexicoGraphicalComparator());
			
			// STEP 5. Calculate the metric value. The value is 1.0 by default
			if (m_utils.distance(normalizedFront[0], normalizedFront[normalizedFront.Length - 1]) == 0.0)
			{
				return 1.0;
			}
			else
			{
				
				double dmean = 0.0;
				
				// STEP 6. Calculate the mean distance between each point and its nearest neighbor
				for (int i = 0; i < normalizedFront.Length; i++)
				{
					dmean += m_utils.distanceToNearestPoint(normalizedFront[i], normalizedFront);
				}
				
				dmean = dmean / (numberOfPoints);
				
				// STEP 7. Calculate the distance to extremal values
				double dExtrems = 0.0;
				for (int i = 0; i < extremValues.Length; i++)
				{
					dExtrems += m_utils.distanceToClosedPoint(extremValues[i], normalizedFront);
				}
				
				// STEP 8. Computing the value of the metric
				double mean = 0.0;
				for (int i = 0; i < normalizedFront.Length; i++)
				{
					mean += System.Math.Abs(m_utils.distanceToNearestPoint(normalizedFront[i], normalizedFront) - dmean);
				}
				
				double val = (dExtrems + mean) / (dExtrems + (numberOfPoints * dmean));
				return val;
			}
		} // generalizedSpread
		
		/// <summary> This class can be invoked from the command line. Three params are required:
		/// 1) the name of the file containing the front,  
		/// 2) the name of the file containig the true Pareto front
		/// 3) the number of objectives
		/// </summary>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			if (args.Length < 3)
			{
				System.Console.Error.WriteLine("Error using GeneralizedSpread. " + "Usage: \n java GeneralizedSpread" + " <SolutionFrontFile> " + " <TrueFrontFile> + <numberOfObjectives>");
				System.Environment.Exit(1);
			}
			
			//Create a new instance of the metric
			GeneralizedSpread qualityIndicator = new GeneralizedSpread();
			//Read the front from the files
            double[][] solutionFront = JARE.qualityIndicator.GeneralizedSpread.m_utils.readFront(args[0]);
            double[][] trueFront = JARE.qualityIndicator.GeneralizedSpread.m_utils.readFront(args[1]);
			
			//Obtain delta value
			double val = qualityIndicator.generalizedSpread(solutionFront, trueFront, (System.Int32.Parse(args[2])));
			
			System.Console.Out.WriteLine(val);
		} // main
	} // GeneralizedSpread
}
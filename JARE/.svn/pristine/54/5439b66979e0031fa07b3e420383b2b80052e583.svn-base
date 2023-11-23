/// <summary> Spread.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.qualityIndicator
{
	
	/// <summary> This class implements the spread metric. It can be used also as command
	/// line program just by typing "java Spread <solutionFrontFile> <trueFrontFile>". 
	/// This metric is only applicable to two bi-objective problems.
	/// Reference: Deb, K., Pratap, A., Agarwal, S., Meyarivan, T.: A fast and 
	/// elitist multiobjective genetic algorithm: NSGA-II. IEEE Trans. 
	/// on Evol. Computation 6 (2002) 182-197
	/// </summary>
	public class Spread
	{

        internal static JARE.qualityIndicator.util.MetricsUtil m_utils; //utils_ is used to access to 
		//the MetricsUtil funcionalities
		
		/// <summary> Constructor.
		/// Creates a new instance of a Spread object 
		/// </summary>
		public Spread()
		{
            m_utils = new JARE.qualityIndicator.util.MetricsUtil();
		} // Delta
		
		
		/// <summary>Calculates the Spread metric. Given the front, the true pareto front as 
		/// <code>double []</code>, and the number of objectives, 
		/// the method returns the value of the metric.
		/// </summary>
		/// <param name="front">The front.
		/// </param>
		/// <param name="trueParetoFront">The true pareto front.
		/// </param>
		/// <param name="numberOfObjectives">The number of objectives.
		/// </param>
		public virtual double spread(double[][] front, double[][] trueParetoFront, int numberOfObjectives)
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
			maximumValue = m_utils.getMaximumValues(trueParetoFront, numberOfObjectives);
			minimumValue = m_utils.getMinimumValues(trueParetoFront, numberOfObjectives);
			
			// STEP 2. Get the normalized front and true Pareto fronts
			normalizedFront = m_utils.getNormalizedFront(front, maximumValue, minimumValue);
			normalizedParetoFront = m_utils.getNormalizedFront(trueParetoFront, maximumValue, minimumValue);
			
			// STEP 3. Sort normalizedFront and normalizedParetoFront;
            System.Array.Sort(normalizedFront, new JARE.qualityIndicator.util.LexicoGraphicalComparator());
            System.Array.Sort(normalizedParetoFront, new JARE.qualityIndicator.util.LexicoGraphicalComparator());
			
			int numberOfPoints = normalizedFront.Length;
			//    int numberOfTruePoints = normalizedParetoFront.length;
			
			// STEP 4. Compute df and dl (See specifications in Deb's description of 
			// the metric)
			double df = m_utils.distance(normalizedFront[0], normalizedParetoFront[0]);
			double dl = m_utils.distance(normalizedFront[normalizedFront.Length - 1], normalizedParetoFront[normalizedParetoFront.Length - 1]);
			
			double mean = 0.0;
			double diversitySum = df + dl;
			
			// STEP 5. Calculate the mean of distances between points i and (i - 1). 
			// (the poins are in lexicografical order)
			for (int i = 0; i < (normalizedFront.Length - 1); i++)
			{
				mean += m_utils.distance(normalizedFront[i], normalizedFront[i + 1]);
			} // for
			
			mean = mean / (double) (numberOfPoints - 1);
			
			// STEP 6. If there are more than a single point, continue computing the 
			// metric. In other case, return the worse value (1.0, see metric's 
			// description).
			if (numberOfPoints > 1)
			{
				for (int i = 0; i < (numberOfPoints - 1); i++)
				{
					diversitySum += System.Math.Abs(m_utils.distance(normalizedFront[i], normalizedFront[i + 1]) - mean);
				} // for
				return diversitySum / (df + dl + (numberOfPoints - 1) * mean);
			}
			else
				return 1.0;
		} 
		
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
				System.Console.Error.WriteLine("Spread::Main: Error using Spread. Usage: \n java " + "Spread <FrontFile> <TrueFrontFile>  " + "<numberOfObjectives>");
				System.Environment.Exit(1);
			} // if
			
			// STEP 1. Create a new instance of the metric
			Spread qualityIndicator = new Spread();
			
			// STEP 2. Read the fronts from the files
            double[][] solutionFront = JARE.qualityIndicator.Spread.m_utils.readFront(args[0]);
            double[][] trueFront = JARE.qualityIndicator.Spread.m_utils.readFront(args[1]);
			
			// STEP 3. Obtain the metric value
			double val = qualityIndicator.spread(solutionFront, trueFront, 2);
			
			System.Console.Out.WriteLine(val);
		} 
	} 
}
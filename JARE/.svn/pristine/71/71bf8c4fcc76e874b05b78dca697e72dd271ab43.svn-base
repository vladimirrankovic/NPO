/// <summary> GenerationalDistance.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.qualityIndicator
{
	
	/// <summary> This class implements the generational distance metric. It can be used also 
	/// as a command line by typing: "java GenerationalDistance <solutionFrontFile>  
	/// <trueFrontFile> <numberOfObjectives>"
	/// Reference: Van Veldhuizen, D.A., Lamont, G.B.: Multiobjective Evolutionary 
	/// Algorithm Research: A History and Analysis. 
	/// Technical Report TR-98-03, Dept. Elec. Comput. Eng., Air Force 
	/// Inst. Technol. (1998)
	/// </summary>
	public class GenerationalDistance
	{
        internal JARE.qualityIndicator.util.MetricsUtil m_utils; //utils_ is used to access to the
		//MetricsUtil funcionalities
		
		internal const double m_pow = 2.0; //pow. This is the pow used for the
		//distances
		
		/// <summary> Constructor.
		/// Creates a new instance of the generational distance metric. 
		/// </summary>
		public GenerationalDistance()
		{
            m_utils = new JARE.qualityIndicator.util.MetricsUtil();
		} // GenerationalDistance
		
		/// <summary> Returns the generational distance value for a given front</summary>
		/// <param name="front">The front 
		/// </param>
		/// <param name="trueParetoFront">The true pareto front
		/// </param>
		public virtual double generationalDistance(double[][] front, double[][] trueParetoFront, int numberOfObjectives)
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
			
			// STEP 3. Sum the distances between each point of the front and the 
			// nearest point in the true Pareto front
			double sum = 0.0;
			for (int i = 0; i < front.Length; i++)
				sum += System.Math.Pow(m_utils.distanceToClosedPoint(normalizedFront[i], normalizedParetoFront), m_pow);
			
			
			// STEP 4. Obtain the sqrt of the sum
			sum = System.Math.Pow(sum, 1.0 / m_pow);
			
			// STEP 5. Divide the sum by the maximum number of points of the front
			double generationalDistance = sum / normalizedFront.Length;
			
			return generationalDistance;
		} // generationalDistance
		
		/// <summary> This class can be invoqued from the command line. Two params are required:
		/// 1) the name of the file containing the front, and 2) the name of the file 
		/// containig the true Pareto front
		/// 
		/// </summary>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			if (args.Length < 2)
			{
				System.Console.Error.WriteLine("GenerationalDistance::Main: Usage: java " + "GenerationalDistance <FrontFile> " + "<TrueFrontFile>  <numberOfObjectives>");
				System.Environment.Exit(1);
			} // if
			
			// STEP 1. Create an instance of Generational Distance
			GenerationalDistance qualityIndicator = new GenerationalDistance();
			
			// STEP 2. Read the fronts from the files
			double[][] solutionFront = qualityIndicator.m_utils.readFront(args[0]);
			double[][] trueFront = qualityIndicator.m_utils.readFront(args[1]);
			
			// STEP 3. Obtain the metric value
			double val = qualityIndicator.generationalDistance(solutionFront, trueFront, (System.Int32.Parse(args[2])));
			
			System.Console.Out.WriteLine(val);
		} // main  
	} // GenerationalDistance
}
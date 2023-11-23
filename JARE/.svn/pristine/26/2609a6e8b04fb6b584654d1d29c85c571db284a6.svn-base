/// <summary> QualityIndicator.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// This class provides methods for calculating the values of quality indicators
/// from a solution set. After creating an instance of this class, which requires
/// the file containing the true Pareto of the problem as a parementer, methods
/// such as getHypervolume(), getSpread(), etc. are available
/// </version>
using System;
using Problem = JARE.Base.Problem;
using SolutionSet = JARE.Base.SolutionSet;
namespace JARE.qualityIndicator
{
	
	/// <summary> QualityIndicator class</summary>
	public class QualityIndicator
	{
		/// <summary> Returns the hypervolume of the true Pareto front</summary>
		/// <returns> The hypervolume of the true Pareto front
		/// </returns>
		virtual public double TrueParetoFrontHypervolume
		{
			get
			{
				return m_trueParetoFrontHypervolume;
			}
			
		}
		internal SolutionSet m_trueParetoFront;
		internal double m_trueParetoFrontHypervolume;
		internal Problem m_problem;
        internal JARE.qualityIndicator.util.MetricsUtil m_utilities;
		
		/// <summary> Constructor</summary>
		/// <param name="paretoFrontFile">
		/// </param>
		public QualityIndicator(Problem problem, System.String paretoFrontFile)
		{
			m_problem = problem;
            m_utilities = new JARE.qualityIndicator.util.MetricsUtil();
			m_trueParetoFront = m_utilities.readNonDominatedSolutionSet(paretoFrontFile);
			m_trueParetoFrontHypervolume = new Hypervolume().hypervolume(m_trueParetoFront.writeObjectivesToMatrix(), m_trueParetoFront.writeObjectivesToMatrix(), m_problem.NumberOfObjectives);
		} 
		
		/// <summary> Returns the hypervolume of solution set</summary>
		/// <param name="solutionSet">
		/// </param>
		/// <returns> The value of the hypervolume indicator
		/// </returns>
		public virtual double getHypervolume(SolutionSet solutionSet)
		{
			return new Hypervolume().hypervolume(solutionSet.writeObjectivesToMatrix(), m_trueParetoFront.writeObjectivesToMatrix(), m_problem.NumberOfObjectives);
		} 
		
		/// <summary> Returns the inverted generational distance of solution set</summary>
		/// <param name="solutionSet">
		/// </param>
		/// <returns> The value of the hypervolume indicator
		/// </returns>
		public virtual double getIGD(SolutionSet solutionSet)
		{
			return new InvertedGenerationalDistance().invertedGenerationalDistance(solutionSet.writeObjectivesToMatrix(), m_trueParetoFront.writeObjectivesToMatrix(), m_problem.NumberOfObjectives);
		} 
		
		/// <summary> Returns the generational distance of solution set</summary>
		/// <param name="solutionSet">
		/// </param>
		/// <returns> The value of the hypervolume indicator
		/// </returns>
		public virtual double getGD(SolutionSet solutionSet)
		{
			return new GenerationalDistance().generationalDistance(solutionSet.writeObjectivesToMatrix(), m_trueParetoFront.writeObjectivesToMatrix(), m_problem.NumberOfObjectives);
		} 
		
		/// <summary> Returns the spread of solution set</summary>
		/// <param name="solutionSet">
		/// </param>
		/// <returns> The value of the hypervolume indicator
		/// </returns>
		public virtual double getSpread(SolutionSet solutionSet)
		{
			return new Spread().spread(solutionSet.writeObjectivesToMatrix(), m_trueParetoFront.writeObjectivesToMatrix(), m_problem.NumberOfObjectives);
		} 
		
		/// <summary> Returns the epsilon indicator of solution set</summary>
		/// <param name="solutionSet">
		/// </param>
		/// <returns> The value of the hypervolume indicator
		/// </returns>
		public virtual double getEpsilon(SolutionSet solutionSet)
		{
			return new Epsilon().epsilon(solutionSet.writeObjectivesToMatrix(), m_trueParetoFront.writeObjectivesToMatrix(), m_problem.NumberOfObjectives);
		} 
	} 
}
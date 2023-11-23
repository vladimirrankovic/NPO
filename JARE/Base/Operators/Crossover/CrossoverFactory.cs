/// <summary> CrossoverFactory.java
/// 
/// </summary>
/// <author>  Juanjo Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
// visnja: Ovo nije potrebno
//using PropUtils = JARE.gui.utils.PropUtils;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
namespace JARE.Base.operators.crossover
{
	
	/// <summary> Class implementing a crossover factory.</summary>
	public class CrossoverFactory
	{
		
		/// <summary> Gets a crossover operator through its name.</summary>
		/// <param name="name">Name of the operator
		/// </param>
		/// <returns> The operator
		/// </returns>
		public static Crossover getCrossoverOperator(System.String name)
		{
			if (name.ToUpper().Equals("SBXCrossover".ToUpper()))
				return new SBXCrossover();
			else if (name.ToUpper().Equals("SinglePointCrossover".ToUpper()))
				return new SinglePointCrossover();
			else if (name.ToUpper().Equals("PMXCrossover".ToUpper()))
				return new PMXCrossover();
			else if (name.ToUpper().Equals("TwoPointsCrossover".ToUpper()))
				return new TwoPointsCrossover();
			else if (name.ToUpper().Equals("HUXCrossover".ToUpper()))
				return new HUXCrossover();
			else if (name.ToUpper().Equals("DifferentialEvolutionCrossover".ToUpper()))
				return new DifferentialEvolutionCrossover();
            else if (name.ToUpper().Equals("WeightsSinglePointCrossover".ToUpper()))
                return new WeightsCrossover(name);
            else if (name.ToUpper().Equals("CCWeightsCrossover".ToUpper()))
                return new WeightsCrossover(name);
            else if (name.ToUpper().Equals("WholeArithmeticCrossover".ToUpper()))
                return new WholeArithmeticCrossover();
			else if (name.ToUpper().Equals("WeightsUniformCrossover".ToUpper()))
				return new WeightsCrossover(name);
			else if (name.ToUpper().Equals("UniformCrossover".ToUpper()))
				return new UniformCrossover();
			else
			{
				Configuration.m_logger.WriteLog("CrossoverFactory.getCrossoverOperator. " + "Operator '" + name + "' not found ");
				throw new SMException("Exception in " + name + ".getCrossoverOperator()");
			} // else        
		} // getCrossoverOperator

        // Visnja: Stavljeno pod komentar jer se nigde ne koristi, a nije rastumaceno kako funkcionise

		/// <summary> Gets a crossover operator through its name.</summary>
		/// <param name="name">Name of the operator
		/// </param>
		/// <returns> The operator
		/// </returns>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //public static Crossover getCrossoverOperator(System.String name, System.Collections.Specialized.NameValueCollection properties)
        //{
        //    if (name.ToUpper().Equals("SBXCrossover".ToUpper()))
        //        return new SBXCrossover(PropUtils.getPropertiesWithPrefix(properties, name + "."));
        //    else if (name.ToUpper().Equals("SinglePointCrossover".ToUpper()))
        //        return new SinglePointCrossover(PropUtils.getPropertiesWithPrefix(properties, name + "."));
        //    else if (name.ToUpper().Equals("PMXCrossover".ToUpper()))
        //        return new PMXCrossover(PropUtils.getPropertiesWithPrefix(properties, name + "."));
        //    else if (name.ToUpper().Equals("TwoPointsCrossover".ToUpper()))
        //        return new TwoPointsCrossover(PropUtils.getPropertiesWithPrefix(properties, name + "."));
        //    else if (name.ToUpper().Equals("HUXCrossover".ToUpper()))
        //        return new HUXCrossover(PropUtils.getPropertiesWithPrefix(properties, name + "."));
        //    else if (name.ToUpper().Equals("DifferentialEvolutionCrossover".ToUpper()))
        //        return new DifferentialEvolutionCrossover(PropUtils.getPropertiesWithPrefix(properties, name + "."));
        //    else
        //    {
        //        Configuration.m_logger.WriteLog("CrossoverFactory.getCrossoverOperator. " + "Operator '" + name + "' not found ");
        //        throw new SMException("Exception in " + name + ".getCrossoverOperator()");
        //    } // else
        //} // getCrossoverOperator
	} // CrossoverFactory
}
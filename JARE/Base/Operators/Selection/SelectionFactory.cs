/// <summary> SelectionFactory.java
/// 
/// </summary>
/// <author>  Juanjo Durillo
/// </author>
/// <version>  1.1
/// </version>
using System;
using Operator = JARE.Base.operators;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
namespace JARE.Base.operators.selection
{
	
	/// <summary> Class implementing a selection operator factory.</summary>
	public class SelectionFactory
	{
		
		/// <summary> Gets a selection operator through its name.</summary>
		/// <param name="name">of the operator
		/// </param>
		/// <returns> the operator
		/// </returns>
		/// <throws>  SMException  </throws>
		public static Operator getSelectionOperator(System.String name)
		{
			if (name.ToUpper().Equals("BinaryTournament".ToUpper()))
				return new BinaryTournament();
			else if (name.ToUpper().Equals("BinaryTournament2".ToUpper()))
				return new BinaryTournament2();
			else if (name.ToUpper().Equals("PESA2Selection".ToUpper()))
				return new PESA2Selection();
			else if (name.ToUpper().Equals("RandomSelection".ToUpper()))
				return new RandomSelection();
			else if (name.ToUpper().Equals("RankingAndCrowdingSelection".ToUpper()))
				return new RankingAndCrowdingSelection();
			else if (name.ToUpper().Equals("DifferentialEvolutionSelection".ToUpper()))
				return new DifferentialEvolutionSelection();
            else if (name.ToUpper().Equals("BestSolutionSelection".ToUpper()))
                return new BestSolutionSelection();
            else if (name.ToUpper().Equals("WorstSolutionSelection".ToUpper()))
                return new WorstSolutionSelection();
            else if (name.ToUpper().Equals("DifferentialEvolutionSelection".ToUpper()))
                return new DifferentialEvolutionSelection();
            else if (name.ToUpper().Equals("RouletteWheelSelection".ToUpper()))
                return new RouletteWheelSelection();
            else if (name.ToUpper().Equals("RouletteWheelSelection2".ToUpper()))
                return new RouletteWheelSelection2();
            else
			{
				Configuration.m_logger.WriteLog("Operator '" + name + "' not found ");
				throw new SMException("Exception in " + name + ".getSelectionOperator()");
			} // else    
		} // getSelectionOperator
	} // SelectionFactory
}
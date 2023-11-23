/// <summary> SinglePointCrossover.java
/// Class representing a single point crossover operator
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.1
/// </version>
using System;
using JARE.Base;
using JARE.Base.variable;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.crossover
{
	
	/// <summary> This class allows to apply a Single Point crossover operator using two parent
	/// solutions.
	/// NOTE: the operator is applied to binary or integer solutions, considering the
	/// whole solution as a single variable.
	/// </summary>
	[Serializable]
	public class SinglePointCrossover:Crossover
	{
		/// <summary> BINARY_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
		private static System.Type BINARY_SOLUTION;
        /// <summary> BINARY_REAL_SOLUTION represents class JARE.base.solutionType.BinaryRealSolutionType</summary>
		private static System.Type BINARY_REAL_SOLUTION;
        /// <summary> INT_SOLUTION represents class JARE.base.solutionType.IntSolutionType</summary>
		private static System.Type INT_SOLUTION;
        /// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type REAL_SOLUTION;
        /// <summary> REAL_WEIGHTS_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type REAL_WEIGHTS_SOLUTION;

		/// <summary> Constructor
		/// Creates a new instance of the single point crossover operator
		/// </summary>
		public SinglePointCrossover()
		{
			try
			{
                BINARY_SOLUTION = System.Type.GetType("JARE.Base.solutionType.BinarySolutionType");
                BINARY_REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.BinaryRealSolutionType");
                INT_SOLUTION = System.Type.GetType("JARE.Base.solutionType.IntSolutionType");
                REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealSolutionType");
                REAL_WEIGHTS_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealWeightsSolutionType");
            }
			catch (System.Exception e)
			{
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // catch
		} // SinglePointCrossover
		
		
		/// <summary> Constructor
		/// Creates a new instance of the single point crossover operator
		/// </summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public SinglePointCrossover(System.Collections.IDictionary properties):this()
		{
		} // SinglePointCrossover
		
		/// <summary> Perform the crossover operation.</summary>
		/// <param name="probability">Crossover probability
		/// </param>
		/// <param name="parent1">The first parent
		/// </param>
		/// <param name="parent2">The second parent
		/// </param>
		/// <returns> An array containig the two offsprings
		/// </returns>
		/// <throws>  SMException </throws>
		public virtual Solution[] doCrossover(double probability, Solution parent1, Solution parent2)
		{
			Solution[] offSpring = new Solution[2];
			offSpring[0] = new Solution(parent1);
			offSpring[1] = new Solution(parent2);
			try
			{
				if (PseudoRandom.randDouble() < probability)
				{
					if ((parent1.Type.GetType() == BINARY_SOLUTION) || (parent1.Type.GetType() == BINARY_REAL_SOLUTION))
					{
						//1. Compute the total number of bits
						int totalNumberOfBits = 0;
						for (int i = 0; i < parent1.DecisionVariables.Length; i++)
						{
							totalNumberOfBits += ((Binary) parent1.DecisionVariables[i]).NumberOfBits;
						}
						
						//2. Calcule the point to make the crossover
						int crossoverPoint = PseudoRandom.randInt(0, totalNumberOfBits - 1);
						
						//3. Compute the variable that containt the crossoverPoint bit
						int variable = 0;
						int acountBits = ((Binary) parent1.DecisionVariables[variable]).NumberOfBits;
						
						while (acountBits < (crossoverPoint + 1))
						{
							variable++;
							acountBits += ((Binary) parent1.DecisionVariables[variable]).NumberOfBits;
						}
						
						//4. Compute the bit into the variable selected
						int diff = acountBits - crossoverPoint;
						int intoVariableCrossoverPoint = ((Binary) parent1.DecisionVariables[variable]).NumberOfBits - diff;
						
						//5. Make the crossover into the the gene;
						Binary offSpring1, offSpring2;
						offSpring1 = (Binary) parent1.DecisionVariables[variable].deepCopy();
						offSpring2 = (Binary) parent2.DecisionVariables[variable].deepCopy();
						
						for (int i = intoVariableCrossoverPoint; i < offSpring1.NumberOfBits; i++)
						{
                            bool swap = offSpring1.m_bits.Get(i);
                            offSpring1.m_bits.Set(i, offSpring2.m_bits.Get(i));
							offSpring2.m_bits.Set(i, swap);
						}
						
						offSpring[0].DecisionVariables[variable] = offSpring1;
						offSpring[1].DecisionVariables[variable] = offSpring2;
						
						//6. Apply the crossover to the other variables
						for (int i = 0; i < variable; i++)
						{
							offSpring[0].DecisionVariables[i] = parent2.DecisionVariables[i].deepCopy();
							
							offSpring[1].DecisionVariables[i] = parent1.DecisionVariables[i].deepCopy();
						}
						
						//7. Decode the results
						for (int i = 0; i < offSpring[0].DecisionVariables.Length; i++)
						{
							((Binary) offSpring[0].DecisionVariables[i]).decode();
							((Binary) offSpring[1].DecisionVariables[i]).decode();
						}
					}
					// Binary or BinaryReal
                    else if ((parent1.Type.GetType() == INT_SOLUTION) || (parent1.Type.GetType() == REAL_SOLUTION)
                            ||(parent1.Type.GetType() == REAL_WEIGHTS_SOLUTION))
					{
						// Integer or Real representation
                        //int crossoverPoint = PseudoRandom.randInt(0, parent1.numberOfVariables() - 1); U ovoj varijanti menjaju sve gene sto je glupo
                        int crossoverPoint = PseudoRandom.randInt(1, parent1.numberOfVariables() - 2);
                        if (parent1.Type.GetType() == INT_SOLUTION)
                        {
						    int valueX1;
						    int valueX2;
                            for (int i = crossoverPoint; i < parent1.numberOfVariables(); i++)
                            {
                                valueX1 = (int)parent1.DecisionVariables[i].getValue();
                                valueX2 = (int)parent2.DecisionVariables[i].getValue();
                                offSpring[0].DecisionVariables[i].setValue(valueX2);
                                offSpring[1].DecisionVariables[i].setValue(valueX1);
                            } // for
                        }
                        else
                        {
                            double valueX1;
                            double valueX2;
                            for (int i = crossoverPoint; i < parent1.numberOfVariables(); i++)
                            {
                                valueX1 = parent1.DecisionVariables[i].getValue();
                                valueX2 = parent2.DecisionVariables[i].getValue();
                                offSpring[0].DecisionVariables[i].setValue(valueX2);
                                offSpring[1].DecisionVariables[i].setValue(valueX1);
                            } // for

                        }
					} // Int or Real representation
				}
			}
			catch (System.InvalidCastException)
			{
				Configuration.m_logger.WriteLog("SinglePointCrossover.doCrossover: Cannot perfom " + "SinglePointCrossover");
                string name = this.GetType().FullName; 
				throw new SMException("Exception in " + name + ".doCrossover()");
			}
			return offSpring;
		} // doCrossover
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing an array of two solutions
		/// </param>
		/// <returns> An object containing an array with the offSprings
		/// </returns>
		/// <throws>  SMException </throws>
		public override System.Object execute(System.Object obj)
		{
			Solution[] parents = (Solution[]) obj;
			
			if (((parents[0].Type.GetType() != BINARY_SOLUTION) || (parents[1].Type.GetType() != BINARY_SOLUTION)) && 
                ((parents[0].Type.GetType() != BINARY_REAL_SOLUTION) || (parents[1].Type.GetType() != BINARY_REAL_SOLUTION)) && 
                ((parents[0].Type.GetType() != INT_SOLUTION) || (parents[1].Type.GetType() != INT_SOLUTION)) &&
                ((parents[0].Type.GetType() != REAL_SOLUTION) || (parents[1].Type.GetType() != REAL_SOLUTION)) &&
                ((parents[0].Type.GetType() != REAL_WEIGHTS_SOLUTION) || (parents[1].Type.GetType() != REAL_WEIGHTS_SOLUTION)))
            {
			
				Configuration.m_logger.WriteLog("SinglePointCrossover.execute: the solutions " + "are not of the right type. The type should be 'Binary' or 'Int', but " + parents[0].Type + " and " + parents[1].Type + " are obtained");
                string name = this.GetType().FullName; 
				throw new SMException("Exception in " + name + ".execute()");
			} // if

            if (parents.Length < 2)
			{
				Configuration.m_logger.WriteLog("SinglePointCrossover.execute: operator " + "needs two parents");
                string name = this.GetType().FullName; 
				throw new SMException("Exception in " + name + ".execute()");
			}
			else
			{
                if (getParameter("probability") == null)
				{
					Configuration.m_logger.WriteLog("SinglePointCrossover.execute: probability " + "not specified");
                    string name = this.GetType().FullName; 
					throw new SMException("Exception in " + name + ".execute()");
				}
			}
            System.Double probability = (System.Double)getParameter("probability");
			
			Solution[] offSpring;
			offSpring = doCrossover(probability, parents[0], parents[1]);
			
			//-> Update the offSpring solutions
			for (int i = 0; i < offSpring.Length; i++)
			{
				offSpring[i].CrowdingDistance = 0.0;
				offSpring[i].Rank = 0;
			}
			return offSpring; //*/
		} // execute
	} // SinglePointCrossover
}
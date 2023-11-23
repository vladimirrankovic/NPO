/// <summary> CCWeightsCrossover.java
/// Class representing a single point crossover operator
/// </summary>
/// <author>  Vladimir Rankovic
/// </author>
/// <author>  
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
	public class CCWeightsCrossover:Crossover
	{
        /// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type REAL_WEIGHTS_SOLUTION;

		/// <summary> Constructor
		/// Creates a new instance of the single point crossover operator
		/// </summary>
		public CCWeightsCrossover()
		{
			try
			{
                REAL_WEIGHTS_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealWeightsSolutionType");

			}
			catch (System.Exception e)
			{
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // catch
        } // CCWeightsCrossover
		
		
		/// <summary> Constructor
		/// Creates a new instance of the single point crossover operator
		/// </summary>
        public CCWeightsCrossover(System.Collections.IDictionary properties) : this()
		{
        } // CCWeightsCrossover
		
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
            if (PseudoRandom.randDouble() < probability)//Otherwise, offspring are simply copies of their parents
            {
                try
                {
                    // Integer or Real representation
                    int counter = 1;

                    if (parent1.Type.GetType() == REAL_WEIGHTS_SOLUTION)
                    {
                        double valueX1;
                        double valueX2;
                        for (int i = 0; i < parent1.numberOfVariables(); i++)
                        {
                            valueX1 = parent1.DecisionVariables[i].getValue();
                            valueX2 = parent2.DecisionVariables[i].getValue();

                            if (valueX1 != 0.0 && valueX2 == 0.0)
                            {
                                if (counter % 2 == 0)
                                {
                                    offSpring[0].DecisionVariables[i].setValue(valueX1);
                                    offSpring[1].DecisionVariables[i].setValue(valueX2);
                                }
                                else
                                {
                                    offSpring[0].DecisionVariables[i].setValue(valueX2);
                                    offSpring[1].DecisionVariables[i].setValue(valueX1);
                                }
                                counter++;
                            }
                            else if (valueX1 == 0.0 && valueX2 != 0.0)
                            {
                                if (counter % 2 == 0)
                                {
                                    offSpring[0].DecisionVariables[i].setValue(valueX2);
                                    offSpring[1].DecisionVariables[i].setValue(valueX1);
                                }
                                else
                                {
                                    offSpring[0].DecisionVariables[i].setValue(valueX1);
                                    offSpring[1].DecisionVariables[i].setValue(valueX2);
                                }
                                counter++;
                            }
                            else if (valueX1 != 0.0 && valueX2 != 0.0)
                            {
                                if (parent1.Fitness > parent2.Fitness)
                                {
                                    offSpring[1].DecisionVariables[i].setValue(valueX1);
                                }
                                else
                                {
                                    offSpring[0].DecisionVariables[i].setValue(valueX2);
                                }
                            }
                        } // for

                    }
                    //else if (parent1.Type.GetType() == INT_SOLUTION)
                    //{
                    //    int valueX1;
                    //    int valueX2;
                    //    for (int i = 0; i < parent1.numberOfVariables(); i++)
                    //    {
                    //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    //        valueX1 = (int)parent1.DecisionVariables[i].getValue();
                    //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    //        valueX2 = (int)parent2.DecisionVariables[i].getValue();
                    //        offSpring[0].DecisionVariables[i].setValue(valueX2);
                    //        offSpring[1].DecisionVariables[i].setValue(valueX1);
                    //    } // for
                    //}
                }
                catch (System.InvalidCastException)
                {
                    Configuration.m_logger.WriteLog("CCWeightsCrossover.doCrossover: Cannot perfom " + "CCWeightsCrossover");
                    string name = this.GetType().FullName;
                    throw new SMException("Exception in " + name + ".doCrossover()");
                }
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

            if ((parents[0].Type.GetType() != REAL_WEIGHTS_SOLUTION) || (parents[1].Type.GetType() != REAL_WEIGHTS_SOLUTION))
			{
				
                Configuration.m_logger.WriteLog("CCWeightsCrossover.execute: the solutions " + "are not of the right type. The type should be 'Binary' or 'Int', but " + parents[0].Type + " and " + parents[1].Type + " are obtained");
				
                string name = this.GetType().FullName; 
				throw new SMException("Exception in " + name + ".execute()");
			} // if

            System.Double probability = (System.Double)getParameter("probability");
            if (parents.Length < 2)
			{
                Configuration.m_logger.WriteLog("CCWeightsCrossover.execute: operator " + "needs two parents");
                string name = this.GetType().FullName; 
				throw new SMException("Exception in " + name + ".execute()");
			}
			else
			{
                if (getParameter("probability") == null)
				{
                    Configuration.m_logger.WriteLog("CCWeightsCrossover.execute: probability " + "not specified");
                    string name = this.GetType().FullName; 
					throw new SMException("Exception in " + name + ".execute()");
				}
			}
			
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
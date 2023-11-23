/// <summary> UniformCrossover
/// Class representing a uniform crossover operator
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
	
	/// <summary> This class allows to apply a Swap crossover operator using two parent
	/// solutions.
	/// NOTE: the operator is applied to binary or integer solutions, considering the
	/// whole solution as a single variable.
	/// </summary>
	[Serializable]
	public class UniformCrossover:Crossover
	{
        /// <summary> BINARY_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type BINARY_SOLUTION;
        /// <summary> BINARY_REAL_SOLUTION represents class JARE.base.solutionType.BinaryRealSolutionType</summary>
        private static System.Type BINARY_REAL_SOLUTION;
        /// <summary> INT_SOLUTION represents class JARE.base.solutionType.IntSolutionType</summary>
        private static System.Type INT_SOLUTION;
        /// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type REAL_SOLUTION;
        /// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type REAL_WEIGHTS_SOLUTION;

		/// <summary> Constructor
		/// Creates a new instance of the single point crossover operator
		/// </summary>
		public UniformCrossover()
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
        } // UniformCrossover
		
		
		/// <summary> Constructor
		/// Creates a new instance of the single point crossover operator
		/// </summary>
        public UniformCrossover(System.Collections.IDictionary properties) : this()
		{
        } // UniformCrossover
		
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

                    if ((parent1.Type.GetType() == INT_SOLUTION) ||
                        (parent1.Type.GetType() == REAL_SOLUTION) ||
                        (parent1.Type.GetType() == REAL_WEIGHTS_SOLUTION))
                    {
                            double valueX1;
                            double valueX2;
                            for (int i = 0; i < parent1.numberOfVariables(); i++)
                            {
                                if (parent1.Type.GetType() == INT_SOLUTION)
                                {
                                    valueX1 = (int)parent1.DecisionVariables[i].getValue();
                                    valueX2 = (int)parent2.DecisionVariables[i].getValue();
                                }
                                else
                                {
                                    valueX1 = parent1.DecisionVariables[i].getValue();
                                    valueX2 = parent2.DecisionVariables[i].getValue();
                                }
                                double swapProbability = 0.5;
                                if (PseudoRandom.randDouble() < swapProbability)
                                {
                                    offSpring[0].DecisionVariables[i].setValue(valueX2);
                                    offSpring[1].DecisionVariables[i].setValue(valueX1);
                                }
                            }
                    }
                    else throw new System.InvalidCastException();
			    }
			    catch (System.InvalidCastException)
			    {
                    Configuration.m_logger.WriteLog("UniformCrossover.doCrossover: Cannot perfom " + "UniformCrossover");
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

            if (
				((parents[0].Type.GetType() != REAL_WEIGHTS_SOLUTION) || (parents[1].Type.GetType() != REAL_WEIGHTS_SOLUTION))
				&& ((parents[0].Type.GetType() != REAL_SOLUTION) || (parents[1].Type.GetType() != REAL_SOLUTION))
				)
			{

                Configuration.m_logger.WriteLog("UniformCrossover.execute: the solutions " + "are not of the right type. The type should be 'Binary' or 'Int', but " + parents[0].Type + " and " + parents[1].Type + " are obtained");
				
                string name = this.GetType().FullName; 
				throw new SMException("Exception in " + name + ".execute()");
			} // if

            if (parents.Length < 2)
			{
                Configuration.m_logger.WriteLog("UniformCrossover.execute: operator " + "needs two parents");
                string name = this.GetType().FullName; 
				throw new SMException("Exception in " + name + ".execute()");
			}
			else
			{
                if (getParameter("probability") == null)
				{
                    Configuration.m_logger.WriteLog("UniformCrossover.execute: probability " + "not specified");
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
    } // UniformCrossover
}
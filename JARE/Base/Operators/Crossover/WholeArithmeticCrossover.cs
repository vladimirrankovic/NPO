/// <summary> WholeArithmeticCrossover.css
/// Class representing a simulated whole arithmetic crossover operator (V. A. F. Dallagnol, J. van den Berg, L. Mous, Portfolio Management Using Value at Risk:
/// A Comparison between Genetic Algorithms and Particle Swarm Optimization, INTERNATIONAL JOURNAL OF INTELLIGENT SYSTEMS, VOL. 24, 766–792 (2009))
/// </summary>
/// <author>  Vladimir Rankovic
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.variable;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
using XReal = JARE.util.wrapper.XReal;
namespace JARE.Base.operators.crossover
{
	
	/// <summary> This class allows to apply a basic crossover operator using two parent
	/// solutions.
	/// NOTE: the operator is applied to Real solutions, so the type of the solutions
	/// must be </code>m_solutionType.Real</code>.
	/// </summary>
	[Serializable]
	public class WholeArithmeticCrossover:Crossover
	{

        /// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type REAL_SOLUTION;
        
        /// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type REAL_WEIGHTS_SOLUTION;
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.ArrayRealSolutionType</summary>
		private static System.Type ARRAY_REAL_SOLUTION;
		
		/// <summary> Constructor
		/// Create a new Basic crossover operator whit a default
		/// index given by <code>DEFAULT_INDEX_CROSSOVER</code>
		/// </summary>
		public WholeArithmeticCrossover()
		{
			try
			{
                REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealSolutionType");
                REAL_WEIGHTS_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealWeightsSolutionType");
                ARRAY_REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.ArrayRealSolutionType");
			}
			catch (System.Exception e)
			{
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // catch
        } // WholeArithmeticCrossover
		
		
		/// <summary> Constructor
		/// Create a new Basic crossover operator whit a default
		/// index given by <code>DEFAULT_INDEX_CROSSOVER</code>
		/// </summary>
        public WholeArithmeticCrossover(System.Collections.IDictionary properties)
            : this()
		{
        } // WholeArithmeticCrossover
		
		/// <summary> Perform the crossover operation. </summary>
		/// <param name="probability">Crossover probability
		/// </param>
		/// <param name="parent1">The first parent
		/// </param>
		/// <param name="parent2">The second parent
		/// </param>
		/// <returns> An array containing the two offsprings
		/// </returns>
		public virtual Solution[] doCrossover(double probability, Solution parent1, Solution parent2)
		{			
			Solution[] offSpring = new Solution[2];
			
			offSpring[0] = new Solution(parent1);
			offSpring[1] = new Solution(parent2);

            if (PseudoRandom.randDouble() < probability)//Otherwise, offspring are simply copies of their parents
            {
                XReal par1 = new XReal(parent1);
                XReal par2 = new XReal(parent2);
                XReal offs1 = new XReal(offSpring[0]);
                XReal offs2 = new XReal(offSpring[1]);

                int numberOfVariables = offs1.NumberOfDecisionVariables;
                double fraction = PseudoRandom.randDouble(0.0, 1.0);
                for (int i = 0; i < numberOfVariables; i++)
                {
                    double par1Value = par1.getValue(i);
                    double par2Value = par2.getValue(i);
                    offs1.setValue(i, par1Value * fraction + par2Value * (1.0 - fraction));
                    offs2.setValue(i, par2Value * fraction + par1Value * (1.0 - fraction));
                }
            }

			return offSpring;
		} // doCrossover
		
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing an array of two parents
		/// </param>
		/// <returns> An object containing the offSprings
		/// </returns>
		public override System.Object execute(System.Object obj)
		{
			Solution[] parents = (Solution[]) obj;
			
			if (((parents[0].Type.GetType() != REAL_SOLUTION) && (parents[1].Type.GetType() != REAL_SOLUTION))
                && ((parents[0].Type.GetType() != ARRAY_REAL_SOLUTION) && (parents[1].Type.GetType() != ARRAY_REAL_SOLUTION))
                && ((parents[0].Type.GetType() != REAL_WEIGHTS_SOLUTION) && (parents[1].Type.GetType() != REAL_WEIGHTS_SOLUTION)))
            {
				
				Configuration.m_logger.WriteLog("WholeArithmeticCrossover.execute: the solutions " + "type " + parents[0].Type + " is not allowed with this operator");
				
                //System.Type cls = typeof(System.String);
                string name = this.GetType().FullName; 
                throw new SMException("Exception in " + name + ".execute()");
			} // if 
			
			System.Double probability = (System.Double) getParameter("probability");
			if (parents.Length < 2)
			{
                Configuration.m_logger.WriteLog("WholeArithmeticCrossover.execute: operator needs two " + "parents");
                //System.Type cls = typeof(System.String);
                string name = this.GetType().FullName; 
                throw new SMException("Exception in " + name + ".execute()");
			}
			else
			{
                if (getParameter("probability") == null)
				{
                    Configuration.m_logger.WriteLog("WholeArithmeticCrossover.execute: probability not " + "specified");
                    //System.Type cls = typeof(System.String);
                    string name = this.GetType().FullName; 
					throw new SMException("Exception in " + name + ".execute()");
				}
			}
			
			Solution[] offSpring;
			offSpring = doCrossover(probability, parents[0], parents[1]);
			
			for (int i = 0; i < offSpring.Length; i++)
			{
				offSpring[i].CrowdingDistance = 0.0;
				offSpring[i].Rank = 0;
			}
			return offSpring; //*/
		} // execute 
	} // BasicCrossover
}
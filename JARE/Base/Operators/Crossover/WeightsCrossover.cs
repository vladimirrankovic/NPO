/// <summary> WeightsCrossover.css
/// Class representing a simulated basic crossover operator for ponder variables
/// </summary>
/// <author>  Vladimir Rankovic
/// </author>
/// <version>  1.0
/// </version>
using System;
using System.Collections.Generic;
using System.Text;
using XReal = JARE.util.wrapper.XReal;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using Crossover = JARE.Base.operators.crossover.Crossover;
using SinglePointCrossover = JARE.Base.operators.crossover.SinglePointCrossover;

namespace JARE.Base.operators.crossover
{
	class WeightsCrossover:Crossover
	{
        private SinglePointCrossover SinglePointCrossover;
        private UniformCrossover UniformCrossoverInstance;
        private CCWeightsCrossover CardinalityConstrainedCrossover;
        private System.String OperatorName;
        ///// <summary> Constructor
        ///// Create a new weights crossover operator with a default
        ///// index given by <code>DEFAULT_INDEX_WEIGHTSCROSSOVER</code>
        ///// </summary>
        //public WeightsCrossover():base()
        //{
        //} // WeightsCrossover

		/// <summary> Constructor
		/// Create a new Basic crossover operator whit a default
		/// index given by <code>DEFAULT_INDEX_WEIGHTSCROSSOVER</code>
		/// </summary>
        public WeightsCrossover(System.String name)
		{
            OperatorName = name;

        } // WeightsCrossover

        /// <summary> Executes the operation</summary>
        /// <param name="object">An object containing an array of two parents
        /// </param>
        /// <returns> An object containing the offSprings
        /// </returns>
        public override System.Object execute(System.Object obj)
        {
            Solution[] offSpring;
            if (OperatorName.ToUpper().Equals("WeightsSinglePointCrossover".ToUpper()))
            {
                SinglePointCrossover = new SinglePointCrossover();
                CopyParameters(SinglePointCrossover);
                
                offSpring = (Solution[])SinglePointCrossover.execute(obj);
            }
            else if (OperatorName.ToUpper().Equals("WeightsUniformCrossover".ToUpper()))
            {
                UniformCrossoverInstance = new UniformCrossover();
                CopyParameters(UniformCrossoverInstance);

                offSpring = (Solution[])UniformCrossoverInstance.execute(obj);
            }
            else if (OperatorName.ToUpper().Equals("CCWeightsCrossover".ToUpper()))
            {
                CardinalityConstrainedCrossover = new CCWeightsCrossover();
                CopyParameters(CardinalityConstrainedCrossover);

                offSpring = (Solution[])CardinalityConstrainedCrossover.execute(obj);
            }
            else
            {
                Configuration.m_logger.WriteLog("WeightsCrossover.execute " + "Crossover operator member'" + OperatorName + "' not found ");
                throw new SMException("Exception in " + OperatorName + "crossover operator constructor");
            } // else        

            Normalize(offSpring);
            return offSpring; //*/
        } // execute 

        private void Normalize(Solution[] offspring)
        {
            for (int j = 0; j < offspring.Length; j++)
            {
                Normalize(offspring[j]);
            }
        }

        private void Normalize(Solution offspring)
        {
            int numberOfVariables = offspring.DecisionVariables.Length;
            double variableSum = 0.0;
            for (int i = 0; i < numberOfVariables; i++)
            {
                variableSum += (double)offspring.DecisionVariables[i].getValue();
            }
            if (variableSum == 0.0)
            {
                Random sampleGenerator = new Random();
                int sample = sampleGenerator.Next(numberOfVariables);
                offspring.DecisionVariables[sample].setValue(1.0);
            }
            //else
            //{
            //    //for (int i = 0; i < numberOfVariables; i++) offspring.DecisionVariables[i].setValue((double)offspring.DecisionVariables[i].getValue()/variableSum);
            //}
            ((JARE.Base.solutionType.RealWeightsSolutionType)offspring.Type).doCorrection(offspring);
            ////////System.IO.StreamWriter writer = new System.IO.StreamWriter("solution.csv", true);
            ////////string tmp = "";
            ////////for (int i = 0; i < offspring.DecisionVariables.Length; i++)
            ////////{
            ////////    tmp += offspring.DecisionVariables[i].ToString() + ",";
            ////////}
            ////////writer.WriteLine(tmp);
            ////////writer.Close();

        }

	}
}

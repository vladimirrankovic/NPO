/// <summary> UniformPonderMutation.css
/// Class representing a uniform ponder mutation operator
/// </summary>
/// <author>  Vladimir Rankovic
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
using XReal = JARE.util.wrapper.XReal;
using Mutation = JARE.Base.operators.mutation.Mutation;
using UniformMutation = JARE.Base.operators.mutation.UniformMutation;
using System.Collections.Generic;
using System.Text;

namespace JARE.Base.operators.mutation
{
	class WeightsMutation:Mutation
	{
        private System.String OperatorName;
        private UniformMutation UniformMutation;
	
		/// <summary> Constructor
		/// Creates a new uniform mutation operator instance
		/// </summary>
        public WeightsMutation(System.String name)
        {
            OperatorName = name;

        } // WeightsMutation


        /// <summary> Executes the operation</summary>
        /// <param name="object">An object containing the solution to mutate
        /// </param>
        /// <throws>  SMException  </throws>
        public override System.Object execute(System.Object obj)
        {
            Solution solution = new Solution();
            if (OperatorName.ToUpper().Equals("WeightsUniformMutation".ToUpper()))
            {
                UniformMutation = new UniformMutation();
                CopyParameters(UniformMutation);
                solution = (Solution)UniformMutation.execute(obj);
            }
            else
            {
                Configuration.m_logger.WriteLog("Operator '" + OperatorName + "' not found ");
                string name2 = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                throw new SMException("Exception in " + name2 + ".execute()");
            }
			
            Normalize(solution);
            return solution;
		} // doMutation  

		/// <summary> Performs the operation</summary>
		/// <param name="solution">The solution to normalize
		/// </param>
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
            //    //for (int i = 0; i < numberOfVariables; i++) offspring.DecisionVariables[i].setValue((double)offspring.DecisionVariables[i].getValue() / variableSum);
            //}
            
            ((JARE.Base.solutionType.RealWeightsSolutionType)offspring.Type).doCorrection(offspring);
            //System.IO.StreamWriter writer = new System.IO.StreamWriter("solution.csv", true);
            //string tmp = "";
            //for (int i = 0; i < offspring.DecisionVariables.Length; i++)
            //{
            //    tmp += offspring.DecisionVariables[i].ToString() + ",";
            //}
            //writer.WriteLine(tmp);
            //writer.Close();

        } //Normalize
	} // UniformMutation
}

/// <summary> SwapMutation.java
/// Class representing a swap mutation operator
/// </summary>
/// <author>  Antonio J.Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
using JARE.Base.variable;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.mutation
{
	
	/// <summary> This class implements a swap mutation.
	/// NOTE: the operator is applied to the first variable of the solutions, and 
	/// the type of those variables must be <code>m_variableType.Permutation</code>.
	/// </summary>
	[Serializable]
	public class SwapMutation:Mutation
	{
		/// <summary> Constructor</summary>
		public SwapMutation()
		{
		} // Constructor
		
		
		/// <summary> Constructor</summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public SwapMutation(System.Collections.IDictionary properties):this()
		{
		} // Constructor
		
		/// <summary> Performs the operation</summary>
		/// <param name="probability">Mutation probability
		/// </param>
		/// <param name="solution">The solution to mutate
		/// </param>
		/// <throws>  SMException  </throws>
		public virtual void  doMutation(double probability, Solution solution)
		{
			int[] permutation;
			int permutationLength;
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                if (solution.DecisionVariables[0].VariableType == System.Type.GetType("JARE.Base.variable.Permutation"))
				{
					
					permutationLength = ((Permutation) solution.DecisionVariables[0]).Length;
					permutation = ((Permutation) solution.DecisionVariables[0]).m_vector;
					
					if (PseudoRandom.randDouble() < probability)
					{
						int pos1;
						int pos2;
						
						pos1 = PseudoRandom.randInt(0, permutationLength - 1);
						pos2 = PseudoRandom.randInt(0, permutationLength - 1);
						
						while (pos1 == pos2)
						{
							if (pos1 == (permutationLength - 1))
								pos2 = PseudoRandom.randInt(0, permutationLength - 2);
							else
								pos2 = PseudoRandom.randInt(pos1, permutationLength - 1);
						} // while
						// swap
						int temp = permutation[pos1];
						permutation[pos1] = permutation[pos2];
						permutation[pos2] = temp;
					} // if
				}
				// if
                else if (solution.Type.GetType() == System.Type.GetType("JARE.Base.solutionType.RealSolutionType") ||
                         solution.Type.GetType() == System.Type.GetType("JARE.Base.solutionType.RealWeightsSolutionType"))
                {
                    if (PseudoRandom.randDouble() < probability)
                    {
                        int pos1 = 0;
                        int pos2 = 0;

                        while (pos1 == pos2)
                        {
                            pos1 = PseudoRandom.randInt(0, solution.DecisionVariables.Length - 1);
                            pos2 = PseudoRandom.randInt(0, solution.DecisionVariables.Length - 1);
                        } // while
                        // swap
                        Real temp = new Real(solution.DecisionVariables[pos1]);
                        solution.DecisionVariables[pos1] = solution.DecisionVariables[pos2];
                        solution.DecisionVariables[pos2] = temp;
                    } // if
                }
                else
				{
					Configuration.m_logger.WriteLog("SwapMutation.doMutation: invalid type. " + "" + solution.DecisionVariables[0].VariableType);
					
                    //System.Type cls = typeof(System.String);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    string name = this.GetType().FullName;
					throw new SMException("Exception in " + name + ".doMutation()");
				}
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // catch               
		} // doMutation
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing the solution to mutate
		/// </param>
		/// <returns> an object containing the mutated solution
		/// </returns>
		/// <throws>  SMException  </throws>
		public override System.Object execute(System.Object obj)
		{
			Solution solution = (Solution) obj;
			
			System.Double probability = (System.Double) getParameter("probability");
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("probability") == null)
			{
				Configuration.m_logger.WriteLog("SwapMutation.execute: probability " + "not specified");
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			}
			
			this.doMutation(probability, solution);
			return solution;
		} // execute  
	} // SwapMutation
}
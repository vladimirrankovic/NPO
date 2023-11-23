/// <summary> PMXCrossover.java
/// Class representing a partially matched (PMX) crossover operator
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.Base.variable;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.crossover
{
	
	/// <summary> This class allows to apply a PMX crossover operator using two parent
	/// solutions.
	/// NOTE: the operator is applied to the first variable of the solutions, and 
	/// the type of those variables must be m_variableType.Permutation.
	/// </summary>
	[Serializable]
	public class PMXCrossover:Crossover
	{
		/// <summary> Constructor</summary>
		public PMXCrossover()
		{
		} // PMXCrossover
		
		
		/// <summary> Constructor</summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public PMXCrossover(System.Collections.IDictionary properties):this()
		{
		} // PMXCrossover
		
		
		/// <summary> Perform the crossover operation</summary>
		/// <param name="probability">Crossover probability
		/// </param>
		/// <param name="parent1">The first parent
		/// </param>
		/// <param name="parent2">The second parent
		/// </param>
		/// <returns> An array containig the two offsprings
		/// </returns>
		/// <throws>  SMException  </throws>
		public virtual Solution[] doCrossover(double probability, Solution parent1, Solution parent2)
		{
			
			Solution[] offspring = new Solution[2];
			
			offspring[0] = new Solution(parent1);
			offspring[1] = new Solution(parent2);
			
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                if (parent1.DecisionVariables[0].VariableType == System.Type.GetType("JARE.Base.variable.Permutation"))
				{
					
					int permutationLength;
					
					permutationLength = ((Permutation) parent1.DecisionVariables[0]).Length;
					
					int[] parent1Vector = ((Permutation) parent1.DecisionVariables[0]).m_vector;
					int[] parent2Vector = ((Permutation) parent2.DecisionVariables[0]).m_vector;
					int[] offspring1Vector = ((Permutation) offspring[0].DecisionVariables[0]).m_vector;
					int[] offspring2Vector = ((Permutation) offspring[1].DecisionVariables[0]).m_vector;
					
					if (PseudoRandom.randDouble() < probability)
					{
						int cuttingPoint1;
						int cuttingPoint2;
						
						//      STEP 1: Get two cutting points
						cuttingPoint1 = PseudoRandom.randInt(0, permutationLength - 1);
						cuttingPoint2 = PseudoRandom.randInt(0, permutationLength - 1);
						while (cuttingPoint2 == cuttingPoint1)
							cuttingPoint2 = PseudoRandom.randInt(0, permutationLength - 1);
						
						if (cuttingPoint1 > cuttingPoint2)
						{
							int swap;
							swap = cuttingPoint1;
							cuttingPoint1 = cuttingPoint2;
							cuttingPoint2 = swap;
						} // if
						//      STEP 2: Get the subchains to interchange
						int[] replacement1 = new int[permutationLength];
						int[] replacement2 = new int[permutationLength];
						for (int i = 0; i < permutationLength; i++)
							replacement1[i] = replacement2[i] = - 1;
						
						//      STEP 3: Interchange   	
						for (int i = cuttingPoint1; i <= cuttingPoint2; i++)
						{
							offspring1Vector[i] = parent2Vector[i];
							offspring2Vector[i] = parent1Vector[i];
							
							replacement1[parent2Vector[i]] = parent1Vector[i];
							replacement2[parent1Vector[i]] = parent2Vector[i];
						} // for
						
						//      STEP 4: Repair offsprings
						for (int i = 0; i < permutationLength; i++)
						{
							if ((i >= cuttingPoint1) && (i <= cuttingPoint2))
								continue;
							
							int n1 = parent1Vector[i];
							int m1 = replacement1[n1];
							
							int n2 = parent2Vector[i];
							int m2 = replacement2[n2];
							
							while (m1 != - 1)
							{
								n1 = m1;
								m1 = replacement1[m1];
							} // while
							while (m2 != - 1)
							{
								n2 = m2;
								m2 = replacement2[m2];
							} // while
							offspring1Vector[i] = n1;
							offspring2Vector[i] = n2;
						} // for
					}
					// if
					else
					{
						Configuration.m_logger.WriteLog("PMXCrossover.doCrossover: invalid type+" + "" + parent1.DecisionVariables[0].VariableType);
                        //System.Type cls = typeof(System.String);
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                        string name = this.GetType().FullName;
						throw new SMException("Exception in " + name + ".doCrossover()");
					} // else
				}
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			}
			return offspring;
		}
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing an array of two solutions 
		/// </param>
		/// <throws>  SMException  </throws>
		public override System.Object execute(System.Object obj)
		{
			Solution[] parents = (Solution[]) obj;
			System.Double crossoverProbability;
	
            crossoverProbability = (System.Double)m_parameters["probability"];
			
			if (parents.Length < 2)
			{
				Configuration.m_logger.WriteLog("PMXCrossover.execute: operator needs two " + "parents");
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			}
			else
			{
				//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
                if (m_parameters["probability"] == null)
				{
					Configuration.m_logger.WriteLog("PMXCrossover.execute: probability not " + "specified");
                    //System.Type cls = typeof(System.String);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    string name = this.GetType().FullName;
					throw new SMException("Exception in " + name + ".execute()");
				}
			}
			
			Solution[] offspring = doCrossover(crossoverProbability, parents[0], parents[1]);
			
			return offspring;
		} // execute
	} // PMXCrossover
}
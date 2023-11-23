/// <summary> TwoPointsCrossover.java
/// Class representing a two points crossover operator
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
	
	/// <summary> This class allows to apply a two points crossover operator using two parent
	/// solutions.
	/// NOTE: the operator is applied to the first variable of the solutions, and 
	/// the type of the solutions must be <code>m_solutionType.Permutation</code>.
	/// </summary>
	[Serializable]
	public class TwoPointsCrossover:Crossover
	{
		
		/// <summary> PERMUTATION_SOLUTION represents class JARE.base.solutionType.PermutationSolutionType</summary>
		private static System.Type PERMUTATION_SOLUTION;
		
		/// <summary> Constructor
		/// Creates a new intance of the two point crossover operator
		/// </summary>
		public TwoPointsCrossover()
		{
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                PERMUTATION_SOLUTION = System.Type.GetType("JARE.Base.solutionType.PermutationSolutionType");
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // catch
		} // TwoPointsCrossover
		
		
		/// <summary> Constructor</summary>
		/// <param name="A">properties containing the Operator parameters
		/// Creates a new intance of the two point crossover operator
		/// </param>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public TwoPointsCrossover(System.Collections.IDictionary properties):this()
		{
		}
		
		
		/// <summary> Perform the crossover operation</summary>
		/// <param name="probability">Crossover probability
		/// </param>
		/// <param name="parent1">The first parent
		/// </param>
		/// <param name="parent2">The second parent
		/// </param>
		/// <returns> Two offspring solutions
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
					if (PseudoRandom.randDouble() < probability)
					{
						int crosspoint1;
						int crosspoint2;
						int permutationLength;
						int[] parent1Vector;
						int[] parent2Vector;
						int[] offspring1Vector;
						int[] offspring2Vector;
						
						permutationLength = ((Permutation) parent1.DecisionVariables[0]).Length;
						parent1Vector = ((Permutation) parent1.DecisionVariables[0]).m_vector;
						parent2Vector = ((Permutation) parent2.DecisionVariables[0]).m_vector;
						offspring1Vector = ((Permutation) offspring[0].DecisionVariables[0]).m_vector;
						offspring2Vector = ((Permutation) offspring[1].DecisionVariables[0]).m_vector;
						
						// STEP 1: Get two cutting points
						crosspoint1 = PseudoRandom.randInt(0, permutationLength - 1);
						crosspoint2 = PseudoRandom.randInt(0, permutationLength - 1);
						
						while (crosspoint2 == crosspoint1)
							crosspoint2 = PseudoRandom.randInt(0, permutationLength - 1);
						
						if (crosspoint1 > crosspoint2)
						{
							int swap;
							swap = crosspoint1;
							crosspoint1 = crosspoint2;
							crosspoint2 = swap;
						} // if
						
						// STEP 2: Obtain the first child
						int m = 0;
						for (int j = 0; j < permutationLength; j++)
						{
							bool exist = false;
							int temp = parent2Vector[j];
							for (int k = crosspoint1; k <= crosspoint2; k++)
							{
								if (temp == offspring1Vector[k])
								{
									exist = true;
									break;
								} // if
							} // for
							if (!exist)
							{
								if (m == crosspoint1)
									m = crosspoint2 + 1;
								offspring1Vector[m++] = temp;
							} // if
						} // for
						
						// STEP 3: Obtain the second child
						m = 0;
						for (int j = 0; j < permutationLength; j++)
						{
							bool exist = false;
							int temp = parent1Vector[j];
							for (int k = crosspoint1; k <= crosspoint2; k++)
							{
								if (temp == offspring2Vector[k])
								{
									exist = true;
									break;
								} // if
							} // for
							if (!exist)
							{
								if (m == crosspoint1)
									m = crosspoint2 + 1;
								offspring2Vector[m++] = temp;
							} // if
						} // for
					} // if 
				}
				// if
				else
				{
					Configuration.m_logger.WriteLog("TwoPointsCrossover.doCrossover: invalid " + "type" + parent1.DecisionVariables[0].VariableType);
                    //System.Type cls = typeof(System.String);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    string name = this.GetType().FullName; 
					throw new SMException("Exception in " + name + ".doCrossover()");
				}
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"

                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // else
			
			return offspring;
		} // makeCrossover
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing an array of two solutions 
		/// </param>
		/// <returns> An object containing an array with the offSprings
		/// </returns>
		/// <throws>  SMException  </throws>
		public override System.Object execute(System.Object obj)
		{
			Solution[] parents = (Solution[]) obj;
			System.Double crossoverProbability;
			
			if ((parents[0].Type.GetType() != PERMUTATION_SOLUTION) || (parents[1].Type.GetType() != PERMUTATION_SOLUTION))
			{
				
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				Configuration.m_logger.WriteLog("TwoPointsCrossover.execute: the solutions " + "are not of the right type. The type should be 'Permutation', but " + parents[0].Type + " and " + parents[1].Type + " are obtained");
			} // if 
			
			
			
			if (parents.Length < 2)
			{
				Configuration.m_logger.WriteLog("SBXCrossover.execute: operator needs two " + "parents");
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			}
			else
			{
				//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
                if (getParameter("probability") == null)
				{
					Configuration.m_logger.WriteLog("SBXCrossover.execute: probability not " + "specified");
                    //System.Type cls = typeof(System.String);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    string name = this.GetType().FullName;
					throw new SMException("Exception in " + name + ".execute()");
				}
			}
            crossoverProbability = (System.Double)getParameter("probability");
			Solution[] offspring = doCrossover(crossoverProbability, parents[0], parents[1]);
			
			return offspring;
		} // execute
	} // TwoPointsCrossover
}
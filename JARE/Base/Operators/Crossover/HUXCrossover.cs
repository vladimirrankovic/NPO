/// <summary> HUXCrossover.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// Class representing a HUX crossover operator
/// </version>
using System;
using JARE.Base.variable;
using JARE.Base;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.crossover
{
	
	/// <summary> This class allows to apply a HUX crossover operator using two parent
	/// solutions.
	/// NOTE: the operator is applied to the first variable of the solutions, and 
	/// the type of the solutions must be binary 
	/// (e.g., <code>m_solutionType.Binary</code> or 
	/// <code>m_solutionType.BinaryReal</code>.
	/// </summary>
	[Serializable]
	public class HUXCrossover:Crossover
	{
		
		/// <summary> BINARY_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
		private static System.Type BINARY_SOLUTION;
		/// <summary> BINARY_REAL_SOLUTION represents class JARE.base.solutionType.BinaryRealSolutionType</summary>
		private static System.Type BINARY_REAL_SOLUTION;
		
		/// <summary> Constructor
		/// Create a new instance of the HUX crossover operator.
		/// </summary>
		public HUXCrossover()
		{
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				BINARY_SOLUTION = System.Type.GetType("JARE.Base.solutionType.BinarySolutionType");
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				BINARY_REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.BinaryRealSolutionType");
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // catch
		} // HUXCrossover
		
		
		/// <summary> Constructor
		/// Create a new intance of the HUX crossover operator.
		/// </summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		
        public HUXCrossover(System.Collections.IDictionary properties):this()
		{
		} // HUXCrossover
	
		/// <summary> Perform the crossover operation</summary>
		/// <param name="probability">Crossover probability
		/// </param>
		/// <param name="parent1">The first parent
		/// </param>
		/// <param name="parent2">The second parent
		/// </param>
		/// <returns> An array containing the two offsprings
		/// </returns>
		/// <throws>  SMException  </throws>
		public virtual Solution[] doCrossover(double probability, Solution parent1, Solution parent2)
		{
			Solution[] offSpring = new Solution[2];
			offSpring[0] = new Solution(parent1);
			offSpring[1] = new Solution(parent2);
			try
			{
				if (PseudoRandom.randDouble() < probability)
				{
					for (int var = 0; var < parent1.DecisionVariables.Length; var++)
					{
						Binary p1 = (Binary) parent1.DecisionVariables[var];
						Binary p2 = (Binary) parent2.DecisionVariables[var];
						
						for (int bit = 0; bit < p1.NumberOfBits; bit++)
						{
							if (p1.m_bits.Get(bit) != p2.m_bits.Get(bit)) 
							{
								if (PseudoRandom.randDouble() < 0.5)
								{
									((Binary) offSpring[0].DecisionVariables[var]).m_bits.Set(bit, p2.m_bits.Get(bit)); 
									((Binary) offSpring[1].DecisionVariables[var]).m_bits.Set(bit, p1.m_bits.Get(bit));
								}
							}
						}
					}
					//7. Decode the results
					for (int i = 0; i < offSpring[0].DecisionVariables.Length; i++)
					{
						((Binary) offSpring[0].DecisionVariables[i]).decode();
						((Binary) offSpring[1].DecisionVariables[i]).decode();
					}
				}
			}
			catch (System.InvalidCastException)
			{
				
				Configuration.m_logger.WriteLog("HUXCrossover.doCrossover: Cannot perfom " + "SinglePointCrossover ");
				//System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".doCrossover()");
			}
			return offSpring;
		} // doCrossover
		
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing an array of two solutions 
		/// </param>
		/// <returns> An object containing the offSprings
		/// </returns>
		public override System.Object execute(System.Object obj)
		{
			Solution[] parents = (Solution[]) obj;
			
			if (((parents[0].Type.GetType() != BINARY_SOLUTION) || (parents[1].Type.GetType() != BINARY_SOLUTION)) && ((parents[0].Type.GetType() != BINARY_REAL_SOLUTION) || (parents[1].Type.GetType() != BINARY_REAL_SOLUTION)))
			{
				
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				Configuration.m_logger.WriteLog("HUXCrossover.execute: the solutions " + "are not of the right type. The type should be 'Binary' of " + "'BinaryReal', but " + parents[0].Type + " and " + parents[1].Type + " are obtained");
				
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			} // if 
			
			
			if (parents.Length < 2)
			{
				Configuration.m_logger.WriteLog("HUXCrossover.execute: operator needs two " + "parents");
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
					Configuration.m_logger.WriteLog("HUXCrossover.execute: probability not " + "specified");
                    //System.Type cls = typeof(System.String);
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    string name = this.GetType().FullName; 
                    throw new SMException("Exception in " + name + ".execute()");
				}
			}
            System.Double probability = (System.Double)getParameter("probability");
			Solution[] offSpring = doCrossover(probability, parents[0], parents[1]);
			
			for (int i = 0; i < offSpring.Length; i++)
			{
				offSpring[i].CrowdingDistance = 0.0;
				offSpring[i].Rank = 0;
			}
			return offSpring;
		} // execute
	} // HUXCrossover
}
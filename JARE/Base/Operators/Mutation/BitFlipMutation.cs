/// <summary> BitFlipMutation.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.1
/// </version>
using System;
using Solution = JARE.Base.Solution;
using JARE.Base.variable;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.mutation
{
	
	/// <summary> This class implements a bit flip mutation operator.
	/// NOTE: the operator is applied to binary or integer solutions, considering the
	/// whole solution as a single variable.
	/// </summary>
	[Serializable]
	public class BitFlipMutation: Mutation
	{
		/// <summary> BINARY_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
		private static System.Type BINARY_SOLUTION;
		/// <summary> BINARY_REAL_SOLUTION represents class JARE.base.solutionType.BinaryRealSolutionType</summary>
		private static System.Type BINARY_REAL_SOLUTION;
		/// <summary> INT_SOLUTION represents class JARE.base.solutionType.IntSolutionType</summary>
		private static System.Type INT_SOLUTION;
		
		/// <summary> Constructor
		/// Creates a new instance of the Bit Flip mutation operator
		/// </summary>
		public BitFlipMutation()
		{
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                BINARY_SOLUTION = System.Type.GetType("JARE.Base.solutionType.BinarySolutionType");
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                BINARY_REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.BinaryRealSolutionType");
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                INT_SOLUTION = System.Type.GetType("JARE.Base.solutionType.IntSolutionType");
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
				//e.printStackTrace();
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // catch
		} // BitFlipMutation
		
		
		/// <summary> Constructor
		/// Creates a new instance of the Bit Flip mutation operator
		/// </summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public BitFlipMutation(System.Collections.IDictionary properties):this()
		{
		} // BitFlipMutation
		
		/// <summary> Perform the mutation operation</summary>
		/// <param name="probability">Mutation probability
		/// </param>
		/// <param name="solution">The solution to mutate
		/// </param>
		/// <throws>  SMException </throws>
		public virtual void  doMutation(double probability, Solution solution)
		{
			try
			{
				if ((solution.Type.GetType() == BINARY_SOLUTION) || (solution.Type.GetType() == BINARY_REAL_SOLUTION))
				{
					for (int i = 0; i < solution.DecisionVariables.Length; i++)
					{
						for (int j = 0; j < ((Binary) solution.DecisionVariables[i]).NumberOfBits; j++)
						{
							if (PseudoRandom.randDouble() < probability)
							{
                                ((Binary)solution.DecisionVariables[i]).m_bits[j] = !((Binary)solution.DecisionVariables[i]).m_bits[j];
							}
						}
					}
					
					for (int i = 0; i < solution.DecisionVariables.Length; i++)
					{
						((Binary) solution.DecisionVariables[i]).decode();
					}
				}
				// if
				else
				{
					// Integer representation
					for (int i = 0; i < solution.DecisionVariables.Length; i++)
						if (PseudoRandom.randDouble() < probability)
						{
							//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
							int val = (int) (PseudoRandom.randInt((int) solution.DecisionVariables[i].getUpperBound(), (int) solution.DecisionVariables[i].getLowerBound()));
							solution.DecisionVariables[i].setValue(val);
						} // if
				} // else
			}
			catch (System.InvalidCastException e1)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				Configuration.m_logger.WriteLog("BitFlipMutation.doMutation: " + "ClassCastException error" + e1.Message);
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName; 
                throw new SMException("Exception in " + name + ".doMutation()");
			}
		} // doMutation
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing a solution to mutate
		/// </param>
		/// <returns> An object containing the mutated solution
		/// </returns>
		/// <throws>  SMException  </throws>
		public override System.Object execute(System.Object obj)
		{
			Solution solution = (Solution) obj;
			
			if ((solution.Type.GetType() != BINARY_SOLUTION) && (solution.Type.GetType() != BINARY_REAL_SOLUTION) && (solution.Type.GetType() != INT_SOLUTION))
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				Configuration.m_logger.WriteLog("BitFlipMutation.execute: the solution " + "is not of the right type. The type should be 'Binary', " + "'BinaryReal' or 'Int', but " + solution.Type + " is obtained");
				
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			} // if 
			
			
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("probability") == null)
			{
				Configuration.m_logger.WriteLog("BitFlipMutation.execute: probability not " + "specified");
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			}
			System.Double probability = (System.Double) getParameter("probability");
			doMutation(probability, solution);
			return solution;
		} // execute
	} // BitFlipMutation
}
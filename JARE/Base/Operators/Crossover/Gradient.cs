/// <summary> Gradiente.java
/// Class representing an initial approximation to the gradient
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
namespace JARE.Base.operators.crossover
{
	
	
	[Serializable]
	public class Gradient : Crossover
	{
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
		private static System.Type REAL_SOLUTION;
		
		/// <summary> Constructor
		/// Create a new Gradient operator    
		/// </summary>
		public Gradient()
		{
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealSolutionType");
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
                try
                {
                    System.IO.StreamWriter bw = new System.IO.StreamWriter("Exception.txt");
                    //UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                    JARE.support.SupportClass.WriteStackTrace(e, bw);
                    bw.Close();
                }
                catch (System.IO.IOException e1)
                {
                    Console.WriteLine("Error acceding to the file");
                    Console.WriteLine(e1.Message);
                }
            }
		} // Gradient
		
		
		/// <summary> Constructor
		/// Create a new Gradient operator    
		/// </summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public Gradient(System.Collections.IDictionary properties):this()
		{
		} // Gradiente
		
		/// <summary> Perform the crossover operation. </summary>
		/// <param name="probability">Crossover probability
		/// </param>
		/// <param name="parent1">The first parent
		/// </param>
		/// <param name="parent2">The second parent
		/// </param>
		/// <returns> An array containing the two offsprings
		/// </returns>
		public virtual Solution[] doCrossover(Solution parent1, Solution parent2)
		{
			
			double[][] gradiente = new double[parent1.NumberOfObjectives()][];
			for (int i = 0; i < parent1.NumberOfObjectives(); i++)
			{
				gradiente[i] = new double[parent1.numberOfVariables()];
			}
			
			for (int i = 0; i < parent1.NumberOfObjectives(); i++)
			{
				double f1, f2, deltaf;
				f1 = parent1.getObjective(i);
				f2 = parent2.getObjective(i);
				deltaf = f1 - f2;
				for (int j = 0; j < parent1.numberOfVariables(); j++)
				{
					double x1, x2, deltax;
					x1 = parent1.DecisionVariables[j].getValue();
					x2 = parent2.DecisionVariables[j].getValue();
					
					deltax = x1 - x2;
					
					if (deltax == 0)
					{
						//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MIN_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						deltax = System.Double.MinValue;
					}
					
					
					gradiente[i][j] = deltaf / deltax;
				}
			}
			
			// We use the gradient to update the position of the solutions
			double[] direccion = new double[parent1.numberOfVariables()];
			for (int i = 0; i < direccion.Length; i++)
				direccion[i] = 0.0;
			
			for (int i = 0; i < parent1.numberOfVariables(); i++)
			{
				for (int j = 0; j < parent1.NumberOfObjectives(); j++)
				{
					direccion[i] = gradiente[j][i];
				}
			}
			
			Solution[] offSpring = new Solution[2];
			offSpring[0] = new Solution(parent1);
			offSpring[1] = new Solution(parent2);
			
			for (int j = 0; j < offSpring.Length; j++)
			{
				for (int i = 0; i < parent1.numberOfVariables(); i++)
				{
					double newValue = offSpring[j].DecisionVariables[i].getValue() - direccion[i];
					if (newValue > parent1.DecisionVariables[i].getUpperBound())
						newValue = parent1.DecisionVariables[i].getUpperBound();
					if (newValue < parent1.DecisionVariables[i].getLowerBound())
						newValue = parent1.DecisionVariables[i].getLowerBound();
					
					offSpring[j].DecisionVariables[i].setValue(newValue);
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
			
			if ((parents[0].Type.GetType() != REAL_SOLUTION) || (parents[1].Type.GetType() != REAL_SOLUTION))
			{
				
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				Configuration.m_logger.WriteLog("SBXCrossover.execute: the solutions " + "are not of the right type. The type should be 'Real', but " + parents[0].Type + " and " + parents[1].Type + " are obtained");
				
				//System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			} // if 
			
			System.Double probability = (System.Double) getParameter("probability");
			if (parents.Length < 2)
			{
				Configuration.m_logger.WriteLog("SBXCrossover.execute: operator needs two " + "parents");
				//System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			}
			
			
			Solution[] offSpring;
			offSpring = doCrossover(parents[0], parents[1]);
			
			
			for (int i = 0; i < offSpring.Length; i++)
			{
				offSpring[i].CrowdingDistance = 0.0;
				offSpring[i].Rank = 0;
			}
			return offSpring; //*/
		} // execute 
	} // SBXCrossover
}
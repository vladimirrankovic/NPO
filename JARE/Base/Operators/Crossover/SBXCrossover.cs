/// <summary> SBXCrossover.java
/// Class representing a simulated binary (SBX) crossover operator
/// </summary>
/// <author>  Juan J. Durillo
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
	
	/// <summary> This class allows to apply a SBX crossover operator using two parent
	/// solutions.
	/// NOTE: the operator is applied to Real solutions, so the type of the solutions
	/// must be </code>m_solutionType.Real</code>.
	/// NOTE: if you use the default constructor, the value of the etc_c parameter is
	/// DEFAULT_INDEX_CROSSOVER. You can change it using the parameter 
	/// "distributionIndex" before invoking the execute() method -- see lines 196-199
	/// </summary>
	[Serializable]
	public class SBXCrossover:Crossover
	{
		
		/// <summary> EPS defines the minimum difference allowed between real values</summary>
		protected internal const double EPS = 1.0e-14;
		
		/// <summary> eta_c stores the index for crossover to use</summary>
		public double eta_c = 20.0;
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
		private static System.Type REAL_SOLUTION;
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.ArrayRealSolutionType</summary>
		private static System.Type ARRAY_REAL_SOLUTION;
		
		/// <summary> Constructor
		/// Create a new SBX crossover operator whit a default
		/// index given by <code>DEFAULT_INDEX_CROSSOVER</code>
		/// </summary>
		public SBXCrossover()
		{
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealSolutionType");
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                ARRAY_REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.ArrayRealSolutionType");
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // catch
		} // SBXCrossover
		
		
		/// <summary> Constructor
		/// Create a new SBX crossover operator whit a default
		/// index given by <code>DEFAULT_INDEX_CROSSOVER</code>
		/// </summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public SBXCrossover(System.Collections.IDictionary properties):this()
		{
			//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			eta_c = (System.Double.Parse((System.String) (properties[(System.String) "eta_c"])));
		} // SBXCrossover
		
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
			
			int i;
			double rand;
			double y1, y2, yL, yu;
			double c1, c2;
			double alpha, beta, betaq;
			double valueX1, valueX2;
			XReal x1 = new XReal(parent1);
			XReal x2 = new XReal(parent2);
			XReal offs1 = new XReal(offSpring[0]);
			XReal offs2 = new XReal(offSpring[1]);
			
			int numberOfVariables = x1.NumberOfDecisionVariables;
			
			if (PseudoRandom.randDouble() <= probability)
			{
				for (i = 0; i < numberOfVariables; i++)
				{
					valueX1 = x1.getValue(i);
					valueX2 = x2.getValue(i);
					if (PseudoRandom.randDouble() <= 0.5)
					{
						if (System.Math.Abs(valueX1 - valueX2) > EPS)
						{
							
							if (valueX1 < valueX2)
							{
								y1 = valueX1;
								y2 = valueX2;
							}
							else
							{
								y1 = valueX2;
								y2 = valueX1;
							} // if                       
							
							yL = x1.getLowerBound(i);
							yu = x1.getUpperBound(i);
							rand = PseudoRandom.randDouble();
							beta = 1.0 + (2.0 * (y1 - yL) / (y2 - y1));
							alpha = 2.0 - System.Math.Pow(beta, - (eta_c + 1.0));
							
							if (rand <= (1.0 / alpha))
							{
								betaq = System.Math.Pow((rand * alpha), (1.0 / (eta_c + 1.0)));
							}
							else
							{
								betaq = System.Math.Pow((1.0 / (2.0 - rand * alpha)), (1.0 / (eta_c + 1.0)));
							} // if
							
							c1 = 0.5 * ((y1 + y2) - betaq * (y2 - y1));
							beta = 1.0 + (2.0 * (yu - y2) / (y2 - y1));
							alpha = 2.0 - System.Math.Pow(beta, - (eta_c + 1.0));
							
							if (rand <= (1.0 / alpha))
							{
								betaq = System.Math.Pow((rand * alpha), (1.0 / (eta_c + 1.0)));
							}
							else
							{
								betaq = System.Math.Pow((1.0 / (2.0 - rand * alpha)), (1.0 / (eta_c + 1.0)));
							} // if
							
							c2 = 0.5 * ((y1 + y2) + betaq * (y2 - y1));
							
							if (c1 < yL)
								c1 = yL;
							
							if (c2 < yL)
								c2 = yL;
							
							if (c1 > yu)
								c1 = yu;
							
							if (c2 > yu)
								c2 = yu;
							
							if (PseudoRandom.randDouble() <= 0.5)
							{
								offs1.setValue(i, c2);
								offs2.setValue(i, c1);
							}
							else
							{
								offs1.setValue(i, c1);
								offs2.setValue(i, c2);
							} // if
						}
						else
						{
							offs1.setValue(i, valueX1);
							offs2.setValue(i, valueX2);
						} // if
					}
					else
					{
						offs1.setValue(i, valueX2);
						offs2.setValue(i, valueX1);
					} // if
				} // if
			} // if
			
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
			
			if (((parents[0].Type.GetType() != REAL_SOLUTION) && (parents[1].Type.GetType() != REAL_SOLUTION)) && ((parents[0].Type.GetType() != ARRAY_REAL_SOLUTION) && (parents[1].Type.GetType() != ARRAY_REAL_SOLUTION)))
			{
				
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				Configuration.m_logger.WriteLog("SBXCrossover.execute: the solutions " + "type " + parents[0].Type + " is not allowed with this operator");
				
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
			
			System.Double distributionIndex = (System.Double) getParameter("distributionIndex");
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("distributionIndex") != null)
			{
				eta_c = distributionIndex;
			} // if
			
			Solution[] offSpring;
			offSpring = doCrossover(probability, parents[0], parents[1]);
			
			
			for (int i = 0; i < offSpring.Length; i++)
			{
				offSpring[i].CrowdingDistance = 0.0;
				offSpring[i].Rank = 0;
			}
			return offSpring; //*/
		} // execute 
	} // SBXCrossover
}
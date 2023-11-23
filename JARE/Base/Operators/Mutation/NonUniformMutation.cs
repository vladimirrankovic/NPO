/// <summary> NonUniformMutation.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
using Solution = JARE.Base.Solution;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
using XReal = JARE.util.wrapper.XReal;
namespace JARE.Base.operators.mutation
{
	
	/// <summary> This class implements a non-uniform mutation operator.
	/// NOTE: the type of the solutions must be <code>m_solutionType.Real</code>
	/// </summary>
	[Serializable]
	public class NonUniformMutation:Mutation
	{
		/// <summary> m_perturbation stores the perturbation value used in the Non Uniform 
		/// mutation operator
		/// </summary>
		//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
		private System.Double m_perturbation;
		
		/// <summary> m_maxIterations stores the maximun number of iterations. </summary>
		//UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
		private System.Int32 m_maxIterations ;
		
		/// <summary> actualIteration_ stores the iteration in which the operator is going to be
		/// applied
		/// </summary>
		//UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
		private System.Int32 m_actualIteration ;
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
		private static System.Type REAL_SOLUTION;
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.ArrayRealSolutionType</summary>
		private static System.Type ARRAY_REAL_SOLUTION;
		
		/// <summary> Constructor
		/// Creates a new instance of the non uniform mutation
		/// </summary>
		public NonUniformMutation()
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
		} // NonUniformMutation
		
		
		/// <summary> Constructor
		/// Creates a new instance of the non uniform mutation
		/// </summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public NonUniformMutation(System.Collections.IDictionary properties):this()
		{
		} // NonUniformMutation
		
		
		/// <summary> Perform the mutation operation</summary>
		/// <param name="probability">Mutation probability
		/// </param>
		/// <param name="solution">The solution to mutate
		/// </param>
		/// <throws>  SMException  </throws>
		public virtual void  doMutation(double probability, Solution solution)
		{
			
			XReal x = new XReal(solution);
			for (int var = 0; var < solution.DecisionVariables.Length; var++)
			{
				if (PseudoRandom.randDouble() < probability)
				{
					double rand = PseudoRandom.randDouble();
					double tmp;
					
					if (rand <= 0.5)
					{
						tmp = delta(x.getUpperBound(var) - x.getValue(var), m_perturbation);
						tmp += x.getValue(var);
					}
					else
					{
						tmp = delta(x.getLowerBound(var) - x.getValue(var), m_perturbation);
						tmp += x.getValue(var);
					}
					
					if (tmp < x.getLowerBound(var))
						tmp = x.getLowerBound(var);
					else if (tmp > x.getUpperBound(var))
						tmp = x.getUpperBound(var);
					
					x.setValue(var, tmp);
				}
			}
		} // doMutation
		
		
		/// <summary> Calculates the delta value used in NonUniform mutation operator</summary>
		private double delta(double y, double bMutationParameter)
		{
			double rand = PseudoRandom.randDouble();
			int it, maxIt;
			it = m_actualIteration;
			maxIt = m_maxIterations;
			
			return (y * (1.0 - System.Math.Pow(rand, System.Math.Pow((1.0 - it / (double) maxIt), bMutationParameter))));
		} // delta
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing a solution
		/// </param>
		/// <returns> An object containing the mutated solution
		/// </returns>
		/// <throws>  SMException  </throws>
		public override System.Object execute(System.Object obj)
		{
			Solution solution = (Solution) obj;
			
			if ((solution.Type.GetType() != REAL_SOLUTION) && (solution.Type.GetType() != ARRAY_REAL_SOLUTION))
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				Configuration.m_logger.WriteLog("NonUniformMutation.execute: the solution " + solution.Type + "is not of the right type");
				
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			} // if 
			
			System.Double probability;
			
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("perturbationIndex") == null)
				m_perturbation = (System.Double) getParameter("perturbationIndex");
			
			//UPGRADE_TODO: The 'System.Int32' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("perturbationIndex") == null)
				m_maxIterations = (System.Int32) getParameter("maxIterations");
			
			m_actualIteration = (System.Int32) getParameter("currentIteration");
			
			
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("probability") == null)
			{
				Configuration.m_logger.WriteLog("NonUniformMutation.execute: probability " + "not specified");
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			}
			probability = (System.Double) getParameter("probability");
			doMutation(probability, solution);
			
			return solution;
		} // execute
	} // NonUniformMutation
}
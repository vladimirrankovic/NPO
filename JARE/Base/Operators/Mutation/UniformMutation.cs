/// <summary> UniformMutation.java
/// Class representing a uniform mutation operator
/// </summary>
/// <author>  Antonio J.Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using Solution = JARE.Base.Solution;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
using XReal = JARE.util.wrapper.XReal;
namespace JARE.Base.operators.mutation
{
	
	/// <summary> This class implements a uniform mutation operator.
	/// NOTE: the type of the solutions must be <code>m_solutionType.Real</code>
	/// </summary>
	[Serializable]
	public class UniformMutation:Mutation
	{
		/// <summary> Stores the value used in a uniform mutation operator</summary>
		private System.Double m_perturbation;

        /// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type REAL_SOLUTION;
        
        /// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
        private static System.Type REAL_WEIGHTS_SOLUTION;
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.ArrayRealSolutionType</summary>
		private static System.Type ARRAY_REAL_SOLUTION;
		
		
		/// <summary> Constructor
		/// Creates a new uniform mutation operator instance
		/// </summary>
		public UniformMutation()
		{
			try
			{
                REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealSolutionType");
                REAL_WEIGHTS_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealWeightsSolutionType");
                ARRAY_REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.ArrayRealSolutionType");
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				//UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
			} // catch
		} // UniformMutation
		
		
		/// <summary> Constructor
		/// Creates a new uniform mutation operator instance
		/// </summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public UniformMutation(System.Collections.IDictionary properties):this()
		{
		} // UniformMutation
		
		
		/// <summary> Performs the operation</summary>
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
					double tmp = (rand - 0.5) * m_perturbation;
					
					tmp += x.getValue(var);
					
					if (tmp < x.getLowerBound(var))
						tmp = x.getLowerBound(var);
					else if (tmp > x.getUpperBound(var))
						tmp = x.getUpperBound(var);
					
					x.setValue(var, tmp);
				} // if
			} // for
		} // doMutation
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing the solution to mutate
		/// </param>
		/// <throws>  SMException  </throws>
		public override System.Object execute(System.Object obj)
		{
			Solution solution = (Solution) obj;

            if ((solution.Type.GetType() != REAL_SOLUTION)
                && (solution.Type.GetType() != REAL_WEIGHTS_SOLUTION)
                && (solution.Type.GetType() != ARRAY_REAL_SOLUTION))
			{
				Configuration.m_logger.WriteLog("UniformMutation.execute: the solution " + "is not of the right type. The type should be 'Real', but " + solution.Type + " is obtained");
				
                string name = this.GetType().FullName; 
                throw new SMException("Exception in " + name + ".execute()");
			} // if 
			
			System.Double probability;
		
			//if (m_perturbation.Equals (null))
				m_perturbation = (System.Double) getParameter("perturbationIndex");
			
			probability = (System.Double) getParameter("probability");
            if (probability.Equals(null))
			{
				Configuration.m_logger.WriteLog("UniformMutation.execute: probability " + "not specified");
                string name = this.GetType().FullName; 
                throw new SMException("Exception in " + name + ".execute()");
			}
			
			doMutation(probability, solution);
			
			return solution;
		} // execute                  
	} // UniformMutation
}
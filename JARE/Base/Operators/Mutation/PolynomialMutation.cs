/// <summary> PolynomialMutation.java</summary>
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
using ArrayReal = JARE.Base.variable.ArrayReal;
namespace JARE.Base.operators.mutation
{
	
	/// <summary> This class implements a polynomial mutation operator. 
	/// NOTE: the operator is applied to Real solutions, so the type of the solutions
	/// must be </code>m_solutionType.Real</code>.
	/// NOTE: if you use the default constructor, the value of the etc_m parameter is
	/// ETA_M_DEFAULT_. You can change it using the parameter
	/// "distributionIndex" before invoking the execute() method -- see lines 116-119
	/// </summary>
	[Serializable]
	public class PolynomialMutation:Mutation
	{
		/// <summary> ETA_M_DEFAULT_ defines a default index for mutation</summary>
		public const double ETA_M_DEFAULT = 20.0;
		
		/// <summary> eta_c stores the index for mutation to use</summary>
		public double eta_m = ETA_M_DEFAULT;
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
		private static System.Type REAL_SOLUTION;
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.ArrayRealSolutionType</summary>
		private static System.Type ARRAY_REAL_SOLUTION;
		
		/// <summary> Constructor
		/// Creates a new instance of the polynomial mutation operator
		/// </summary>
		public PolynomialMutation()
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
		} // PolynomialMutation
		
		/// <summary> Constructor.
		/// Create a new PolynomialMutation operator with an specific index
		/// </summary>
		public PolynomialMutation(double eta):this()
		{
			eta_m = eta;
		} // PolynomialMutation

        // Visnja: Stavljeno pod komentar jer se nigde ne koristi, a nije rastumaceno kako funkcionise
		
		/// <summary> Constructor.
		/// Create a new PolynomialMutation operator with an specific index
		/// </summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //public PolynomialMutation(System.Collections.IDictionary properties):this()
        //{
        //    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //    eta_m = (System.Double.Parse((System.String) properties.Get("eta_m_")));
        //} // PolynomialMutation
		
		
		/// <summary> Perform the mutation operation</summary>
		/// <param name="probability">Mutation probability
		/// </param>
		/// <param name="solution">The solution to mutate
		/// </param>
		/// <throws>  SMException  </throws>
		public virtual void  doMutation(double probability, Solution solution)
		{
			double rnd, delta1, delta2, mut_pow, deltaq;
			double y, yl, yu, val, xy;
			XReal x = new XReal(solution);
			for (int var = 0; var < solution.numberOfVariables(); var++)
			{
				if (PseudoRandom.randDouble() <= probability)
				{
					y = x.getValue(var);
					yl = x.getLowerBound(var);
					yu = x.getUpperBound(var);
					delta1 = (y - yl) / (yu - yl);
					delta2 = (yu - y) / (yu - yl);
					rnd = PseudoRandom.randDouble();
					mut_pow = 1.0 / (eta_m + 1.0);
					if (rnd <= 0.5)
					{
						xy = 1.0 - delta1;
						val = 2.0 * rnd + (1.0 - 2.0 * rnd) * (System.Math.Pow(xy, (eta_m + 1.0)));
						deltaq = System.Math.Pow(val, mut_pow) - 1.0;
					}
					else
					{
						xy = 1.0 - delta2;
						val = 2.0 * (1.0 - rnd) + 2.0 * (rnd - 0.5) * (System.Math.Pow(xy, (eta_m + 1.0)));
						deltaq = 1.0 - (System.Math.Pow(val, mut_pow));
					}
					y = y + deltaq * (yu - yl);
					if (y < yl)
						y = yl;
					if (y > yu)
						y = yu;
					x.setValue(var, y);
				}
			} // for
		} // doMutation
		
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
				Configuration.m_logger.WriteLog("PolynomialMutation.execute: the solution " + "type " + solution.Type + " is not allowed with this operator");
				
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			} // if 
			
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("probability") == null)
			{
				Configuration.m_logger.WriteLog("PolynomialMutation.execute: probability " + "not specified");
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
                throw new SMException("Exception in " + name + ".execute()");
			}
			
			System.Double distributionIndex = (System.Double) getParameter("distributionIndex");
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
			if (getParameter("distributionIndex") != null)
			{
				eta_m = distributionIndex;
			} // if
            System.Double probability = (System.Double)getParameter("probability");
			doMutation(probability, solution);
			return solution;
		} // execute
	} // PolynomialMutation
}
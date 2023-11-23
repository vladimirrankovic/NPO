/// <summary> DifferentialEvolutionCrossover.java
/// Class representing the crossover operator used in differential evolution
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using Solution = JARE.Base.Solution;
using SolutionType = JARE.Base.SolutionType;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
using XReal = JARE.util.wrapper.XReal;
namespace JARE.Base.operators.crossover
{
	
	/// <summary> Differential evolution crossover operators
	/// Comments:
	/// - The operator receives two parameters: the current individual and an array
	/// of three parent individuals
	/// - The best and rand variants depends on the third parent, according whether
	/// it represents the current of the "best" individual or a randon one. 
	/// The implementation of both variants are the same, due to that the parent 
	/// selection is external to the crossover operator. 
	/// - Implemented variants:
	/// - rand/1/bin (best/1/bin)
	/// - rand/1/exp (best/1/exp)
	/// - current-to-rand/1 (current-to-best/1)
	/// - current-to-rand/1/bin (current-to-best/1/bin)
	/// - current-to-rand/1/exp (current-to-best/1/exp)
	/// </summary>
	[Serializable]
	public class DifferentialEvolutionCrossover:Crossover
	{
		/// <summary> DEFAULT_CR defines a default CR (crossover operation control) value</summary>
		public const double DEFAULT_CR = 0.5;
		
		/// <summary> DEFAULT_F defines the default F (Scaling factor for mutation) value</summary>
		private const double DEFAULT_F = 0.5;
		
		/// <summary> DEFAULT_K defines a default K value used in variants current-to-rand/1
		/// and current-to-best/1
		/// </summary>
		public const double DEFAULT_K = 0.5;
		
		/// <summary> DEFAULT_VARIANT defines the default DE variant</summary>
		
		private const System.String DEFAULT_DE_VARIANT = "rand/1/bin";
		
		/// <summary> REAL_SOLUTION represents class JARE.base.solutionType.RealSolutionType</summary>
		private static System.Type REAL_SOLUTION;
		
		/// <summary> ARRAY_REAL_SOLUTION represents class JARE.base.solutionType.ArrayRealSolutionType</summary>
		private static System.Type ARRAY_REAL_SOLUTION;
		
		public double m_CR;
		public double m_F;
		public double m_K;
		private System.String m_DE_Variant; // DE variant (rand/1/bin, rand/1/exp, etc.)
		
		/// <summary> Constructor</summary>
		public DifferentialEvolutionCrossover()
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
			}
			m_CR = DEFAULT_CR;
			m_F = DEFAULT_F;
			m_K = DEFAULT_K;
			m_DE_Variant = DEFAULT_DE_VARIANT;
		} // Constructor
		
		// visnja: Proveriti sa Bobanom

		/// <summary> Constructor</summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //public DifferentialEvolutionCrossover(System.Collections.IDictionary properties):this()
        //{
        //    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //    m_CR = (System.Double.Parse((System.String) properties.Get("m_CR")));
        //    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //    m_F = (System.Double.Parse((System.String) properties.Get("m_F")));
        //    //UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //    m_K = (System.Double.Parse((System.String) properties.Get("m_k")));
        //    m_DE_Variant = properties.Get("DE_Variant_");
        //} // Constructor
		
		/// <summary> Executes the operation</summary>
		/// <param name="object">An object containing an array of three parents
		/// </param>
		/// <returns> An object containing the offSprings
		/// </returns>
		public override System.Object execute(System.Object obj)
		{
			System.Object[] parameters = (System.Object[]) obj;
			Solution current = (Solution) parameters[0];
			Solution[] parent = (Solution[]) parameters[1];
			
			Solution child;
			
			if (((parent[0].Type.GetType() != REAL_SOLUTION) && (parent[1].Type.GetType() != REAL_SOLUTION) && (parent[2].Type.GetType() != REAL_SOLUTION)) && ((parent[0].Type.GetType() != ARRAY_REAL_SOLUTION) && (parent[1].Type.GetType() != ARRAY_REAL_SOLUTION) && (parent[2].Type.GetType() != ARRAY_REAL_SOLUTION)))
			{
				
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				Configuration.m_logger.WriteLog("DifferentialEvolutionCrossover.execute: " + " the solutions " + "are not of the right type. The type should be 'Real' or 'ArrayReal', but " + parent[0].Type + " and " + parent[1].Type + " and " + parent[2].Type + " are obtained");
				
				//System.Type cls = typeof(string);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			}
			
			System.Double CR = (System.Double) getParameter("CR");
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("CR") != null)
			{
				m_CR = CR;
			} // if
			System.Double F = (System.Double) getParameter("F");
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("F") != null)
			{
				m_F = F;
			} // if
			System.Double K = (System.Double) getParameter("K");
			//UPGRADE_TODO: The 'System.Double' structure does not have an equivalent to NULL. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1291'"
            if (getParameter("K") != null)
			{
				m_K = K;
			} // if
			
			// STEP 2. Checking the variant
			System.String DE_Variant = (System.String) getParameter("DE_VARIANT");
			if (DE_Variant != null)
			{
				m_DE_Variant = DE_Variant;
			} // if
			
			int jrand;
			
			child = new Solution(current);
			
			XReal xParent0 = new XReal(parent[0]);
			XReal xParent1 = new XReal(parent[1]);
			XReal xParent2 = new XReal(parent[2]);
			XReal xCurrent = new XReal(current);
			XReal xChild = new XReal(child);
			
			int numberOfVariables = xParent0.NumberOfDecisionVariables;
			jrand = (int) (PseudoRandom.randInt(0, numberOfVariables - 1));
			
			// STEP 4. Checking the DE variant
			if ((String.CompareOrdinal(m_DE_Variant, "rand/1/bin") == 0) || (String.CompareOrdinal(m_DE_Variant, "best/1/bin") == 0))
			{
				for (int j = 0; j < numberOfVariables; j++)
				{
					if (PseudoRandom.randDouble(0, 1) < m_CR || j == jrand)
					{
						double val;
						val = xParent2.getValue(j) + m_F * (xParent0.getValue(j) - xParent1.getValue(j));
						if (val < xChild.getLowerBound(j))
							val = xChild.getLowerBound(j);
						if (val > xChild.getUpperBound(j))
							val = xChild.getUpperBound(j);
						
						xChild.setValue(j, val);
					}
					else
					{
						double val;
						val = xCurrent.getValue(j);
						xChild.setValue(j, val);
					} // else
				} // for
			}
			// if
			else if ((String.CompareOrdinal(m_DE_Variant, "rand/1/exp") == 0) || (String.CompareOrdinal(m_DE_Variant, "best/1/exp") == 0))
			{
				CR = m_CR;
				for (int j = 0; j < numberOfVariables; j++)
				{
					if (PseudoRandom.randDouble(0, 1) < CR || j == jrand)
					{
						double val;
						val = xParent2.getValue(j) + m_F * (xParent0.getValue(j) - xParent1.getValue(j));
						
						if (val < xChild.getLowerBound(j))
							val = xChild.getLowerBound(j);
						if (val > xChild.getUpperBound(j))
							val = xChild.getUpperBound(j);
						
						xChild.setValue(j, val);
					}
					else
					{
						CR = 0.0;
						double val;
						val = xCurrent.getValue(j);
						xChild.setValue(j, val);
					} // else
				} // for		
			}
			// if
			else if ((String.CompareOrdinal(m_DE_Variant, "current-to-rand/1") == 0) || (String.CompareOrdinal(m_DE_Variant, "current-to-best/1") == 0))
			{
				for (int j = 0; j < numberOfVariables; j++)
				{
					double val;
					val = xCurrent.getValue(j) + m_K * (xParent2.getValue(j) - xCurrent.getValue(j)) + m_F * (xParent0.getValue(j) - xParent1.getValue(j));
					
					if (val < xChild.getLowerBound(j))
						val = xChild.getLowerBound(j);
					if (val > xChild.getUpperBound(j))
						val = xChild.getUpperBound(j);
					
					xChild.setValue(j, val);
				} // for		
			}
			// if
			else if ((String.CompareOrdinal(m_DE_Variant, "current-to-rand/1/bin") == 0) || (String.CompareOrdinal(m_DE_Variant, "current-to-best/1/bin") == 0))
			{
				for (int j = 0; j < numberOfVariables; j++)
				{
					if (PseudoRandom.randDouble(0, 1) < m_CR || j == jrand)
					{
						double val;
						val = xCurrent.getValue(j) + m_K * (xParent2.getValue(j) - xCurrent.getValue(j)) + m_F * (xParent0.getValue(j) - xParent1.getValue(j));
						
						if (val < xChild.getLowerBound(j))
							val = xChild.getLowerBound(j);
						if (val > xChild.getUpperBound(j))
							val = xChild.getUpperBound(j);
						
						xChild.setValue(j, val);
					}
					else
					{
						double val;
						val = xCurrent.getValue(j);
						xChild.setValue(j, val);
					} // else
				} // for
			}
			// if
			else if ((String.CompareOrdinal(m_DE_Variant, "current-to-rand/1/exp") == 0) || (String.CompareOrdinal(m_DE_Variant, "current-to-best/1/exp") == 0))
			{
				CR = m_CR;
				for (int j = 0; j < numberOfVariables; j++)
				{
					if (PseudoRandom.randDouble(0, 1) < CR || j == jrand)
					{
						double val;
						val = xCurrent.getValue(j) + m_K * (xParent2.getValue(j) - xCurrent.getValue(j)) + m_F * (xParent0.getValue(j) - xParent1.getValue(j));
						
						if (val < xChild.getLowerBound(j))
							val = xChild.getLowerBound(j);
						if (val > xChild.getUpperBound(j))
							val = xChild.getUpperBound(j);
						
						xChild.setValue(j, val);
					}
					else
					{
						CR = 0.0;
						double val;
						val = xCurrent.getValue(j);
						xChild.setValue(j, val);
					} // else
				} // for		
			}
			// if		
			else
			{
				Configuration.m_logger.WriteLog("DifferentialEvolutionCrossover.execute: " + " unknown DE variant (" + m_DE_Variant + ")");
				//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
                string name = this.GetType().FullName;
				throw new SMException("Exception in " + name + ".execute()");
			} // else
			return child;
		}
	} // DifferentialEvolutionCrossover
}
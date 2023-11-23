/// <summary> XReal.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Wrapper for accessing real-coded solutions
/// </version>
using System;
using Solution = JARE.Base.Solution;
using SolutionType = JARE.Base.SolutionType;
using ArrayReal = JARE.Base.variable.ArrayReal;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
namespace JARE.util.wrapper
{
	
	public class XReal
	{
        ///// <summary> Returns the number of variables of the solution</summary>
        ///// <returns>
        ///// </returns>
        
        internal Solution m_solution;
        internal SolutionType m_type;

        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        private static System.Type REAL_SOLUTION;
        private static System.Type REAL_WEIGHTS_SOLUTION;
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        private static System.Type BINARY_REAL_SOLUTION;
        //UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        private static System.Type ARRAY_REAL_SOLUTION;

        /// <summary> Constructor</summary>
        public XReal()
        {
            try
            {
                //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                //REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.RealSolutionType");
               
                //Visnja: typeof je efikasniji od GetType
                REAL_SOLUTION = typeof(JARE.Base.solutionType.RealSolutionType);
                REAL_WEIGHTS_SOLUTION = typeof(JARE.Base.solutionType.RealWeightsSolutionType);
                
                //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                //BINARY_REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.BinaryRealSolutionType");

                BINARY_REAL_SOLUTION = typeof(JARE.Base.solutionType.BinaryRealSolutionType);
                
                //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                //ARRAY_REAL_SOLUTION = System.Type.GetType("JARE.Base.solutionType.ArrayRealSolutionType");

                ARRAY_REAL_SOLUTION = typeof(JARE.Base.solutionType.ArrayRealSolutionType);
            }
            //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
            catch (System.Exception e)
            {
                //UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
                JARE.support.SupportClass.WriteStackTrace(e, Console.Error);
            }
        } // Constructor

        /// <summary> Constructor</summary>
        /// <param name="solution">
        /// </param>
        public XReal(Solution solution):this()
        {
            m_type = solution.Type;
            m_solution = solution;
        }

        /// <summary> Gets value of a variable</summary>
        /// <param name="index">Index of the variable
        /// </param>
        /// <returns> The value of the variable
        /// </returns>
        /// <throws>  SMException </throws>
        public virtual double getValue(int index)
        {
            if ((m_type.GetType() == REAL_SOLUTION) || (m_type.GetType() == BINARY_REAL_SOLUTION) || (m_type.GetType() == REAL_WEIGHTS_SOLUTION))
            {
                return m_solution.DecisionVariables[index].getValue();
            }
            else if (m_type.GetType() == ARRAY_REAL_SOLUTION)
            {
                return ((ArrayReal) (m_solution.DecisionVariables[0])).m_array[index];
            }
            else
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                Configuration.m_logger.WriteLog("JARE.util.wrapper.XReal.getValue, solution type " + m_type + "+ invalid");
            }
            return 0.0;
        }
        virtual public int NumberOfDecisionVariables
        {
            get
            {
                if ((m_type.GetType() == REAL_SOLUTION) || (m_type.GetType() == BINARY_REAL_SOLUTION) || (m_type.GetType() == REAL_WEIGHTS_SOLUTION))
                    return m_solution.DecisionVariables.Length;
                else if (m_type.GetType() == ARRAY_REAL_SOLUTION)
                    return ((ArrayReal)(m_solution.DecisionVariables[0])).Length;
                else
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    Configuration.m_logger.WriteLog("JARE.util.wrapper.XReal.size, solution type " + m_type + "+ invalid");
                }
                return 0;
            }
            // size

        }
        /// <summary> Sets the value of a variable</summary>
        /// <param name="index">Index of the variable
        /// </param>
        /// <param name="value">Value to be assigned
        /// </param>
        /// <throws>  SMException </throws>
        public virtual void  setValue(int index, double val)
        {
            if (m_type.GetType() == REAL_SOLUTION)
                m_solution.DecisionVariables[index].setValue(val);
            else if (m_type.GetType() == ARRAY_REAL_SOLUTION)
                ((ArrayReal)(m_solution.DecisionVariables[0])).m_array[index] = val;
            else if (m_type.GetType() == REAL_WEIGHTS_SOLUTION)
                m_solution.DecisionVariables[index].setValue(val);
            else
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                Configuration.m_logger.WriteLog("JARE.util.wrapper.XReal.setValue, solution type " + m_type + "+ invalid");
            }
        } // setValue	

        /// <summary> Gets the lower bound of a variable</summary>
        /// <param name="index">Index of the variable
        /// </param>
        /// <returns> The lower bound of the variable
        /// </returns>
        /// <throws>  SMException </throws>
        public virtual double getLowerBound(int index)
        {
            if ((m_type.GetType() == REAL_SOLUTION) || (m_type.GetType() == BINARY_REAL_SOLUTION) || (m_type.GetType() == REAL_WEIGHTS_SOLUTION))
                return m_solution.DecisionVariables[index].getLowerBound();
            else if (m_type.GetType() == ARRAY_REAL_SOLUTION)
                return ((ArrayReal) (m_solution.DecisionVariables[0])).getLowerBound(index);
            else
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                Configuration.m_logger.WriteLog("JARE.util.wrapper.XReal.getLowerBound, solution type " + m_type + "+ invalid");
            }
            return 0.0;
        } // getLowerBound

        /// <summary> Gets the upper bound of a variable</summary>
        /// <param name="index">Index of the variable
        /// </param>
        /// <returns> The upper bound of the variable
        /// </returns>
        /// <throws>  SMException </throws>
        public virtual double getUpperBound(int index)
        {
            if ((m_type.GetType() == REAL_SOLUTION) || (m_type.GetType() == BINARY_REAL_SOLUTION) || (m_type.GetType() == REAL_WEIGHTS_SOLUTION))
                return m_solution.DecisionVariables[index].getUpperBound();
            else if (m_type.GetType() == ARRAY_REAL_SOLUTION)
                return ((ArrayReal) (m_solution.DecisionVariables[0])).getUpperBound(index);
            else
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                Configuration.m_logger.WriteLog("JARE.util.wrapper.XReal.getUpperBound, solution type " + m_type + "+ invalid");
            }

            return 0.0;
        } // getUpperBound
    } // XReal
}
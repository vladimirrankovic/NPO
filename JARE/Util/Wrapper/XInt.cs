/// <summary> XInt.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// 
/// Wrapper for accessing integer-coded solutions
/// </version>
using System;
using Solution = JARE.Base.Solution;
using SolutionType = JARE.Base.SolutionType;
using ArrayInt = JARE.Base.variable.ArrayInt;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
namespace JARE.util.wrapper
{
	
	public class XInt
	{
		/// <summary> Returns the number of variables of the solution</summary>
		/// <returns>
		/// </returns>
        //virtual public int NumberOfDecisionVariables
        //{
        //    get
        //    {
        //        if (m_type.GetType() == INT_SOLUTION)
        //            return m_solution.DecisionVariables.Length;
        //        else if (m_type.GetType() == ARRAY_INT_SOLUTION)
        //            return ((ArrayInt) (m_solution.DecisionVariables[0])).Length;
        //        else
        //        {
        //            // TODO ????
        //            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
        //            Configuration.m_logger.severe("JARE.util.wrapper.XInt.size, solution type " + m_type + "+ invalid");
        //        }
        //        return 0;
        //    }
        //    // size
			
        //}
        //internal Solution m_solution;
        //internal SolutionType m_type;
		
        ////UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        ////private static Class < ? > INT_SOLUTION;
        ////UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        ////private static Class < ? > ARRAY_INT_SOLUTION;
		
        ///// <summary> Constructor</summary>
        //public XInt()
        //{
        //    try
        //    {
        //        //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //        //INT_SOLUTION = System.Type.GetType("JARE.Base.solutionType.IntSolutionType");
        //        //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //        //ARRAY_INT_SOLUTION = System.Type.GetType("JARE.Base.solutionType.ArrayIntSolutionType");
        //    }
        //    //UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
        //    catch (System.Exception e)
        //    {
        //        // TODO Auto-generated catch block
        //        //UPGRADE_ISSUE: Method 'java.lang.ClassNotFoundException.printStackTrace' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangClassNotFoundExceptionprintStackTrace'"
        //        e.printStackTrace();
        //    }
        //} // Constructor
		
        ///// <summary> Constructor</summary>
        ///// <param name="solution">
        ///// </param>
        //public XInt(Solution solution):this()
        //{
        //    m_type = solution.Type;
        //    m_solution = solution;
        //}
		
        ///// <summary> Gets value of a variable</summary>
        ///// <param name="index">Index of the variable
        ///// </param>
        ///// <returns> The value of the variable
        ///// </returns>
        ///// <throws>  SMException </throws>
        //public virtual int getValue(int index)
        //{
        //    if (m_type.GetType() == INT_SOLUTION)
        //    {
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        return (int) m_solution.DecisionVariables[index].getValue();
        //    }
        //    else if (m_type.GetType() == ARRAY_INT_SOLUTION)
        //    {
        //        return ((ArrayInt) (m_solution.DecisionVariables[0])).array_[index];
        //    }
        //    else
        //    {
        //        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
        //        Configuration.m_logger.severe("JARE.util.wrapper.XInt.getValue, solution type " + m_type + "+ invalid");
        //    }
        //    return 0;
        //} // Get value
		
        ///// <summary> Sets the value of a variable</summary>
        ///// <param name="index">Index of the variable
        ///// </param>
        ///// <param name="value">Value to be assigned
        ///// </param>
        ///// <throws>  SMException </throws>
        //public virtual void  setValue(int index, int val)
        //{
        //    if (m_type.GetType() == INT_SOLUTION)
        //        m_solution.DecisionVariables[index].setValue(val);
        //    else if (m_type.GetType() == ARRAY_INT_SOLUTION)
        //        ((ArrayInt) (m_solution.DecisionVariables[0])).array_[index] = val;
        //    else
        //    {
        //        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
        //        Configuration.m_logger.severe("JARE.util.wrapper.XInt.setValue, solution type " + m_type + "+ invalid");
        //    }
        //} // setValue	
		
        ///// <summary> Gets the lower bound of a variable</summary>
        ///// <param name="index">Index of the variable
        ///// </param>
        ///// <returns> The lower bound of the variable
        ///// </returns>
        ///// <throws>  SMException </throws>
        //public virtual int getLowerBound(int index)
        //{
        //    if (m_type.GetType() == INT_SOLUTION)
        //    {
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        return (int) m_solution.DecisionVariables[index].getLowerBound();
        //    }
        //    else if (m_type.GetType() == ARRAY_INT_SOLUTION)
        //    {
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        return (int) ((ArrayInt) (m_solution.DecisionVariables[0])).getLowerBound(index);
        //    }
        //    else
        //    {
        //        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
        //        Configuration.m_logger.severe("JARE.util.wrapper.Xreal.getLowerBound, solution type " + m_type + "+ invalid");
        //    }
        //    return 0;
        //} // getLowerBound
		
        ///// <summary> Gets the upper bound of a variable</summary>
        ///// <param name="index">Index of the variable
        ///// </param>
        ///// <returns> The upper bound of the variable
        ///// </returns>
        ///// <throws>  SMException </throws>
        //public virtual int getUpperBound(int index)
        //{
        //    if (m_type.GetType() == INT_SOLUTION)
        //    {
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        return (int) m_solution.DecisionVariables[index].getUpperBound();
        //    }
        //    else if (m_type.GetType() == ARRAY_INT_SOLUTION)
        //    {
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        return (int) ((ArrayInt) (m_solution.DecisionVariables[0])).getUpperBound(index);
        //    }
        //    else
        //    {
        //        //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
        //        Configuration.m_logger.severe("JARE.util.wrapper.Xreal.getUpperBound, solution type " + m_type + "+ invalid");
        //    }
			
        //    return 0;
        //} // getUpperBound
	} // XInt
}
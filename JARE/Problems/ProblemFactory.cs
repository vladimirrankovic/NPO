/// <summary> ProblemFactory.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Problem = JARE.Base.Problem;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using SupportClass = JARE.support.SupportClass;
namespace JARE.problems
{
	
	/// <summary> This class represents a factory for problems</summary>
	public class ProblemFactory
	{
		/// <summary> Creates an object representing a problem</summary>
		/// <param name="name">Name of the problem
		/// </param>
		/// <param name="params">Parameters characterizing the problem
		/// </param>
		/// <returns> The object representing the problem
		/// </returns>
		/// <throws>  SMException  </throws>
		public virtual Problem getProblem(System.String name, System.Object[] parameters)
		{
			// Params are the arguments
			// The number of argument must correspond with the problem constructor params
			
			System.String baseString = "JARE.problems.";
			if (name.Substring(0, (name.Length - 1) - (0)).ToUpper().Equals("DTLZ".ToUpper()))
				baseString += "DTLZ.";
			else if (name.Substring(0, (name.Length - 1) - (0)).ToUpper().Equals("WFG".ToUpper()))
				baseString += "WFG.";
			else if (name.Substring(0, (name.Length - 1) - (0)).ToUpper().Equals("ZDT".ToUpper()))
				baseString += "ZDT.";
			else if (name.Substring(0, (name.Length - 3) - (0)).ToUpper().Equals("ZZJ07".ToUpper()))
				baseString += "ZZJ07.";
			else if (name.Substring(0, (name.Length - 3) - (0)).ToUpper().Equals("LZ09".ToUpper()))
				baseString += "LZ09.";
			else if (name.Substring(0, (name.Length - 4) - (0)).ToUpper().Equals("ZZJ07".ToUpper()))
				baseString += "ZZJ07.";
			else if (name.Substring(0, (name.Length - 3) - (0)).ToUpper().Equals("LZ06".ToUpper()))
				baseString += "LZ06.";
			else if (name.Substring(0, (name.Length - 4) - (0)).ToUpper().Equals("CEC2009".ToUpper()))
				baseString += "cec2009Competition.";
			else if (name.Substring(0, (name.Length - 5) - (0)).ToUpper().Equals("CEC2009".ToUpper()))
				baseString += "cec2009Competition.";
			
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Type problemClass = System.Type.GetType(baseString + name);
				System.Reflection.ConstructorInfo[] constructors = problemClass.GetConstructors();
				int i = 0;
				//find the constructor
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Constructor.getParameterTypes' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				while ((i < constructors.Length) && (constructors[i].GetParameters().Length != parameters.Length))
				{
					i++;
				}
				// constructors[i] is the selected one constructor
				Problem problem = (Problem) constructors[i].Invoke(parameters);
				return problem;
			}
			// try
			catch (System.Exception e)
			{
				Configuration.m_logger.WriteLog("ProblemFactory.getProblem: " + "Problem '" + name + "' does not exist. " + "Please, check the problem names in JARE/problems");
				throw new SMException("Exception in " + name + ".getProblem()");
			} // catch            
		}
		
        //VLADA CONVERT - CELA FUNKCIJA JE STAVLJENA POD KOMENTAR JER SE ODNOSI NA GUI!!!
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //public virtual Problem getProblem(System.String name, System.Collections.Specialized.NameValueCollection parameters)
        //{
        //    // Params are the arguments
        //    // The number of argument must correspond with the problem constructor params

        //    System.String baseString = "JARE.gui.problems.";
        //    if (name.Substring(0, (name.Length - 1) - (0)).StartsWith("DTLZ"))
        //        baseString += "DTLZ.";
        //    else if (name.Substring(0, (name.Length - 1) - (0)).StartsWith("WFG"))
        //        baseString += "WFG.";
        //    else if (name.Substring(0, (name.Length - 1) - (0)).StartsWith("ZDT"))
        //        baseString += "ZDT.";
        //    else if (name.Substring(0, (name.Length - 3) - (0)).StartsWith("ZZJ07"))
        //        baseString += "ZZJ07.";
        //    else if (name.Substring(0, (name.Length - 3) - (0)).StartsWith("LZ09"))
        //        baseString += "LZ09.";
        //    else if (name.Substring(0, (name.Length - 4) - (0)).StartsWith("ZZJ07"))
        //        baseString += "ZZJ07.";
        //    else if (name.Substring(0, (name.Length - 3) - (0)).StartsWith("LZ06"))
        //        baseString += "LZ06.";
        //    else if (name.Substring(0, (name.Length - 4) - (0)).StartsWith("CEC2009"))
        //        baseString += "cec2009Competition.";
        //    else if (name.Substring(0, (name.Length - 5) - (0)).StartsWith("CEC2009"))
        //        baseString += "cec2009Competition.";

        //    try
        //    {
        //        //VLADA CONVERT - OSTAVLJENO ZA KOMPAJLIRANJE!!!
        //        //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
        //        System.Type problemClass = System.Type.GetType(baseString + name);
        //        //Constructor constructors = problemClass.getConstructor(Properties.class);
        //        //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //        System.Reflection.ConstructorInfo constructors = problemClass.GetConstructor(typeof(System.Collections.Specialized.NameValueCollection));

        //        Problem problem = (Problem)constructors.newInstance(parameters);

        //        return problem;
        //    }
        //    // try
        //    catch (System.Exception e)
        //    {
        //        SupportClass.WriteStackTrace(e, Console.Error);
        //        Configuration.m_logger.WriteLog("ProblemFactory.getProblem: " + "Problem '" + name + "' does not exist. " + "Please, check the problem names in JARE/problems");
        //        throw new SMException("Exception in " + name + ".getProblem()");
        //    } // catch
        //}

	}
}
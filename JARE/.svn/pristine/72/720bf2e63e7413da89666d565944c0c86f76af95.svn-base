/// <summary> VariableFactory.java
/// 
/// </summary>
/// <author>  Juanjo Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Variable = JARE.Base.variable;
using JARE.Base.variable;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
namespace JARE.Base.variable
{
	
	/// <summary> This class is intended to be used as a static Factory to obtains variables. </summary>
	public class VariableFactory
	{
		
		/// <summary> Obtains an instance of a <code>Variable</code> given its name.</summary>
		/// <param name="name">The name of the class from which we want to obtain an instance
		/// object
		/// </param>
		/// <throws>  SMException  </throws>
		public static Variable getVariable(System.String name)
		{
			Variable variable = null;
			System.String baseLocation = "JARE.Base.variable.";
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Type c = System.Type.GetType(baseLocation + name);
				variable = (Variable) System.Activator.CreateInstance(c);
				return variable;
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			//UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.UnauthorizedAccessException)
			{
				Configuration.m_logger.WriteLog("VariableFactory.getVariable: " + "IllegalAccessException ");
				System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.String name2 = cls.FullName;
				throw new SMException("Exception in " + name2 + ".getVariable()");
			}			
            catch (System.Exception)
			{
				Configuration.m_logger.WriteLog("VariableFactory.getVariable: " + "ClassNotFoundException ");
				System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.String name2 = cls.FullName;
				throw new SMException("Exception in " + name2 + ".getVariable()");
			}

            // Visnja: izbaceno zato sto je podrazumevano prethodnim exception-om

            //catch (System.Exception e2)
            //{
            //    Configuration.m_logger.WriteLog("VariableFactory.getVariable: " + "InstantiationException ");
            //    System.Type cls = typeof(System.String);
            //    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //    System.String name2 = cls.FullName;
            //    throw new SMException("Exception in " + name2 + ".getVariable()");
            //}

		} // getVariable      
	} //VariabeFactory
}
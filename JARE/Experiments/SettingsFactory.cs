/// <summary> SettingsFactory.java</summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
namespace JARE.experiments
{
	
	/// <summary> This class represents a factory for problems</summary>
	public class SettingsFactory
	{
		/// <summary> Creates a settings object</summary>
		/// <param name="name">Name of the algorithm
		/// </param>
		/// <param name="params">Parameters
		/// </param>
		/// <returns> The settings object
		/// </returns>
		/// <throws>  SMException  </throws>
		/// <summary> Creates a settings object</summary>
		/// <param name="name">Name of the algorithm
		/// </param>
		/// <param name="params">Parameters
		/// </param>
		/// <returns> The settings object
		/// </returns>
		/// <throws>  SMException  </throws>
		public virtual Settings getSettingsObject(System.String algorithmName, System.Object[] parameters)
		{
			// Params are the arguments
			// The only argument is the problem to solve
			
			System.String baseString = "JARE.experiments.settings." + algorithmName + "_Settings";
			
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Type problemClass = System.Type.GetType(baseString);
				System.Reflection.ConstructorInfo[] constructors = problemClass.GetConstructors();
				int i = 0;
				//find the constructor
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Constructor.getParameterTypes' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				while ((i < constructors.Length) && (constructors[i].GetParameters().Length != parameters.Length))
				{
					i++;
				}
				// constructors[i] is the selected one constructor
				Settings algorithmSettings = (Settings) constructors[i].Invoke(parameters);
				return algorithmSettings;
			}
			// try
			catch (System.Exception e)
			{
				Configuration.m_logger.WriteLog("SettingsFactory.getSettingsObject: " + "Settings '" + baseString + "' does not exist. " + "Please, check the algorithm name in JARE/metaheuristics");
				throw new SMException("Exception in " + baseString + ".getSettingsObject()");
			} // catch            
		} // getSttingsObject    
	} // SettingsFactory
}
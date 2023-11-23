/// <summary> StandardStudy.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//////using Logger = java.util.logging.Logger;
using Logger = SharpMetal.util.Logger;
using Algorithm = SharpMetal.Base.Algorithm;
using Problem = SharpMetal.Base.Problem;
using SMException = SharpMetal.util.SMException;
using SupportClass = SharpMetal.support.SupportClass;
namespace SharpMetal.experiments
{
	
	/// <author>  Antonio J. Nebro
	/// </author>
	public class GUIBasedStudy:Experiment
	{
		
		/// <summary> Configures the algorithms in each independent run</summary>
		/// <param name="problem">The problem to solve
		/// </param>
		/// <param name="problemIndex">
		/// </param>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public System.Collections.Specialized.NameValueCollection[] parameters = null;
		
		
		public override void  algorithmSettings(Problem problem, int problemIndex, Algorithm[] algorithm)
		{
			try
			{
				int numberOfAlgorithms = m_algorithmNameList.Length;
				
				if (!m_paretoFrontFile[problemIndex].Equals(""))
				{
					for (int i = 0; i < numberOfAlgorithms; i++)
					{
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						parameters[i]["PARETO_FRONT_FILE"] = m_paretoFrontFile[problemIndex];
						System.Object[] settingsParams = new System.Object[]{problem};
						algorithm[i] = (new SettingsFactory()).getSettingsObject(m_algorithmNameList[i], settingsParams).configure(parameters[i]);
					}
				} // if
			}
			catch (System.ArgumentException ex)
			{
                ////////Logger.getLogger(typeof(GUIBasedStudy).FullName).log(Level.SEVERE, null, ex);
			}
			catch (System.UnauthorizedAccessException ex)
			{
                ////////Logger.getLogger(typeof(GUIBasedStudy).FullName).log(Level.SEVERE, null, ex);
			}
			catch (SMException ex)
			{
				SupportClass.WriteStackTrace(ex, Console.Error);
                ////////Logger.getLogger(typeof(StandardStudy).FullName).log(Level.SEVERE, null, ex);
			}
		}
	} // StandardStudy
}
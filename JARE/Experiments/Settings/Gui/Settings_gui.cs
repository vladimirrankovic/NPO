/// <summary> Settings.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// Abstract Settings class
/// </version>
using System;
using Algorithm = SharpMetal.Base.Algorithm;
using Problem = SharpMetal.Base.Problem;
using Settings = SharpMetal.experiments.Settings;
using SMException = SharpMetal.util.SMException;
namespace SharpMetal.experiments.settings.gui
{
	
	public abstract class Settings_gui:Settings
	{
		
		
		/// <summary> Constructor</summary>
		public Settings_gui()
		{
		} // Constructor
		
		
		/// <summary> Default configure method</summary>
		/// <returns> A problem with the default configuration
		/// </returns>
		/// <throws>  SharpMetal.util.SMException </throws>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual Algorithm configure(System.Collections.Specialized.NameValueCollection settings, Problem problem)
		{
			m_problem = problem;
			return configure(settings);
		} // configure
	} // Settings_gui
}
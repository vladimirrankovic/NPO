/// <summary> ConfigurationsContainer.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// This interface should be implemented by all the GUI components which are aimed
/// at storing configurations. Each configuration is stored in a properties
/// item
/// </version>
using System;
namespace jmetal.gui
{
	
	public interface ConfigurationsContainer
	{
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		void  addConfiguration(System.Collections.Specialized.NameValueCollection configuration, System.String name);
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		System.Collections.Specialized.NameValueCollection getConfiguration(System.String name);
	} // ConfigurationsContainer
}
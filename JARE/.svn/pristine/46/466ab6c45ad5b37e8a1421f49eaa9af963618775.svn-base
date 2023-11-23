/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// Configuration.java
/// 
/// This class is aimed to provide a java Properties with all the information of the jMetal elements
/// </version>
using System;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Logger = java.util.logging.Logger;
namespace jmetal.gui.utils
{
	
	
	
	public class Configuration
	{
		/// <summary> Returns a <code>Properties</code> object containing all the information related to jMetal configurable
		/// elements
		/// </summary>
		/// <returns> A <code>Properties</code> object with the information, null in other case
		/// </returns>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public static System.Collections.Specialized.NameValueCollection Settings
		{
			get
			{
				if (jMetalProperties_ == null)
				{
					try
					{
						jMetalProperties_ = PropUtils.load(guiDataFile_);
					}
					catch (System.IO.FileNotFoundException fnfe)
					{
						try
						{
							return createConfiguration();
						}
						catch (System.Exception e)
						{
							Logger.getLogger(typeof(Configuration).FullName).log(Level.SEVERE, null, e);
						}
					}
					catch (System.IO.IOException ex)
					{
						Logger.getLogger(typeof(Configuration).FullName).log(Level.SEVERE, null, ex);
					}
				}
				return jMetalProperties_;
			}
			// getSettings
			
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private static System.Collections.Specialized.NameValueCollection jMetalProperties_;
		
		public static System.String guiDataFile_ = "gui.data";
		
		
		/// <summary> Creates a new configuration file with the default information contained in jMetal.</summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public static System.Collections.Specialized.NameValueCollection createConfiguration()
		{
			try
			{
				//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
				System.IO.FileStream out_Renamed = new System.IO.FileStream(guiDataFile_, System.IO.FileMode.Create);
				out_Renamed.Close();
			}
			catch (System.Exception e)
			{
				Logger.getLogger(typeof(Configuration).FullName).log(Level.SEVERE, null, e);
			}
			(new PrintAlgorithmsInfo()).printAlgorithmInfo();
			(new PrintProblemsInfo()).printProblemsInfo();
			(new PrintOperatorsInfo()).printOperatorInfo();
			return PropUtils.load(guiDataFile_);
		} // createConfiguration
		
		
		/// <summary> Refresh the information related to configurable jMetal elements 
		/// elements
		/// </summary>
		public static void  reload()
		{
			if (jMetalProperties_ == null)
			{
				try
				{
					jMetalProperties_ = PropUtils.load(guiDataFile_);
				}
				catch (System.IO.IOException ex)
				{
					Logger.getLogger(typeof(Configuration).FullName).log(Level.SEVERE, null, ex);
				}
			}
		} // reload
		
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public static void  save(System.Collections.Specialized.NameValueCollection properties)
		{
			System.IO.FileStream os = null;
			try
			{
				SupportClass.MapSupport.PutAll(jMetalProperties_, properties);
				//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString_boolean'"
				os = SupportClass.GetFileStream(Configuration.guiDataFile_, false);
				try
				{
					//UPGRADE_ISSUE: Method 'java.util.Properties.store' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilPropertiesstore_javaioOutputStream_javalangString'"
					properties.store(os, "--No comments--");
				}
				catch (System.IO.IOException ex)
				{
					Logger.getLogger(typeof(Configuration).FullName).log(Level.SEVERE, null, ex);
				}
				try
				{
					os.Close();
				}
				catch (System.IO.IOException ex)
				{
					Logger.getLogger(typeof(Configuration).FullName).log(Level.SEVERE, null, ex);
				}
			}
			catch (System.IO.FileNotFoundException ex)
			{
				Logger.getLogger(typeof(Configuration).FullName).log(Level.SEVERE, null, ex);
			}
			finally
			{
				try
				{
					os.Close();
				}
				catch (System.IO.IOException ex)
				{
					Logger.getLogger(typeof(Configuration).FullName).log(Level.SEVERE, null, ex);
				}
			}
		}
	} // Configuration
}
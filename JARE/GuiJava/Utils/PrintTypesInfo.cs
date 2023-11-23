/// <summary> PrintClassInfo.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  0.1
/// 
/// This class provides some utilities for writting classes information
/// 
/// </version>
using System;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Logger = java.util.logging.Logger;
namespace jmetal.gui.utils
{
	
	
	public class PrintTypesInfo
	{
		
		/// <summary> Constructor
		/// This constructor does nothing by default
		/// </summary>
		public PrintTypesInfo()
		{
			// Do nothing
		}
		
		
		/// <summary> Print class information</summary>
		public virtual void  printTypesInfo(System.String typeName, System.String packageName)
		{
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			System.Type auxClass = System.Type.GetType(packageName + "." + typeName);
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection properties = Configuration.Settings;
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			properties["SOLUTION_TYPE" + "." + typeName] = "";
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			properties[typeName + "package"] = packageName;
			
			
			//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString_boolean'"
			System.IO.FileStream os = SupportClass.GetFileStream(Configuration.guiDataFile_, true);
			//UPGRADE_ISSUE: Method 'java.util.Properties.store' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilPropertiesstore_javaioOutputStream_javalangString'"
			properties.store(os, "--No comments--");
			os.Close();
		}
		
		
		/*
		* Writes the information of problem classes contained in the jMetal default package
		*/
		public virtual void  printTypesInfo()
		{
			try
			{
				printTypesInfo("ArrayRealSolutionType", "jmetal.base.solutionType");
				printTypesInfo("BinaryRealSolutionType", "jmetal.base.solutionType");
				printTypesInfo("BinarySolutionType", "jmetal.base.solutionType");
				printTypesInfo("IntRealSolutionType", "jmetal.base.solutionType");
				printTypesInfo("IntSolutionType", "jmetal.base.solutionType");
				printTypesInfo("PermutationSolutionType", "jmetal.base.solutionType");
				printTypesInfo("RealSolutionType", "jmetal.base.solutionType");
			}
			catch (System.Exception ex)
			{
				Logger.getLogger(typeof(PrintProblemsInfo).FullName).log(Level.SEVERE, null, ex);
			}
		}
		
		
		/// <summary> Example main</summary>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			(new PrintTypesInfo()).printTypesInfo();
		}
	}
}
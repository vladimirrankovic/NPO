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
	
	
	public class PrintClassInfo
	{
		
		public const System.String ALGORITHM_LABEL = "Algorithm";
		public const System.String ALGORITHM_PACKAGE_LABEL = ".package";
		public const System.String ALGORITHM_PARAM_LABEL = ".param";
		public const System.String ALGORITHM_PARAM_VALUE = ".value";
		public const System.String ALGORITHM_OPERATOR = ".operator";
		
		/// <summary> Constructor
		/// This constructor does nothing by default
		/// </summary>
		public PrintClassInfo()
		{
			// Do nothing
		}
		
		/// <summary> Print class information</summary>
		public virtual void  printAlgorithmInfo(System.String algorithmName, System.String packageName)
		{
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			System.Type auxClass = System.Type.GetType("jmetal.experiments.settings." + algorithmName + "_Settings");
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection properties = PropUtils.load(Configuration.guiDataFile_);
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			properties[ALGORITHM_LABEL + "." + algorithmName] = "";
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			properties[algorithmName + ALGORITHM_PACKAGE_LABEL] = packageName;
			
			
			
			System.Reflection.FieldInfo[] fields = auxClass.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Static);
			//UPGRADE_TODO: Method 'java.lang.Class.newInstance' was converted to 'System.Activator.CreateInstance' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassnewInstance'"
			System.Object objeto = System.Activator.CreateInstance(auxClass);
			
			for (int i = 0; i < fields.Length; i++)
			{
				try
				{
					//UPGRADE_ISSUE: Method 'java.lang.reflect.Field.getModifiers' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangreflectFieldgetModifiers'"
					//UPGRADE_ISSUE: Field 'java.lang.reflect.Modifier.PUBLIC' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangreflectModifier'"
					if (fields[i].getModifiers() == Modifier.PUBLIC)
					{
						if (fields[i].FieldType.Equals(typeof(double)))
						{
							
							System.String key = algorithmName + ALGORITHM_PARAM_LABEL + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = "double";
							key = algorithmName + ".DEFAULT" + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = ((double) fields[i].GetValue(objeto)).ToString();
						}
						else if (fields[i].FieldType.Equals(typeof(int)))
						{
							
							System.String key = algorithmName + ALGORITHM_PARAM_LABEL + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = "int";
							key = algorithmName + ".DEFAULT" + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = ((System.Int32) fields[i].GetValue(objeto)).ToString();
						}
						else if (fields[i].FieldType.Equals(typeof(jmetal.base_Renamed.operator_Renamed.mutation.Mutation)))
						{
							System.String key = algorithmName + ALGORITHM_PARAM_LABEL + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = "Mutation";
							key = algorithmName + ".DEFAULT" + fields[i].Name;
							
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							System.String aux = fields[i].GetValue(objeto).GetType().FullName;
							int lastIndex = aux.LastIndexOf('.');
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = aux.Substring(lastIndex + 1);
						}
						else if (fields[i].FieldType.Equals(typeof(jmetal.base_Renamed.operator_Renamed.crossover.Crossover)))
						{
							System.String key = algorithmName + ALGORITHM_PARAM_LABEL + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = "Crossover";
							key = algorithmName + fields[i].Name;
							key = algorithmName + ".DEFAULT" + fields[i].Name;
							
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							System.String aux = fields[i].GetValue(objeto).GetType().FullName;
							int lastIndex = aux.LastIndexOf('.');
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = aux.Substring(lastIndex + 1);
						}
						else if (fields[i].FieldType.Equals(typeof(jmetal.base_Renamed.operator_Renamed.selection.Selection)))
						{
							System.String key = algorithmName + ALGORITHM_PARAM_LABEL + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = "Mutation";
							key = algorithmName + fields[i].Name;
							key = algorithmName + ".DEFAULT" + fields[i].Name;
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							System.String aux = fields[i].GetValue(objeto).GetType().FullName;
							int lastIndex = aux.LastIndexOf('.');
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = aux.Substring(lastIndex + 1);
						}
					}
				}
				catch (System.ArgumentException ex)
				{
					Logger.getLogger(typeof(PrintClassInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.UnauthorizedAccessException ex)
				{
					Logger.getLogger(typeof(PrintClassInfo).FullName).log(Level.SEVERE, null, ex);
				}
			}
			//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString_boolean'"
			System.IO.FileStream os = SupportClass.GetFileStream(Configuration.guiDataFile_, true);
			//UPGRADE_ISSUE: Method 'java.util.Properties.store' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilPropertiesstore_javaioOutputStream_javalangString'"
			properties.store(os, "--No comments--");
			os.Close();
		}
		
		
		/// <summary> Example main</summary>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			try
			{
				PrintClassInfo p = new PrintClassInfo();
				
				p.printAlgorithmInfo("AbYSS", "jmetal.metaheuristics.abyss");
				p.printAlgorithmInfo("CellDE", "jmetal.metaheuristics.cellde");
				p.printAlgorithmInfo("GDE3", "jmetal.metaheuristics.gde3");
				p.printAlgorithmInfo("IBEA", "jmetal.metaheuristics.ibea");
				//p.printAlgorithmInfo("MOCell", "jmetal.metaheuristics.mocell");
				p.printAlgorithmInfo("MOEAD", "jmetal.metaheuristics.moead");
				p.printAlgorithmInfo("NSGAII", "jmetal.metaheuristics.nsgaII");
				p.printAlgorithmInfo("OMOPSO", "jmetal.metaheuristics.omposo");
				p.printAlgorithmInfo("PAES", "jmetal.metaheuristics.paes");
				p.printAlgorithmInfo("SMPSO", "jmetal.metaheuristics.smpso");
				p.printAlgorithmInfo("SPEA2", "jmetal.metaheuristics.spea2");
				//p.printAlgorithmInfo("SMPSO", "jmetal.metaheuristics.smpso");
				//  p.printProblemInfo("jmetal.problems.ZDT.ZDT1");
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception ex)
			{
				Logger.getLogger(typeof(PrintClassInfo).FullName).log(Level.SEVERE, null, ex);
			}
		}
	}
}
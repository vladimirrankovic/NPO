/// <summary> PrintClassInfo.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// 
/// This class provides some utilities for writting classes information
/// 
/// </version>
using System;
//UPGRADE_TODO: The type 'java.util.logging.Level' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Level = java.util.logging.Level;
//UPGRADE_TODO: The type 'java.util.logging.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Logger = java.util.logging.Logger;
using Problem = jmetal.base_Renamed.Problem;
namespace jmetal.gui.utils
{
	
	
	public class PrintProblemsInfo
	{
		
		/// <summary> Constructor
		/// This constructor does nothing by default
		/// </summary>
		public PrintProblemsInfo()
		{
			// Do nothing
		}
		
		
		/// <summary> Print class information</summary>
		public virtual void  printProblemInfo(System.String problemName, System.String packageName)
		{
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			System.Type auxClass = System.Type.GetType(packageName + "." + problemName + "_gui");
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection properties = Configuration.Settings;
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			properties["PROBLEM" + "." + problemName] = "";
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			properties[problemName + ".PACKAGE"] = packageName;
			
			
			
			System.Type superClass = auxClass.BaseType;
			System.Reflection.ConstructorInfo[] constructors = superClass.GetConstructors();
			int selectedConstructor = - 1;
			int selectedConstructorLength = 0;
			for (int i = 0; i < constructors.Length; i++)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Constructor.getParameterTypes' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				if (constructors[i].GetParameters().Length > selectedConstructorLength)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Constructor.getParameterTypes' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
					if ((constructors[i].GetParameters().Length > 0) && (constructors[i].GetParameters()[0] != typeof(System.Collections.Specialized.NameValueCollection)))
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Constructor.getParameterTypes' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						selectedConstructorLength = constructors[i].GetParameters().Length;
						selectedConstructor = i;
					}
				}
			}
			
			//UPGRADE_TODO: The differences in the expected value  of parameters for method 'java.lang.Class.getDeclaredField'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			System.Reflection.FieldInfo fields = auxClass.GetField("parameterList", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Static);
			//UPGRADE_TODO: Method 'java.lang.Class.newInstance' was converted to 'System.Activator.CreateInstance' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassnewInstance'"
			System.Object objeto = System.Activator.CreateInstance(auxClass);
			System.String[] parameterList = (System.String[]) fields.GetValue(objeto);
			
			
			
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Constructor.getParameterTypes' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			System.Type[] parameters = constructors[selectedConstructor].GetParameters();
			for (int i = 0; i < parameterList.Length; i++)
			{
				try
				{
					
					if (parameters[i].Equals(typeof(double)) || parameters[i].Equals(typeof(System.Double)))
					{
						System.String key = problemName + ".PARAMETER." + parameterList[i];
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						properties[key] = "double";
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						//UPGRADE_TODO: The differences in the expected value  of parameters for method 'java.lang.Class.getDeclaredField'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
						properties[problemName + ".DEFAULT." + parameterList[i]] = (System.String) auxClass.GetField(parameterList[i], System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Static).GetValue(objeto);
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						properties[problemName + ".ORDER." + parameterList[i]] = i + "";
					}
					else if ((parameters[i].Equals(typeof(int))) || (parameters[i].Equals(typeof(System.Int32))))
					{
						System.String key = problemName + ".PARAMETER." + parameterList[i];
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						properties[key] = "int";
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						//UPGRADE_TODO: The differences in the expected value  of parameters for method 'java.lang.Class.getDeclaredField'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
						properties[problemName + ".DEFAULT." + parameterList[i]] = auxClass.GetField(parameterList[i], System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Static).GetValue(objeto).ToString();
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						properties[problemName + ".ORDER." + parameterList[i]] = i + "";
					}
					else if ((parameters[i].Equals(typeof(System.String)) && (parameterList[i].Equals("SolutionType"))))
					{
						System.String key = problemName + ".PARAMETER." + parameterList[i];
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						properties[key] = "SolutionType";
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						//UPGRADE_TODO: The differences in the expected value  of parameters for method 'java.lang.Class.getDeclaredField'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
						properties[problemName + ".DEFAULT." + parameterList[i]] = (System.String) auxClass.GetField(parameterList[i], System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Static).GetValue(objeto);
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						properties[problemName + ".ORDER." + parameterList[i]] = i + "";
						
						//  properties.setProperty(algorithmName+fields[i].getName(),".Mutation");
					}
				}
				catch (System.ArgumentException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
			}
			
			
			Configuration.save(properties);
		}
		
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual Problem getProblem(System.String name, System.Collections.Specialized.NameValueCollection properties)
		{
			Problem problem = null;
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection aProblem = properties;
			System.Collections.IList parameters = new System.Collections.ArrayList();
			
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection problemParameters = PropUtils.getPropertiesWithPrefix(aProblem, ".PARAMETER");
			
			int i = 0;
			System.Collections.IEnumerator iterator = new SupportClass.HashSetSupport(problemParameters).GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iterator.MoveNext())
			{
				i++;
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.String next = (System.String) iterator.Current;
				System.String type = problemParameters.Get(next);
				
				int index = System.Int32.Parse((System.String) aProblem.Get(".ORDER" + next));
				if (index > parameters.Count)
					index = parameters.Count;
				
				
				if (type.Equals("int"))
				{
					parameters.Insert(index, System.Int32.Parse(aProblem.Get(".VALUE" + next)));
				}
				else if (type.Equals("double"))
				{
					//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					parameters.Insert(index, System.Double.Parse(aProblem.Get(".VALUE" + next)));
				}
				else if (type.Equals("SolutionType"))
				{
					parameters.Insert(index, (System.String) (aProblem.Get(".VALUE" + next)));
				}
			}
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				System.Reflection.ConstructorInfo[] c = System.Type.GetType(aProblem.Get(".PACKAGE") + "." + name).GetConstructors();
				int index = 0;
				System.Reflection.ConstructorInfo selected = c[index];
				
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.reflect.Constructor.getParameterTypes' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				while (selected.GetParameters().Length != i)
				{
					selected = c[++index];
				}
				try
				{
					System.Object[] a = SupportClass.ICollectionSupport.ToArray(parameters);
					
					problem = (Problem) selected.Invoke(SupportClass.ICollectionSupport.ToArray(parameters));
				}
				//UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
				catch (System.Exception ex)
				{
					Logger.getLogger(typeof(PrintProblemsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.UnauthorizedAccessException ex)
				{
					Logger.getLogger(typeof(PrintProblemsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.ArgumentException ex)
				{
					Logger.getLogger(typeof(PrintProblemsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.Reflection.TargetInvocationException ex)
				{
					Logger.getLogger(typeof(PrintProblemsInfo).FullName).log(Level.SEVERE, null, ex);
				}
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception ex)
			{
				Logger.getLogger(typeof(PrintProblemsInfo).FullName).log(Level.SEVERE, null, ex);
			}
			
			return problem;
		}
		
		
		
		/*
		* Writes the information of problem classes contained in the jMetal default package
		*/
		public virtual void  printProblemsInfo()
		{
			try
			{
				printProblemInfo("ZDT1", "jmetal.gui.problems.ZDT");
				printProblemInfo("ZDT2", "jmetal.gui.problems.ZDT");
				printProblemInfo("ZDT3", "jmetal.gui.problems.ZDT");
				printProblemInfo("ZDT4", "jmetal.gui.problems.ZDT");
				//       printProblemInfo("ZDT5", "jmetal.gui.problems.ZDT");
				printProblemInfo("ZDT6", "jmetal.gui.problems.ZDT");
				printProblemInfo("DTLZ1", "jmetal.gui.problems.DTLZ");
				printProblemInfo("DTLZ2", "jmetal.gui.problems.DTLZ");
				printProblemInfo("DTLZ3", "jmetal.gui.problems.DTLZ");
				printProblemInfo("DTLZ4", "jmetal.gui.problems.DTLZ");
				printProblemInfo("DTLZ5", "jmetal.gui.problems.DTLZ");
				printProblemInfo("DTLZ6", "jmetal.gui.problems.DTLZ");
				printProblemInfo("DTLZ7", "jmetal.gui.problems.DTLZ");
				printProblemInfo("WFG1", "jmetal.gui.problems.WFG");
				printProblemInfo("WFG2", "jmetal.gui.problems.WFG");
				printProblemInfo("WFG3", "jmetal.gui.problems.WFG");
				printProblemInfo("WFG4", "jmetal.gui.problems.WFG");
				printProblemInfo("WFG5", "jmetal.gui.problems.WFG");
				printProblemInfo("WFG6", "jmetal.gui.problems.WFG");
				printProblemInfo("WFG7", "jmetal.gui.problems.WFG");
				printProblemInfo("WFG8", "jmetal.gui.problems.WFG");
				printProblemInfo("WFG9", "jmetal.gui.problems.WFG");
				printProblemInfo("Schaffer", "jmetal.gui.problems");
				printProblemInfo("ConstrEx", "jmetal.gui.problems");
				printProblemInfo("Fonseca", "jmetal.gui.problems");
				printProblemInfo("Golinski", "jmetal.gui.problems");
				printProblemInfo("Kursawe", "jmetal.gui.problems");
				//        printProblemInfo("OKA1", "jmetal.gui.problems");
				//        printProblemInfo("OKA2", "jmetal.gui.problems");
				printProblemInfo("Osyczka2", "jmetal.gui.problems");
				printProblemInfo("Srinivas", "jmetal.gui.problems");
				printProblemInfo("Tanaka", "jmetal.gui.problems");
				//        printProblemInfo("Viennet2", "jmetal.gui.problems");
				//        printProblemInfo("Viennet3", "jmetal.gui.problems");
				//        printProblemInfo("Viennet4", "jmetal.gui.problems");
				//        printProblemInfo("Water", "jmetal.gui.problems");
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
			(new PrintProblemsInfo()).printProblemsInfo();
		}
	}
}
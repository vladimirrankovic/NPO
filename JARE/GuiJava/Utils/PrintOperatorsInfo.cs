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
using Operator = jmetal.base_Renamed.Operator;
namespace jmetal.gui.utils
{
	
	
	public class PrintOperatorsInfo
	{
		
		public const System.String OPERATOR_LABEL = "OPERATOR";
		public const System.String OPERATOR_PACKAGE_LABEL = ".PACKAGE";
		public const System.String OPERATOR_PARAM_LABEL = ".PARAMETER.";
		public const System.String OPERATOR_PARAM_VALUE = ".VALUE.";
		public const System.String OPERATOR_OPERATOR = ".OPERATOR.";
		
		/// <summary> Constructor
		/// This constructor does nothing by default
		/// </summary>
		public PrintOperatorsInfo()
		{
			// Do nothing
		}
		
		/// <summary> Print class information</summary>
		public virtual void  printOperatorInfo(System.String operatorName, System.String packageName)
		{
			
			
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			System.Type auxClass = System.Type.GetType(packageName + "." + operatorName);
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection properties = Configuration.Settings;
			
			System.Reflection.FieldInfo[] fields = auxClass.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Static);
			//UPGRADE_TODO: Method 'java.lang.Class.newInstance' was converted to 'System.Activator.CreateInstance' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangClassnewInstance'"
			System.Object objeto = System.Activator.CreateInstance(auxClass);
			
			// Which kind of operator is this?
			if (objeto.GetType().BaseType.Equals(typeof(jmetal.base_Renamed.operator_Renamed.crossover.Crossover)))
			{
				//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
				properties["Crossover." + operatorName] = "";
			}
			else if (objeto.GetType().BaseType.Equals(typeof(jmetal.base_Renamed.operator_Renamed.mutation.Mutation)))
			{
				//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
				properties["Mutation." + operatorName] = "";
			}
			else if (objeto.GetType().BaseType.Equals(typeof(jmetal.base_Renamed.operator_Renamed.selection.Selection)))
			{
				//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
				properties["Selection." + operatorName] = "";
			}
			
			//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
			properties[operatorName + OPERATOR_PACKAGE_LABEL] = packageName;
			
			
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
							System.String key = operatorName + OPERATOR_PARAM_LABEL + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = "double";
							key = operatorName + ".DEFAULT." + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = ((double) fields[i].GetValue(objeto)).ToString();
						}
						else if (fields[i].FieldType.Equals(typeof(int)))
						{
							System.String key = operatorName + OPERATOR_PARAM_LABEL + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = "int";
							key = operatorName + ".DEFAULT." + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = ((System.Int32) fields[i].GetValue(objeto)).ToString();
						}
					}
				}
				catch (System.ArgumentException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.UnauthorizedAccessException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
			}
			this.getOperator("PolynomialMutation", properties);
			Configuration.save(properties);
		}
		
		
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual Operator getOperator(System.String name, System.Collections.Specialized.NameValueCollection properties)
		{
			
			
			Operator operator_Renamed = null;
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection anOperator = PropUtils.getPropertiesWithPrefix(properties, name);
			
			
			// Reading the parameters of the operator
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection problemParameters = PropUtils.getPropertiesWithPrefix(anOperator, ".PARAMETER.");
			try
			{
				try
				{
					System.String packageName = (Configuration.Settings).Get(name + ".PACKAGE");
					//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					operator_Renamed = (Operator) System.Activator.CreateInstance(System.Type.GetType(packageName + "." + name));
				}
				//UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
				catch (System.Exception ex)
				{
					Logger.getLogger(typeof(PrintOperatorsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.UnauthorizedAccessException ex)
				{
					Logger.getLogger(typeof(PrintOperatorsInfo).FullName).log(Level.SEVERE, null, ex);
				}
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception ex)
			{
				Logger.getLogger(typeof(PrintOperatorsInfo).FullName).log(Level.SEVERE, null, ex);
			}
			
			
			int i = 0;
			System.Collections.IEnumerator iterator = new SupportClass.HashSetSupport(problemParameters).GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iterator.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.String next = (System.String) iterator.Current;
				System.String type = problemParameters.Get(next);
				
				if (type.Equals("int"))
				{
					operator_Renamed.setParameter(next, (System.Object) System.Int32.Parse(anOperator.Get(".VALUE." + next)));
				}
				else if (type.Equals("double"))
				{
					//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					operator_Renamed.setParameter(next, (System.Object) System.Double.Parse(anOperator.Get(".VALUE." + next)));
				}
			}
			return operator_Renamed;
		}
		
		
		
		
		/*
		* Writes the information of operator classes contained in the jMetal default package
		*/
		public virtual void  printOperatorInfo()
		{
			try
			{
				//printOperatorInfo("BitFlipMutation", "jmetal.base.operator.mutation");
				//printOperatorInfo("NonUniformMutation", "jmetal.base.operator.mutation");
				printOperatorInfo("PolynomialMutation", "jmetal.base.operator.mutation");
				//printOperatorInfo("SwapMutation", "jmetal.base.operator.mutation");
				//printOperatorInfo("UniformMutation", "jmetal.base.operator.mutation");
				//printOperatorInfo("DifferentialEvolutionCrossover", "jmetal.base.operator.crossover");
				//printOperatorInfo("HUXCrossover", "jmetal.base.operator.crossover");
				//printOperatorInfo("PMXCrossover", "jmetal.base.operator.crossover");
				printOperatorInfo("SBXCrossover", "jmetal.base.operator.crossover");
				//printOperatorInfo("SinglePointCrossover", "jmetal.base.operator.crossover");
				//printOperatorInfo("TwoPointsCrossover", "jmetal.base.operator.crossover");
			}
			catch (System.Exception ex)
			{
				Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
			}
		}
		
		/// <summary> Example main</summary>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			(new PrintOperatorsInfo()).printOperatorInfo();
		}
	}
}
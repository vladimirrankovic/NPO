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
using Algorithm = jmetal.base_Renamed.Algorithm;
using Operator = jmetal.base_Renamed.Operator;
using Problem = jmetal.base_Renamed.Problem;
using DifferentialEvolutionCrossover = jmetal.base_Renamed.operator_Renamed.crossover.DifferentialEvolutionCrossover;
using MutationLocalSearch = jmetal.base_Renamed.operator_Renamed.localSearch.MutationLocalSearch;
using MutationFactory = jmetal.base_Renamed.operator_Renamed.mutation.MutationFactory;
using BinaryTournament = jmetal.base_Renamed.operator_Renamed.selection.BinaryTournament;
using BinaryTournament2 = jmetal.base_Renamed.operator_Renamed.selection.BinaryTournament2;
using SelectionFactory = jmetal.base_Renamed.operator_Renamed.selection.SelectionFactory;
using ZDT1 = jmetal.problems.ZDT.ZDT1;
using JMException = jmetal.util.JMException;
namespace jmetal.gui.utils
{
	
	
	public class PrintAlgorithmsInfo
	{
		
		public const System.String ALGORITHM_LABEL = "Algorithm";
		public const System.String ALGORITHM_PACKAGE_LABEL = ".PACKAGE";
		public const System.String ALGORITHM_PARAM_LABEL = ".PARAMETER.";
		public const System.String ALGORITHM_PARAM_VALUE = ".VALUE.";
		public const System.String ALGORITHM_OPERATOR_LABEL = ".OPERATOR.";
		
		/// <summary> Constructor
		/// This constructor does nothing by default
		/// </summary>
		public PrintAlgorithmsInfo()
		{
			// Do nothing
		}
		
		/// <summary> Print class information</summary>
		public virtual void  printAlgorithmInfo(System.String algorithmName, System.String packageName)
		{
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			System.Type auxClass = System.Type.GetType("jmetal.experiments.settings." + algorithmName + "_Settings");
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection properties = Configuration.Settings;
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
							key = algorithmName + ".DEFAULT." + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = ((double) fields[i].GetValue(objeto)).ToString();
						}
						else if (fields[i].FieldType.Equals(typeof(int)))
						{
							
							System.String key = algorithmName + ALGORITHM_PARAM_LABEL + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = "int";
							key = algorithmName + ".DEFAULT." + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = ((System.Int32) fields[i].GetValue(objeto)).ToString();
						}
						else if (fields[i].FieldType.Equals(typeof(jmetal.base_Renamed.operator_Renamed.mutation.Mutation)))
						{
							System.String key = algorithmName + ALGORITHM_PARAM_LABEL + fields[i].Name;
							//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
							properties[key] = "Mutation";
							key = algorithmName + ".DEFAULT." + fields[i].Name;
							
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
							key = algorithmName + ".DEFAULT." + fields[i].Name;
							
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
							key = algorithmName + ".DEFAULT." + fields[i].Name;
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
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.UnauthorizedAccessException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
			}
			Configuration.save(properties);
		}
		
		
		
		//  public Algorithm getAlgorithm(String name, Properties properties, Problem problem) {
		//
		//    Algorithm algorithm = null;
		//    List parameters = new ArrayList();
		//
		//
		//
		//    try {
		//
		//        Constructor c = null;
		//        try {
		//            String classe = Configuration.getSettings().getProperty(".PACKAGE") + "." + name;
		//            c = Class.forName(classe).getConstructor(new Class[]{jmetal.base.Problem.class});
		//        } catch (NoSuchMethodException ex) {
		//            Logger.getLogger(PrintAlgorithmsInfo.class.getName()).log(Level.SEVERE, null, ex);
		//        } catch (SecurityException ex) {
		//            Logger.getLogger(PrintAlgorithmsInfo.class.getName()).log(Level.SEVERE, null, ex);
		//        }
		//        Object [] params = {problem};
		//
		//        try {
		//            algorithm = (Algorithm) c.newInstance(params);
		//        } catch (InstantiationException ex) {
		//            Logger.getLogger(PrintAlgorithmsInfo.class.getName()).log(Level.SEVERE, null, ex);
		//        } catch (IllegalAccessException ex) {
		//            Logger.getLogger(PrintAlgorithmsInfo.class.getName()).log(Level.SEVERE, null, ex);
		//        } catch (IllegalArgumentException ex) {
		//            Logger.getLogger(PrintAlgorithmsInfo.class.getName()).log(Level.SEVERE, null, ex);
		//        } catch (InvocationTargetException ex) {
		//            Logger.getLogger(PrintAlgorithmsInfo.class.getName()).log(Level.SEVERE, null, ex);
		//        }
		//
		//    } catch (ClassNotFoundException ex) {
		//        Logger.getLogger(PrintOperatorsInfo.class.getName()).log(Level.SEVERE, null, ex);
		//    }
		//
		//
		//
		//    Iterator iterator = algorithmParameter.keySet().iterator();
		//    List<String> parameterList = new ArrayList<String>();
		//    while (iterator.hasNext())
		//        parameterList.add((String)iterator.next());
		//
		//
		//    Properties algorithmOperator = PropUtils.getPropertiesWithPrefix(anAlgorithm, ".OPERATOR.");
		//    iterator = algorithmOperator.keySet().iterator();
		//    System.out.println(algorithmOperator);
		//    while (iterator.hasNext()) {
		//       String next = (String) iterator.next();
		//       Operator op = ((new PrintOperatorsInfo().getOperator(anAlgorithm.getProperty(".DEFAULT."+ next), properties)));
		//
		//       Iterator iterator2 = parameterList.iterator();
		//
		//       while (iterator2.hasNext()) {
		//           String parameterName = (String)iterator2.next();
		//           System.out.println("ParameterName: " + parameterName);
		//
		//
		//           if (parameterName.startsWith(next)) {
		//
		//              String type = algorithmParameter.getProperty(parameterName);
		//              System.out.println("El tipo es: "+type);
		//
		//              System.out.println("El nombre del paramtero es: "+parameterName.substring(next.length()).toLowerCase());
		//
		//              if (type.equals("int")) {
		//                op.setParameter(parameterName.substring(next.length()).toLowerCase(), new Integer(anAlgorithm.getProperty(".DEFAULT."+ parameterName)));
		//                //System.out.println(next + " " + new Integer(anAlgorithm.getProperty(".DEFAULT."+ next)));
		//              } else if (type.equals("double")) {
		//                op.setParameter(parameterName.substring(next.length()).toLowerCase(), new Double(anAlgorithm.getProperty(".DEFAULT."+ parameterName)));
		//                //System.out.println(next + " " + new Double(anAlgorithm.getProperty(".DEFAULT."+ next)));
		//              }
		//              iterator.remove();
		//           }
		//
		//       }
		//       algorithm.addOperator(next, op);
		//    }
		//
		//    iterator = parameterList.iterator();
		//
		//    while (iterator.hasNext()) {
		//       String next = (String) iterator.next();
		//       String type = algorithmParameter.getProperty(next);
		//
		//       if (type.equals("int")) {
		//           algorithm.setInputParameter(next, new Integer(anAlgorithm.getProperty(".DEFAULT."+ next)));
		//           System.out.println(next + " " + new Integer(anAlgorithm.getProperty(".DEFAULT."+ next)));
		//       } else if (type.equals("double")) {
		//           algorithm.setInputParameter(next, new Double(anAlgorithm.getProperty(".DEFAULT."+ next)));
		//           System.out.println(next + " " + new Double(anAlgorithm.getProperty(".DEFAULT."+ next)));
		//       } else if (type.equals("Crossover")) {
		//
		//           System.out.println(".DEFAULT."+ next);
		//           System.out.println(anAlgorithm.getProperty(".DEFAULT."+ next));
		//
		//           Operator op = ((new PrintOperatorsInfo().getOperator(anAlgorithm.getProperty(".DEFAULT."+ next), properties)));
		//           algorithm.addOperator(next, op);
		//       } else if (type.equals("Mutation")) {
		//
		//           System.out.println(".DEFAULT."+ next);
		//           System.out.println(anAlgorithm.getProperty(".DEFAULT."+ next));
		//
		//           Operator op = ((new PrintOperatorsInfo().getOperator(anAlgorithm.getProperty(".DEFAULT."+ next), properties)));
		//           algorithm.addOperator(next, op);
		//       }
		//
		//    }
		//
		//
		//      return Algorithm;
		//  }
		
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public virtual Algorithm getAlgorithm(System.String name, System.Collections.Specialized.NameValueCollection properties, Problem p)
		{
			Algorithm algorithm = null;
			System.Collections.IList parameters = new System.Collections.ArrayList();
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection anAlgorithm = properties;
			
			
			// Reading the parameters of the operator
			
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection algorithmParameter = PropUtils.getPropertiesWithPrefix(anAlgorithm, ".PARAMETER.");
			try
			{
				
				System.Reflection.ConstructorInfo c = null;
				try
				{
					//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					c = System.Type.GetType(anAlgorithm.Get(".PACKAGE") + "." + name).GetConstructor(new System.Type[]{typeof(jmetal.base_Renamed.Problem)});
				}
				catch (System.MethodAccessException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.Security.SecurityException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				System.Object[] params_Renamed = new System.Object[]{p};
				
				try
				{
					algorithm = (Algorithm) c.Invoke(params_Renamed);
				}
				//UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
				catch (System.Exception ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.UnauthorizedAccessException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.ArgumentException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
				catch (System.Reflection.TargetInvocationException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception ex)
			{
				Logger.getLogger(typeof(PrintOperatorsInfo).FullName).log(Level.SEVERE, null, ex);
			}
			
			
			
			System.Collections.IEnumerator iterator = new SupportClass.HashSetSupport(algorithmParameter).GetEnumerator();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			List < String > parameterList = new ArrayList < String >();
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iterator.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				parameterList.add((System.String) iterator.Current);
			}
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.Specialized.NameValueCollection algorithmOperator = PropUtils.getPropertiesWithPrefix(anAlgorithm, ".OPERATOR.");
			
			
			// AbYSS has a SBX and a Polynomial Mutation Operators
			if (name.Equals("AbYSS"))
			{
				//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
				algorithmOperator["crossover"] = "Crossover";
				//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
				algorithmOperator["mutation"] = "Mutation";
				//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
				anAlgorithm[".VALUE.crossover"] = "SBXCrossover";
				//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
				anAlgorithm[".VALUE.mutation"] = "PolynomialMutation";
			}
			
			
			iterator = new SupportClass.HashSetSupport(algorithmOperator).GetEnumerator();
			// Configuring and adding the operators
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iterator.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.String next = (System.String) iterator.Current;
				Operator op = ((new PrintOperatorsInfo().getOperator(anAlgorithm.Get(".VALUE." + next), properties)));
				System.Collections.IEnumerator iterator2 = parameterList.iterator();
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				while (iterator2.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					System.String parameterName = (System.String) iterator2.Current;
					if (parameterName.StartsWith(next))
					{
						System.String type = algorithmParameter.Get(parameterName);
						if (type.Equals("int"))
						{
							op.setParameter(parameterName.Substring(next.Length).ToLower(), (System.Object) System.Int32.Parse(anAlgorithm.Get(".VALUE." + parameterName)));
						}
						else if (type.Equals("double"))
						{
							//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
							System.Double doubleValue = new Double(System.Double.Parse(anAlgorithm.Get(".VALUE." + parameterName)));
							if (System.Double.IsNaN(doubleValue))
							{
								doubleValue = 1.0 / p.NumberOfVariables;
							}
							op.setParameter(parameterName.Substring(next.Length).ToLower(), (System.Object) doubleValue);
						}
						//UPGRADE_ISSUE: Method 'java.util.Iterator.remove' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilIteratorremove'"
						iterator2.remove();
					}
				}
				
				if (name.Equals("AbYSS") && next.Equals("mutation"))
				{
					
					// SPECIAL CASE: AbYSS
					Operator improvement = new MutationLocalSearch(p, op);
					
					int rounds = System.Int32.Parse(anAlgorithm.Get(".VALUE.improvementRounds"));
					improvement.setParameter("improvementRounds", rounds);
					algorithm.addOperator("improvement", improvement);
				}
				else
				{
					// Base case
					algorithm.addOperator(next, op);
				}
			}
			
			
			if (name.Equals("GDE3") || name.Equals("CellDE"))
			{
				Operator df = new DifferentialEvolutionCrossover();
				//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				double F = System.Double.Parse(anAlgorithm.Get(".VALUE.F"));
				//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				double CR = System.Double.Parse(anAlgorithm.Get(".VALUE.CR"));
				algorithm.addOperator("crossover", df);
				
				if (name.Equals("GDE3"))
				{
					Operator selection = null;
					try
					{
						// Add the operators to the algorithm
						selection = SelectionFactory.getSelectionOperator("DifferentialEvolutionSelection");
					}
					catch (JMException ex)
					{
						Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
					}
					algorithm.addOperator("selection", selection);
				}
				else
				{
					algorithm.addOperator("selection", new BinaryTournament());
				}
			}
			else if (name.Equals("NSGAII"))
			{
				algorithm.addOperator("selection", new BinaryTournament2());
			}
			else if (name.Equals("SMPSO"))
			{
				Operator mutation;
				try
				{
					mutation = MutationFactory.getMutationOperator("PolynomialMutation");
					mutation.setParameter("probability", 1.0 / p.NumberOfVariables);
					mutation.setParameter("distributionIndex", 20.0);
					algorithm.addOperator("mutation", mutation);
				}
				catch (JMException ex)
				{
					Logger.getLogger(typeof(PrintAlgorithmsInfo).FullName).log(Level.SEVERE, null, ex);
				}
			}
			else
			{
				algorithm.addOperator("selection", new BinaryTournament());
			}
			
			
			
			iterator = parameterList.iterator();
			// Adding operators and parameters
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iterator.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.String next = (System.String) iterator.Current;
				System.String type = algorithmParameter.Get(next);
				
				if (type.Equals("int"))
				{
					algorithm.setInputParameter(next, (System.Object) System.Int32.Parse(anAlgorithm.Get(".VALUE." + next)));
				}
				else if (type.Equals("double"))
				{
					//UPGRADE_TODO: The differences in the format  of parameters for constructor 'java.lang.Double.Double'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					algorithm.setInputParameter(next, (System.Object) System.Double.Parse(anAlgorithm.Get(".VALUE." + next)));
				}
				
				/*   else if (type.equals("Crossover")) {
				Operator op = ((new PrintOperatorsInfo().getOperator(anAlgorithm.getProperty(".VALUE."+ next), properties)));
				algorithm.addOperator(next, op);
				} else if (type.equals("Mutation")) {
				Operator op = ((new PrintOperatorsInfo().getOperator(anAlgorithm.getProperty(".VALUE."+ next), properties)));
				algorithm.addOperator(next, op);
				}*/
			}
			return algorithm;
		}
		
		
		
		
		
		
		/*
		* Writes the information of problem classes contained in the jMetal default package
		*/
		public virtual void  printAlgorithmInfo()
		{
			try
			{
				//printAlgorithmInfo("AbYSS", "jmetal.metaheuristics.abyss");
				//printAlgorithmInfo("CellDE", "jmetal.metaheuristics.cellde");
				//printAlgorithmInfo("GDE3", "jmetal.metaheuristics.gde3");
				//printAlgorithmInfo("IBEA", "jmetal.metaheuristics.ibea");
				//printAlgorithmInfo("MOCell", "jmetal.metaheuristics.mocell");
				//printAlgorithmInfo("MOEAD", "jmetal.metaheuristics.moead");
				//printAlgorithmInfo("NSGAII", "jmetal.metaheuristics.nsgaII");
				//printAlgorithmInfo("OMOPSO", "jmetal.metaheuristics.omopso");
				//printAlgorithmInfo("PAES", "jmetal.metaheuristics.paes");
				//printAlgorithmInfo("SMPSO", "jmetal.metaheuristics.smpso");
				//printAlgorithmInfo("SPEA2", "jmetal.metaheuristics.spea2");
				//printAlgorithmInfo("SMPSO", "jmetal.metaheuristics.smpso");
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
			//        try {
			//            //(new PrintAlgorithmsInfo()).printAlgorithmInfo();
			//            Algorithm alg = new PrintAlgorithmsInfo().getAlgorithm("NSGAII", Configuration.getSettings(), new ZDT1("Real"));
			//            Operator selection = new jmetal.base.operator.selection.BinaryTournament2();
			//            alg.addOperator("selection", selection);
			//            alg.execute().printObjectivesToFile("FUNsiona");
			//        } catch (JMException ex) {
			//            Logger.getLogger(PrintAlgorithmsInfo.class.getName()).log(Level.SEVERE, null, ex);
			//        } catch (ClassNotFoundException ex) {
			//            Logger.getLogger(PrintAlgorithmsInfo.class.getName()).log(Level.SEVERE, null, ex);
			//        }
			(new PrintAlgorithmsInfo()).printAlgorithmInfo();
		}
	} // PrintAlgorithmInfo
}
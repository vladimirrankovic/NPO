/// <summary> PropUtils.java
/// 
/// </summary>
/// <author>  Francisco Chicano
/// </author>
/// <version>  1.0
/// 
/// This class provides some utilities for working with properties.
/// Thanks to Francisco Chicano.
/// </version>
using System;
namespace SharpMetal
{
	
	public abstract class PropUtils:System.Object
	{
		
		public const char LABEL_RIGHT_DELIMITER = '>';
		public const char LABEL_LEFT_DELIMITER = '<';
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        static public System.Collections.Generic.Dictionary<string, string> getPropertiesWithPrefix(System.Collections.Generic.Dictionary<string, string> pro, System.String prefix)
		{
			System.Collections.IEnumerator en;
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
            System.Collections.Generic.Dictionary<string, string> aux = new System.Collections.Generic.Dictionary<string, string>();
			
			en = pro.Keys.GetEnumerator();
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (; en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				System.String nom = (System.String) en.Current;
				
				if (nom.StartsWith(prefix))
				{
					
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					aux[nom.Substring(prefix.Length)] = pro.Get(nom);
				}
			}
			
			return aux;
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        static public System.Collections.Generic.Dictionary<string, string> putPrefixToProperties(string prefix, System.Collections.Generic.Dictionary<string, string> pro)
		{
			System.Collections.IEnumerator en;
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
            System.Collections.Generic.Dictionary<string, string> res = new System.Collections.Generic.Dictionary<string, string>();
			
			en = pro.Keys.GetEnumerator();
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (; en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				System.String nom = (System.String) en.Current;
				
				//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
				res[prefix + nom] = pro.Get(nom);
			}
			
			return res;
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"

        static public System.Collections.Generic.Dictionary<string, string> substituteLabels(System.Collections.Generic.Dictionary<string, string> b, System.Collections.Generic.Dictionary<string, string> labels)
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
			System.Collections.Generic.Dictionary <string, string> res = new System.Collections.Generic.Dictionary<string, string>();
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            System.Collections.Generic.Dictionary<string, string> aux;
			System.Collections.IEnumerator en;
			string key;
			string val;
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (en = b.Keys.GetEnumerator(); en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				key = ((System.String) en.Current);
				
				val = b.Get(key);
				
				val.Trim();
				
				if (isLabel(val))
				{
					/*
					if (labels.getProperty(value) != null)
					{
					res.setProperty (key, labels.getProperty (value));
					}
					*/
					aux = getPropertiesWithPrefix(labels, val);
					aux = putPrefixToProperties(key, aux);
					
					SharpMetal.support.MapSupport.PutAll(res, aux);
				}
				else
				{
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					res[key] = val;
				}
			}
			
			return res;
		}
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        static public System.Collections.Generic.Dictionary<string, string> dereferenceProperties(System.Collections.Generic.Dictionary<string, string> pro)
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			//UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
            System.Collections.Generic.Dictionary<string, string> res = new System.Collections.Generic.Dictionary<string, string>();
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            System.Collections.Generic.Dictionary<string, string> aux;
			System.Collections.IEnumerator en;
			System.String key;
			System.String value_Renamed;
			
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (en = pro.Keys.GetEnumerator(); en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				key = ((string) en.Current);
				
				value_Renamed = pro.Get(key);
				
				value_Renamed.Trim();
				
				if (isLabel(value_Renamed))
				{
					/*
					if (labels.getProperty(value) != null)
					{
					res.setProperty (key, labels.getProperty (value));
					}
					*/
					
					System.String lab = value_Renamed.Substring(1, (value_Renamed.Length - 1) - (1));
					
					aux = getPropertiesWithPrefix(pro, lab);
					
					if ((aux.Count == 0))
					{
						//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
						res[key] = value_Renamed;
					}
					else
					{
						aux = putPrefixToProperties(key, aux);
					}
					
					SharpMetal.support.MapSupport.PutAll(res, aux);
				}
				else
				{
					//UPGRADE_TODO: Method 'java.util.Properties.setProperty' was converted to 'System.Collections.Specialized.NameValueCollection.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiessetProperty_javalangString_javalangString'"
					res[key] = value_Renamed;
				}
			}
			
			return res;
		}
		
		static public bool isLabel(System.String str)
		{
			return (str.IndexOf((System.Char) LABEL_LEFT_DELIMITER) == 0 && str.IndexOf((System.Char) LABEL_RIGHT_DELIMITER) == str.Length - 1);
		}
		
        // TODO
		[STAThread]
        //static public void  Main(System.String[] argv)
        //{
        //    //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //    //UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
        //    System.Collections.Generic.Dictionary<string, string> b = new System.Collections.Generic.Dictionary<string, string>();
        //    //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //    //UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
        //    System.Collections.Generic.Dictionary<string, string> delta = new System.Collections.Generic.Dictionary<string, string>();
			
        //    //UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
        //    System.IO.Stream isbase = new System.IO.FileStream(argv[0], System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //    //InputStream isdelta = new FileInputStream (argv[1]);
        //    b.
        //    //UPGRADE_TODO: Method 'java.util.Properties.load' was converted to 'System.Collections.Specialized.NameValueCollection' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiesload_javaioInputStream'"
        //    b = new System.Collections.IDictionary(System.Configuration.ConfigurationSettings.AppSettings);
        //    //delta.load(isdelta);
			
        //    //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //    System.Collections.Generic.Dictionary<string, string> res = dereferenceProperties(b);
        //}
		
		
		/// <param name="file">The file containing the properties
		/// </param>
		/// <returns> A <code>Properties</code> object
		/// </returns>
		/// <throws>  java.io.FileNotFoundException </throws>
		/// <throws>  java.io.IOException </throws>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //static public System.Collections.IDictionary load(System.String file)
        //{
        //    //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        //    //UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
        //    System.Collections.IDictionary properties = new System.Collections.IDictionary();
        //    //UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
        //    System.IO.FileStream in_Renamed = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //    //UPGRADE_TODO: Method 'java.util.Properties.load' was converted to 'System.Collections.Specialized.NameValueCollection' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiesload_javaioInputStream'"
        //    properties = new System.Collections.IDictionary(System.Configuration.ConfigurationSettings.AppSettings);
        //    in_Renamed.Close();
        //    return properties;
        //} // load
		
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        static public System.Collections.Generic.Dictionary<string, string> setDefaultParameters(System.Collections.Generic.Dictionary<string, string> properties, string algorithmName)
		{
			
			// Parameters and Results are duplicated because of a Concurrent Modification Exception
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            System.Collections.Generic.Dictionary<string, string> parameters = PropUtils.getPropertiesWithPrefix(properties, algorithmName + ".DEFAULT");
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            System.Collections.Generic.Dictionary<string, string> results = PropUtils.getPropertiesWithPrefix(properties, algorithmName + ".DEFAULT");
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			
			while (iterator.hasNext())
			{
				System.String parameter = parameters.Get((System.String) iterator.next());
				
				//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
                System.Collections.Generic.Dictionary<string, string> subParameters = PropUtils.getPropertiesWithPrefix(properties, parameter + ".DEFAULT");
				
				if (subParameters != null)
				{
					PropUtils.putPrefixToProperties(parameter, subParameters);
					SharpMetal.support.MapSupport.PutAll(results, PropUtils.putPrefixToProperties(parameter + ".", subParameters));
				}
			}
			return results;
		} //
		
		
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
        static public System.Collections.IDictionary setDefaultParameters2(System.Collections.Generic.Dictionary<string, string> properties, string algorithmName)
		{
			
			// Parameters and Results are duplicated because of a Concurrent Modification Exception
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.Collections.IDictionary parameters = PropUtils.getPropertiesWithPrefix(properties, algorithmName);
			
			return parameters;
		} //
	}
}
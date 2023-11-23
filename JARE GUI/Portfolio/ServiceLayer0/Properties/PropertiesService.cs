using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace POUI.ServiceLayer.Properties
{
    public class PropertiesService
    {
        public static Properties Properties { get; set; }

        public static Dictionary<string, string> PropertiesFromFile(string fileName)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            StreamReader reader = new StreamReader(fileName);
            string line = string.Empty;
            while((line = reader.ReadLine()) != null)
            {
                string[] words = line.Split('=');
                properties.Add(words[0], words[1]);
            }
            reader.Close();

            return properties;
        }

        public static void PropertiesToFile(Dictionary<string, string> properties, string fileName)
        {
            StreamWriter writer = new StreamWriter(fileName);
            foreach(KeyValuePair<string, string> kvp in properties)
            {
                writer.WriteLine(kvp.Key + "=" + kvp.Value);
            }

            writer.Close();
        }
    }
}

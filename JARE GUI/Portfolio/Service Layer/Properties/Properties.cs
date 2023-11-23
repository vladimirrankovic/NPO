using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioGUI.ServiceLayer.Properties
{
    public class Properties
    {
        private Dictionary<string, string> _dictionary;

        public Properties()
        {
            _dictionary = new Dictionary<string, string>();
        }

        public Properties(int capacity)
        {
            _dictionary = new Dictionary<string, string>(capacity);
        }

        public Properties(Dictionary<string, string> properties)
        {
            _dictionary = properties;
        }

        public void AddProperty(string key, string value)
        {
            _dictionary.Add(key, value);
        }   
    }
}

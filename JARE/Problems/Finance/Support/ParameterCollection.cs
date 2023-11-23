using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.problems.Finance;

namespace JARE.problems.Finance.Parameters
{
    public class ParameterCollection
    {
        protected internal Dictionary<string, Object> m_parameters;

        public ParameterCollection()
        {
            m_parameters = new Dictionary<string, object>();
        }

        public ParameterCollection(Dictionary<string, Object> parameters)
        {
            m_parameters = new Dictionary<string, object>(parameters);
        }

        /// <summary> Sets a new <code>Object</code> parameter to the operator.</summary>
        /// <param name="name">The parameter name.
        /// </param>
        /// <param name="value">Object representing the parameter.
        /// </param>
        public virtual void setParameter(string name, System.Object value)
        {
            m_parameters.Add(name.ToUpper(), value);
        }
        public virtual void setParameter(string name, System.Object value, bool replaceExisting = false)
        {
            if (ContainsParameter(name) && replaceExisting == true)
            {
                m_parameters.Remove(name.ToUpper());
            }
            setParameter(name, value);
        }
        public virtual System.Object getParameter(string name)
        {
            try
            {
                return m_parameters[name.ToUpper()];
            }
            catch (KeyNotFoundException)
            {
                throw new Exception("Parameter "+name+" does not exist!");
                //return null;
            }
        }
        public virtual bool ContainsParameter(string name)
        {
            return m_parameters.ContainsKey(name.ToUpper());
        }
        public virtual void CopyParametersTo(ref Dictionary<string, Object> parameters)
        {
            parameters = new Dictionary<string, object>(m_parameters);
        }
        public virtual void CopyParametersFrom(Dictionary<string, Object> parameters)
        {
            m_parameters = new Dictionary<string, object>(parameters);
        }
        public void Clear()
        {
            m_parameters.Clear();
        }

    }
   
}

/// <summary> Operator.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using SMException = JARE.util.SMException;
using System.Collections;
using System.Collections.Generic;
namespace JARE.Base
{
	
	/// <summary> Class representing an operator</summary>
	[Serializable]
	public abstract class Operator
	{
		
		/// <summary> Stores the current operator parameters. 
		/// It is defined as a Map of pairs <<code>String</code>, <code>Object</code>>, 
		/// and it allow objects to be accessed by their names, which  are specified 
		/// by the string.
		/// </summary>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		//protected Map < String, Object > parameters_;

        protected internal Dictionary <string, Object> m_parameters;
		
		/// <summary> Constructor.</summary>
		public Operator()
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			
            //parameters = new HashMap < String, Object >();

            m_parameters = new Dictionary<string, Object>();

		} // Operator
		
		/// <summary> Abstract method that must be defined by all the operators. When invoked, 
		/// this method executes the operator represented by the current object.
		/// </summary>
		/// <param name="object"> This param inherits from Object to allow different kinds 
		/// of parameters for each operator. For example, a selection 
		/// operator typically receives a <code>SolutionSet</code> as 
		/// a parameter, while a mutation operator receives a 
		/// <code>Solution</code>.
		/// </param>
		/// <returns> An object reference. The returned value depends on the operator. 
		/// </returns>
		abstract public System.Object execute(System.Object obj);
		
		
		/// <summary> Sets a new <code>Object</code> parameter to the operator.</summary>
		/// <param name="name">The parameter name.
		/// </param>
		/// <param name="value">Object representing the parameter.
		/// </param>
		public virtual void setParameter(string name, System.Object value)
		{
            m_parameters.Add(name.ToUpper(), value);
		}

        /// <summary> Returns an object representing a parameter of the <code>Operator</code></summary>
        /// <param name="name">The parameter name.
        /// </param>
        /// <returns> the parameter.
        /// </returns>
        public virtual System.Object getParameter(string name)
        {
            try
            {
                return m_parameters[name.ToUpper()];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("There is no such parameter");
                return null;
            }
        }

        /// <summary> Copies parameters of the <code>Operator</code></summary>
        /// <param name="name"> Dictionary.
        /// </param>
        /// <returns>
        /// </returns>
        /// 
        public virtual void CopyParameters(Operator other)
        {
            Dictionary<string, Object>.Enumerator Enumerator = m_parameters.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                KeyValuePair<string, Object> cur = Enumerator.Current;
                other.setParameter(cur.Key, cur.Value);
            }
        } 
	} 
}
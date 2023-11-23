/// <summary> ArrayReal
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using Problem = JARE.Base.Problem;
using Variable = JARE.Base.Variable;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.variable
{
	
	
	/// <summary> Implements a decision variable representing an array of real values.
	/// The real values of the array have their own bounds.
	/// </summary>
	[Serializable]
	public class ArrayReal :Variable
	{
		/// <summary> Returns the length of the arrayReal.</summary>
		/// <returns> The length
		/// </returns>
		virtual public int Length
		{
			get
			{
				return m_size;
			}
			// getLength
			
		}
		/// <summary> Problem using the type</summary>
		internal Problem m_problem;
		
		/// <summary> Stores an array of integer values</summary>
		public System.Double[] m_array;
		
		/// <summary> Stores the length of the array</summary>
		public int m_size;
		
		/// <summary> Constructor</summary>
		public ArrayReal()
		{
			m_problem = null;
			m_size = 0;
			m_array = null;
		} // Constructor
		
		/// <summary> Constructor</summary>
		/// <param name="size">Size of the array
		/// </param>
		public ArrayReal(int size, Problem problem)
		{
			m_problem = problem;
			m_size = size;
			m_array = new System.Double[m_size];
			
			for (int i = 0; i < m_size; i++)
			{
				m_array[i] = PseudoRandom.randDouble() * (m_problem.getUpperLimit(i) - m_problem.getLowerLimit(i)) + m_problem.getLowerLimit(i);
			} // for
		} // Constructor
		
		/// <summary> Copy Constructor</summary>
		/// <param name="arrayInt">The arrayDouble to copy
		/// </param>
		public ArrayReal(ArrayReal arrayReal)
		{
			m_problem = arrayReal.m_problem;
			m_size = arrayReal.m_size;
			m_array = new System.Double[m_size];
			
			for (int i = 0; i < m_size; i++)
			{
				m_array[i] = arrayReal.m_array[i];
			} 
		} 
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"

        public override Variable deepCopy()
		{
			return new ArrayReal(this);
		} 
		
		/// <summary> getValue</summary>
		/// <param name="index">Index of value to be returned
		/// </param>
		/// <returns> the value in position index
		/// </returns>
        /// 

		public virtual double getValue(int index)
		{
			if ((index >= 0) && (index < m_size))
				return m_array[index];
			else
			{
				Configuration.m_logger.WriteLog("JARE.Base.variable.ArrayReal.getValue: index value (" + index + ") invalid");
				throw new SMException("JARE.Base.variable.ArrayReal: index value (" + index + ") invalid");
			} // if
		} // getValue
		
		/// <summary> setValue</summary>
		/// <param name="index">Index of value to be returned
		/// </param>
		/// <param name="value">The value to be set in position index
		/// </param>
		public virtual void  setValue(int index, double val)
		{
			if ((index >= 0) && (index < m_size))
				m_array[index] = val;
			else
			{
				Configuration.m_logger.WriteLog("JARE.Base.variable.ArrayReal.setValue: index value (" + index + ") invalid");
				throw new SMException("JARE.Base.variable.ArrayReal: index value (" + index + ") invalid");
			} // else
		} // setValue
		
		/// <summary> Get the lower bound of a value</summary>
		/// <param name="index">The index of the value
		/// </param>
		/// <returns> the lower bound
		/// </returns>
		public virtual double getLowerBound(int index)
		{
			if ((index >= 0) && (index < m_size))
				return m_problem.getLowerLimit(index);
			else
			{
				Configuration.m_logger.WriteLog("JARE.Base.variable.ArrayReal.getLowerBound: index value (" + index + ") invalid");
				throw new SMException("JARE.Base.variable.getLowerBound: index value (" + index + ") invalid");
			} // else	
		} // getLowerBound
		
		/// <summary> Get the upper bound of a value</summary>
		/// <param name="index">The index of the value
		/// </param>
		/// <returns> the upper bound
		/// </returns>
		public virtual double getUpperBound(int index)
		{
			if ((index >= 0) && (index < m_size))
				return m_problem.getUpperLimit(index);
			else
			{
				Configuration.m_logger.WriteLog("JARE.Base.variable.ArrayReal.getUpperBound: index value (" + index + ") invalid");
				throw new SMException("JARE.Base.variable.getUpperBound: index value (" + index + ") invalid");
			} // else
		} // getLowerBound
		
		/// <summary> Returns a string representing the object</summary>
		/// <returns> The string
		/// </returns>
		public override string ToString()
		{
			string str;
			
			str = "";
			for (int i = 0; i < (m_size - 1); i++)
				str += (m_array[i] + " ");
			
			str += m_array[m_size - 1];
			return str;
		} // toString  
	} // ArrayReal
}
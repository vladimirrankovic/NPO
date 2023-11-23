/// <summary> ArrayInt
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using Variable = JARE.Base.Variable;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.variable
{
	
	
	/// <summary> Implements a decision variable representing an array of integers.
	/// The integer values of the array have their own bounds.
	/// </summary>
	[Serializable]
	public class ArrayInt:Variable
	{
		/// <summary> Returns the length of the arrayInt.</summary>
		/// <returns> The length
		/// </returns>
		virtual public int Length
		{
			get
			{
				return m_size;
			}
		}
		
		/// <summary> Stores an array of integer values</summary>
		public int[] m_array;
		
		/// <summary> Stores the length of the array</summary>
		public int m_size;
		
		/// <summary> Store the lower and upper bounds of each int value of the array in case of
		/// having each one different limits
		/// </summary>
		private int[] m_lowerBounds;
		private int[] m_upperBounds;
		
		/// <summary> Constructor</summary>
		public ArrayInt()
		{
			m_lowerBounds = null;
			m_upperBounds = null;
			m_size = 0;
			m_array = null;
		} 

		/// <summary> Constructor</summary>
		/// <param name="size">Size of the array
		/// </param>
		public ArrayInt(int size)
		{
			m_size = size;
			m_array = new int[m_size];
			
			m_lowerBounds = new int[m_size];
			m_upperBounds = new int[m_size];
		} 
		
		/// <summary> Constructor </summary>
		/// <param name="size">The size of the array
		/// </param>
		/// <param name="lowerBound">Lower bounds
		/// </param>
		/// <param name="upperBound">Upper bounds
		/// </param>
		public ArrayInt(int size, double[] lowerBounds, double[] upperBounds)
		{
			m_size = size;
			m_array = new int[m_size];
			
			m_lowerBounds = new int[m_size];
			m_upperBounds = new int[m_size];
			
			for (int i = 0; i < m_size; i++)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m_lowerBounds[i] = (int) lowerBounds[i];
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				m_upperBounds[i] = (int) upperBounds[i];
				m_array[i] = PseudoRandom.randInt(m_lowerBounds[i], m_upperBounds[i]);
			} // for
		} // Constructor

        //VISNJA
        public ArrayInt(int size, int[] lowerBounds, int[] upperBounds)
        {
            m_size = size;
            m_array = new int[m_size];

            m_lowerBounds = new int[m_size];
            m_upperBounds = new int[m_size];

            for (int i = 0; i < m_size; i++)
            {
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                m_lowerBounds[i] = lowerBounds[i];
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
               m_upperBounds[i] = upperBounds[i];
                m_array[i] = PseudoRandom.randInt(m_lowerBounds[i], m_upperBounds[i]);
            } // for
        } // Constructor
		
		/// <summary> Copy Constructor</summary>
		/// <param name="arrayInt">The arrayInt to copy
		/// </param>
		public ArrayInt(ArrayInt arrayInt)
		{
			m_size = arrayInt.m_size;
			m_array = new int[m_size];
			
			m_lowerBounds = new int[m_size];
			m_upperBounds = new int[m_size];
			
			for (int i = 0; i < m_size; i++)
			{
				m_array[i] = arrayInt.m_array[i];
				m_lowerBounds[i] = arrayInt.m_lowerBounds[i];
				m_upperBounds[i] = arrayInt.m_upperBounds[i];
			} // for
		} // Copy Constructor
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public override Variable deepCopy()
		{
			return new ArrayInt(this);
		} // deepCopy
		
		/// <summary> getValue</summary>
		/// <param name="index">Index of value to be returned
		/// </param>
		/// <returns> the value in position index
		/// </returns>
		public virtual int getValue(int index)
		{
			if ((index >= 0) && (index < m_size))
				return m_array[index];
			else
			{
				Configuration.m_logger.WriteLog("JARE.Base.variable.ArrayInt.getValue: index value (" + index + ") invalid");
				throw new SMException("JARE.Base.variable.ArrayInt: index value (" + index + ") invalid");
			} // if
		} // getValue
		
		/// <summary> setValue</summary>
		/// <param name="index">Index of value to be returned
		/// </param>
		/// <param name="value">The value to be set in position index
		/// </param>
		public virtual void  setValue(int index, int val)
		{
			if ((index >= 0) && (index < m_size))
				m_array[index] = val;
			else
			{
				Configuration.m_logger.WriteLog("JARE.Base.variable.ArrayInt.setValue: index value (" + index + ") invalid");
				throw new SMException("JARE.Base.variable.ArrayInt: index value (" + index + ") invalid");
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
				return m_lowerBounds[index];
			else
			{
				Configuration.m_logger.WriteLog("JARE.Base.variable.ArrayInt.getLowerBound: index value (" + index + ") invalid");
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
				return m_upperBounds[index];
			else
			{
				Configuration.m_logger.WriteLog("JARE.Base.variable.ArrayInt.getUpperBound: index value (" + index + ") invalid");
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
			for (int i = 0; i < m_size; i++)
				str += (m_array[i] + " ");
			
			return str;
		} // toString  
	} // ArrayInt
}
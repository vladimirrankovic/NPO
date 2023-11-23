/// <summary> Int.java</summary>
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
	
	/// <summary> This class implements an integer decision variable </summary>
	[Serializable]
	public class Int:Variable
	{
		private int m_value; //Stores the value of the variable
		private int m_lowerBound; //Stores the lower limit of the variable
		private int m_upperBound; //Stores the upper limit of the variable
		
		/// <summary> Constructor</summary>
		public Int()
		{
			m_lowerBound = System.Int32.MinValue;
			m_upperBound = System.Int32.MaxValue;
			m_value = 0;
		} 
		
		/// <summary> Constructor</summary>
		/// <param name="lowerBound">Variable lower bound
		/// </param>
		/// <param name="upperBound">Variable upper bound
		/// </param>
		public Int(int lowerBound, int upperBound)
		{
			m_lowerBound = lowerBound;
			m_upperBound = upperBound;
			m_value = PseudoRandom.randInt(lowerBound, upperBound);
		} 
		
		/// <summary> Constructor</summary>
		/// <param name="value">Value of the variable
		/// </param>
		/// <param name="lowerBound">Variable lower bound
		/// </param>
		/// <param name="upperBound">Variable upper bound
		/// </param>
		public Int(int val, int lowerBound, int upperBound):base()
		{
			
			m_value = val;
			m_lowerBound = lowerBound;
			m_upperBound = upperBound;
		}
		
		/// <summary> Copy constructor.</summary>
		/// <param name="variable">Variable to be copied.
		/// </param>
		/// <throws>  SMException  </throws>
		public Int(Variable variable)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m_lowerBound = (int) variable.getLowerBound();
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m_upperBound = (int) variable.getUpperBound();
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m_value = (int) variable.getValue();
		} 
		
		/// <summary> Returns the value of the variable.</summary>
		/// <returns> the value.
		/// </returns>
		public override double getValue()
		{
			return m_value;
		} 
		
		/// <summary> Assigns a value to the variable.</summary>
		/// <param name="value">The value.
		/// </param>
		public override void  setValue(double val)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m_value = (int) val;
		} 
		
		/// <summary> Creates an exact copy of the <code>Int</code> object.</summary>
		/// <returns> the copy.
		/// </returns>
		public override Variable deepCopy()
		{
			try
			{
				return new Int(this);
			}
			catch (SMException)
			{
				Configuration.m_logger.WriteLog("Int.deepCopy.execute: SMException");
				return null;
			}
		} 
		
		/// <summary> Returns the lower bound of the variable.</summary>
		/// <returns> the lower bound.
		/// </returns>
		public override double getLowerBound()
		{
			return m_lowerBound;
		} 
		
		/// <summary> Returns the upper bound of the variable.</summary>
		/// <returns> the upper bound.
		/// </returns>
		public override double getUpperBound()
		{
			return m_upperBound;
		} 
		
		/// <summary> Sets the lower bound of the variable.</summary>
		/// <param name="lowerBound">The lower bound value.
		/// </param>
		public override void  setLowerBound(double lowerBound)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m_lowerBound = (int) lowerBound;
		}
		
		/// <summary> Sets the upper bound of the variable.</summary>
		/// <param name="upperBound">The new upper bound value.
		/// </param>
		public override void  setUpperBound(double upperBound)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			m_upperBound = (int) upperBound;
		} 
		
		/// <summary> Returns a string representing the object</summary>
		/// <returns> The string
		/// </returns>
		public override System.String ToString()
		{
			return m_value + "";
		} 
	}
}
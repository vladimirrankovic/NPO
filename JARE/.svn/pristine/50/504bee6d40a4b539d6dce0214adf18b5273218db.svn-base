/// <summary> Real.java
/// 
/// </summary>
/// <author>  juanjo durillo 
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
	
	
	/// <summary> This class implements a Real value decision variable</summary>
	[Serializable]
	public class Real:Variable
	{
		
		/// <summary> Stores the value of the real variable</summary>
		private double m_value;
		
		/// <summary> Stores the lower bound of the real variable</summary>
		private double m_lowerBound;
		
		/// <summary> Stores the upper bound of the real variable</summary>
		private double m_upperBound;
		
		/// <summary> Constructor</summary>
		public Real()
		{
		}
		
		
		/// <summary> Constructor</summary>
		/// <param name="lowerBound">Lower limit for the variable
		/// </param>
		/// <param name="upperBound">Upper limit for the variable
		/// </param>
		public Real(double lowerBound, double upperBound)
		{
			m_lowerBound = lowerBound;
			m_upperBound = upperBound;
			m_value = PseudoRandom.randDouble() * (upperBound - lowerBound) + lowerBound;
		} 
		
		
		/// <summary> Copy constructor.</summary>
		/// <param name="variable">The variable to copy.
		/// </param>
		/// <throws>  SMException  </throws>
		public Real(Variable variable)
		{
			m_lowerBound = variable.getLowerBound();
			m_upperBound = variable.getUpperBound();
			m_value = variable.getValue();
		} 
		
		/// <summary> Gets the value of the <code>Real</code> variable.</summary>
		/// <returns> the value.
		/// </returns>
		public override double getValue()
		{
			return m_value;
		} 
		
		/// <summary> Sets the value of the variable.</summary>
		/// <param name="value">The value.
		/// </param>
		public override void  setValue(double val)
		{
			m_value = val;
		} 
		
		/// <summary> Returns a exact copy of the <code>Real</code> variable</summary>
		/// <returns> the copy
		/// </returns>
		public override Variable deepCopy()
		{
			try
			{
				return new Real(this);
			}
			catch (SMException)
			{
				Configuration.m_logger.WriteLog("Real.deepCopy.execute: SMException");
				return null;
			}
		} 
		
		
		/// <summary> Gets the lower bound of the variable.</summary>
		/// <returns> the lower bound.
		/// </returns>
		public override double getLowerBound()
		{
			return m_lowerBound;
		} //getLowerBound
		
		/// <summary> Gets the upper bound of the variable.</summary>
		/// <returns> the upper bound.
		/// </returns>
		public override double getUpperBound()
		{
			return m_upperBound;
		} // getUpperBound
		
		
		/// <summary> Sets the lower bound of the variable.</summary>
		/// <param name="lowerBound">The lower bound.
		/// </param>
		public override void  setLowerBound(double lowerBound)
		{
			m_lowerBound = lowerBound;
		} // setLowerBound
		
		/// <summary> Sets the upper bound of the variable.</summary>
		/// <param name="upperBound">The upper bound.
		/// </param>
		public override void  setUpperBound(double upperBound)
		{
			m_upperBound = upperBound;
		} // setUpperBound
		
		
		/// <summary> Returns a string representing the object</summary>
		/// <returns> the string
		/// </returns>
		public override System.String ToString()
		{
			return m_value + "";
		} //toString
	} // Real
}
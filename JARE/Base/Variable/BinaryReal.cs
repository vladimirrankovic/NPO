/// <summary> BinaryReal.java
/// 
/// </summary>
/// <author>  Juan J. durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Variable = JARE.Base.Variable;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.variable
{
	
	/// <summary>This class extends the Binary class to represent a Real variable encoded by
	/// a binary string
	/// </summary>
	[Serializable]
	public class BinaryReal:Binary
	{
		
		/// <summary> Defines the default number of bits used for binary coded variables.</summary>
		public const int DEFAULT_PRECISION = 30;
		
		/// <summary> Stores the real value of the variable</summary>
		private double m_value;
		
		/// <summary> Stores the lower limit for the variable</summary>
		private double m_lowerBound;
		
		/// <summary> Stores the upper limit for the variable</summary>
		private double m_upperBound;
		
		/// <summary> Constructor.</summary>
		public BinaryReal():base()
		{
		} //BinaryReal
		
		/// <summary> Constructor</summary>
		/// <param name="numberOfBits">Length of the binary string.
		/// </param>
		/// <param name="lowerBound">The lower limit for the variable
		/// </param>
		/// <param name="upperBound">The upper limit for the variable.
		/// </param>
		public BinaryReal(int numberOfBits, double lowerBound, double upperBound):base(numberOfBits)
		{
			m_lowerBound = lowerBound;
			m_upperBound = upperBound;
			
			decode();
		} //BinaryReal
		
		/// <summary> Copy constructor</summary>
		/// <param name="variable">The variable to copy
		/// </param>
		public BinaryReal(BinaryReal variable):base(variable)
		{
			
			m_lowerBound = variable.m_lowerBound;
			m_upperBound = variable.m_upperBound;
			/*
			numberOfBits_ = variable.numberOfBits_;
			
			m_bits = new BitSet(numberOfBits_);
			for (int i = 0; i < numberOfBits_; i++)
			m_bits.set(i,variable.m_bits.get(i));
			*/
			m_value = variable.m_value;
		} //BinaryReal
		
		
		/// <summary> Decodes the real value encoded in the binary string represented
		/// by the <code>BinaryReal</code> object. The decoded value is stores in the 
		/// <code>value_</code> field and can be accessed by the method
		/// <code>getValue</code>.
		/// </summary>
		public override void decode()
		{
			double val = 0.0;
			for (int i = 0; i < m_numberOfBits ; i++)
			{
				if (m_bits.Get(i))
				{
					val += System.Math.Pow(2.0, i);
				}
			}
			
			m_value = val * (m_upperBound - m_lowerBound) / (System.Math.Pow(2.0, m_numberOfBits) - 1.0);
			m_value += m_lowerBound;
		} //decode
		
		/// <summary> Returns the double value of the variable.</summary>
		/// <returns> the double value.
		/// </returns>
		public override double getValue()
		{
			return m_value;
		} //getValue
		
		/// <summary> Creates an exact copy of a <code>BinaryReal</code> object.</summary>
		/// <returns> The copy of the object
		/// </returns>
		public override Variable deepCopy()
		{
			return new BinaryReal(this);
		} //deepCopy
		
		/// <summary> Returns the lower bound of the variable.</summary>
		/// <returns> the lower bound.
		/// </returns>
		public override double getLowerBound()
		{
			return m_lowerBound;
		} // getLowerBound
		
		/// <summary> Returns the upper bound of the variable.</summary>
		/// <returns> the upper bound.
		/// </returns>
		public override double getUpperBound()
		{
			return m_upperBound;
		} // getUpperBound
		
		/// <summary> Sets the lower bound of the variable.</summary>
		/// <param name="lowerBound">the lower bound.
		/// </param>
		public override void  setLowerBound(double lowerBound)
		{
			m_lowerBound = lowerBound;
		} // setLowerBound
		
		/// <summary> Sets the upper bound of the variable.</summary>
		/// <param name="upperBound">the upper bound.
		/// </param>
		public override void  setUpperBound(double upperBound)
		{
			m_upperBound = upperBound;
		} // setUpperBound
		
		/// <summary> Returns a string representing the object.</summary>
		/// <returns> the string.
		/// </returns>
		public override System.String ToString()
		{
			return m_value + "";
		} // toString
	}
}
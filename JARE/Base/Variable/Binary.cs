/// <summary> Binary.java
/// 
/// </summary>
/// <author>  Juanjo Durillo
/// </author>
/// <version>  1.0
/// 
/// </version>
using System;
using Variable = JARE.Base.Variable;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.variable
{
	
	/// <summary> This class implements a generic binary string variable.It can be used as
	/// a base class other binary string based classes (e.g., binary coded integer
	/// or real variables).
	/// </summary>
	[Serializable]
	public class Binary:Variable
	{
		/// <summary> Returns the length of the binary string.</summary>
		/// <returns> The length
		/// </returns>
		virtual public int NumberOfBits
		{
			get
			{
				return m_numberOfBits;
			}
		}
		
		/// <summary> Stores the bits constituting the binary string. It is
		/// implemented using a BitSet object
		/// </summary>
		public System.Collections.BitArray m_bits;
		
		/// <summary> Store the length of the binary string</summary>
		protected internal int m_numberOfBits;
		
		/// <summary> Default constructor.</summary>
		public Binary()
		{
		} 
		
		
		/// <summary>  Constructor</summary>
		/// <param name="numberOfBits">Length of the bit string
		/// </param>
		public Binary(int numberOfBits)
		{
			m_numberOfBits = numberOfBits;
			
			m_bits = new System.Collections.BitArray((m_numberOfBits % 64 == 0?m_numberOfBits / 64:m_numberOfBits / 64 + 1) * 64);
			for (int i = 0; i < m_numberOfBits; i++)
			{
				if (PseudoRandom.randDouble() < 0.5)
				{
                    m_bits.Set(i, true);
				}
				else
				{
					m_bits.Set(i, false);
				}
			}
		} 
		
		/// <summary> Copy constructor.</summary>
		/// <param name="variable">The Binary variable to copy.
		/// </param>
		public Binary(Binary variable)
		{
			m_numberOfBits = variable.m_numberOfBits;
			
			m_bits = new System.Collections.BitArray((m_numberOfBits % 64 == 0?m_numberOfBits / 64:m_numberOfBits / 64 + 1) * 64);
			for (int i = 0; i < m_numberOfBits; i++)
			{
				m_bits.Set(i, variable.m_bits.Get(i));
			}
		} 
		
		/// <summary> This method is intended to be used in subclass of <code>Binary</code>, 
		/// for examples the classes, <code>BinaryReal</code> and <code>BinaryInt<codes>. 
		/// In this classes, the method allows to decode the 
		/// value enconded in the binary string. As generic variables do not encode any
		/// value, this method do noting 
		/// </summary>
		public virtual void  decode()
		{
			
		} 
		
		/// <summary> Creates an exact copy of a Binary object</summary>
		/// <returns> An exact copy of the object.
		/// 
		/// </returns>
		public override Variable deepCopy()
		{
			return new Binary(this);
		}

        /// <summary> Returns the value of the ith bit.</summary>
        /// <param name="bit">The bit to retrieve
        /// </param>
        /// <returns> The ith bit
        /// </returns>
        public virtual bool getIth(int bit)
        {
            return m_bits.Get(bit);
        } //getNumberOfBits

        /// <summary> Returns the number of bits set to one.</summary>
        /// <returns> The number of bits set to one
        /// </returns>
        public int cardinality()
        {
            int cardinality = 0;
            int i = 0;
            while (i < m_bits.Count)
            {
                if (m_bits.Get(i))
                {
                    cardinality++;
                }
                i++;
            }
            return cardinality;
        } //cardinality
		
		/// <summary> Sets the value of the ith bit.</summary>
		/// <param name="bit">The bit to set
		/// </param>
		public virtual void  setIth(int bit, bool val)
		{
			m_bits.Set(bit, val);
		} //getNumberOfBits
		
		
		/// <summary> Obtain the hamming distance between two binary strings</summary>
		/// <param name="other">The binary string to compare
		/// </param>
		/// <returns> The hamming distance
		/// </returns>
		public virtual int hammingDistance(Binary other)
		{
			int distance = 0;
			int i = 0;
			while (i < m_bits.Count)
			{
				if (m_bits.Get(i) != other.m_bits.Get(i))
				{
					distance++;
				}
				i++;
			}
			return distance;
		} // hammingDistance
		
		/// <summary>  </summary>
		public override string ToString()
		{
			string result;
			
			result = "";
			for (int i = 0; i < m_numberOfBits; i++)
				if (m_bits.Get(i))
					result = result + "1";
				else
					result = result + "0";
			
			return result;
		} // toString
	} // Binary
}
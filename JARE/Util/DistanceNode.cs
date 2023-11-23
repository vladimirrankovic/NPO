/// <summary> DistanceNode.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.util
{
	
	
	/// <summary> This is an auxiliar class for calculating the SPEA2 environmental selection.
	/// Each instance of DistanceNode contains two parameter called
	/// <code>reference_</code> and <code>m_distance</code>.
	/// <code>reference_</code> indicates one <code>Solution</code> in a
	/// <code>SolutionSet</code> and <code>m_distance</code> represents the m_distance
	/// to this solution.
	/// </summary>
	public class DistanceNode
	{
		/// <summary> Gets the distance</summary>
		/// <returns> the distance
		/// </returns>
		/// <summary> Sets the distance to a <code>Solution</code></summary>
		/// <param name="distance">The distance
		/// </param>
		virtual public double Distance
		{
			get
			{
				return m_distance;
			}
			// getDistance
			
			set
			{
				m_distance = value;
			}
			// setDistance
			
		}
		/// <summary> Sets the reference to a <code>Solution</code></summary>
		/// <param name="reference">The reference
		/// </param>
		
		/// <summary> Gets the reference</summary>
		/// <returns> the reference
		/// </returns>
		virtual public int Reference
		{
			get
			{
				return m_reference;
			}
            set
			{
				m_reference = value;
			}
		}
		
		/// <summary> Indicates the position of a <code>Solution</code> in a 
		/// <code>SolutionSet</code>.
		/// </summary>
		private int m_reference;
		
		/// <summary> Indicates the distance to the <code>Solution</code> represented by 
		/// <code>reference_</code>.
		/// </summary>
		private double m_distance;
		
		/// <summary> Constructor.</summary>
		/// <param name="distance">The distance to a <code>Solution</code>.
		/// </param>
		/// <param name="reference">The position of the <code>Solution</code>.
		/// </param>
		public DistanceNode(double distance, int reference)
		{
			m_distance = distance;
			m_reference = reference;
		} // DistanceNode
	} // DistanceNode
}
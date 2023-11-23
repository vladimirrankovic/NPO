/// <summary> Permutation.java
/// 
/// </summary>
/// <author>  juanjo durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using System.Collections.Generic;
using System.Collections;
using Variable = JARE.Base.Variable;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.variable
{
	
	/// <summary> Implements a permutation of integer decision variable</summary>
	[Serializable]
	public class Permutation:Variable
	{
		/// <summary> Returns the length of the permutation.</summary>
		/// <returns> The length
		/// </returns>
		virtual public int Length
		{
			get
			{
				return m_size;
			}
			//getNumberOfBits
			
		}
		
		/// <summary> Stores a permutation of <code>int</code> values</summary>
		public int[] m_vector;
		
		/// <summary> Stores the length of the permutation</summary>
		public int m_size;
		
		/// <summary> Constructor</summary>
		public Permutation()
		{
			m_size = 0;
			m_vector = null;
		} //Permutation
		
		/// <summary> Constructor</summary>
		/// <param name="size">Length of the permutation
		/// </param>
		/*
		public Permutation(int size) {
		setVariableType(m_variableType.Permutation) ;
		
		size_   = size;
		m_vector = new int[size_];
		
		int [] randomSequence = new int[size_];
		
		for(int k = 0; k < size_; k++){
		int num           = PseudoRandom.randInt();
		randomSequence[k] = num;
		m_vector[k]        = k;
		} 
		
		// sort value and store index as fragment order
		for(int i = 0; i < size_-1; i++){
		for(int j = i+1; j < size_; j++) {
		if(randomSequence[i] > randomSequence[j]){
		int temp          = randomSequence[i];
		randomSequence[i] = randomSequence[j];
		randomSequence[j] = temp;
		
		temp       = m_vector[i];
		m_vector[i] = m_vector[j];
		m_vector[j] = temp;
		}
		}
		}
		} //Permutation
		* */
		
		/// <summary> Constructor</summary>
		/// <param name="size">Length of the permutation
		/// This constructor has been contributed by Madan Sathe
		/// </param>
		public Permutation(int size)
		{
			m_size = size;
			m_vector = new int[m_size];
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			
            List < int > randomSequence = new List < int > (m_size);
			
			for (int i = 0; i < m_size; i++)
				randomSequence.Add(i);

            JARE.support.CollectionsSupport.Shuffle(randomSequence);

            for (int j = 0; j < randomSequence.Count; j++)
                m_vector[j] = randomSequence[j]; 
		} // Constructor
		
		
		/// <summary> Copy Constructor</summary>
		/// <param name="permutation">The permutation to copy
		/// </param>
		public Permutation(Permutation permutation)
		{
			m_size = permutation.m_size;
			m_vector = new int[m_size];
			
			for (int i = 0; i < m_size; i++)
			{
				m_vector[i] = permutation.m_vector[i];
			}
		} //Permutation
		
		
		/// <summary> Create an exact copy of the <code>Permutation</code> object.</summary>
		/// <returns> An exact copy of the object.
		/// </returns>
		public override Variable deepCopy()
		{
			return new Permutation(this);
		} //deepCopy
		
		/// <summary> Returns a string representing the object</summary>
		/// <returns> The string
		/// </returns>
		public override System.String ToString()
		{
			string str;
			
			str = "";
			for (int i = 0; i < m_size; i++)
				str += (m_vector[i] + " ");
			
			return str;
		} // toString  

        //private static Random random = new Random();

        //public void ShuffleListInt(IList<int> list)
        //{
        //    if (list.Count > 1)
        //    {
        //        for (int i = list.Count - 1; i >= 0; i--)
        //        {
        //            int tmp = list[i];
        //            int randomIndex = random.Next(i + 1);

        //            //Swap elements
        //            list[i] = list[randomIndex];
        //            list[randomIndex] = tmp;
        //        }
        //    }
        //}

	}
}
/// <summary> LocalSearch.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Operator = JARE.Base.operators;
namespace JARE.Base.operators.localSearch
{
	
	
	/// <summary> Abstract class representing a generic local search operator</summary>
	[Serializable]
	public abstract class LocalSearch:Operator
	{
		/// <summary> Returns the number of evaluations made by the local search operator</summary>
		public abstract int Evaluations{get;}
	} // LocalSearch
}
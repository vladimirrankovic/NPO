/// <summary> SMException.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
namespace JARE.util
{
	
	
	/// <summary> JARE exception class</summary>
	[Serializable]
	public class SMException: System.Exception
	{
		
		/// <summary> Constructor</summary>
		/// <param name="Error">message
		/// </param>
		public SMException(string message):base(message)
		{
		} // JmetalException
	}
}
using System;

namespace BinderClientCSharp
{
	public class ParseException : System.Exception
	{
		/// <summary>
		/// Construct an exception with an error message.
		/// </summary>
		/// <param name="message">A descriptive message for the exception</param>
		public ParseException( string message ) : base( message )
		{
		}
	}
}


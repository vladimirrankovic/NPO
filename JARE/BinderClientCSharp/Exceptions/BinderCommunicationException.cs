using System;

namespace BinderClientCSharp
{
	public class BinderCommunicationException : Exception
	{
		public BinderCommunicationException ()
		{

		}

		public BinderCommunicationException (string message)
			: base(message)
		{

		}

		public BinderCommunicationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}


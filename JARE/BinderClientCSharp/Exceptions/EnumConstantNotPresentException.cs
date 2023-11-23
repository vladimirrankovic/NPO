using System;

namespace BinderClientCSharp
{
	public class EnumConstantNotPresentException : Exception
	{
		public EnumConstantNotPresentException ()
		{

		}

		public EnumConstantNotPresentException (string message)
			: base(message)
		{

		}

		public EnumConstantNotPresentException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}


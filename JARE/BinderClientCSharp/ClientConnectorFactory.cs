using System;

namespace BinderClientCSharp
{
	public class ClientConnectorFactory
	{

		//not needed currently - Djordje Krecar - March 2015
		//public static ClientConnector createClientConnector() /*throws BinderCommunicationException*/ {
		//	return createClientConnector(System.getProperties());
		//}


		public static IClientConnector createClientConnector(JavaProperties prop) /*throws BinderCommunicationException*/ {

		//not needed currently - Djordje Krecar - March 2015
		//	if (prop.getProperty("UseMPI", "no").equalsIgnoreCase("yes"))
		//		return new MPIClientConnector(prop);
		//	else
				return new ClientConnectorImpl(prop);
		}
	}
}


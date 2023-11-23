using System;
using System.Net.Sockets;

namespace BinderClientCSharp
{
	public interface IWorkerConnector
	{
		void log(Object message);
		void log(Object message, Exception e);
		NetworkStream getInputStream();
		NetworkStream getOutputStream();
		AccessType getAccessType();
		String getClientAccessString();
		String getWorkerExternalParameters();
		IClientConnector createClientConnector(JavaProperties prop);
		IClientConnector createClientConnector(JavaProperties prop, bool delegateCredentials);
	}
}


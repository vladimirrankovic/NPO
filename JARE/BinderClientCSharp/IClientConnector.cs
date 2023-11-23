using System;
using System.IO;
using System.Net.Sockets;

namespace BinderClientCSharp
{
	public interface IClientConnector
	{
		void connect (); // throws BinderCommunicationException;
		void disconnect (); // throws BinderCommunicationException;
		CEInfo[] executeCEListMatch (); // throws BinderCommunicationException;;
		String getWorkerRoutingInfo();
		String getWorkerErrorDesc();
		String getRoutingInfo();
		NetworkStream getInputStream (); // throws BinderCommunicationException;
		NetworkStream getOutputStream (); // throws BinderCommunicationException;
		bool isIOAvailable();
		AccessType getAccessType();
		String getWorkerAccessString();
	}
}


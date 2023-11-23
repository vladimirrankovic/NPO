using System;
using System.Net.Sockets;

namespace BinderClientCSharp
{
	[Serializable]
	public class SocketWrapper
	{
		private static readonly long serialVersionUID = 3832716787566761661L;
		[NonSerialized]
		protected  Socket socket;
		protected ProtocolExchange protocolExchange;
		[NonSerialized]
		protected NetworkStream inputStream = null;
		[NonSerialized]
		protected NetworkStream outputStream = null;

		public SocketWrapper(Socket socket, ProtocolExchange protocolExchange) /*throws IOException*/
		{
			this.socket = socket;
			this.protocolExchange = protocolExchange;
			this.inputStream = new NetworkStream(socket,true);
			this.outputStream = new NetworkStream(socket, true);
		}

		public void close() /*throws IOException*/ 
		{
			// if (!socket.isInputShutdown())
			// socket.shutdownInput();
			// if (!socket.isOutputShutdown())
			// socket.shutdownOutput();
			// if (!socket.isClosed())
			if (socket != null)
				socket.Close();
		}

		public NetworkStream getIs()
		{
			return inputStream;
		}

		public NetworkStream getOs()
		{
			return outputStream;
		}

		public ProtocolExchange getProtocolExchange() 
		{
			return protocolExchange;
		}

		public Socket getSocket() 
		{
			return socket;
		}
	}
}


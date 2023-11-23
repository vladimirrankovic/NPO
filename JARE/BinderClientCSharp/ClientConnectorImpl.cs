using System;
using log4net;
using log4net.Config;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace BinderClientCSharp
{
	public class ClientConnectorImpl : IClientConnector
	{
		private SocketWrapper clientSW = null;
		private /*ServerSocket*/ Socket serverSocket = null;
		private JavaProperties properties = null;
		public static readonly int PROTOCOL_VERSION = BinderUtil.PROTOCOL_VERSION;
		private static readonly int CONNECTION_TIMEOUT = 2000;
		private bool socketAvailable = false;
		private bool createProxy = false;
		private BinderSocketFactory socketFactory;

		public ClientConnectorImpl(JavaProperties prop) /*throws BinderCommunicationException */
		{
			this.properties = prop;

			bool useSSL = prop.GetProperty("UseSSL", "yes").Equals("yes",StringComparison.OrdinalIgnoreCase);

			/* block below is needed for SSL - Djordje Krecar 04.03.2015.
			if (useSSL) {
				if (prop.getProperty(ContextWrapper.CREDENTIALS_STORE_FILE) != null
				    || (prop.getProperty(ContextWrapper.CREDENTIALS_CERT_FILE) != null && prop
				    .getProperty(ContextWrapper.CREDENTIALS_KEY_FILE) != null))
					createProxy = true;
			}
			*/

			try {
				initSocketFactory();
			} catch (Exception e) {
				throw new BinderCommunicationException("Error initializing socket factory", e);
			}
		}


		private void initSocketFactory() /*throws NoSuchAlgorithmException, CertificateException, IOException*/
		{
			if (!createProxy)
			{
				socketFactory = new BinderSocketFactory(properties);
			} 

			/* block below is needed for proxy - Djordje Krecar 04.03.2015.
			else {// need to create proxy first
				String proxy = createProxy();
				if (logger.isTraceEnabled()) {
					logger.trace("Generated proxy stream: " + proxy);
				}
				System.setProperty(ContextWrapper.GRID_PROXY_STREAM, proxy);
				// clear certs so that generated proxy is read
				properties.remove(ContextWrapper.CREDENTIALS_CERT_FILE);
				properties.remove(ContextWrapper.CREDENTIALS_STORE_FILE);
				socketFactory = new BinderSocketFactory(properties);
			}
			*/
		}

		//only skeleton of the method because we do not use proxy (yet)
		private String CreateProxy() /*throws IOException*/ 
		{
			return "";
		}


		public CEInfo[] executeCEListMatch() /*throws BinderCommunicationException*/
		{
			logger.Info("Initiating connection to the binder in order to execute the query.");
			Socket socket = initConn();
			logger.Info("Connection established.");

		/*
		 * We use local SocketWrapper in case there is some client-worker
		 * connection already established.
		 */

			CEInfo[] ceInfo = null;
			SocketWrapper clientSW = null;
			ProtocolExchange protocolExchange = new ProtocolExchange();
			try {
				clientSW = new SocketWrapper(socket, protocolExchange);
				NetworkStream inputStream = clientSW.getIs();
				exchangeProtocolHeader(clientSW, ConnectionType.CE_QUERY);

				ceInfo = protocolExchange.receiveCEQueryResult(inputStream);
			} 
			catch (IOException e) 
			{
				logger.Error("I/O error occured, disconnecting...", e);
				closeSocket(socket);
				throw new BinderCommunicationException("I/O error occured, disconnecting...", e);
			} 
			catch (Exception e) 
			{
				logger.Error("Error occured while proccesing query.", e);
				closeSocket(socket);
				throw new BinderCommunicationException("Error occured while proccesing query.", e);
			}
			/* After the result is received we disconnect. */
			logger.Debug("Query finished, disconnecting from the binder.");
			closeSocket(socket);
			return ceInfo;
		}

		private Socket initConn() /*throws BinderCommunicationException */
		{
			Socket socket = null;
			try {
				String address = properties.GetProperty("BinderAddress");
				int port = int.Parse(properties.GetProperty("BinderPort", "4566"));
				socket = socketFactory.createSocket();
				socket.Connect(address, port);
				socket.NoDelay = true;
			}
			 catch (IOException e) 
			{
				logger.Error("I/O error occured, disconnecting", e);
				closeSocket(socket);
				throw new BinderCommunicationException("Error occured while initiating connection to the binder", e);
			} 
			catch (/*GeneralSecurityException*/Exception e) 
			{
				logger.Error("Security error occured, unable to connect to the service", e);
				closeSocket(socket);
				throw new BinderCommunicationException("Security error occured, unable to connect to the service", e);
			}
			return socket;
		}


		public void connect() /*throws BinderCommunicationException*/
		{
			logger.Info("Initiating connection to the binder.");
			Socket socket = initConn();
			logger.Info("Connection established.");

			try
			{
				ProtocolExchange protocolExchange = new ProtocolExchange();
				clientSW = new SocketWrapper(socket, protocolExchange);
				NetworkStream output = clientSW.getOs();
				NetworkStream input = clientSW.getIs();
				exchangeProtocolHeader(clientSW, ConnectionType.CLIENT);

				initServerSocket(protocolExchange);
				/* access type is determined when worker info is read */
				readWorkerInfo(input, output);
				logger.Debug("Client exchanged headers with the binder.");

				switch (clientSW.getProtocolExchange().getAccessType())
				{
					case AccessType.BINDER:
					logger.Debug("Communication via binder chosen.");
					/* Socket is available now */
					socketAvailable = true;
					break;
					case AccessType.DIRECT:
					logger.Debug("Attempting to establish a direct connection to the worker...");

						try 
						{
							/* close the socket to the binder */
							clientSW.close();
							if (serverSocket == null)
								throw new IOException("Unable to open a listening socket.");

							socket = serverSocket.Accept();
							socket.NoDelay = true;

							/* establish a new socket to the worker */
							clientSW = new SocketWrapper(socket, protocolExchange);
							/* Socket is available now */
							socketAvailable = true;
							logger.Debug("Connection with the worker established.");
						} 
						catch (IOException e) 
						{
							logger.Error("Error while trying to connect to the worker.");
							socket.Close(); /* check? */
							throw e;
						}
					break;
					case AccessType.CUSTOM:
					logger.Debug("Custom communication chosen, disconnecting from binder...");
					clientSW.close();
					break;
					case AccessType.UNKNOWN:
					logger.Error("Unsupported communication chosen, disconnecting from binder...");
					clientSW.close();
					throw new ArgumentException("Unsupported communication chosen, disconnecting from binder...");
				}

			} 
			catch (IOException e) 
			{
				logger.Error("I/O error occured, disconnecting...", e);
				disconnect();
				throw new BinderCommunicationException("I/O error occured, disconnecting...", e);
			} 
			catch (BinderCommunicationException e) 
			{
				disconnect();
				logger.Error(e);
				throw e;
			} 
			catch (Exception e) 
			{
				logger.Error("Error occured while trying to connect.", e);
				disconnect();
				throw new BinderCommunicationException("Error occured while trying to connect.", e);
			}
		}

		private void initServerSocket(ProtocolExchange protocolExchange) {
		/*
		 * Simple workaround to init server socket before worker tries to
		 * connect if direct connection is chosen. Check of access type is
		 * incomplete, but must be done in order to accept connection properly.
		 * 
		 * Note: worker access string is ""!
		 */
			protocolExchange.setClientAccessString(properties.GetProperty("AccessString", ""));

			if (protocolExchange.getAccessType() == AccessType.DIRECT) {
				try 
				{
					// serverSocket = new
					// ServerSocket(protocolExchange.getClientHostPort());
					serverSocket = socketFactory.createServerSocket(protocolExchange.getClientHostPort());

				} 
				catch (Exception e) 
				{
					serverSocket = null;
				}
			}
		}

		private void exchangeProtocolHeader(SocketWrapper clientSW, ConnectionType connType) /*throws IOException, GeneralSecurityException */
		{
			Socket socket = clientSW.getSocket();
			NetworkStream output = clientSW.getOs();
			ProtocolExchange protocolExchange = clientSW.getProtocolExchange();
			/* protocol version */
			protocolExchange.setClientProtocolVersion(PROTOCOL_VERSION);
			/* connection type (0 - client; 1 - worker) */
			protocolExchange.setConnectionType(connType);
			// /* server selection hint */
			// protocolExchange.setServerSelectionHint(properties.getProperty("ServerSelectionHint",
			// "ANY"));

			/* candidate CEs */
			protocolExchange.setClientCandidateCEs(properties.GetProperty("CandidateCE", ""));
			/* application ID */
			protocolExchange.setClientApplicationID(properties.GetProperty("ApplicationID"));
			/* accessString describing connection between client and worker */
			protocolExchange.setClientAccessString(properties.GetProperty("AccessString", ""));
			/* required wall clock time */
			protocolExchange.setClientRequiredWallClockTime(long.Parse(properties.GetProperty("RequiredWallClockTime")));
			/* client credentials data */
			protocolExchange.setClientProxyKeyData(getEncodedProxyKey());
			// dont send certificate, it will be sent by SSL
			protocolExchange.setClientProxyCertData(new byte[0][]);
			/* routing info */
			String routingInfo = (protocolExchange.getClientAccessString().Equals("",StringComparison.OrdinalIgnoreCase)) ? "\n\tClient => "
				+ "localhost"/*((IPEndPoint) socket.RemoteEndPoint).Address*/ + " : " + ((IPEndPoint) socket.RemoteEndPoint).Port
					: "\n\tClient => accessString = " + protocolExchange.getClientAccessString();
			protocolExchange.setClientRoutingInfo(routingInfo);

			protocolExchange.sendClientHeader(output);

		}

		private byte[] getEncodedProxyKey() /*throws IOException */
		{
			/* block below is needed for SSL - Djordje Krecar 04.03.2015.
			PrivateKey key = socketFactory.getClientPrivateKey();
			if (key != null)
				return key.getEncoded ();
			else
			*/
				return new byte[0];

			// BouncyCastleOpenSSLKey key = new
			// BouncyCastleOpenSSLKey(userCred.getPrivateKey());
			// ByteArrayOutputStream output = new ByteArrayOutputStream();
			// key.writeTo(output);
			// return output.toByteArray();
		}

		private void closeSocket(Socket socket) {
			try {
				socket.Close();
			} catch (IOException e) {
				logger.Error("Error closing socket.", e);
			}
		}

		private void readWorkerInfo(NetworkStream input, NetworkStream output) /*throws EOFException, IOException, BinderCommunicationException */
		{
			ProtocolExchange protocolExchange = clientSW.getProtocolExchange();
			/* Maybe check the connection type to make sure worker responded. */
			protocolExchange.receiveHeader(input);
			protocolExchange.receiveWorkerResponse(input);
			logger.Debug("Routing info received from worker: " + protocolExchange.getWorkerRoutingInfo());

			String workerErrorDesc = protocolExchange.getWorkerErrorDescription();
			if (!workerErrorDesc.Equals("")) {
				logger.Error("Error description received from worker: \n" + workerErrorDesc);
				throw new BinderCommunicationException(workerErrorDesc);
			}
		}

		public void disconnect() /*throws BinderCommunicationException */
		{
			/* Socket is not available after disconnecting. */
			socketAvailable = false;
			try 
			{
				if (clientSW != null)
					clientSW.close();
				if (serverSocket != null)
					serverSocket.Close();
				logger.Info("Disconnected from binder.");
			}
			catch (IOException e) 
			{
				logger.Error("Error occured while disconnecting.", e);
				throw new BinderCommunicationException("Error occured while disconnecting.", e);
			}
		}

		public String getWorkerRoutingInfo() 
		{
			return clientSW.getProtocolExchange().getWorkerRoutingInfo();
		}

		public String getWorkerErrorDesc() 
		{
			return clientSW.getProtocolExchange().getWorkerErrorDescription();
		}

		public String getRoutingInfo() 
		{
			return clientSW.getProtocolExchange().getClientRoutingInfo();
		}

		public bool isIOAvailable()
		{
			return socketAvailable;
		}

		public NetworkStream getInputStream() /*throws BinderCommunicationException */
		{
			try 
			{
				return socketAvailable ?  new NetworkStream (clientSW.getSocket())  : null;
			} 
			catch (IOException e) 
			{
				logger.Error(e, e);
				disconnect(); /* needed? */
				throw new BinderCommunicationException("Error occurred while accessing the InputStream.", e);
			}
		}

		public NetworkStream getOutputStream()/* throws BinderCommunicationException */
		{
			try 
			{
				return socketAvailable ? new NetworkStream (clientSW.getSocket()) : null;
			} 
			catch (IOException e) 
			{
				logger.Error(e, e);
				disconnect(); /* needed? */
				throw new BinderCommunicationException("Error occurred while accessing the OutputStream.", e);
			}
		}

		public AccessType getAccessType()
		{
			return clientSW.getProtocolExchange().getAccessType();
		}

		public String getWorkerAccessString() 
		{
			return clientSW.getProtocolExchange().getWorkerAccessString();
		}

		private static readonly ILog logger = 
			LogManager.GetLogger(typeof(ClientConnectorImpl));
	}
}


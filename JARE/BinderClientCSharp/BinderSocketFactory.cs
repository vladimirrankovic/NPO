using System;
using System.Web;
using System.Net.Sockets;
//using System.Data.Linq;
using System.Security.Policy;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace BinderClientCSharp
{
	public class BinderSocketFactory
	{
		//ContextWrappers are needed for SSL - Djordje Krecar 04.03.2015.
		//private ContextWrapper clientContext;
		//private ContextWrapper serverContext;

		private JavaProperties config;
		public readonly bool USE_SSL;
		private bool requireClientAuth;
		private static readonly String KEY_ALG = "RSA";

		public BinderSocketFactory (JavaProperties config)
		{
			USE_SSL = config.GetProperty("UseSSL", "yes").Equals("yes",StringComparison.OrdinalIgnoreCase);

			if (!USE_SSL)
				// dont need wrappers
				return;

			/* block below is needed for SSL - Djordje Krecar 04.03.2015.
			requireClientAuth = config.GetProperty("RequireClientAuth", "yes").Equals("yes",StringComparison.OrdinalIgnoreCase);
			this.config = config;
			*/
		}

		// Method reworked - Only creating is done here now.
		public Socket createSocket() /*throws UnknownHostException, SSLException, IOException,
		GeneralSecurityException*/
		{

			if (!USE_SSL) {

				return new Socket (AddressFamily.InterNetwork, 
				                  SocketType.Stream, ProtocolType.Tcp);
			} else
				return null; 
			/* block below is needed for SSL - Djordje Krecar 04.03.2015.
			if (clientContext == null){

				clientContext = new ContextWrapper(config);
			}

			// do we require client auth for client sockets?
			return clientContext.getSocketFactory().createSocket(host, port);
			*/
		}

		public /*Server*/Socket createServerSocket(int port) /*throws SSLException, IOException, GeneralSecurityException */
		{
			if (!USE_SSL) {
				Socket s = new Socket (AddressFamily.InterNetwork, 
				                    SocketType.Stream, ProtocolType.Tcp);
				((IPEndPoint)s.LocalEndPoint).Port = port;

				return s;
			} else
				return null;

				//return new /*Server*/Socket(port);

			/* block below is needed for SSL - Djordje Krecar 04.03.2015.
			if (serverContext == null)
				serverContext = new ContextWrapper(config);

			SSLServerSocket serverSocket = (SSLServerSocket) serverContext.getServerSocketFactory().createServerSocket(port);
			if (requireClientAuth)
				serverSocket.setNeedClientAuth(requireClientAuth);
			return serverSocket;
			*/
		}

		public X509Certificate[] getClientCertificateChain() 
		{
			//if (clientContext == null)
				return null;

			/* block below is needed for SSL - Djordje Krecar 04.03.2015.
			String alias = clientContext.getKeyManager().getClientAliases(KEY_ALG, null)[0];
			return clientContext.getKeyManager().getCertificateChain(alias);
			*/
		}

		/*public PrivateKey getClientPrivateKey() 
		{
			//if (clientContext == null)
				return null;

			// block below is needed for SSL - Djordje Krecar 04.03.2015.
			//String alias = clientContext.getKeyManager().getClientAliases(KEY_ALG, null)[0];
			//return clientContext.getKeyManager().getPrivateKey(alias);
			/
		}*/

		public X509Certificate[] getServerCertificateChain() 
		{
			//if (serverContext == null)
				return null;

			/* block below is needed for SSL - Djordje Krecar 04.03.2015.
			String alias = serverContext.getKeyManager().getServerAliases(KEY_ALG, null)[0];
			return serverContext.getKeyManager().getCertificateChain(alias);
			*/
		}

		/*public PrivateKey getServerPrivateKey() 
		{
			//if (serverContext == null)
				return null;

			// block below is needed for SSL - Djordje Krecar 04.03.2015.
			//String alias = serverContext.getKeyManager().getServerAliases(KEY_ALG, null)[0];
			//return serverContext.getKeyManager().getPrivateKey(alias);

		}*/
	}
}


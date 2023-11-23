using System;
using System.IO;
using System.Net.Sockets;


namespace BinderClientCSharp
{
	[Serializable]
	public class ProtocolExchange
	{
		private static readonly long serialVersionUID = 341715075766568770L;

		/* common part of the header */
		private ConnectionType connectionType;
		private AccessType accessType;

		/* client part of the header */
		[Serializable]
		private class ClientHeader 
		{
			private static readonly long serialVersionUID = 3739368088141185365L;
			internal int protocolVersion;
			internal String candidateCEs = "";
			internal String applicationID;
			internal long requiredWallClockTime;
			internal String accessString = "";
			internal String routingInfo = "";

			internal String clientHost = "undefined";
			internal int clientHostPort = -1;
			internal byte[] proxyKeyData;
			internal byte[][] proxyCertData;


			public String toString() 
			{
				return "Client Header - version: " + protocolVersion + " candidateCEs: " + candidateCEs + " applicationID: "
					+ applicationID;
			}
		}

		[Serializable]
		private class WorkerHeader
		{
			private static readonly long serialVersionUID = 5052001056716650373L;
			internal int protocolVersion;
			internal String jobID;
			internal String ceName;
			internal String applicationList;
			internal String accessString = "";
			internal String maxWallClockTime; /* in minutes */
			internal JobStatus jobStatus = JobStatus.FINISHED;

			public String toString()
			{
				return "Worker Header - version: " + protocolVersion + " CE Name: " + ceName + " jobID: " + jobID;
			}
		}

		[Serializable]
		private class WorkerResponse
		{
			private static readonly long serialVersionUID = -5604683180638625871L;
			internal String routingInfo = "";
			internal String errorDescription = "";

			public String toString() 
			{
				return "Worker Response - routing info: " + routingInfo + " error description: " + errorDescription;
			}
		}

		private ClientHeader clientHeader = new ClientHeader();
		private WorkerHeader workerHeader = new WorkerHeader();
		private WorkerResponse workerResponse = new WorkerResponse();

		public ProtocolExchange() 
		{
		}

		public static void exchangeFields(ProtocolExchange client, ProtocolExchange worker)
		{

			// copy clients header to the worker 
			worker.setClientAccessString(client.getClientAccessString());
			worker.setClientApplicationID(client.getClientApplicationID());
			worker.setClientCandidateCEs(client.getClientCandidateCEs());
			worker.setClientProtocolVersion(client.getClientProtocolVersion());
			worker.setClientRequiredWallClockTime(client.getClientRequiredWallClockTime());
			worker.setClientRoutingInfo(client.getClientRoutingInfo());
			worker.setClientProxyKeyData(client.getClientProxyKeyData());
			worker.setClientProxyCertData(client.getClientProxyCertData());

			// copy workers header to client 
			client.setWorkerAccessString(worker.getWorkerAccessString());
			client.setWorkerApplicationList(worker.getWorkerApplicationList());
			client.setWorkerMaxWallClockTime(worker.getWorkerMaxWallClockTime());
			client.setWorkerCeName(worker.getWorkerCeName());
			client.setWorkerJobID(worker.getWorkerJobID());
			client.setWorkerJobStatus(worker.getWorkerJobStatus());
			client.setWorkerProtocolVersion(client.getWorkerProtocolVersion());

			// copy workers response to client 
			client.setWorkerRoutingInfo(worker.getWorkerRoutingInfo());
			client.setWorkerErrorDescription(worker.getWorkerErrorDescription());

		}

		private AccessType determineAccessType()
		{
			String clientAccessString = clientHeader.accessString;
			String workerAccessString = workerHeader.accessString;
			if (clientAccessString.Equals("") && workerAccessString.Equals(""))
				return accessType = AccessType.BINDER;

			if (workerAccessString.Equals("") && clientAccessString.StartsWith("direct:")) {
				String address = clientAccessString.Substring (7);
				int ind = address.LastIndexOf(':');
				if (ind > 0) {
					clientHeader.clientHost = address.Substring(0, ind);
					try {
						clientHeader.clientHostPort = int.Parse(address.Substring(ind + 1));
					} catch (FormatException e) {
						return accessType = AccessType.UNKNOWN;
					}
					return accessType = AccessType.DIRECT;
				}
			}

			if (workerAccessString.Equals("") && clientAccessString.StartsWith("custom:")) {
				return accessType = AccessType.CUSTOM;
			}

			return accessType = AccessType.UNKNOWN;
		}

		public void receiveHeader(NetworkStream ns) /*throws EOFException, IOException */
		{
			BinaryReader input = new BinaryReader(ns);
			int protocolVersion = BinderUtil.readInt (input);
			connectionType = Enums.toConnType(BinderUtil.readInt (input));
			switch (connectionType) {
				case ConnectionType.CLIENT:
				clientHeader.protocolVersion = protocolVersion;
				receiveClientHeader(input);
				break;
				case ConnectionType.WORKER:
				workerHeader.protocolVersion = protocolVersion;
				receiveWorkerHeader(input);
				break;
				case ConnectionType.CE_QUERY:
				/* We consider it a client and expect full client header. */
				clientHeader.protocolVersion = protocolVersion;
				receiveClientHeader(input);
				break;
			}
		}

		
		private void receiveClientHeader(BinaryReader input)
		{
			// /* server selection hint */
			// clientHeader.serverSelectionHint = Util.readString(in);

			/* list of CEs client has chosen */

			clientHeader.candidateCEs = BinderUtil.readString(input);
			/* application name */
			clientHeader.applicationID = BinderUtil.readString(input);
			/* access string for describing connection between client and worker */
			clientHeader.accessString = BinderUtil.readString(input);
			/* clients wall clock time */
			clientHeader.requiredWallClockTime = BinderUtil.readInt (input);

			/* -- client credentials encoded -- */
			/* client proxy key */
			clientHeader.proxyKeyData = BinderUtil.readBytes(input);
			/* client cert chain length */
			clientHeader.proxyCertData = new byte[BinderUtil.readInt (input)][];
			/* client cert chain encoded */
			for (int i = 0; i < clientHeader.proxyCertData.Length; i++)
				clientHeader.proxyCertData[i] = BinderUtil.readBytes(input);

			/* routing info */
			clientHeader.routingInfo = BinderUtil.readString(input);
		}

		private void receiveWorkerHeader(BinaryReader input) /*throws EOFException, IOException*/ 
		{
			/* worker jobID */
			workerHeader.jobID = BinderUtil.readString(input);
			/* worker sends his name */
			workerHeader.ceName = BinderUtil.readString(input);
			/* supported applications */
			workerHeader.applicationList = BinderUtil.readString(input);
			/* access string for describing connection between client and worker */
			workerHeader.accessString = BinderUtil.readString(input);
			/* remaining wall clock time for the worker job */
			workerHeader.maxWallClockTime = BinderUtil.readString(input);
			/* job status reported, if FINISHED job will be removed */
			workerHeader.jobStatus = Enums.toJobStatus(BinderUtil.readInt (input));
		}

		public void receiveWorkerResponse(NetworkStream ns) /*throws EOFException, IOException */
		{
			BinaryReader input = new BinaryReader (ns);
			/* worker routing info */
			workerResponse.routingInfo = BinderUtil.readString(input);
			/* worker error description */
			workerResponse.errorDescription = BinderUtil.readString(input);
		}

		public void sendClientHeader(NetworkStream ns) /*throws IOException*/
		{
			BinaryWriter output = new BinaryWriter (ns);
			/* protocol version */
					//output.Write(clientHeader.protocolVersion);
			BinderUtil.writeInt (output,clientHeader.protocolVersion);
			/* connection type (0 - client; 1 - worker, 3 - ClientQuery) TEST! */
					//output.Write((int)connectionType);
			BinderUtil.writeInt (output,(int)connectionType);
			// /* server selection description */
			// Util.writeString(out, protocolExchange.getServerSelectionHint());

			/* candidate CEs */
			BinderUtil.writeString(output, clientHeader.candidateCEs);
			/* application ID */
			BinderUtil.writeString(output, clientHeader.applicationID);
			/* accessString describing connection between client and worker */
			BinderUtil.writeString(output, clientHeader.accessString);
			/* required wall clock time */
					//output.Write(clientHeader.requiredWallClockTime);
			BinderUtil.writeLong (output,clientHeader.requiredWallClockTime);

			/* -- client credentials encoded -- */
			/* client proxy key */
			BinderUtil.writeBytes(output, clientHeader.proxyKeyData);
			/* client cert chain length */
					//output.Write(clientHeader.proxyCertData.Length);
			BinderUtil.writeInt (output,clientHeader.proxyCertData.Length);
			/* client cert chain encoded */

			if(clientHeader.proxyCertData.Length != 0)
			{
				foreach (byte[] cert in clientHeader.proxyCertData)
					BinderUtil.writeBytes(output, cert);
			}

			/* routing info */
			BinderUtil.writeString(output, clientHeader.routingInfo);
		}

		public void sendWorkerHeader(BinaryWriter output) /*throws IOException*/
		{
			/* protocol version */
			BinderUtil.writeInt (output, workerHeader.protocolVersion);
			//output.Write(workerHeader.protocolVersion);
			/* connection type (0 - client; 1 - worker) */
			BinderUtil.writeInt (output, (int)ConnectionType.WORKER);
			//output.Write((int)ConnectionType.WORKER);
			/* worker jobId */
			BinderUtil.writeString(output, workerHeader.jobID);
			/* gridCE */
			BinderUtil.writeString(output, workerHeader.ceName);
			/* application list supported by worker */
			BinderUtil.writeString(output, workerHeader.applicationList);
			/* accessString describing connection type between client and worker */
			BinderUtil.writeString(output, workerHeader.accessString);
			/* remaining wall clock time for the worker job */
			BinderUtil.writeString(output, workerHeader.maxWallClockTime);
			/* whether worker job should be removed or not. */
			BinderUtil.writeInt (output, (int)workerHeader.jobStatus);
		}

		public void sendWorkerResponse(BinaryWriter output) /*throws IOException*/
		{
			/* worker routing info */
			BinderUtil.writeString(output, workerResponse.routingInfo);
			/* worker error description */
			BinderUtil.writeString(output, workerResponse.errorDescription);
		}

		public void sendChallenge(BinaryWriter output, byte[] challenge) /*throws IOException */
		{
			BinderUtil.writeBytes (output, challenge);
		}

		public byte[] receiveChallenge(BinaryReader input) /* throws IOException */
		{
			return BinderUtil.readBytes (input);
		}

		public CEInfo[] receiveCEQueryResult(NetworkStream ns) /*throws EOFException, IOException */
		{
			BinaryReader input = new BinaryReader (ns);
			/* the size of result array */
			int size = BinderUtil.readInt (input);// input.ReadInt32();
			CEInfo[] ceInfo = new CEInfo[size];
			/* array of CEInfo serialized */
			for (int i = 0; i < size; i++) {
				ceInfo[i] = new CEInfo();
				ceInfo[i].customReadObject(input);
			}
			return ceInfo;
		}

		public void sendCEQueryResult(BinaryWriter output, CEInfo[] ceInfo) /*throws IOException */
		{
			/* the size of result array */
			int size = ceInfo != null ? ceInfo.Length : 0;
			BinderUtil.writeInt (output,size);
			/* array of CEInfo serialized */
			for (int i = 0; i < size; i++) {
				ceInfo[i].customWriteObject(output);
			}
		}

		public ConnectionType getConnectionType() 
		{
			return connectionType;
		}

		public void setConnectionType(ConnectionType connectionType) 
		{
			this.connectionType = connectionType;
		}

		public AccessType getAccessType() 
		{
			determineAccessType();
			return accessType;
		}

		public String toString() 
		{
			// TODO needs refactoring
			String s1 = (clientHeader.applicationID != null) ? clientHeader.toString() : "";
			String s2 = (workerHeader.ceName != null) ? workerHeader.toString() : "";
			String s3 = (workerResponse != null) ? workerResponse.toString() : "";
			return s1 + s2 + s3;
		}

		/* client related getters & setters */
		public int getClientProtocolVersion() 
		{
			return clientHeader.protocolVersion;
		}

		public void setClientProtocolVersion(int protocolVersion)
		{
			clientHeader.protocolVersion = protocolVersion;
		}

		public String getClientCandidateCEs() 
		{
			return clientHeader.candidateCEs;
		}

		public void setClientCandidateCEs(String candidateCEs) 
		{
			clientHeader.candidateCEs = candidateCEs;
		}

		public String getClientApplicationID() 
		{
			return clientHeader.applicationID;
		}

		public void setClientApplicationID(String applicationID)
		{
			clientHeader.applicationID = applicationID;
		}

		public long getClientRequiredWallClockTime() 
		{
			return clientHeader.requiredWallClockTime;
		}

		public void setClientRequiredWallClockTime(long requiredWallClockTime) 
		{
			clientHeader.requiredWallClockTime = requiredWallClockTime;
		}

		public String getClientAccessString() 
		{
			return clientHeader.accessString;
		}

		public void setClientAccessString(String accessString)
		{
			clientHeader.accessString = accessString;
		}

		public String getClientRoutingInfo() 
		{
			return clientHeader.routingInfo;
		}

		public void setClientRoutingInfo(String routingInfo)
		{
			clientHeader.routingInfo = routingInfo;
		}

		public String getClientHost()
		{
			return clientHeader.clientHost;
		}

		public int getClientHostPort() 
		{
			return clientHeader.clientHostPort;
		}

		public byte[] getClientProxyKeyData() 
		{
			return clientHeader.proxyKeyData;
		}

		public void setClientProxyKeyData(byte[] data)
		{
			clientHeader.proxyKeyData = data;
		}

		public byte[][] getClientProxyCertData() 
		{
			return clientHeader.proxyCertData;
		}

		public void setClientProxyCertData(byte[][] data)
		{
			clientHeader.proxyCertData = data;
		}

		/* end of client getters & setters */

		/* worker related getters & setters */
		public int getWorkerProtocolVersion() 
		{
			return workerHeader.protocolVersion;
		}

		public void setWorkerProtocolVersion(int protocolVersion)
		{
			workerHeader.protocolVersion = protocolVersion;
		}

		public String getWorkerJobID() 
		{
			return workerHeader.jobID;
		}

		public void setWorkerJobID(String jobID)
		{
			workerHeader.jobID = jobID;
		}

		public String getWorkerCeName()
		{
			return workerHeader.ceName;
		}

		public void setWorkerCeName(String ceName) 
		{
			workerHeader.ceName = ceName;
		}

		public String getWorkerApplicationList() 
		{
			return workerHeader.applicationList;
		}

		public void setWorkerApplicationList(String applicationList) 
		{
			workerHeader.applicationList = applicationList;
		}

		public String getWorkerAccessString() 
		{
			return workerHeader.accessString;
		}

		public void setWorkerAccessString(String accessString)
		{
			workerHeader.accessString = accessString;
		}

		public String getWorkerMaxWallClockTime()
		{
			return workerHeader.maxWallClockTime;
		}

		public void setWorkerMaxWallClockTime(String maxWallClockTime) 
		{
			workerHeader.maxWallClockTime = maxWallClockTime;
		}

		public JobStatus getWorkerJobStatus() 
		{
			return workerHeader.jobStatus;
		}

		public void setWorkerJobStatus(JobStatus jobStatus)
		{
			workerHeader.jobStatus = jobStatus;
		}

		public String getWorkerRoutingInfo() 
		{
			return workerResponse.routingInfo;
		}

		public void setWorkerRoutingInfo(String routingInfo) 
		{
			workerResponse.routingInfo = routingInfo;
		}

		public String getWorkerErrorDescription()
		{
			return workerResponse.errorDescription;
		}

		public void setWorkerErrorDescription(String errorDescription)
		{
			workerResponse.errorDescription = errorDescription;
		}



	}
}


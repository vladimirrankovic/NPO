using System;
using System.IO;
using log4net;
using log4net.Config;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;

namespace BinderClientCSharp
{
	public class TestClient
	{

		private String prefix = "PING-PONG ";
		private JavaProperties properties = null;
		private IClientConnector clientConn = null;
		private readonly int waitTime;

		//initialise properties file if it's in current directory
		public TestClient() 
		{
			this.waitTime = 1000;
			init("client.properties");
		}

		public TestClient(String propFilePath) 
		{
			this.waitTime = 1000;
			init(propFilePath);
		}

		/* added for test purposes */
		public TestClient(String propFilePath, String waitTime) 
		{
			this.waitTime = int.Parse(waitTime);
			init(propFilePath);
			/* override property for test */
			properties.SetProperty("RequiredWallClockTime", (3 * this.waitTime).ToString());
		}

		private void init(String propFilePath) 
		{
			/* Read client properties from the file. */
			properties = new JavaProperties();
			FileStream fileStream = null;

			try 
			{
				fileStream = new FileStream(propFilePath, FileMode.Open);
				properties.Load(fileStream);
			} 
			catch (FileNotFoundException e1) 
			{
				logger.Error("Client properties file not found!", e1);
				Environment.Exit (1);
			} 
			catch (IOException e2) 
			{
				logger.Error("Error while reading client properties file!", e2);
				Environment.Exit (1);
			}
			finally
			{
				if(fileStream != null)
					fileStream.Close();
			}
		}

		public String receiveData(BinaryReader input)
		{
			Console.WriteLine("ggggggggggggggggggggggggggggggggggggggggggggggggggggggPRIMA PODATKE");
			return BinderUtil.readString (input);
		}

		private void sendData(String s, BinaryWriter output) 
		{
			Console.WriteLine("ggggggggggggggggggggggggggggggggggggggggggggggggggggggSALJE PODATKE");
			BinderUtil.writeString(output, s);
		}

		private void communicate() 
		{
			NetworkStream nin = clientConn.getInputStream();
			NetworkStream nout = clientConn.getOutputStream();
			BinaryReader reader = new BinaryReader (nin);
			BinaryWriter writer = new BinaryWriter (nout);

			for (int i = 1; i < 4; i++) 
			{
				Console.WriteLine("gggggggggggggggggggggggggggggggggg "+ prefix + i + ". time");
				sendData(prefix + i + ". time", writer);
				//testCEs();
				Thread.Sleep(this.waitTime);
				Console.WriteLine(receiveData(reader));
			}

			Console.WriteLine("Sending \"enough\" keyword");
			sendData("enough", writer);
		}

		private void testCEs()
		{

			CEInfo[] ceInfos = clientConn.executeCEListMatch();
			int size = ceInfos.Length;
			Console.WriteLine("CE list match report by the binder, total of " + size + " CEs matched.");

			foreach (CEInfo ceInfo in ceInfos)
				Console.WriteLine(ceInfo);
		}

		public void  run() 
		{
			logger.Info("Starting test client...");

			try
			{
				clientConn = ClientConnectorFactory.createClientConnector(properties);
				testCEs();
				clientConn.connect();
				/* actual client communication */
				communicate();
				//FileSendTesting();
				//FileReceiveTesting();
				/* client finished */
				logger.Info("End of client, disconnecting from binder...");
				clientConn.disconnect();
			} 
			catch (Exception e) 
			{
				logger.Error("Something's wrong!", e);
				Environment.Exit (1);
			}
		}

		public static void main(String[] args) {

			if (args.Length == 0) 
			{
				TestClient tc = new TestClient("../../files/client.properties");
				Thread testClientThread = new Thread(new ThreadStart(tc.run));
				testClientThread.Start ();

			} 
			else if (args.Length == 1)
			{
				TestClient tc = new TestClient(args[0]);
				Thread testClientThread = new Thread(new ThreadStart(tc.run));
				testClientThread.Start ();
			} 
			else if (args.Length == 2) 
			{
				TestClient tc = new TestClient(args[0],args[1]);
				Thread testClientThread = new Thread(new ThreadStart(tc.run));
				testClientThread.Start ();
			} 
			else 
			{
				Console.WriteLine("Usage: TestClient [filepath]");
				Console.WriteLine(" - [filepath] is an optional properties file name (client.properties is default)");
			}
		}

		void FileSendTesting ()
		{
			NetworkStream nin = clientConn.getInputStream();
			NetworkStream nout = clientConn.getOutputStream();
			BinaryReader reader = new BinaryReader (nin);
			BinaryWriter writer = new BinaryWriter (nout);

			UtilPbfs.sendFile ("testR.zip", reader, writer);

			sendData("enough", writer);
		}

		void FileReceiveTesting ()
		{
			NetworkStream nin = clientConn.getInputStream();
			NetworkStream nout = clientConn.getOutputStream();
			BinaryReader reader = new BinaryReader (nin);
			BinaryWriter writer = new BinaryWriter (nout);

			UtilPbfs.receiveFile ("primljen3.zip", reader, writer);
			UtilPbfs.unZIP("primljen3.zip");

			sendData("enough", writer);
		}

		private static readonly ILog logger = 
			LogManager.GetLogger(typeof(TestClient));
	}
}


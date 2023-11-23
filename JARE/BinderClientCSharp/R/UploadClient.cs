using System;
using System.IO;
using log4net;
using System.Threading;
using System.Net.Sockets;
using System.Text;

namespace BinderClientCSharp
{
	public class UploadClient
	{
		private JavaProperties properties = null;
		private IClientConnector clientConn = null;
		private readonly int waitTime;
		private String ulazni_fajl;
		public StringBuilder resultOutput;

		public UploadClient (String[] args)
		{
			this.waitTime = 100;
			ulazni_fajl = args[0];
			resultOutput = new StringBuilder ();
			init();
		}

		private void init() 
		{
			/* Read client properties from the file. */
			properties = new JavaProperties();
			FileStream fileStream = null;
            string propFile = Directory.GetCurrentDirectory() + "\\ViaBinder\\UploadClient.properties";


			try 
			{
				fileStream = new FileStream(propFile, FileMode.Open);
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

		private void communicate() 
		{

			Thread.Sleep(waitTime);

			try 
			{
				NetworkStream nin = clientConn.getInputStream();
				NetworkStream nout = clientConn.getOutputStream();
				BinaryReader reader = new BinaryReader (nin);
				BinaryWriter writer = new BinaryWriter (nout);

                string fileName = Path.GetFileName(ulazni_fajl);
				// Posalji prvo ime fajla
				BinderUtil.writeString(writer, fileName);

				// A onda sam fajl 
				UtilPbfs.sendFile(ulazni_fajl, reader, writer);

				// Ispis teksta dobijenog od servera na stdout klijenta			
				String poruka = BinderUtil.readString(reader);

				resultOutput.Append(poruka);
			}
			catch (FileNotFoundException e) 
			{		
				logger.Error("File not found " + e. Message, e );
			} 
			catch (IOException e)
			{
				logger.Error("Input/output error", e);
			}
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
			logger.Info("Starting UPLOAD client...");

			try
			{
				clientConn = ClientConnectorFactory.createClientConnector(properties);
				testCEs();
				clientConn.connect();
				/* actual client communication */
				communicate();
				/* client finished */
				logger.Info("End of UPLOAD client, disconnecting from binder...");
				clientConn.disconnect();
			} 
			catch (Exception e) 
			{
				logger.Error("Something's wrong!", e);
				Environment.Exit (1);
			}
		}

		public static void main(String[] args)
		{
			UploadClient uploadClient = new UploadClient(args);
			Thread uploadClientThread = new Thread(new ThreadStart(uploadClient.run));
			uploadClientThread.Start ();
		}


		private static readonly ILog logger = 
			LogManager.GetLogger(typeof(UploadClient));
	}
}


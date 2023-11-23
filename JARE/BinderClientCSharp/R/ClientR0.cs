using System;
using log4net;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Text;

namespace BinderClientCSharp
{
	public class ClientR0
	{
		private JavaProperties properties = null;
		private IClientConnector clientConn = null;
		private readonly int waitTime;
		private double[] parameters;
		private String ulazni_fajl;
		public StringBuilder resultOutput;

		public ClientR0(String[] args) 
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

			try 
			{
				fileStream = new FileStream("ClientR.properties", FileMode.Open);
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

				// Prebacivanje relevantnih fajlova na server
				UtilPbfs.sendFile(ulazni_fajl, reader, writer);

				// Ispis teksta dobijenog od servera na stdout klijenta			
				String line = BinderUtil.readString(reader);

				if (line.Equals("OK",StringComparison.OrdinalIgnoreCase)) 
				{							
					resultOutput.Append(";"+line+";");

					parameters = BinderUtil.readDoubles(reader);

					resultOutput.Append(parameters.Length+";");

					for (int i = 0; i < parameters.Length; i++)
					{
						resultOutput.Append(parameters[i].ToString() + ";");
					}
				}
				else
				{
					resultOutput.AppendLine(";"+line+";"+"EXE_FAILED;x;");
				}

				line = BinderUtil.readString(reader);
			}
			catch (FileNotFoundException e) 
			{		
				logger.Error("File not found " + e. Message, e );
			} 
			catch (IOException e)
			{
				logger.Error("Input/output error", e);
			}

			Console.WriteLine(resultOutput);
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

		public static void main(String[] args)
		{
			ClientR0 cR = new ClientR0(args);
			Thread clientRThread = new Thread(new ThreadStart(cR.run));
			clientRThread.Start ();
		}


		private static readonly ILog logger = 
			LogManager.GetLogger(typeof(ClientR));
	}
}


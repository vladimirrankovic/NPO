using System;
using System.IO;
using log4net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace BinderClientCSharp
{
	public class ClientExternal
	{
		private JavaProperties properties = null;
		private IClientConnector clientConn = null;
		private readonly int waitTime;
        protected BinaryReader input;
        protected BinaryWriter output;
		private static readonly ILog logger = LogManager.GetLogger(typeof(ClientExternal));
		protected double[] parameters;
		public StringBuilder resultOutput;
        protected string outputMessage;

        public ClientExternal(String[] args, BinderClientCSharp.JavaProperties properties) 
		{
			this.waitTime = 1000;
			resultOutput = new StringBuilder ();
            this.properties = properties;
            outputMessage = "SUCCESS";

            prepareInput(args);
        }

        protected virtual void prepareInput(String[] args)
        {
			parameters =new double[args.Length];
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    parameters[i] = double.Parse(args[i]);
                }
            }
            catch (FormatException e)
            {
                logger.Error("Bad format of input arguments: " + e.Message);
            }
        }

//#warning Usko grlo - sve niti ucitavaju podateke iz jednog istog fajla
//        protected void init(String propFilePath) 
//        {
//            /* Read client properties from the file. */
//            properties = new JavaProperties();
//            FileStream fileStream = null;

//            try
//            {
//                fileStream = new FileStream(propFilePath, FileMode.Open);
//                properties.Load(fileStream);
//            } 
//            catch (FileNotFoundException e1) 
//            {
//                logger.Error("Client properties file not found!", e1);
//                Environment.Exit (1);
//            } 
//            catch (IOException e2) 
//            {
//                logger.Error("Error while reading client properties file!", e2);
//                Environment.Exit (1);
//            }
//            finally
//            {
//                if (fileStream != null)
//                    fileStream.Close();
//            }
//        }


		protected void communicate() 
		{
			Thread.Sleep(waitTime);

			try
			{
				input = new BinaryReader(clientConn.getInputStream());
				output = new BinaryWriter(clientConn.getOutputStream());

				// Prebacivanje relevantnih fajlova na server
				//UtilPbfs.sendFile("timeSeries.csv", input, output);

                prepareCommunication();

				//BinderUtil.writeDoubles(output,parameters);

				// Ispis teksta dobijenog od servera na stdout klijenta			
				String line = BinderUtil.readString(input);

                if (line.Equals(outputMessage, StringComparison.OrdinalIgnoreCase))
				{				
					parameters = BinderUtil.readDoubles(input);

					//Console.Write(";"+line+";");
                    //resultOutput.Append(";" + line + ";");
                    resultOutput.Append(";" + "SUCCESS" + ";");
                    resultOutput.Append(";" + parameters.Length + ";");

					for (int i = 0; i < parameters.Length; i++) 
					{
						//Console.Write(parameters[i].ToString()+";");
						resultOutput.Append(parameters[i].ToString()+";");
					}
				}
				else
				{
					//Console.WriteLine(";"+line+";"+"EXE_FAILED;x;");
					resultOutput.AppendLine(";"+line+";"+"EXE_FAILED;x;");
				}

				line = BinderUtil.readString(input);
			}

			catch (FileNotFoundException e) 
			{		
				logger.Error("File not found " + e.Message, e );
			}
			catch (IOException e)
			{
				logger.Error("Input/output error", e);
			}
		}

        protected virtual void prepareCommunication()
        {
            BinderUtil.writeDoubles(output, parameters);
        }


        protected void testCEs()  
		{
			CEInfo[] ceInfos = clientConn.executeCEListMatch();
			int size = ceInfos.Length;
			Console.WriteLine("CE list match report by the binder, total of " + size + " CEs matched.");
			foreach (CEInfo ceInfo in ceInfos)
				Console.WriteLine(ceInfo);
		}

		public void run()
		{
			logger.Info("Starting External client...");
			try {
				clientConn = ClientConnectorFactory.createClientConnector(properties);
				testCEs();
				clientConn.connect();
				/* actual client communication */
				communicate();
				/* clieInt finished */
				logger.Info("End of External client, disconnecting from binder...");
				clientConn.disconnect();
			} 
			catch (BinderCommunicationException be)
			{
				logger.Error("Communication with the binder failed.", be);

				if(be.Message.Contains("No ready jobs available"))
					resultOutput.AppendLine(";No ready jobs available;");

			} 
			catch (Exception e) 
			{
				logger.Error("Something's wrong!", e);
				Environment.Exit(1);
			}
		}

        //public static void main(String[] args)
        //{		
        //    // argumenti su niz parametara na osnovu kojih se vrsi evaluacija
        //    ClientExternal tc = new ClientExternal(args, properties);
        //    Thread clientExternalThread = new Thread(new ThreadStart(tc.run));
        //    clientExternalThread.Start();
        //}
	}
}


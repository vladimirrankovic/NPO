using System;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace BinderClientCSharp
{
	
	public class WorkerExternal : IWorkerHandler
	{
		private BinaryReader input;
		private BinaryWriter output;

		private StreamReader ProcessInput;
		//private BufferedWriter ProcessOutput;

		public void run(IWorkerConnector workerConnector)
		{

			workerConnector.log("Started a new EXTERNAL worker handler thread!");

			try 
			{
				input = new BinaryReader(workerConnector.getInputStream());
				output = new BinaryWriter(workerConnector.getOutputStream());			

				// Primi fajl
				UtilPbfs.receiveFile("ponders_recv.csv", input, output);

				Process p = new Process();
				p.StartInfo.RedirectStandardError = true;
				p.StartInfo.Arguments = "./run_exe.sh";

				try
				{					
					p.Start();

					workerConnector.log("Startovao R script");

					// preuzimanje izlaza iz skripta sa stdout i prosledjivanje klijentu 

					ProcessInput = p.StandardOutput;//new BufferedReader(new InputStreamReader(pr.getInputStream()));
					String s = ProcessInput.ReadLine();
					workerConnector.log("Primio sam " + s);
					// Ako skript nije digao exception
					if (s.Equals("SUCCESS",StringComparison.OrdinalIgnoreCase))
					{

						BinderUtil.writeString(output, "SUCCESS");
						// prvo primi broj rezultata koje ce ocitati
						int len = int.Parse(ProcessInput.ReadLine());
						double[] results = new double[len];
						// prima jedan po jedan rezultat
						for (int i = 0; i < len; i++) {
							results[i] = double.Parse(ProcessInput.ReadLine());
						}	
						BinderUtil.writeDoubles(output, results);
					} 
					else
					{
						workerConnector.log("Greska je " + s);
						BinderUtil.writeString(output, s);
					}

					//BinderUtil.writeString(out,ProcessInput.readLine());

					p.WaitForExit();
					ProcessInput.Close();
					p.Dispose();
					workerConnector.log("Finished exe");
					BinderUtil.writeString( output, "-finished-" );
				}
				catch (IOException e)
				{
					workerConnector.log("MojExe nije startovao kako treba>>>> " + e.Message);
				}
				catch(ThreadInterruptedException e)
				{
					Thread.CurrentThread.Interrupt();
				}

			} 
			catch (FileNotFoundException e) 
			{
				workerConnector.log("ServerDispatcherThread:   *** ERROR *** File not found ", e);			
			}
			catch (IOException e) 
			{
				workerConnector.log("ServerDispatcherThread:   *** ERROR *** IO Error occured while binder communicated with the client!!!", e);			
			} 
			catch (Exception e) 
			{
				workerConnector.log("*** ERROR ***   Unknown error occured.", e);
			}

			workerConnector.log("EXTERNAL worker handler end.");
		}
	}
}


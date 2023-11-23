using System;
using System.Threading;
using log4net;
using log4net.Config;
//using ICSharpCode.SharpZipLib.Zip;

namespace BinderClientCSharp
{
    //class MainClass
    //{
    //    static String relativePathToPropFile = "../../files/client.properties";

    //    public static void Main (string[] args)
    //    {
    //        //logger configuration
    //        BasicConfigurator.Configure();

    //        //do various tests and exit from application
    //        //Testing ();

    //        int numClients = 1;

    //        if (args.Length == 1)
    //            int.TryParse(args[0],out numClients);

    //        string[] argss = new string[]{"2.4","1.5"};
    //        ClientExternal tc = new ClientExternal(argss);
    //        Thread testClientThread = new Thread(new ThreadStart(tc.run));
    //        testClientThread.Start();

    //        return;

    //        try 
    //        {
    //            while (true) 
    //            {
    //                Random random = new Random();

    //                for (int i = 0; i < numClients; i++)
    //                {
    //                    int waitTime = (int) ((random.NextDouble() * 1000) + 1000);
    //                    logger.Debug("Starting new client with wait time = " + waitTime + ".");
    //                    InitNewTestClientThread(waitTime);
    //                }

    //                logger.Info("===== Finished first portion - lots of quick jobs. Waiting 1min. =====");
    //                Thread.Sleep(60000);

    //                for (int i = 0; i < numClients / 2; i++)
    //                {
    //                    int waitTime = (int) ((random.NextDouble() * 5000) + 10000);
    //                    logger.Debug("Starting new client with wait time = " + waitTime + ".");
    //                    InitNewTestClientThread(waitTime);
    //                }

    //                logger.Info("===== Finished second portion - moderate amount of moderate jobs. Waiting 1min. =====");
    //                Thread.Sleep(60000);

    //                for (int i = 0; i < numClients / 4; i++) 
    //                {
    //                    int waitTime = (int) ((random.NextDouble()  * 20000) + 240000);
    //                    logger.Debug("Starting new client with wait time = " + waitTime + ".");

    //                    InitNewTestClientThread(waitTime);
    //                }

    //                logger.Info("===== Finished third portion - small amount of slow jobs. Waiting 1min. =====");
    //                Thread.Sleep(60000);
				
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            logger.Error(e, e);
    //        }
		
    //    }

    //    static void InitNewTestClientThread (int waitTime)
    //    {
    //        TestClient tc = new TestClient(relativePathToPropFile, waitTime + "");
    //        Thread testClientThread = new Thread(new ThreadStart(tc.run));
    //        testClientThread.Start();
    //    }

    //    static void Testing ()
    //    {
    //        string[] fileNames = new string[] { "t1", "t2","t3" };
    //        UtilPbfs.createZIP ("test3.zip", fileNames);
    //        UtilPbfs.unZIP ("lose.zip");
    //        UtilPbfs.unZIP ("test3.zip");

    //        logger.Info ("Testing finished!");
    //        Environment.Exit (1);
    //    }

    //    private static readonly ILog logger = 
    //        LogManager.GetLogger(typeof(MainClass));
    //}
}
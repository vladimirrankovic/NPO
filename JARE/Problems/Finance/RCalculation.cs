using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using JARE.Base;
using System.Diagnostics;
using JARE.Base.solutionType;
using JARE.problems.Finance;
using JARE.problems.Finance.DataTypes;
using JARE.problems.Finance.Parameters;
using PortfolioOptimizationGA = JARE.metaheuristics.singleObjective.geneticAlgorithm.PortfolioOptimizationGA;
using RDotNet;
using UploadClient = BinderClientCSharp.UploadClient;

namespace JARE.problems
{
    public class RCalculation
    {
        //REngine
        protected REngine engine;
        protected string RVersion = "R-3.1.1";
        protected string RScriptPath;
        protected bool usingRScriptFile = true;
        protected ExecutionType executionType = ExecutionType.singleThread;
        protected List<EvaluationCriteria> evaluationCriteria;
        protected bool viaRDotNet;

        public RCalculation(List<EvaluationCriteria> evaluationCriteria, string inputRScriptPath = "", ExecutionType executionType = ExecutionType.singleThread)
        {
            this.evaluationCriteria = evaluationCriteria;
            this.executionType = executionType;
            if (inputRScriptPath == "") RScriptPath = Directory.GetCurrentDirectory();
            else RScriptPath = inputRScriptPath;
            if(executionType == ExecutionType.singleThread) Init();
            viaRDotNet = false;
        }

        //R preparation
        protected void Init()
        {
            bool is64Bit = System.Environment.Is64BitProcess;
            Console.WriteLine("Is process 64-bit?", is64Bit);

            string relativePath = "C:\\Program Files\\R\\";
            //RVersion = "R-3.1.1";
            RVersion = "R-3.2.2";
            //RVersion = "R-3.2.4revised";
            string Path = relativePath + RVersion;
            
            //uzmi putanju
            string rhome = System.Environment.GetEnvironmentVariable("R_HOME");

            //ako putanja ne postoji setuj je
            if (string.IsNullOrEmpty(rhome))
                rhome = Path;

            //setuj sistemske promenljive
            System.Environment.SetEnvironmentVariable("R_HOME", rhome);

            string path = System.Environment.GetEnvironmentVariable("PATH");
            if (!path.Contains(RVersion))
            {
                if (is64Bit == true)
                {
                    System.Environment.SetEnvironmentVariable("PATH", System.Environment.GetEnvironmentVariable("PATH") + ";" + rhome + @"\bin\x64");
                }
                else
                {
                    System.Environment.SetEnvironmentVariable("PATH", System.Environment.GetEnvironmentVariable("PATH") + ";" + rhome + @"\bin\i386");
                }
            }

            if (viaRDotNet)
            {
                REngine.SetDllDirectory(rhome);
                engine = REngine.GetInstanceFromID("RDotNet");
                if (engine == null) engine = REngine.CreateInstance("RDotNet");
            }
        }

        protected void UploadFiles(string UploadBatPath, string argument)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;
            
            string batFile = UploadBatPath + "\\UploadClient.bat";
            string argumentFile = correctPathString(UploadBatPath + argument);

            processInfo = new ProcessStartInfo(batFile, argumentFile);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = UploadBatPath;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            string temp = argument.Remove(0, 1);
            StreamWriter sw = new System.IO.StreamWriter(String.Format("{0}\\Upload_{1}_log.txt", UploadBatPath, temp));
            sw.Write(output);
            sw.Dispose();

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();
        }

        protected void UploadFiles2(string UploadBatPath, string argument)
        {
            String[] file = new String[1] { UploadBatPath + argument };
            UploadClient uploadClient = new UploadClient(file);
            uploadClient.run();
        }
       
        protected string arrayToString<T>(T[] list)
        {
            string strArray = string.Empty;

            foreach (T t in list)
            {
                strArray += t.ToString() + ",";
            }

            return strArray.Substring(0, strArray.Length - 1);
        }
        
        public string correctPathString(string path)
        {
            string correctedPath = path;
            if (correctedPath.Contains(" ")) correctedPath = '"' + correctedPath + '"';

            //string[] separatedPath = path.Split('\\');

            //for(int i = 0; i < separatedPath.Length; i++)
            //{
            //    if (separatedPath[i].Contains(" ")) separatedPath[i] = '"' + separatedPath[i] + '"';
            //}
            //for (int i = 0; i < separatedPath.Length - 1; i++)
            //{
            //    correctedPath += separatedPath[i] + "\\";
            //}
            //correctedPath += separatedPath[separatedPath.Length - 1];

            return correctedPath; 
        }
 
    }
}

  
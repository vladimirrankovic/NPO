using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.Base;
using System.Threading;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.IO;

namespace JARE.EvaluationPool
{
    public class WorkerTaskForBinderTPL
    {
        string script, dir,varFile;
        public static event EventHandler<EventArgs> OK_FAILED;
        int numOfResults, procTimeout;
        StreamWriter sw;
        EvaluationResult er;

        public WorkerTaskForBinderTPL(string script, string varFile, string dir, int numOfResults, int procTimeout)
        {
            this.dir = dir;
            this.script = script;
            this.numOfResults = numOfResults;
            this.varFile = varFile;
            this.procTimeout = procTimeout;
        }

        public EvaluationResult ExecuteScript(Solution solution)
        {
            //Pravljeno za slucaj trazenja minimuma

            // napravi argument bat-fajla, koji ce sadrzati putanju do fajla u koji su upisani parametri koji se variraju
            // kao i broj parametara
            Guid g;
            g = Guid.NewGuid();
            string filename = varFile + g.ToString() + ".dat";
            string args = filename + " " + solution.numberOfVariables().ToString();

            // u varFile upisati vrednosti parametara
            sw = new StreamWriter(filename);
            for (int i = 0; i < solution.numberOfVariables(); i++)
            {
                sw.WriteLine(solution.DecisionVariables[i].getValue());
            }
            sw.Close();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = script;
            startInfo.WorkingDirectory = dir;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = args;
            er = new EvaluationResult();
            er.Message = "";
            er.Result = new double[numOfResults];


   
            Process exeProcess = null;
            try
            {
                DateTime startTime = DateTime.Now;

                exeProcess = Process.Start(startInfo);
                
                if (exeProcess.WaitForExit(procTimeout))
                {
                    string output = exeProcess.StandardOutput.ReadToEnd();
                    string[] rezultat = output.Split(';');
                    if (rezultat.Contains("OK"))
                    {
                        er.Status = EvaluationStatus.DONE;
                        double evaluationTime = (DateTime.Now - startTime).TotalSeconds;
                        er.Message = rezultat[1] + " Evaluation time = "+evaluationTime+" s.";
                        int OK_index = -1;
                        for (int j = 0; j < rezultat.Count(); j++)
                        {
                            if (rezultat[j] == "OK")
                            {
                                OK_index = j;
                                break;
                            }
                        }
                        
                        for (int i = 0; i < numOfResults; i++)
                        {
                            er.Result[i] = double.Parse(rezultat[i + OK_index + 2]);
                        }

                        if(OK_FAILED != null)
                            OK_FAILED(er.Message, null);
                    }
                    //kada nema spremnih jobova
                    else if (output.Contains("No ready jobs available"))
                    {
                        er.Status = EvaluationStatus.NO_READY_JOBS;
                        er.Message = "No ready jobs available";
                        if (OK_FAILED != null)
                            OK_FAILED(er.Message, null);
                    }

                    //kada padne evaluacija
                else if (rezultat.Contains("EXE_FAILED"))
                {
                        er.Status = EvaluationStatus.FAILED;
                        er.Message = "EXE_FAILED " + rezultat[1];
                        for (int i = 0; i < numOfResults; i++)
                        {
                            er.Result[i] = double.MaxValue;
                        }
                        if (OK_FAILED != null)
                            OK_FAILED(er.Message, null);
                }
                // kad iz nekog razloga worker ne odradi evaluaciju 
                else
                {
                    er.Message = "FAILED";
                        er.Status = EvaluationStatus.FAILED;
                        if (OK_FAILED != null)
                            OK_FAILED(er.Message, null);
                    }
                    return er;
                }
                else
                {
                    er.Message = "Timeout expired";
                    er.Status = EvaluationStatus.FAILED;
                    if (OK_FAILED != null)
                        OK_FAILED(er.Message, null);
                    return er;
                }
            }
            catch (Exception ex)
            {
                er.Message = "EXCEPTION " + ex.Message;
                er.Status = EvaluationStatus.FAILED;
                for (int i = 0; i < numOfResults; i++)
                {
                    er.Result[i] = double.MaxValue;
                }
                if (OK_FAILED != null)
                    OK_FAILED(er.Message, null);
                return er;
            }
            finally
            {
                if (!exeProcess.HasExited)
                {
                    try
                    {
                        exeProcess.Kill();
                    }
                    catch (InvalidOperationException)
                    {
                        // Process is already finished so nothing to do
                    }
                }
            }

        }
    }
}

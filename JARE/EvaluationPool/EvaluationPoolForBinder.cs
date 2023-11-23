using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using JARE.Base;


namespace JARE.EvaluationPool
{
    public class EvaluationPoolForBinder : EvaluationPool
    {
        
        
        IWBEvaluation problem;
        string script, dir, varFile;
        int errorCounter = 0;
        int numOfThreadsAvailable, procTimeout;
        ManualResetEvent[] workerIsFree;

        //int[] threads;
        public EvaluationPoolForBinder(IWBEvaluation problem, string script, string dir, string varFile, int numOfWorkers, int procTimeout)
            : base()
        {
            this.problem = problem;
            this.script = script;
            this.dir = dir;
            this.varFile = varFile;
            this.procTimeout = procTimeout;
            
            this.numOfThreadsAvailable = numOfWorkers;
            workerIsFree = new ManualResetEvent[this.numOfThreadsAvailable];
        }

        protected override void RunEvaluation()
        {
            for (int i = 0; i < numOfThreadsAvailable; i++)
            {
                workerIsFree[i] = new ManualResetEvent(true);
            }

            WorkerTaskForBinder task;
            while (Count() > 0)
                {
                    int freeThreadIndex = WaitHandle.WaitAny(workerIsFree);

                    Solution solution = Dequeue();
                    task = new WorkerTaskForBinder(solution, workerIsFree[freeThreadIndex], script, varFile, dir,((Problem)problem).m_numberOfObjectives,procTimeout);
                    workerIsFree[freeThreadIndex].Reset();
                    ThreadPool.QueueUserWorkItem(Do, task);
                }
            WaitHandle.WaitAll(workerIsFree);
        }

        private void Do(Object o)
        {
            EvaluationResult er;
            WorkerTaskForBinder s = (WorkerTaskForBinder)o;
            
            
            //TimeSpan UnixTimeSpan;

            try
            {
                er = s.ExecuteScript(s.Solution);

                if (er.Status == EvaluationStatus.DONE)
                {
                    //goodCounter++;
                    Console.WriteLine(er.Message + " ------------------------ ");
                    problem.JobResultsToObjectives(er, s.Solution);
                    
                   // UnixTimeSpan = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0));
                    //problem.PrintResultArrivalTime((long)UnixTimeSpan.TotalSeconds);  
                   
                }
                else if (er.Status == EvaluationStatus.NO_READY_JOBS)
                {
                    Console.WriteLine("Message: " + er.Message);
                    SendSolution(s.Solution);
                    Thread.Sleep(5000);
                }
                else
                {
                    Console.WriteLine("Message: " + er.Message);
                    SendSolution(s.Solution);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Other Error: " + e.Message);
                SendSolution(s.Solution);
                //Thread.Sleep(100);
            }
            finally
            {
                s.a.Set();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JARE.Base;
using System.IO;


namespace JARE.EvaluationPool
{
    public class EvaluationPoolForBinderTPL : EvaluationPool
    {
        System.IO.StreamWriter sw;
        string varFile;
        IWBEvaluation problem;
        string script, dir;
        int procTimeout;
        Task[] taskArray;
        List<Task> listOfTasks;
        string externalPropertiesLocation;

        public EvaluationPoolForBinderTPL(IWBEvaluation problem, string script, string dir, string varFile, int procTimeout, string externalPropertiesLocation)
            : base()
        {
            this.problem = problem;
            this.script = script;
            this.varFile = varFile;

            this.dir = dir;
            this.procTimeout = procTimeout;
            listOfTasks = new List<Task>();
            this.externalPropertiesLocation = externalPropertiesLocation;
        }

        //Moved from ClientExternal
        private BinderClientCSharp.JavaProperties init()
        {
            /* Read client properties from the file. */
            BinderClientCSharp.JavaProperties properties = new BinderClientCSharp.JavaProperties();

            try
            {
                properties.Load(new FileStream(externalPropertiesLocation, FileMode.Open));
            }
            catch (FileNotFoundException e1)
            {
                Environment.Exit(1);
            }
            catch (IOException e2)
            {
                Environment.Exit(1);
            }

            return properties;
        }

        protected override void RunEvaluation()
        {
            // broj onih koji su poslati na evaluaciju treba da bude jednak broju vracenih
            // da bi se izaslo iz petlje
            //Main thread now loads External.properties
            BinderClientCSharp.JavaProperties properties = init();
            int onEvaluation = 0, freeTaskIndex;
            while (!shouldStop && (onEvaluation != 0 || Count() > 0))
            {
                while (!shouldStop && Count() > 0)
                {
                    Solution solution = Dequeue();
                    listOfTasks.Add(Task.Factory.StartNew(() => DoWork(solution,properties)));
                    onEvaluation++;
                }
                freeTaskIndex = Task.WaitAny(listOfTasks.ToArray());
                onEvaluation--;
                listOfTasks.RemoveAt(freeTaskIndex);              
            }
        }
         

        private void DoWork(Solution solution,BinderClientCSharp.JavaProperties properties)
        {
            EvaluationResult er;
            //WorkerTaskForBinderTPL workerTask = new WorkerTaskForBinderTPL(script, varFile, dir, ((Problem)problem).m_numberOfObjectives, procTimeout);
            WorkerTaskForBinderTPLNative workerTask = new WorkerTaskForBinderTPLNative(script, varFile, dir, ((Problem)problem).m_numberOfObjectives, procTimeout, properties);

            //TimeSpan UnixTimeSpan;
            try
            {
                er = workerTask.ExecuteScript(solution);

                if (er.Status == EvaluationStatus.DONE)
                {
                    Console.WriteLine(er.Message + " ------------------------ ");
                    problem.JobResultsToObjectives(er, solution);
                    // UnixTimeSpan = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0));
                    //problem.PrintResultArrivalTime((long)UnixTimeSpan.TotalSeconds);  
                }
                else if (er.Status == EvaluationStatus.NO_READY_JOBS)
                {
                    Console.WriteLine("Message: " + er.Message);
                    Thread.Sleep(3000);
                    SendSolution(solution);
                }
                else
                {
                    Console.WriteLine("Message: " + er.Message);
                    SendSolution(solution);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Other Error: " + e.Message);
                SendSolution(solution);
            }            
        }
    }
}

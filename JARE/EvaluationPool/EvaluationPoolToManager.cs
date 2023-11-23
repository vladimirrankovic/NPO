using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpMetal.Base;

namespace SharpMetal.EvaluationPool
{
    class EvaluationPoolToManager : EvaluationPool
    {
        protected IWSEvaluation m_problem;
        protected WCFManagerClient client;
        private String[] jobIDPackage;
        private List<JobDescriptor> listOfJobDescriptors = new List<JobDescriptor>();

        public EvaluationPoolToManager(IWSEvaluation p, WCFManagerClient client)
            : base()
        {
            m_problem = p;
            this.client = client;
        }

        protected override void RunEvaluation()
        {
            JobDescriptor task;
            //TraceMsg1("RunEvaluation method started");
            String jobID = "";
            while (Count() > 0)
            {
                Solution s = Dequeue();

                //task = new JobDescriptor(jp, g.ToString() + " " + j.ToString(), temp);
                WCFManagerLibrary.JobParameters jp = new WCFManagerLibrary.JobParameters();
                jp.Parameters = (m_problem.VariablesToJobParameters(s)).Parameters;
                try
                {
                    jobID = client.doJob_UI(jp);
                }
                catch (Exception e)
                {
                    Console.WriteLine("doJob_UI neuspesan " + e.Message);
                }   
                task = new JobDescriptor(jp);
                task.S = s;
                task.JobID = jobID;
                listOfJobDescriptors.Add(task);
            }
            int br = 0;
            Console.WriteLine("Count " + listOfJobDescriptors.Count);
            jobIDPackage = new String[listOfJobDescriptors.Count];
            foreach (JobDescriptor jd in listOfJobDescriptors)
            {
              //  Console.WriteLine(jd.JobID);
                jobIDPackage[br++] = jd.JobID;
            }
        }
        public override void Wait()
        {
            Console.WriteLine("POCEO SA CEKANJEM");
            base.Wait();
            client.waitDone_package_UI(jobIDPackage);
            WCFManagerLibrary.JobIDandResult[] rl = client.getResultPackage_UI(jobIDPackage);
            for (int i = 0; i < rl.Length; i++)
            {
                foreach(JobDescriptor jd in listOfJobDescriptors)
                    if (jd.JobID == rl[i].JobID)
                    {
                        WcfEvaluationServiceLibrary.EvaluationResult er = new WcfEvaluationServiceLibrary.EvaluationResult();
                        er.Result = rl[i].Result;
                        er.Message = rl[i].Message;
                        m_problem.JobResultsToObjectives(er, jd.S);
                        break;
                    }
            }


            //foreach (JobDescriptor jd in listOfJobDescriptors)
            //{
            //    WCFManagerLibrary.JobResult rezultat;
            //    rezultat = client.getResult_UI(jd.JobID);
            //    WcfEvaluationServiceLibrary.EvaluationResult er = new WcfEvaluationServiceLibrary.EvaluationResult();
            //    er.Result = rezultat.Result;
            //    er.Message = rezultat.Message;
            //    m_problem.JobResultsToObjectives(er, jd.S);

            ////    jd.Result = rezultat;
            // //   jd.S.setObjective(0, -1 * rezultat.Result[0]);
            // //   jd.S.setObjective(1, -1 * rezultat.Result[1]);
            //    //Console.WriteLine("Result received for " + s.JobNo);

            //}
            ////TraceMsg1("RunEvaluation method finshed");
            listOfJobDescriptors.RemoveAll(u => u == u);
            
        }
    }

    public class JobDescriptor
    {
        /// <summary>
        /// Izlazni podaci, tj. rezultat obrade job parametara
        /// </summary>
        WCFManagerLibrary.JobResult result;
        /// <summary>
        /// Ulazni podaci za do, u nasem slucaju parametri za evaluaciju
        /// </summary>
        WCFManagerLibrary.JobParameters jobParameters;
        /// <summary>
        /// neka vrsta opisa zadatka (jednog) zbog evidncije
        /// </summary>
        String jobNo;
        /// <summary>
        /// prekidac  za jedan job, da li je zavrsen ili ne
        /// potreban je zbog toga sto UI mora da ceka da se obave svi pjob-ovi jedne generacije
        /// da bi mogao da nastavi dalje
        /// </summary>
        String jobID;
        Solution s;

        public String JobID { set { jobID = value; } get { return jobID; } }
        public String JobNo { get { return jobNo; } }
        public WCFManagerLibrary.JobResult Result { set { result = value; } get { return result; } }
        public WCFManagerLibrary.JobParameters JobParameters { get { return jobParameters; } }
        public Solution S { set { s = value; } get { return s; } }

        public JobDescriptor(WCFManagerLibrary.JobParameters jobParameters, String jobNo)
        {
            this.jobParameters = jobParameters;
            this.jobNo = jobNo;
        }

        public JobDescriptor(WCFManagerLibrary.JobParameters jobParameters)
        {
            this.jobParameters = jobParameters;
        }
    }

}

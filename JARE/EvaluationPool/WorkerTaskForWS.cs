using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.Base;
using System.Threading;


namespace JARE.EvaluationPool
{
    public class WorkerTaskForWS : WorkerTask
    {
        WcfEvaluationServiceClient worker;

        public WcfEvaluationServiceClient Worker { get { return worker; } }
        public string workerName;

        public WorkerTaskForWS(WcfEvaluationServiceClient worker, string workerName, Solution s, ManualResetEvent a) : base(s,a)
        {
            this.worker = worker;
            this.workerName = workerName;
        }
    }
}

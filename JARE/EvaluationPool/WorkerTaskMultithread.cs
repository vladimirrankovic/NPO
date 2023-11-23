using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.Base;
using System.Threading;

namespace JARE.EvaluationPool
{
    class WorkerTaskMultithread : WorkerTask
    {
        public int ThreadNo { get { return No; } }
        int No;

        public WorkerTaskMultithread(Solution s, ManualResetEvent a, int threadNo)
            : base(s, a)
        {
            this.No = threadNo;
        }
    }
}

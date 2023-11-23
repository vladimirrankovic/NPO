using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.Base;
using System.Threading;


namespace JARE.EvaluationPool
{
    public class WorkerTask
    {
        internal Solution solution;

        public Solution Solution { get { return solution; } }
        public ManualResetEvent a;

        public WorkerTask(Solution s, ManualResetEvent a)
        {
            this.solution = s;
            this.a = a;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.Base;

namespace JARE.EvaluationPool
{
    public interface IWBEvaluation
    {
        void JobResultsToObjectives(EvaluationResult er, Solution s);
        //void PrintResultArrivalTime(long t);
    }
}

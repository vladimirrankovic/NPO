using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.Base;

namespace JARE.EvaluationPool
{
   public interface IWSEvaluation
    {
        WcfEvaluationServiceLibrary.JobParameters VariablesToJobParameters(Solution s);
        void JobResultsToObjectives(WcfEvaluationServiceLibrary.EvaluationResult er, Solution s);
        string WriteError(Solution s);
    }
}

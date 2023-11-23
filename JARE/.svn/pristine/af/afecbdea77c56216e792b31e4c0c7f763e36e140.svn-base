using System;
using System.Collections.Generic;
using System.Text;
using JARE.Base;

namespace JARE.EvaluationPool
{
    //public delegate void EvaluationFunctionDelegate(Solution solution);

    class EvaluationPoolSequentional:EvaluationPool
    {
        protected Problem m_problem;

        //EvaluationFunctionDelegate evaluate;
        public EvaluationPoolSequentional(Problem p)
            : base()
        {
            m_problem = p;
        }

        protected override void RunEvaluation()
        {
            //TraceMsg1("RunEvaluation method started");
            while (Count() > 0)
            {
                Solution s = Dequeue();
                m_problem.evaluate(s);
            }
            //TraceMsg1("RunEvaluation method finshed");
        }
    }
}
//TraceMsg("Pre evaluacije", s); TraceMsg("Posle evaluacije", s); 
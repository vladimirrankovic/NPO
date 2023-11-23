using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE;
using JARE.Base;
using JARE.Base.solutionType;
using JARE.Base.variable;
using JARE.Base.operators;

namespace JARE.problems.singleObjective
{
    public class MaxSin2 : Problem
    {
        public MaxSin2(System.String solutionType)
		{
			m_numberOfVariables = 1;
			m_numberOfObjectives = 1;
			m_numberOfConstraints = 0;
			m_problemName = "MaxSin2";
			
			m_solutionType = new RealSolutionType(this);

            m_lowerLimit = new double[m_numberOfVariables];
            m_upperLimit = new double[m_numberOfVariables];

            m_upperLimit[0] = 100.00;
            m_lowerLimit[0] = -100.00;
			
		} 
        public override void evaluate(Solution solution)
        {
            Variable[] decisionVariables = solution.DecisionVariables;

            double x = decisionVariables[0].getValue();
            double f = 0.5 - (Math.Pow(Math.Sin(x),2) - 0.5)/Math.Pow(1 + 0.001 * Math.Pow(x,2),2);

            solution.setObjective(0, -1 * f);
        }
    }
}

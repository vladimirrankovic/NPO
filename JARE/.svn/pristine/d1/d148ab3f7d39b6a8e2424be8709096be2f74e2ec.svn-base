using System;
using System.Collections.Generic;
using System.Text;
using SharpMetal.Base;
using System.Configuration;

using BinaryRealSolutionType = SharpMetal.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = SharpMetal.Base.solutionType.RealSolutionType;

namespace SharpMetal.problems
{
    public abstract class External : SharpMetal.Base.Problem
    {
        public External(Problem problem)
        {
            m_numberOfVariables = problem.m_numberOfVariables;
            m_numberOfObjectives = problem.m_numberOfObjectives;
            m_numberOfConstraints = problem.m_numberOfConstraints;
            m_problemName = problem.m_problemName;

            m_lowerLimit = problem.m_lowerLimit;
            m_upperLimit = problem.m_upperLimit;

            m_solutionType = problem.m_solutionType;
        }

        public override void evaluate(SharpMetal.Base.Solution solution)
        {
            Variable[] decisionVariables = solution.DecisionVariables;
            double[] f = new double[m_numberOfObjectives];
            double[] x = new double[m_numberOfVariables];

            for (int i = 0; i < decisionVariables.Length; i++)
            {
                x[i] = (double)(decisionVariables[i].getValue());
            }
            string loc = System.Configuration.ConfigurationManager.AppSettings.GetValues("LokacijaAplikacijeIbarObjectives")[0];
            string app = loc + "IbarObjectives.exe";
            f = Execute(x, m_numberOfObjectives,app);


            for (int i = 0; i < m_numberOfObjectives; i++)
            {
                solution.setObjective(i, f[i]);
            }
        }

        public abstract double[] Execute(double[] x, int numberOfObjectives, string appPath);

    }
}

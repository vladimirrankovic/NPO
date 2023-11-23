using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE;
using JARE.Base;
using JARE.Base.solutionType;
using JARE.Base.variable;

namespace JARE.problems.singleObjective
{
    public class testZaMilovana : Problem
    {
        public testZaMilovana(int numberOfBits)
        {
            m_numberOfVariables = 1;
            m_numberOfObjectives = 1;
            m_numberOfConstraints = 0;
            m_problemName = "Milovan";

            m_solutionType = new BinarySolutionType(this);
            m_variableType = new System.Type[m_numberOfVariables];

            m_length = new int[m_numberOfVariables];

            m_variableType[0] = System.Type.GetType("JARE.Base.variable.Binary");
            m_length[0] = numberOfBits;
        }

        public override void evaluate(Solution solution)
        {
            Binary variable;

            variable = ((Binary)solution.DecisionVariables[0]);

            int[] value = new int[NumberOfBits];

            for (int i = 0; i < NumberOfBits; i++)
            {
                if (variable.getIth(i))
                    value[i] = 1;
                else value[i] = 0;
            }

            double[] x = new double[29] { 0, 0.2, 0.4, 0.6, 0.8, 1, 1.2, 1.4, 1.6, 1.8, 2, 2.2, 2.4, 2.6, 2.8, 3, 3.2, 3.4, 3.6, 3.8, 4, 4.2, 4.4, 4.6, 4.8, 5, 5.2, 5.4, 5.6 };
            double[] f_observed = new double[29] {0,1.233346654,2.507091712,3.783212367,5.026780454,6.207354924,7.30019543,8.28724865,9.157868015,9.909238154,10.54648713,
                11.08248202,
11.5373159,
11.93750686,
12.31494075,
12.70560004,
13.14812928,
13.68229449,
14.34739778,
15.18071055,
16.21598752,
17.48212114,
19.00198963,
20.79154498,
22.85917696,
25.20537863,
27.82272672,
30.69617756,
33.80366681};

            double[] f_calculated = new double[29];

            for (int i = 0; i < x.Length; i++)
            {
                f_calculated[i] = value[0] * x[i] + value[1] * Math.Pow(x[i], 2) + value[2] * Math.Pow(x[i], 3) +
                   value[3] * Math.Cos(x[i]) + value[4] * Math.Exp(x[i]) + value[5] * Math.Sqrt(x[i]) + value[6] * 5 * Math.Sin(x[i]) + value[7] * Math.Sin(2 * x[i]);
            }

            solution.setObjective(0, RMSE(f_observed, f_calculated));

        }

        double RMSE(double[] observed, double[] simulated)
        {
            int n = observed.Length;
            double s = 0.0;

            for (int i = 0; i < n; i++)
            {
                s += Math.Pow(observed[i] - simulated[i], 2);
            }

            return Math.Sqrt(s / (n - 1));
        }
    }
}

using System;
using JARE.Base;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;
namespace JARE.problems
{

    /// <summary> Class representing problem Poloni. This problem has two objectives to be
    /// MAXIMIZED. As JARE always minimizes, the rule Max(f(x)) = -Min(f(-x)) must
    /// be applied.
    /// </summary>
    [Serializable]
    public class Binh2 : Problem
    {
       
            
           
        /// <summary> Constructor.
        /// Creates a default instance of the Poloni problem
        /// </summary>
        /// <param name="solutionType">The solution type must "Real" or "BinaryReal".
        /// </param>
        public Binh2(System.String solutionType)
        {
            m_numberOfVariables = 2;
            m_numberOfObjectives = 2;
            m_numberOfConstraints = 2;
            m_problemName = "Binh2";

            m_lowerLimit = new double[m_numberOfVariables];
            m_upperLimit = new double[m_numberOfVariables];
            m_lowerLimit[0] = 0.0;
            m_lowerLimit[1] = 0.0;
            m_upperLimit[0] = 5.0;
            m_upperLimit[1] = 3.0;

            


            if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
                m_solutionType = new BinaryRealSolutionType(this);
            else if (String.CompareOrdinal(solutionType, "Real") == 0)
                m_solutionType = new RealSolutionType(this);
            else
            {
                System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
                System.Environment.Exit(-1);
            }
        } 

        /// <summary> Evaluates a solution </summary>
        /// <param name="solution">The solution to evaluate
        /// </param>
        /// <throws>  SMException  </throws>
        public override void evaluate(Solution solution)
        {
            double[] x = new double[m_numberOfVariables];
            Variable[] decisionVariables = solution.DecisionVariables;

            x[0] = decisionVariables[0].getValue();
            x[1] = decisionVariables[1].getValue();

         
            double[] f = new double[m_numberOfObjectives];


            f[0] = 4.0 * Math.Pow(x[0], 2) + 4.0 * Math.Pow(x[1], 2);
            f[1] = Math.Pow(x[0] - 5.0, 2) + Math.Pow(x[1] - 5.0, 2);

            solution.setObjective(0, f[0]);
            solution.setObjective(1, f[1]);
        } // evaluate


        public override void evaluateConstraints(Solution solution)
        {
            double[] x = new double[m_numberOfVariables];
            Variable[] decisionVariables = solution.DecisionVariables;

            x[0] = decisionVariables[0].getValue();
            x[1] = decisionVariables[1].getValue();
            double[] constraint = new double[this.NumberOfConstraints];

            //constraint[0] = (Math.Pow(x[0] - 5.0, 2) + Math.Pow(x[1],2) - 25.0) / (-25.0) ;
            //constraint[1] = (Math.Pow(x[0] - 8.0, 2) + Math.Pow(x[1] + 3.0, 2.0)-7.7) / 7.7;

            constraint[0] = -Math.Pow(x[0] - 5.0, 2) - Math.Pow(x[1], 2) + 25.0;
            constraint[1] = Math.Pow(x[0] - 8.0, 2) + Math.Pow(x[1] + 3.0, 2.0) - 7.7;
            

            double total = 0.0;
            int number = 0;
            for (int i = 0; i < this.NumberOfConstraints; i++)
                if (constraint[i] < 0.0)
                {
                    total += constraint[i];
                    number++;
                }

            solution.OverallConstraintViolation = total;
            solution.NumberOfViolatedConstraint = number;
        } // evaluateConstraints 
    } // Poloni
}
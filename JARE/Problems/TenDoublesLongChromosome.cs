using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.Base;
using JARE;
using JARE.Base.solutionType;
using JARE.problems;

namespace JARE.problems
{
    public class TenDoublesLongChromosome : JARE.Base.Problem, EvaluationPool.IWBEvaluation
    {
        string logFilePath;
        System.IO.StreamWriter sw;


        public TenDoublesLongChromosome(string solutionType)
        {
            string dir = String.Format("{0}\\TenDoublesLongChromosome", JARE.Properties.Settings.Default.LogFilesLocation);
            
            if (!System.IO.Directory.Exists(dir))
            {
                m_directory = System.IO.Directory.CreateDirectory(dir);
            }
            m_subdirectory = System.IO.Directory.CreateDirectory(String.Format("{0}\\Rezultati{1}_{2}_{3}_{4}_{5}", dir,
               DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute));
            logFilePath = String.Format("{0}\\{1}\\error_{2}_{3}_{4}_{5}_{6}.txt", dir, m_subdirectory.Name, DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);

            if (System.IO.File.Exists(logFilePath))
            {
                System.IO.File.Delete(logFilePath);
            }
            else
            {
                sw = System.IO.File.CreateText(logFilePath);
                sw.Close();
            }

            m_numberOfVariables = 10; // (k^BB)_p, (k^BB)_i, w1, w2, w3, maxFlowChange, (k^F)_p, (k^F)_i, T_FF
            m_numberOfObjectives = 2;
            m_numberOfConstraints = 0;
            m_problemName = "TenDoublesLongChromosome";

            m_lowerLimit = new double[m_numberOfVariables];
            m_upperLimit = new double[m_numberOfVariables];

            //(k^BB)_p, (k^BB)_i
            m_lowerLimit[0] = 1.0; m_upperLimit[0] = 100.0;
            m_lowerLimit[1] = 0.0; m_upperLimit[1] = 10.0;

            //w1, w2, w3
            m_lowerLimit[2] = 0.0; m_upperLimit[2] = 1.0;
            m_lowerLimit[3] = 0.0; m_upperLimit[3] = 1.0;
            m_lowerLimit[4] = 0.0; m_upperLimit[4] = 1.0;

            //maxFlowChange
            m_lowerLimit[5] = 300.0; m_upperLimit[5] = 450.0;

            // (k^F)_p, (k^F)_i, T_FF
            m_lowerLimit[6] = 1.0; m_upperLimit[6] = 100.0;
            m_lowerLimit[7] = 0.0; m_upperLimit[7] = 10.0;
            m_lowerLimit[8] = 61; m_upperLimit[8] = 1000;
            m_lowerLimit[9] = 0.0; m_upperLimit[9] = 1.0;


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

        public override void evaluate(JARE.Base.Solution solution)
        {
        }

        public override void PrintGenerationReport(SolutionSet population, SolutionSet bestFront, int genNO)
        {
            System.IO.StreamWriter swPopulation = new System.IO.StreamWriter(String.Format("{0}\\Population_{1:000}.csv", m_subdirectory.FullName, genNO));
            System.IO.StreamWriter swBestSubfront = new System.IO.StreamWriter(String.Format("{0}\\BestSubFront_{1:000}.csv", m_subdirectory.FullName, genNO));

            // Write population
            int solutionIndex = 0;
            swPopulation.WriteLine("Red.br.,income,denivelationFitness,BukBijelaKp,BukBijelaKi,w1,w2,w3,maxFlowChange,FocaKp,FocaKi,Tff,x"); 
            foreach (Solution sol in population.m_solutionsList)
            {
                Variable[] variable = sol.DecisionVariables;
              
                swPopulation.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                    ++solutionIndex, (-1) * sol.getObjective(0), (-1) * sol.getObjective(1), 
                    variable[0].getValue(), variable[1].getValue(), variable[2].getValue(), variable[3].getValue(), variable[4].getValue(), variable[5].getValue(),
                    variable[6].getValue(), variable[7].getValue(), variable[8].getValue(),variable[9].getValue());
            }

            swPopulation.Close();

            // Write best subfront
            int frontPointIndex = 0;
            swBestSubfront.WriteLine("Red.br.,income,denivelationFitness,BukBijelaKp,BukBijelaKi,w1,w2,w3,maxFlowChange,FocaKp,FocaKi,Tff,x");

            foreach (Solution sol in bestFront.m_solutionsList)
            {
                Variable[] variable = sol.DecisionVariables;

                swBestSubfront.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                    ++frontPointIndex, (-1) * sol.getObjective(0), (-1) * sol.getObjective(1),
                    variable[0].getValue(), variable[1].getValue(), variable[2].getValue(), variable[3].getValue(), variable[4].getValue(), variable[5].getValue(),
                    variable[6].getValue(), variable[7].getValue(), variable[8].getValue(),variable[9].getValue());
            }
            swBestSubfront.Close();
        }

        
        //public WcfEvaluationServiceLibrary.JobParameters VariablesToJobParameters(Solution s)
        //{
        //    int n = s.numberOfVariables();
        //    Variable[] variable = s.DecisionVariables;
        //    WcfEvaluationServiceLibrary.JobParameters jp = new WcfEvaluationServiceLibrary.JobParameters();
        //    jp.Parameters = new double[n];

        //    for (int i = 0; i < n; i++)
        //    {
        //        jp.Parameters[i] = variable[i].getValue();
        //    }
        //    return jp;
        //}

        //public void JobResultsToObjectives(WcfEvaluationServiceLibrary.EvaluationResult er, Solution s)
        //{
        //    s.setObjective(0, (-1) * er.Result[0]);
        //    s.setObjective(1, (-1) * er.Result[1]);
        //}

        //public string WriteError(Solution s)
        //{
        //    Variable[] variable = s.DecisionVariables;

        //    return "BukBijela_Kp " + variable[0].getValue() +
        //        "BukBijela_Ki " + variable[1].getValue() +
        //        "w1 " + variable[2].getValue() +
        //        "w2 " + variable[3].getValue() +
        //        "w3 " + variable[4].getValue() +
        //        "maxFlowChange " + variable[5].getValue() +
        //        "Foca_Kp " + variable[6].getValue() +
        //        "Foca_Ki " + variable[7].getValue() +
        //        "Tff " + variable[8].getValue() +
        //        "x" + variable[9].getValue();
        //}


        void EvaluationPool.IWBEvaluation.JobResultsToObjectives(EvaluationPool.EvaluationResult er, Solution s)
        {
            s.setObjective(0, (-1) * er.Result[0]);
            s.setObjective(1, (-1) * er.Result[1]);
        }
    }
}

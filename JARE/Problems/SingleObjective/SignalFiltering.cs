using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE;
using JARE.Base;
using JARE.Base.solutionType;
using JARE.Base.variable;
using System.IO;
using JARE.util;
//using Calibration;
using JARE;

namespace JARE.problems.singleObjective
{
    public struct SignalFilteringParameter
    {
        public string Name;
        public double lowerLimit;
        public double upperLimit;
    }

    public class SignalFilteringParameters
    {
        public List<SignalFilteringParameter> parameterList;

        public SignalFilteringParameters()
        {
            parameterList = new List<SignalFilteringParameter>();
        }
    }

    public class SignalFiltering : Problem
    {
        public static SignalFilteringParameters parametersForXML;// = new Parameters();
        public static SignalFilteringParameters parametersFromXML;// = new Parameters();
        public TimeSeries ObservedSignal;

        public SignalFiltering()
        {
            parametersForXML = new SignalFilteringParameters();
            parametersFromXML = new SignalFilteringParameters();

            ReadParameters();

            m_numberOfVariables = parametersFromXML.parameterList.Count;
            m_numberOfObjectives = 1;
            m_numberOfConstraints = 0;
            m_problemName = "SignalFiltering";

            m_solutionType = new RealSolutionType(this);
            m_variableType = new System.Type[m_numberOfVariables];

            m_length = new int[m_numberOfVariables];

            m_variableType[0] = System.Type.GetType("JARE.Base.variable.Real");
            m_lowerLimit = new double[m_numberOfVariables];
            m_upperLimit = new double[m_numberOfVariables];
            for (int i = 0; i < m_numberOfVariables; i++)
            {
                m_lowerLimit[i] = parametersFromXML.parameterList[i].lowerLimit;
                m_upperLimit[i] = parametersFromXML.parameterList[i].upperLimit;
            }

            ReadObservedSignal("Signal.csv");
        }

        public void ReadObservedSignal(string path)
        {
            ObservedSignal = new TimeSeries();
            ObservedSignal.fromCSV(path);            
        }

        private TimeSpan DetermineTimeStep(TimeSeries TS)
        {
            TimeSpan timeStep = TS.GetPointTime(1) - TS.GetPointTime(0);
            return timeStep;
        }

        public override void evaluate(Solution solution)
        {
            int pointCount = ObservedSignal.Count();

            double RMSE = 0.0;
            DateTime startDate = ObservedSignal.GetValue(0).Time;

            for (int i = 0; i < pointCount; i++)
            {
                double observedValue = ObservedSignal.GetValue(i).Value;
                DateTime currentDate = ObservedSignal.GetValue(i).Time;
                TimeSpan ts = currentDate - startDate;

                double calculatedValue = 0.0;
                double initialLevel = solution.DecisionVariables[0].getValue();
                double increase = solution.DecisionVariables[1].getValue();
                calculatedValue += (initialLevel + ts.Days*increase);
                int j = 2;
                while(j < parametersFromXML.parameterList.Count())
                {
                    double amplitude = solution.DecisionVariables[j++].getValue();                    
                    double omega = solution.DecisionVariables[j++].getValue();
                    calculatedValue += amplitude * Math.Sin(omega * ts.Days);
                }

                RMSE += Math.Pow(observedValue - calculatedValue, 2);
            }
            RMSE = Math.Pow(RMSE / pointCount, 0.5);
            solution.Fitness = RMSE;
            solution.setObjective(0, RMSE);
        }

        public TimeSeries getCalculatedTimeSeries(Solution solution)
        {
            TimeSeries calculatedTS = new TimeSeries();
            int pointCount = ObservedSignal.Count();

            DateTime startDate = ObservedSignal.GetValue(0).Time;

            for (int i = 0; i < pointCount; i++)
            {
                double observedValue = ObservedSignal.GetValue(i).Value;
                DateTime currentDate = ObservedSignal.GetValue(i).Time;
                TimeSpan ts = currentDate - startDate;

                double calculatedValue = 0.0;
                double initialLevel = solution.DecisionVariables[0].getValue();
                double increase = solution.DecisionVariables[1].getValue();
                calculatedValue += (initialLevel + ts.Days * increase);
                int j = 2;
                while (j < parametersFromXML.parameterList.Count())
                {
                    double amplitude = solution.DecisionVariables[j++].getValue();
                    double omega = solution.DecisionVariables[j++].getValue();
                    calculatedValue += amplitude * Math.Sin(omega * ts.Days);
                }
                calculatedTS.AddValue(currentDate, calculatedValue, 1.0);
            }
            return calculatedTS;
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

        private void ReadParameters()
        {
            string path;
            //string xmlString = Util.SerializeObject<SignalFilteringParameters>(parametersForXML);
            //path = "SignalFilteringParameters.xml";
            //StreamWriter sw = new StreamWriter(path);
            //sw.Write(xmlString);
            //sw.Close();

            parametersFromXML.parameterList.Clear();
            path = "SignalFilteringParameters.xml";
            parametersFromXML = JARE.util.Util.Deserialize<SignalFilteringParameters>(path);
        }
        public void PrintResults(StreamWriter sw, JARE.Base.SolutionSet resultPopulation, string inputString = "", bool Headline = true)
        {
            string str = string.Empty;
            string strPonder = string.Empty;
            string headLine = string.Empty;
            string strParameters = string.Empty;
            System.Collections.Generic.Dictionary<string, double> parametersForOptimalPortfolio = new System.Collections.Generic.Dictionary<string, double>();

            headLine = "RMSE";

            for (int i = 0; i < parametersFromXML.parameterList.Count; i++)
            {
                headLine += "," + parametersFromXML.parameterList[i].Name;
            }
            sw.WriteLine(headLine);


            for (int i = 0; i < resultPopulation.size(); i++)
            {
                Solution solution = resultPopulation.getSolution(i);

                strPonder = string.Empty;
                for (int kk = 0; kk < solution.numberOfVariables(); kk++)
                {
                    strPonder += (solution.DecisionVariables[kk].getValue().ToString("E6") + ", ");
                }

                str = solution.Fitness.ToString() + "," + strPonder;
                sw.WriteLine(str);
            }
        }

    }
}

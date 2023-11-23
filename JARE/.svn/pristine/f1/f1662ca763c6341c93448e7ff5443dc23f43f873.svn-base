using System;
using System.Collections.Generic;
using System.Text;
using JARE.Base;
using JARE.Base.solutionType;

namespace JARE.problems
{
    public class TimeSerie
    {
        public string name = "";
        public struct DayValue
        {
            public DateTime Day;
            public double Value;
        }

        public System.Collections.Generic.List<DayValue> DataSerie = new System.Collections.Generic.List<DayValue>();
        public TimeSerie(TimeSerie other)
        {
            name = other.name;
            foreach(DayValue tmp in other.DataSerie) DataSerie.Add(tmp);
        }
        public TimeSerie()
        {
        }
        
    }
    public class PortfolioOptimization : Problem
    {
        public enum EvaluationCriteria
        {
            none = 0,
            maximalAverageReturn = 1,
            minimalStandardDeviation = 2,
            maximalSharpIndex = 3,
            maximalSortino = 4,
            maximalVaR = 5,
            maximalcVaR = 6,
            maximalExpWeightedVaR = 7,
            maximalExpWeightedcVaR = 8,
            maximalSkewness = 9,
            maximalKurtosis = 10
        }
        protected System.Collections.Generic.List<TimeSerie> Funds;
        public System.Collections.Generic.List<double> ponder;
        protected System.Collections.Generic.List<double> Return;
        public TimeSerie Portfolio;
        public TimeSerie ReturnList;
        public int pointCountPerMonth;
        protected int evaluationEndDateIndex;
        protected int evaluationPeriod;
        protected int ReturnPeriod;
        protected double targetRate;
        protected double varThreshold;
        public EvaluationCriteria[] evaluationCriteria;
        public double[] weightParameter;
        public double[] fitnessCriteriaExtremeValues;

        public PortfolioOptimization(int numberOfObjectives, System.Collections.Generic.List<TimeSerie> Funds,
                                     DateTime evaluationEndDate, int evaluationPeriod, int ReturnPeriod,
                                     double targetRate, double varThreshold, int[] evaluationCriteria,
                                     double[] weightParameter, double[] fitnessCriteriaExtremeValues) : base()
		{
            this.Funds = Funds;
            this.evaluationCriteria = new EvaluationCriteria[2];
            this.evaluationCriteria[0] = (EvaluationCriteria)evaluationCriteria[0];
            this.evaluationCriteria[1] = (EvaluationCriteria)evaluationCriteria[1];
            this.pointCountPerMonth = 1;
            this.evaluationEndDateIndex = GetEvaluationEndDateIndex(evaluationEndDate);
            this.evaluationPeriod = evaluationPeriod;
            this.ReturnPeriod = ReturnPeriod;
            this.targetRate = targetRate * 0.01;//percentage
            this.varThreshold = varThreshold;
            this.weightParameter = new double[weightParameter.Length];
            this.weightParameter = weightParameter;
            this.fitnessCriteriaExtremeValues = new double[fitnessCriteriaExtremeValues.Length];
            this.fitnessCriteriaExtremeValues = fitnessCriteriaExtremeValues;


            ponder = new System.Collections.Generic.List<double>();
            Return = new System.Collections.Generic.List<double>();
            Portfolio = new TimeSerie();
            ReturnList = new TimeSerie();

            m_numberOfVariables = this.Funds.Count;
            m_numberOfObjectives = numberOfObjectives;
            m_numberOfConstraints = 0;
            m_problemName = "PortfolioOptimization";

            m_lowerLimit = new double[m_numberOfVariables];
            m_upperLimit = new double[m_numberOfVariables];
            for (int i = 0; i < m_numberOfVariables; i++)
            {
                m_upperLimit[i] = 0.0;
                m_lowerLimit[i] = 1.0;
            }
            m_solutionType = new RealSolutionType(this);
        }

        //public Variable[] createVariables()
        //{
        //    System.Threading.Thread.Sleep(100);

        //    //Variable[] variables = base.createVariables();
        //    Variable[] variables = base.createVariables();

        //    Random sampleGenerator = new Random();
        //    Random chromosomeGenerator = new Random();
        //    int[] choosenVariables = new int[variables.Length];
        //    int counter = 0;
        //    int sample;
        //    double valueLimit = 1.0;

        //    for (int i = 0; i < variables.Length; i++) variables[i].setValue(0.0);
        //    while (counter < variables.Length - 1)
        //    {
        //        sample = sampleGenerator.Next(variables.Length);
        //        if (choosenVariables[sample] == 0)
        //        {
        //            variables[sample].setValue(chromosomeGenerator.NextDouble() * valueLimit);
        //            valueLimit = valueLimit - variables[sample].getValue();
        //            counter++;
        //            choosenVariables[sample] = 1;
        //        }
        //    }

        //    for (int i = 0; i < variables.Length; i++)
        //    {
        //        if (variables[i].getValue() == 0.0)
        //        {
        //            variables[i].setValue(valueLimit);
        //            break;
        //        }
        //    }

        //    return variables;
        //} // createVaribles

        public override void evaluate(Solution solution)
        {
            Variable[] variable = solution.DecisionVariables;

            ponder.Clear();
            for (int i = 0; i < m_numberOfVariables; i++) ponder.Add(variable[i].getValue());

            GeneratePortfolio();
            if (m_numberOfObjectives == 1) solution.Fitness = EvaluationFunction();
            else
            {
                double objectiveValue;
                weightParameter[0] = 1.0;
                weightParameter[1] = 0.0;
                objectiveValue = -EvaluationFunction();
                solution.setObjective(0, objectiveValue);
                weightParameter[0] = 0.0;
                weightParameter[1] = 1.0;
                objectiveValue = -EvaluationFunction();
                solution.setObjective(1, objectiveValue);
            }
        }

        private double EvaluationFunction()
        {
            double evaluationParameter = 0.0;
            double[] evaluationParameters = new double[evaluationCriteria.Length];
            int evaluationPeriodDuration = evaluationPeriod * pointCountPerMonth;
            int previousPeriodDuration = ReturnPeriod * pointCountPerMonth;//Return period

            double averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, ReturnList);
            for (int i = 0; i < evaluationCriteria.Length; i++)
            {
                if (evaluationCriteria[i] != EvaluationCriteria.none)
                {
                    switch (evaluationCriteria[i])
                    {
                        case EvaluationCriteria.maximalAverageReturn:
                            evaluationParameters[i] = averageReturn;
                            break;
                        case EvaluationCriteria.minimalStandardDeviation:
                            evaluationParameters[i] = -CalculateStandardDeviationOnPopulation(ReturnList, averageReturn);
                            break;
                        case EvaluationCriteria.maximalSharpIndex:
                            double stdevp = CalculateStandardDeviationOnPopulation(ReturnList, averageReturn);
                            evaluationParameters[i] = CalculateSharpIndex(averageReturn, stdevp, targetRate);
                            break;
                        case EvaluationCriteria.maximalSortino:
                            double downsideRisk = CalculateDownsideRiskOnPopulation(ReturnList, targetRate);
                            evaluationParameters[i] = CalculateSortino(averageReturn, downsideRisk, targetRate);
                            break;
                        case EvaluationCriteria.maximalVaR:
                            double tmp = 0.0;
                            evaluationParameters[i] = CalculateVaR(ReturnList, varThreshold, ref tmp);
                            break;
                        case EvaluationCriteria.maximalcVaR:
                            double cVaR = 0.0;
                            CalculateVaR(ReturnList, varThreshold, ref cVaR);
                            evaluationParameters[i] = cVaR;
                            break;
                        case EvaluationCriteria.maximalExpWeightedVaR:
                            tmp = 0.0;
                            evaluationParameters[i] = CalculateExpWeightedVaR(ReturnList, varThreshold, ref tmp);
                            break;
                        case EvaluationCriteria.maximalExpWeightedcVaR:
                            double expWeightedcVaR = 0.0;
                            CalculateExpWeightedVaR(ReturnList, varThreshold, ref expWeightedcVaR);
                            evaluationParameters[i] = expWeightedcVaR;
                            break;
                        case EvaluationCriteria.maximalSkewness:
                            double Skewness = 0.0;
                            Skewness = CalculateSkewness(evaluationPeriodDuration, previousPeriodDuration);
                            evaluationParameters[i] = Skewness;
                            break;
                        case EvaluationCriteria.maximalKurtosis:
                            double Kurtosis = 0.0;
                            Kurtosis = CalculateKurtosis(evaluationPeriodDuration, previousPeriodDuration);
                            evaluationParameters[i] = Kurtosis;
                            break;
                    }
                }
                evaluationParameter += evaluationParameters[i] * weightParameter[i] / fitnessCriteriaExtremeValues[i];
            }
            return evaluationParameter;
        }

        public double GenerateReturnList(int evaluationPeriodDuration, int previousPeriodDuration, TimeSerie SolutionTimeSerie)
        {
            bool bReturnListInput = false;
            SolutionTimeSerie.DataSerie.Clear();
            double Return;
            double ReturnSum = 0.0;
            double currentValue;
            double previousValue;
            int startIndex = Portfolio.DataSerie.Count - evaluationPeriodDuration;
            TimeSerie.DayValue dayValue = new TimeSerie.DayValue();
            if (bReturnListInput == true)
            {
                for (int i = 0; i < evaluationPeriodDuration; i++)
                {
                    SolutionTimeSerie.DataSerie.Add(Portfolio.DataSerie[startIndex + i]);
                }
            }
            else
            {
                for (int i = 0; i < evaluationPeriodDuration; i++)
                {
                    currentValue = Portfolio.DataSerie[startIndex + i].Value;
                    previousValue = Portfolio.DataSerie[startIndex - previousPeriodDuration + i].Value;
                    Return = (currentValue - previousValue) / previousValue;
                    ReturnSum += Return;

                    dayValue.Day = Portfolio.DataSerie[startIndex + i].Day;
                    dayValue.Value = Return;

                    SolutionTimeSerie.DataSerie.Add(dayValue);
                }
            }

            double averageReturn = (ReturnSum / evaluationPeriodDuration);
            return averageReturn;
        }

        private double CalculateStandardDeviationOnPopulation(TimeSerie InputTimeSerie, double averageOfTimeSerie)
        {
            double standardDeviation = 0.0;
            double tmpSum = 0.0;

            tmpSum = 0.0;
            for (int i = 0; i < InputTimeSerie.DataSerie.Count; i++)
            {
                tmpSum += (InputTimeSerie.DataSerie[i].Value - averageOfTimeSerie) * (InputTimeSerie.DataSerie[i].Value - averageOfTimeSerie);
            }
            standardDeviation = Math.Pow(tmpSum / InputTimeSerie.DataSerie.Count, 0.5);
            return standardDeviation;
        }

        private double CalculateDownsideRiskOnPopulation(TimeSerie InputTimeSerie, double targetRate)
        {
            double downsideRisk = 0.0;
            double tmpSum = 0.0;
            for (int i = 0; i < InputTimeSerie.DataSerie.Count; i++)
            {
                tmpSum += (InputTimeSerie.DataSerie[i].Value - targetRate) * (InputTimeSerie.DataSerie[i].Value - targetRate);
            }
            downsideRisk = Math.Pow(tmpSum / InputTimeSerie.DataSerie.Count, 0.5);
            return downsideRisk;
        }

        private double CalculateVaR(TimeSerie InputTimeSerie, double varThreshold, ref double cVaR)
        {
            TimeSerie InputTimeSerieCopy = new TimeSerie(InputTimeSerie);
            int numberOfWorstReturns = (int)(InputTimeSerie.DataSerie.Count * varThreshold / 100.0 + 0.5);
            double VaR = 1.0e+10;
            cVaR = 0.0;
            int removedReturnIndex = 0;

            for (int i = 0; i < numberOfWorstReturns; i++)
            {
                VaR = 1.0e+10;
                for (int j = 0; j < InputTimeSerieCopy.DataSerie.Count; j++)
                {
                    if (InputTimeSerieCopy.DataSerie[j].Value < VaR)
                    {
                        VaR = InputTimeSerieCopy.DataSerie[j].Value;
                        removedReturnIndex = j;
                    }
                }
                InputTimeSerieCopy.DataSerie.RemoveAt(removedReturnIndex);
                cVaR += VaR;
            }
            cVaR /= numberOfWorstReturns;
            return VaR;
        }
        private double CalculateExpWeightedVaR(TimeSerie InputTimeSerie, double varThreshold, ref double cVaR)
        {
            TimeSerie InputTimeSerieCopy = new TimeSerie(InputTimeSerie);
            double VaR = 1.0e+10;
            cVaR = 0.0;
            int removedReturnIndex = 0;
            int numberOfWorstReturns = 0;
            double acumulatedProbability = 0.0;
            double lambda = 0.9;
            int origCount = InputTimeSerieCopy.DataSerie.Count;
            List<int> removedIndexes = new List<int>();

            while (acumulatedProbability <= varThreshold*0.01)
            {
                VaR = 1.0e+10;
                for (int j = 0; j < InputTimeSerieCopy.DataSerie.Count; j++)
                {
                    if (InputTimeSerieCopy.DataSerie[j].Value < VaR)
                    {
                        VaR = InputTimeSerieCopy.DataSerie[j].Value;
                        removedReturnIndex = j;
                    }
                }
                int origIndex = removedReturnIndex;
                for (int i = removedIndexes.Count-1; i >= 0; i--) 
                {
                    if(origIndex >= removedIndexes[i]) origIndex++;
                }
                removedIndexes.Add(removedReturnIndex);
                double power = (origCount - 1) - origIndex;
                acumulatedProbability += Math.Pow(lambda,power)*(1.0-lambda);

                InputTimeSerieCopy.DataSerie.RemoveAt(removedReturnIndex);
                numberOfWorstReturns++;
                cVaR += VaR;
            }
            cVaR /= numberOfWorstReturns;
            return VaR;
        }
        public double CalculateSkewness(int evaluationPeriodDuration, int previousPeriodDuration)
        {
            double averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, ReturnList);
            double standardDeviation = CalculateStandardDeviationOnPopulation(ReturnList, averageReturn);

            double tmpSum = 0.0;
            for (int i = 0; i < ReturnList.DataSerie.Count; i++)
            {
                tmpSum += Math.Pow(ReturnList.DataSerie[i].Value - averageReturn, 3.0);
            }
            double Skewness = (evaluationPeriodDuration * tmpSum) / ((evaluationPeriodDuration - 1) * (evaluationPeriodDuration - 2) * Math.Pow(standardDeviation, 3.0));
            return Skewness;
        }

        public double CalculateKurtosis(int evaluationPeriodDuration, int previousPeriodDuration)
        {
            double averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, ReturnList);
            double standardDeviation = CalculateStandardDeviationOnPopulation(ReturnList, averageReturn);

            double tmp = 0.0;
            for (int i = 0; i < ReturnList.DataSerie.Count; i++)
            {
                tmp += Math.Pow(ReturnList.DataSerie[i].Value - averageReturn, 4.0);
            }
            int n = evaluationPeriodDuration;
            double Kurtosis = (n * (n + 1) * tmp) / ((n - 1) * (n - 2) * (n - 3) * Math.Pow(standardDeviation, 4.0)) - 3 * (n - 1) * (n - 1) / ((n - 2) * (n - 3));
            return Kurtosis;
        }

        private double CalculateSharpIndex(double averageReturn, double stdevp, double interestRate)
        {
            double sharpIndex = (averageReturn - interestRate) / stdevp;
            return sharpIndex;
        }
        private double CalculateSortino(double averageReturn, double downsideRisk, double interestRate)
        {
            double sortinoIndex = (averageReturn - interestRate) / downsideRisk;
            return sortinoIndex;
        }

        public void GeneratePortfolio()
        {
            Portfolio.name = "Portfolio";

            int periodDuration = (evaluationPeriod + ReturnPeriod) * pointCountPerMonth;

            Portfolio.DataSerie.Clear();
            int startIndex = evaluationEndDateIndex - periodDuration + 1;

            for (int i = 0; i < periodDuration; i++)
            {
                TimeSerie.DayValue dayValue = new TimeSerie.DayValue();
                dayValue.Day = Funds[0].DataSerie[startIndex + i].Day;
                dayValue.Value = 0.0;
                for (int j = 0; j < Funds.Count; j++) dayValue.Value += ponder[j] * Funds[j].DataSerie[startIndex + i].Value;
                Portfolio.DataSerie.Add(dayValue);
            }
        }
        protected int GetEvaluationEndDateIndex(DateTime evaluationEndDate)
        {
            int endDateIndex = Funds[0].DataSerie.Count - 1;
            for (int i = 0; i < Funds[0].DataSerie.Count; i++)
            {
                if (evaluationEndDate == Funds[0].DataSerie[i].Day)
                {
                    endDateIndex = i;
                    break;
                }
            }
            return endDateIndex;
        }

        public string GetParametersForOptimalPortfolio(double[] parametersForOptimalPortfolio)
        {
            //none = 0,
            //maximalAverageReturn = 1,
            //minimalStandardDeviation = 2,
            //maximalSharpIndex = 3,
            //maximalSortino = 4,
            //maximalVaR = 5,
            //maximalExpWeightedVaR = 6,
            //maximalcVaR = 7,
            //maximalSkewness = 8
           
            GeneratePortfolio();
            string str = "";
            parametersForOptimalPortfolio[0] = 0.0;
            //Calculating average Return
            int evaluationPeriodDuration = evaluationPeriod * pointCountPerMonth;
            int previousPeriodDuration = ReturnPeriod * pointCountPerMonth;//Return period

            double tmp = 0.0;
            double averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, ReturnList); ;
            str += "averageReturn=" + "," + averageReturn.ToString("F10");
            parametersForOptimalPortfolio[1] = averageReturn;

            //Calculating standard deviation and downside risk
            double standardDeviation = CalculateStandardDeviationOnPopulation(ReturnList, averageReturn);
            str += ", standardDeviation=" + "," + standardDeviation.ToString("F10");
            parametersForOptimalPortfolio[2] = standardDeviation;
            double downsideRisk = CalculateDownsideRiskOnPopulation(ReturnList, targetRate);
            //str += ", downsideRisk=" + "," + downsideRisk.ToString("F10");
            double sharpIndex = CalculateSharpIndex(averageReturn, standardDeviation, targetRate);
            str += ", sharpIndex=" + "," + sharpIndex.ToString("F10");
            parametersForOptimalPortfolio[3] = sharpIndex;
            double sortinoIndex = CalculateSortino(averageReturn, downsideRisk, targetRate);
            str += ", sortinoIndex=" + "," + sortinoIndex.ToString("F10");
            parametersForOptimalPortfolio[4] = sortinoIndex;

            double cVaR = 0.0;
            double VaR = CalculateVaR(ReturnList, varThreshold, ref cVaR);
            str += ", VaR=" + "," + VaR.ToString("F10");
            parametersForOptimalPortfolio[5] = VaR;
            str += ", cVaR=" + "," + cVaR.ToString("F10");
            parametersForOptimalPortfolio[6] = cVaR;

            double expcVaR = 0.0;
            double expVaR = CalculateExpWeightedVaR(ReturnList, varThreshold, ref expcVaR);
            str += ", expVaR=" + "," + expVaR.ToString("F10");
            parametersForOptimalPortfolio[7] = expVaR;
            str += ", expcVaR=" + "," + expcVaR.ToString("F10");
            parametersForOptimalPortfolio[8] = expcVaR;

            tmp = 0.0;
            for (int i = 0; i < ReturnList.DataSerie.Count; i++)
            {
                tmp += Math.Pow(ReturnList.DataSerie[i].Value - averageReturn, 3.0);
            }
            int periodDuration = ReturnList.DataSerie.Count;
            int n = periodDuration;
            double Skewness = (n * tmp) / ((n - 1) * (n - 2) * Math.Pow(standardDeviation, 3.0));
            str += ", Skewness=" + "," + Skewness.ToString("F10");
            parametersForOptimalPortfolio[9] = Skewness;
            
            tmp = 0.0;
            for (int i = 0; i < ReturnList.DataSerie.Count; i++)
            {
                tmp += Math.Pow(ReturnList.DataSerie[i].Value - averageReturn, 4.0);
            }
            double Kurtosis = (n*(n+1)*tmp) / ((n-1)*(n-2)*(n-3)*Math.Pow(standardDeviation, 4.0))-3*(n-1)*(n-1)/((n-2)*(n-3));
            str += ", Kurtosis" + "," + Kurtosis.ToString("F10");
            parametersForOptimalPortfolio[10] = Kurtosis;

            return str;
        }
    }
}

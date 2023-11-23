using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using JARE.Base;
using JARE.Base.solutionType;
using JARE.problems.Finance;
using JARE.problems.Finance.DataTypes;
using JARE.problems.Finance.Parameters;
using PortfolioOptimizationGA = JARE.metaheuristics.singleObjective.geneticAlgorithm.PortfolioOptimizationGA;


namespace JARE.problems
{
    public class Backtesting
    {
        public ParameterCollection m_parameters;
        PortfolioOptimizationSO po;

        private AssetSet Assets;
        DateTime backtestingDate;
        int evaluationPeriod;
        int ReturnPeriod;
        DecisionVariableType variableType;
        int backtestingPeriod;
        EvaluationCriteria EC;
        bool Canceled = false;
        ExecutionType executionType;

        public delegate void RebalanceDayChangedHandler(int rebalanceDayCounter);
        public event RebalanceDayChangedHandler RebalanceDayChanged;
        public delegate void GenerationChangedHandler(string generationCounter);
        public event GenerationChangedHandler GenerationChanged;

        public Backtesting(EvaluationCriteria EC, DateTime backtestingDate, int backtestingPeriod, DecisionVariableType variableType = DecisionVariableType.sharesInvestedRealValues, ExecutionType executionType = ExecutionType.singleThread)
		{
            this.backtestingDate = backtestingDate;
            this.variableType = variableType;
            this.backtestingPeriod = backtestingPeriod;
            this.EC = EC;
            m_parameters = new ParameterCollection();
            this.executionType = executionType;
        }

        public double Execute(StreamWriter sw = null)
        {
            Assets = (AssetSet)m_parameters.getParameter("RebalanceAssetTimeSeriesSet");
            evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
            ReturnPeriod = (int)m_parameters.getParameter("returnEvaluationPeriod");

            int violationsCount = 0;
            int correctionCount = 0;
            double capitalRequirements = -1;
            bool InR = true;
            if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.VaRGarch && InR)
            {
                ExecuteInR(out capitalRequirements, out violationsCount, out correctionCount);
            }
            else
            {

                JARE.Base.SolutionSet s = new SolutionSet();
                DateTime currentDate = backtestingDate;
                DateTime backtestingStartDate = GetBacktestingStartDate();
                double[] weights = (double[])m_parameters.getParameter("activePortfolio");
                TimeSeries returnTimeSeries = new TimeSeries();
                TimeSeries riskTimeSeries = new TimeSeries();

                AssetSet evaluationTimeSeriesSet = Assets;
                int counter = 0;
                int i = 0;
                while (counter <= backtestingPeriod)//jedan dan vise da bi se uporedila ocena VaR za sutra i realizovani return
                {
                    if (Canceled) break;
                    currentDate = backtestingStartDate.AddDays(i);
                    //Get today's assets values
                    double[] currentAssetsValues = new double[evaluationTimeSeriesSet.Count];
                    if (!evaluationTimeSeriesSet.GetValues(currentDate, currentAssetsValues))
                    {
                        i++;
                        continue;
                    }

                    int currentIndex;
                    if (!Assets.GetItemIndex(currentDate, out currentIndex)) throw new Exception("Input Date is not valid.");
                    //DateTime optimizationDate = Assets.GetDay(currentIndex - evaluationPeriod + 1);
                    if (po == null)
                    {
                        po = new PortfolioOptimizationSO(EC, variableType, evaluationTimeSeriesSet.Count, evaluationTimeSeriesSet.Count, 0.0, 1.0);
                        po.m_parameters.setParameter("optimizationDate", currentDate);
                        po.m_parameters.setParameter("AssetTimeSeriesSet", Assets);
                        po.m_parameters.setParameter("returnEvaluationPeriod", ReturnPeriod);
                        po.m_parameters.setParameter("evaluationPeriod", evaluationPeriod);
                        po.m_parameters.setParameter("varThreshold", (double)m_parameters.getParameter("varThreshold"));
                        po.m_parameters.setParameter("targetRate", m_parameters.getParameter("targetRate"));
                        po.m_parameters.setParameter("lambda", m_parameters.getParameter("lambda"));
                        if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.VaRGarch)
                        {
                            po.m_parameters.setParameter("RScriptPath", (string)m_parameters.getParameter("RScriptPath"));
                        }
                        po.Init();
                    }
                    else
                    {
                        po.m_parameters.setParameter("optimizationDate", currentDate, true);
                    }
                    JARE.Base.Solution Portfolio = new Solution(po);
                    for (int j = 0; j < weights.Length; j++) Portfolio.DecisionVariables[j].setValue(weights[j]);

                    System.Collections.Generic.Dictionary<string, double> portfolioParameters = new System.Collections.Generic.Dictionary<string, double>();
                    GetCurrentParameters(po, Portfolio, portfolioParameters);

                    returnTimeSeries.DataSeries.Add(new DayValue(currentDate, portfolioParameters["actualReturn"]));
                    riskTimeSeries.DataSeries.Add(new DayValue(currentDate, portfolioParameters[((EvaluationCriteriaReturnBased)EC).CriteriaType.ToString()]));

                    bool headline = true;
                    if (counter > 0) headline = false;
                    if(sw != null) PrintResults(sw, portfolioParameters, Portfolio, currentDate.ToShortDateString() + ",", headline);

                    counter++;
                    i++;

                    // raise event
                    OnRebalanceDayChanged(counter);
                }

                if (Canceled) return capitalRequirements;

                capitalRequirements = CalculateCapitalRequirements(returnTimeSeries, riskTimeSeries, ref violationsCount, ref correctionCount);
            }
            if (sw != null)
            {
                sw.WriteLine("Correction Count," + correctionCount.ToString("F10"));
                sw.WriteLine("Violations Count," + violationsCount.ToString("F10"));
                sw.WriteLine("Capital Requirements," + capitalRequirements.ToString("F10"));
            }
            
            return capitalRequirements;
        }

        public void ExecuteInR(out double capitalRequirements, out int violationsCount, out int correctionCount)
        {
            double VaRThreshold = (double)m_parameters.getParameter("varThreshold");
            double[] weights = (double[])m_parameters.getParameter("activePortfolio");//Shares invested in portfolio
            string RScriptPath = (string)m_parameters.getParameter("RScriptPath");
            AssetSet stressTimeSeriesSet = (AssetSet)m_parameters.getParameter("stressTimeSeriesSet");
            List<EvaluationCriteria> evaluationCriteriaList = new List<EvaluationCriteria>();
            EvaluationCriteriaReturnBased EC = new EvaluationCriteriaReturnBased();
            ((EvaluationCriteriaReturnBased)EC).CriteriaType = EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR;
            evaluationCriteriaList.Add(EC);

            RCapitalRequirements CRInR = new RCapitalRequirements(Assets, backtestingDate, evaluationPeriod, backtestingPeriod, ReturnPeriod, variableType, VaRThreshold, RScriptPath, executionType, evaluationCriteriaList, stressTimeSeriesSet);
            CRInR.CalculateCapitalRequirements(weights, out capitalRequirements, out violationsCount, out correctionCount);
        }

        private void GetCurrentParameters(PortfolioOptimizationSO portfolioOptimization, JARE.Base.Solution solution, System.Collections.Generic.Dictionary<string, double> portfolioParameters)
        {
            portfolioOptimization.solution = solution;
            portfolioOptimization.GetParametersForOptimalPortfolio(solution, portfolioParameters);
            portfolioOptimization.CopyGARCHParameters();
        }

        private double CalculateCapitalRequirements(TimeSeries returnTimeSeries, TimeSeries inputRiskTimeSeries, ref int violationsCounter, ref int correctionCount)
        {
            double capitalRequirements;
            //int violationsCounter = 0;
            double averageRisk = 0.0;
            int averageRiskCalculationPeriod = 60;

            //Correction of exceptions in GARCH VaR model
            TimeSeries riskTimeSeries = new TimeSeries(inputRiskTimeSeries);
            CorrectGarchVaRExceptions(riskTimeSeries, ref correctionCount);

            for (int i = 1; i < returnTimeSeries.DataSeries.Count; i++)
            {
                double todayDayReturn = returnTimeSeries.DataSeries[i].Value;
                double riskEstimationForToday = riskTimeSeries.DataSeries[i-1].Value;
                if (todayDayReturn < -riskEstimationForToday) violationsCounter++;
            }

            for (int i = 0; i < averageRiskCalculationPeriod; i++)
            {
                averageRisk += riskTimeSeries.DataSeries[returnTimeSeries.DataSeries.Count - 1 - averageRiskCalculationPeriod + i].Value;
            }

            averageRisk /= averageRiskCalculationPeriod;

            double k = 0.0;
            if (violationsCounter <= 4) k = 0.0;
            else if (violationsCounter == 5) k = 0.4;
            else if (violationsCounter == 6) k = 0.5;
            else if (violationsCounter == 7) k = 0.65;
            else if (violationsCounter == 8) k = 0.75;
            else if (violationsCounter == 9) k = 0.85;
            else if (violationsCounter >= 10) k = 1;

            double Comp1 = riskTimeSeries.DataSeries[riskTimeSeries.DataSeries.Count - 1].Value;
            double Comp2 = (3 + k) * averageRisk;

            capitalRequirements = Math.Max(Comp1, Comp2) * Math.Sqrt(10);

            return capitalRequirements; 
        }
        
        private void CorrectGarchVaRExceptions(TimeSeries riskTimeSeries, ref int correctionCount)
        {
            correctionCount = 0;
            double firstValueDifferentFromOne = 0.0;
            if(((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.VaRGarch)
            {
                //Find the first VaR estimation within time series successfully calculated with GARCH model
                foreach(DayValue val in riskTimeSeries.DataSeries)
                {
                    if (val.Value != 1)
                    {
                        firstValueDifferentFromOne = val.Value;
                        break;
                    }
                }
                //Asign the first value in time series the first successfully calculated VaR estimation
                riskTimeSeries.DataSeries[0].Value = firstValueDifferentFromOne;
                //For each Var estimation if calculation was not successfull asign previous value
                for (int i = 1; i < riskTimeSeries.DataSeries.Count; i++)
                {
                    if (riskTimeSeries.DataSeries[i].Value == 1)
                    {
                        riskTimeSeries.DataSeries[i].Value = riskTimeSeries.DataSeries[i - 1].Value;
                        correctionCount++;
                    }
                }
            }
        }

        public void PrintResults(StreamWriter sw, System.Collections.Generic.Dictionary<string, double> portfolioParameters, JARE.Base.Solution solution, string inputString = "", bool Headline = true)
        {
            string str = string.Empty;
            string strPonder = string.Empty;
            string headLine = string.Empty;
            string strParameters = string.Empty;
            bool headLineWritten = false;

            str = string.Empty;
            //Write head line
            if (Headline == true && !headLineWritten)
            {
                System.Collections.Generic.Dictionary<string, double>.Enumerator HeadLineEnum = portfolioParameters.GetEnumerator();
                for (int kk = 0; kk < portfolioParameters.Count; kk++)
                {
                    HeadLineEnum.MoveNext();
                    headLine += (HeadLineEnum.Current.Key + ",");
                }
                HeadLineEnum.Dispose();
                for (int j = 1; j <= solution.DecisionVariables.Length; j++) str += "ponder " + j.ToString() + ",";
                headLine += str;
                if (inputString != "") headLine = " ," + headLine;
                sw.WriteLine(headLine);
                headLineWritten = true;
            }
            //Write parameters
            System.Collections.Generic.Dictionary<string, double>.Enumerator Enum = portfolioParameters.GetEnumerator();
            strParameters = string.Empty;
            for (int kk = 0; kk < portfolioParameters.Count; kk++)
            {
                Enum.MoveNext();
                strParameters += (Enum.Current.Value.ToString("F10") + ",");
            }

            strPonder = string.Empty;
            for (int kk = 1; kk <= solution.DecisionVariables.Length; kk++)
            {
                strPonder += (solution.DecisionVariables[kk - 1].getValue().ToString("E6") + ", ");
            }

            str = inputString + strParameters + strPonder;
            sw.WriteLine(str);
        }

        protected DateTime GetBacktestingStartDate()
        {
            DateTime backtestingStartDate;
            int backtestingDateIndex;

            if (!Assets.GetItemIndex(backtestingDate, out backtestingDateIndex)) throw new Exception("Input Date is not valid.");
            int backtestingStartDateIndex = backtestingDateIndex - backtestingPeriod;
            backtestingStartDate = Assets.GetDay(backtestingStartDateIndex);

            return backtestingStartDate;
        }

        public void OnRebalanceDayChanged(int rebalanceDayCounter)
        {
            if (RebalanceDayChanged != null) RebalanceDayChanged(rebalanceDayCounter);
        }
        public void OnGenerationChanged(string generationChanged)
        {
            if (GenerationChanged != null) GenerationChanged(generationChanged); 
        }
        public void OperationCanceled(bool Canceled)
        {
            this.Canceled = Canceled;
        }
    }
}

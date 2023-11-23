using System;
using System.Collections.Generic;
using System.Text;
using JARE.Base;
using JARE.Base.solutionType;
using Microsoft;
using RDotNet;
using System.Linq;
using System.IO;
using System.Threading;
using JARE.problems.Finance.DataTypes;
using JARE.problems.Finance.Parameters;
using JARE.EvaluationPool;
using JARE.problems;


namespace JARE.problems.Finance
{
    public enum ExecutionType
    {
        singleThread,
        viaBinder
    }
    
    public abstract class PortfolioOptimization : Problem, JARE.EvaluationPool.IWBEvaluation
    {
        /// <summary>
        /// List of predefined optimal solutions
        /// </summary>
        protected Dictionary<string, Solution> m_initialSolutions = null;
 
        /// <summary>
        /// Additional parameters
        /// </summary>
        public ParameterCollection m_parameters;
 
        /// <summary>
        /// Current solution
        /// </summary>
        public Solution solution;

        /// <summary>
        /// Time series of portfolio values
        /// </summary>
        public TimeSeries PortfolioTimeSeries;
 
        /// <summary>
        /// Time series of portfolio Credit Exposure values
        /// </summary>
        public TimeSeries PortfolioCreditExposureTimeSeries;

        /// <summary>
        /// Time series for criteria evaluation (time series of portfolio returns, time series of portfolio credit exposure)
        /// </summary>
        public TimeSeries EvaluationTimeSeries;

        /// <summary>
        /// The optimization date
        /// </summary>
        //public DateTime optimizationDate;

        /// <summary>
        /// Return period; Default = 1: 1-day return.
        /// </summary>
        protected int ReturnPeriod = 1;

        /// <summary>
        /// Portfolio weights type (constant capital invested, capital invested, shares invested, shares proportion invested)
        /// </summary>
        public DecisionVariableType m_VariableType;

        /// <summary>
        /// Portfolio cardinality (maximal number of assets included in portfolio)
        /// </summary>
        protected int m_Cardinality;

        /// <summary> RETURN_BASED_CRITERIA represents class JARE.problems.Finance.DataTypes.EvaluationCriteriaReturnBased</summary>
        public static System.Type RETURN_BASED_CRITERIA;
        /// <summary> CREDIT_BASED_CRITERIA represents class JARE.problems.Finance.DataTypes.EvaluationCriteriaCreditExposureBased</summary>
        public static System.Type CREDIT_BASED_CRITERIA;
        /// <summary> PARAMETER_BASED_CRITERIA represents class JARE.problems.Finance.DataTypes.EvaluationCriteriaParameterBased</summary>
        public static System.Type PARAMETER_BASED_CRITERIA;
        /// <summary>
        /// TIMESERIES_BASED_CRITERIA represents class JARE.problems.Finance.DataTypes.EvaluationCriteriaTimeSeriesBased
        /// </summary>
        public static System.Type TIMESERIES_BASED_CRITERIA;

        //VaRGARCH members
        protected VaRGARCH m_VaRGARCH;
        
        //GARCH parameters
        protected internal Dictionary<string, Object> m_GARCHparameters;

        //Number of evaluated individuals 
        protected int totalIndividuals = 0;

        //Number of individuals failed to evaluate
        protected int failedIndividuals = 0;

        //Number of consecutive failed evaluations
        protected int consecutiveFailedEvaluations = 0;

        //Stream writer for printing failed individuals into file
        protected StreamWriter writer = null;

        //Execution type of problem solving
        public ExecutionType executionType = ExecutionType.singleThread;
        
        /// <summary>
        /// Instance of class for calculation capital requirements in R using script file
        /// </summary>
        protected RCapitalRequirements CRInR;


        public PortfolioOptimization(DecisionVariableType variableType, int numberOfVariables, int cardinality, double lowerBound, double upperBound, ExecutionType executionType)
            : base()
		{
            m_parameters = new ParameterCollection();
            m_GARCHparameters = new Dictionary<string, Object>();
            m_numberOfVariables = numberOfVariables;
            m_problemName = "PortfolioOptimization";

            m_Cardinality = cardinality;
            this.executionType = executionType;

            //this.optimizationDate = optimizationDate;
            //this.evaluationPeriod = evaluationPeriod;
            //m_parameters.setParameter("returnEvaluationPeriod", returnEvaluationPeriod);
            m_VariableType = variableType;

            PortfolioTimeSeries = new TimeSeries();
            EvaluationTimeSeries = new TimeSeries();
            PortfolioCreditExposureTimeSeries = new TimeSeries();
            
            RETURN_BASED_CRITERIA = System.Type.GetType("JARE.problems.Finance.DataTypes.EvaluationCriteriaReturnBased");
            CREDIT_BASED_CRITERIA = System.Type.GetType("JARE.problems.Finance.DataTypes.EvaluationCriteriaCreditExposureBased");
            PARAMETER_BASED_CRITERIA = System.Type.GetType("JARE.problems.Finance.DataTypes.EvaluationCriteriaParameterBased");
            TIMESERIES_BASED_CRITERIA = System.Type.GetType("JARE.problems.Finance.DataTypes.EvaluationCriteriaTimeSeriesBased");
            
            m_numberOfConstraints = 0;

            m_lowerLimit = new double[m_numberOfVariables];
            m_upperLimit = new double[m_numberOfVariables];
            for (int i = 0; i < m_numberOfVariables; i++)
            {
                m_upperLimit[i] = upperBound;
                m_lowerLimit[i] = lowerBound;
            }

            if (m_VariableType == DecisionVariableType.sharesInvestedIntValues)
            {
                m_solutionType = new IntSolutionType(this);
            }
            else
            {
                RealWeightsSolutionType m_solutionType = (RealWeightsSolutionType) new RealWeightsSolutionType(this);
                m_solutionType.Cardinality = m_Cardinality;
            }
        }



        protected DateTime GetEvaluationStartDate(DateTime OptimizationDate, AssetSet input, int evaluationPeriod, int returnPeriod)
        {
            int optimizationDateIndex;
            input.GetItemIndex(OptimizationDate, out optimizationDateIndex);
            int startIndex = optimizationDateIndex - evaluationPeriod + 1;
            DateTime StartDate = input.GetDay(startIndex);
            return StartDate;
        }

        #region Evaluation time series generation

        protected void GeneratePortfolioTimeSeries(Solution portfolioSolution, AssetSet input, TimeSeries output)
        {
            DateTime optimizationDate = (DateTime)m_parameters.getParameter("optimizationDate");
            int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
            if (!input.Validated) DataValidation(input);

            int periodDuration = (evaluationPeriod + ReturnPeriod);
            output.DataSeries.Clear();
            DateTime evaluationStartDate = GetEvaluationStartDate(optimizationDate, input, evaluationPeriod, ReturnPeriod);
            int evaluationStartDateIndex;
            input.GetItemIndex(evaluationStartDate, out evaluationStartDateIndex);
            int StartDateIndex = evaluationStartDateIndex - ReturnPeriod;

            if (m_VariableType == DecisionVariableType.sharesInvestedRealValues || 
                m_VariableType == DecisionVariableType.constantCapitalInvested ||
                m_VariableType == DecisionVariableType.sharesInvestedIntValues)
            {
                for (int i = 0; i < periodDuration; i++)
                {
                    DayValue dayValue = new DayValue();
                    dayValue.Day = input.GetDayValue(0, StartDateIndex + i).Day;
                    dayValue.Value = 0.0;
                    for (int j = 0; j < input.Count; j++) dayValue.Value += portfolioSolution.DecisionVariables[j].getValue() * input.GetDayValue(j, StartDateIndex + i).Value;
                    output.DataSeries.Add(dayValue);
                }
            }
            else if (m_VariableType == DecisionVariableType.capitalInvested)
            {
                double[] sharesInvested = new double[input.Count];
                double[] assetsValues = new double[input.Count];
                //Pretpostavka je da je pocetna vrednost portfolija 1.
                double portfolioValue = 1.0;
                //If portfolio value is set outside read input value (Portfolio rebalance case)
                if (m_parameters.ContainsParameter("portfolioValue")) portfolioValue = (double)m_parameters.getParameter("portfolioValue");
                //Poslednji dan evaluacije
                int evaluationEndDateIndex = evaluationStartDateIndex + evaluationPeriod - 1;
                //Vrednost holdingsa se racuna na osnovu capital invested weights i vrednosti aseta na poslednji dan evaluacije
                //zato sto capital invested weights predstavljaju strukturu portfolija poslednjeg dana evaluacije
                if (!input.GetValues(input.GetDayValue(0, evaluationEndDateIndex).Day, assetsValues)) throw new Exception("Asset values not found for specified date");
                //Odredjivanje holdingsa na poslednji dan evaluacije
                for (int j = 0; j < input.Count; j++) sharesInvested[j] = (portfolioSolution.DecisionVariables[j].getValue() * portfolioValue) / assetsValues[j];

                for (int i = 0; i < periodDuration; i++)
                {
                    DayValue dayValue = new DayValue();
                    dayValue.Day = input.GetDayValue(0, StartDateIndex + i).Day;
                    dayValue.Value = 0.0;
                    for (int j = 0; j < input.Count; j++) dayValue.Value += sharesInvested[j] * input.GetDayValue(j, StartDateIndex + i).Value;
                    output.DataSeries.Add(dayValue);
                }
            }
            //else if (m_VariableType == DecisionVariableType.sharesInvested) throw new Exception("Function GeneratePortfolioTimeSeries not implemented for specified decision variable");
        }


        protected void GeneratePortfolioCreditExposureTimeSeries(AssetSet creditExposureTimeSeriesSet, AssetSet assetTimeSeriesSet, TimeSeries output)
        {
            DateTime optimizationDate = (DateTime)m_parameters.getParameter("optimizationDate");
            int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
            if (!creditExposureTimeSeriesSet.Validated) DataValidation(creditExposureTimeSeriesSet);
            if (!assetTimeSeriesSet.Validated) DataValidation(assetTimeSeriesSet);
            //Portfolio credit exposure time series has length evaluation period + 1 in oder to calculate difference based criteria
            int periodDuration = (evaluationPeriod + 1);

            output.DataSeries.Clear();
            DateTime evaluationStartDate = GetEvaluationStartDate(optimizationDate, creditExposureTimeSeriesSet, evaluationPeriod, ReturnPeriod);
            int assetsStartDateIndex;
            creditExposureTimeSeriesSet.GetItemIndex(evaluationStartDate, out assetsStartDateIndex);
            int assetsStartIndex = assetsStartDateIndex - 1;
            
            int CDSStartDateIndex;
            assetTimeSeriesSet.GetItemIndex(evaluationStartDate, out CDSStartDateIndex);
            int CDSStartIndex = CDSStartDateIndex - 1;

            double[] sharesInvested = new double[creditExposureTimeSeriesSet.Count];
            double[] currentCapitalInvested = new double[creditExposureTimeSeriesSet.Count];
            double[] assetsValues = new double[creditExposureTimeSeriesSet.Count];
            double portfolioValue = 1.0;//Pretpostavka je da je pocetna vrednost portfolija 1.

            if (!creditExposureTimeSeriesSet.GetValues(creditExposureTimeSeriesSet.GetDay(assetsStartIndex), assetsValues)) throw new Exception("Asset values not found for specified date");
            
            //Calculate shares invested in assets
            if (m_VariableType == DecisionVariableType.capitalInvested)
            {
                for (int j = 0; j < creditExposureTimeSeriesSet.Count; j++) sharesInvested[j] = (solution.DecisionVariables[j].getValue() * portfolioValue) / assetsValues[j];
            }
            else if (m_VariableType == DecisionVariableType.sharesInvestedRealValues)
            {
                double[] capitalInvested = new double[solution.numberOfVariables()];

                double capitalInvestedSum = 0.0;
                for (int j = 0; j < solution.numberOfVariables(); j++) capitalInvestedSum += solution.DecisionVariables[j].getValue() * assetsValues[j];
                for (int j = 0; j < solution.numberOfVariables(); j++) capitalInvested[j] = (solution.DecisionVariables[j].getValue() * assetsValues[j]) / capitalInvestedSum;
                for (int j = 0; j < solution.numberOfVariables(); j++) sharesInvested[j] = (capitalInvested[j] * portfolioValue) / assetsValues[j];
            }
            else throw new Exception("Credit Exposure Based Evaluation does not support specified decision variable type");

            for (int i = 0; i < periodDuration; i++)
            {
                //Calculate current capital invested weights
                if (!creditExposureTimeSeriesSet.GetValues(creditExposureTimeSeriesSet.GetDay(assetsStartIndex + i), assetsValues)) throw new Exception("Asset values not found for specified date");
                //Calculate current portfolio value
                portfolioValue = 0.0;
                for (int j = 0; j < creditExposureTimeSeriesSet.Count; j++) portfolioValue += sharesInvested[j] * assetsValues[j];
                //Recalculate current capital invested weights using current portfolio value
                for (int j = 0; j < creditExposureTimeSeriesSet.Count; j++) currentCapitalInvested[j] = (sharesInvested[j] * assetsValues[j]) / portfolioValue;

                DayValue dayValue = new DayValue();
                dayValue.Day = assetTimeSeriesSet.GetDay(CDSStartIndex + i);
                dayValue.Value = 0.0;
                for (int j = 0; j < assetTimeSeriesSet.Count; j++) dayValue.Value += currentCapitalInvested[j] * assetTimeSeriesSet.GetDayValue(j, CDSStartIndex + i).Value;
                output.DataSeries.Add(dayValue);
            }
        }

        public double GenerateReturnList(int evaluationPeriodDuration, int previousPeriodDuration, TimeSeries outputTimeSeries)
        {
            outputTimeSeries.DataSeries.Clear();
            double Return;
            double ReturnSum = 0.0;
            double currentValue;
            double previousValue;
            int startIndex = PortfolioTimeSeries.DataSeries.Count - evaluationPeriodDuration;
            int returnEvaluationPeriod; //Period za koji se racuna prosecni prinos
            if (m_parameters.ContainsParameter("returnEvaluationPeriod"))
            {
                returnEvaluationPeriod = (int)m_parameters.getParameter("returnEvaluationPeriod");
            }
            else returnEvaluationPeriod = evaluationPeriodDuration;

            if (m_VariableType == DecisionVariableType.constantCapitalInvested)
            {
                for (int i = 0; i < evaluationPeriodDuration; i++)
                {
                    Return = PortfolioTimeSeries.DataSeries[startIndex + i].Value;
                    if (i >= (evaluationPeriodDuration - returnEvaluationPeriod)) ReturnSum += Return;

                    DayValue dayValue = new DayValue(PortfolioTimeSeries.DataSeries[startIndex + i].Day, Return);
                    outputTimeSeries.DataSeries.Add(dayValue);
                }
            }
            else
            {
                for (int i = 0; i < evaluationPeriodDuration; i++)
                {
                    currentValue = PortfolioTimeSeries.DataSeries[startIndex + i].Value;
                    previousValue = PortfolioTimeSeries.DataSeries[startIndex - previousPeriodDuration + i].Value;
                    Return = (currentValue - previousValue) / previousValue;
                    //Return = Math.Log(currentValue/previousValue);
                    //ReturnSum += Return;
                    if (i >= (evaluationPeriodDuration - returnEvaluationPeriod)) ReturnSum += Return;

                    DayValue dayValue = new DayValue(PortfolioTimeSeries.DataSeries[startIndex + i].Day, Return);
                    outputTimeSeries.DataSeries.Add(dayValue);
                }
            }

            double averageReturn = (ReturnSum / returnEvaluationPeriod);
            return averageReturn;
        }

        public void GenerateAbsoluteBasedCreditExposureEvaluationTS()
        {
            EvaluationTimeSeries.DataSeries.Clear();
            double currentValue;
            for (int i = 1; i < PortfolioCreditExposureTimeSeries.DataSeries.Count; i++)
            {
                currentValue = PortfolioCreditExposureTimeSeries.DataSeries[i].Value;
                DayValue dayValue = new DayValue(PortfolioCreditExposureTimeSeries.DataSeries[i].Day, currentValue);
                EvaluationTimeSeries.DataSeries.Add(dayValue);
            }
        }

        public void GenerateDifferenceBasedCreditExposureEvaluationTS()
        {
            EvaluationTimeSeries.DataSeries.Clear();
            double currentValue;
            double previousValue;
            for (int i = 1; i < PortfolioCreditExposureTimeSeries.DataSeries.Count; i++)
            {
                currentValue = PortfolioCreditExposureTimeSeries.DataSeries[i].Value;
                previousValue = PortfolioCreditExposureTimeSeries.DataSeries[i - 1].Value;
                double difference = (currentValue - previousValue) / previousValue;

                DayValue dayValue = new DayValue(PortfolioCreditExposureTimeSeries.DataSeries[i].Day, difference);
                EvaluationTimeSeries.DataSeries.Add(dayValue);
            }
        }
        #endregion

        #region Evaluation functions
        protected double EvaluationFunction(EvaluationCriteria EC)
        {
            if (EC.GetType() == RETURN_BASED_CRITERIA)
            {
                return EvaluationFunctionReturnBased((EvaluationCriteriaReturnBased)EC);
            }
            else if (EC.GetType() == CREDIT_BASED_CRITERIA)
            {
                return EvaluationFunctionCreditExposureBased((EvaluationCriteriaCreditExposureBased)EC);
            }
            else if (EC.GetType() == PARAMETER_BASED_CRITERIA)
            {
                return EvaluationFunctionParameterBased((EvaluationCriteriaParameterBased)EC);
            }
            else throw new Exception("Portfolio optimization.EvaluationFunction: Unknown criteria type");
        }

        protected double EvaluationFunctionReturnBased(EvaluationCriteriaReturnBased EC)
        {
            double evaluationParameter = 0.0;
            int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
            int evaluationPeriodDuration = evaluationPeriod;
            int previousPeriodDuration = ReturnPeriod;//Return period
            double averageReturn;
            switch (EC.CriteriaType)
            {
                case EvaluationCriteriaReturnBased.criteriaType.AverageReturn:
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    evaluationParameter = -averageReturn;//MO odredjuje minimum - VAZNO!!! - OVO JE GLUPO. TREBALO BI DA SE DORADI DA MOZE ZA SVAKI 
                                                                                            //KRITERIJUM DA SE DEFINISE DA LI SE CILJA MINIMUM ILI MAKSIMUM
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.StandardDeviation:
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    evaluationParameter = CalculateStandardDeviationOnPopulation(EvaluationTimeSeries, averageReturn);//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.SharpIndex:
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    double stdevp = CalculateStandardDeviationOnPopulation(EvaluationTimeSeries, averageReturn);
                    evaluationParameter = -CalculateSharpIndex(averageReturn, stdevp);//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.Sortino:
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    double downsideRisk = CalculateDownsideRiskOnPopulation(EvaluationTimeSeries);
                    evaluationParameter = -CalculateSortino(averageReturn, downsideRisk);//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.VaR:
                    double tmp = 0.0;
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    evaluationParameter = -CalculateVaR(EvaluationTimeSeries, ref tmp);//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.cVaR:
                    double cVaR = 0.0;
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    CalculateVaR(EvaluationTimeSeries, ref cVaR);
                    evaluationParameter = -cVaR;//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.ExpWeightedVaR:
                    tmp = 0.0;
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    evaluationParameter = -CalculateExpWeightedVaR(EvaluationTimeSeries, ref tmp);//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.ExpWeightedcVaR:
                    double expWeightedcVaR = 0.0;
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    CalculateExpWeightedVaR(EvaluationTimeSeries, ref expWeightedcVaR);
                    evaluationParameter = -expWeightedcVaR;//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.Skewness:
                    double Skewness = 0.0;
                    Skewness = CalculateSkewness(evaluationPeriodDuration, previousPeriodDuration);
                    evaluationParameter = -Skewness;//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.Kurtosis:
                    double Kurtosis = 0.0;
                    Kurtosis = CalculateKurtosis(evaluationPeriodDuration, previousPeriodDuration);
                    evaluationParameter = Kurtosis;//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.VaRGarch:
                    double VaRGarch = 0.0;
                    VaRGarch = m_VaRGARCH.CalculateVaRGarch(solution);
                    evaluationParameter = VaRGarch;
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.SharpVaR:
                    tmp = 0.0;
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    double VaR = -CalculateVaR(EvaluationTimeSeries, ref tmp);
                    evaluationParameter = -CalculateSharpIndex(averageReturn, VaR);//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.SharpCVar:
                    cVaR = 0.0;
                    averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
                    CalculateVaR(EvaluationTimeSeries, ref cVaR);
                    evaluationParameter = -CalculateSharpIndex(averageReturn, -cVaR);//MO odredjuje minimum
                    break;
                case EvaluationCriteriaReturnBased.criteriaType.ReplicationStDev:
                    TimeSeries ReplicationDifferenceList = new TimeSeries();
                    averageReturn = GenerateReplicationDifferenceList(evaluationPeriodDuration, previousPeriodDuration, ReplicationDifferenceList);
                    evaluationParameter = CalculateStandardDeviationOnPopulation(ReplicationDifferenceList, averageReturn);//MO odredjuje minimum
                    break;
                //case EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR:
                //    double CR = 0.0;
                    //double[] weights = solution.DecisionVariables.ToArray();
                    //evaluationParameter = CRInR.CalculateCapitalRequirements(
                case EvaluationCriteriaReturnBased.criteriaType.DrawDown:
                    evaluationParameter = -CalculateDrawdown(evaluationPeriodDuration);//MO odredjuje minimum
                    break;
                default: throw new Exception("Evaluation function is not defined for choosen criteria.");
            }
            return evaluationParameter;
        }

        protected double EvaluationFunctionCreditExposureBased(EvaluationCriteriaCreditExposureBased EC)
        {
            double evaluationParameter = 0.0;
            int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
            int evaluationPeriodDuration = evaluationPeriod;
            int previousPeriodDuration = ReturnPeriod;//Return period
            double average;
            switch (EC.CriteriaType)
            {
                case EvaluationCriteriaCreditExposureBased.criteriaType.minimalAverageCreditExposure:
                    //GenerateAbsoluteBasedCreditExposureEvaluationTS();
                    GenerateDifferenceBasedCreditExposureEvaluationTS();
                    average = CalculateAverageOfPopulation(EvaluationTimeSeries);
                    evaluationParameter = average;
                    break;
                case EvaluationCriteriaCreditExposureBased.criteriaType.minimalStandardDeviationCreditExposure:
                    GenerateDifferenceBasedCreditExposureEvaluationTS();
                    average = CalculateAverageOfPopulation(EvaluationTimeSeries);
                    evaluationParameter = CalculateStandardDeviationOnPopulation(EvaluationTimeSeries, average);//MO odredjuje minimum
                    break;
                default: throw new Exception("Evaluation function is not defined for choosen criteria.");
            }
            return evaluationParameter;
        }

        protected double EvaluationFunctionParameterBased(EvaluationCriteriaParameterBased EC)
        {
            double evaluationParameter = 0.0;
            switch (EC.CriteriaType)
            {
                case EvaluationCriteriaParameterBased.criteriaType.YieldToMaturity:
                    evaluationParameter = -CalculateYieldToMaturity(solution);
                    break;
                case EvaluationCriteriaParameterBased.criteriaType.SCR:
                    evaluationParameter = CalculateSCR(solution);
                    break;
                default: throw new Exception("Evaluation function is not defined for choosen criteria.");
            }
            return evaluationParameter;
        }

        protected void OnREvaluationException(Solution solution, Exception ex)
        {
            string PrintDirectory = (string)m_parameters.getParameter("PrintDirectory");

            writer = new System.IO.StreamWriter(PrintDirectory + "\\failed-individuals.txt", true);
            failedIndividuals++;
            writer.WriteLine(failedIndividuals.ToString());
            writer.WriteLine(arrayToString(solution.DecisionVariables.ToArray()));
            writer.WriteLine(ex.Message);
            Solution tmpSolution = new Solution(this);
            solution.DecisionVariables = tmpSolution.DecisionVariables;
            writer.Close();
        }
        #endregion

        #region Criteria calculation functions
        protected double CalculateAverageOfPopulation(TimeSeries InputTimeSeries)
        {
            double average = 0.0;

            for (int i = 0; i < InputTimeSeries.DataSeries.Count; i++)
            {
                average += InputTimeSeries.DataSeries[i].Value;
            }
            average = average / InputTimeSeries.DataSeries.Count;
            return average;
        }
        protected double CalculateStandardDeviationOnPopulation(TimeSeries InputTimeSeries, double averageOfTimeSeries)
        {
            double standardDeviation = 0.0;
            double tmpSum = 0.0;

            tmpSum = 0.0;
            for (int i = 0; i < InputTimeSeries.DataSeries.Count; i++)
            {
                tmpSum += (InputTimeSeries.DataSeries[i].Value - averageOfTimeSeries) * (InputTimeSeries.DataSeries[i].Value - averageOfTimeSeries);
            }
            standardDeviation = Math.Pow(tmpSum / InputTimeSeries.DataSeries.Count, 0.5);
            return standardDeviation;
        }
        protected double CalculateDownsideRiskOnPopulation(TimeSeries InputTimeSeries)
        {
            double targetRate = (double)m_parameters.getParameter("targetRate") * 0.01;//target rate is defined as percentage (f.i. 5%)
            double downsideRisk = 0.0;
            double tmpSum = 0.0;
            for (int i = 0; i < InputTimeSeries.DataSeries.Count; i++)
            {
                tmpSum += (InputTimeSeries.DataSeries[i].Value - targetRate) * (InputTimeSeries.DataSeries[i].Value - targetRate);
            }
            downsideRisk = Math.Pow(tmpSum / InputTimeSeries.DataSeries.Count, 0.5);
            return downsideRisk;
        }
        protected double CalculateVaR(TimeSeries InputTimeSeries, ref double cVaR)
        {
            double varThreshold = (double)m_parameters.getParameter("varThreshold");
            TimeSeries InputTimeSeriesCopy = new TimeSeries(InputTimeSeries);
            double WorstReturns = InputTimeSeries.DataSeries.Count * varThreshold;
            int numberOfWorstReturns = (int)(InputTimeSeries.DataSeries.Count * varThreshold);
            if (WorstReturns > numberOfWorstReturns) numberOfWorstReturns += 1;
            double VaR = 1.0e+10;
            double previousVaR = 0.0;
            cVaR = 0.0;
            int removedReturnIndex = 0;


            for (int i = 1; i <= numberOfWorstReturns; i++)
            {
                VaR = 1.0e+10;
                for (int j = 0; j < InputTimeSeriesCopy.DataSeries.Count; j++)
                {
                    if (InputTimeSeriesCopy.DataSeries[j].Value < VaR)
                    {
                        VaR = InputTimeSeriesCopy.DataSeries[j].Value;
                        removedReturnIndex = j;
                    }
                }
                InputTimeSeriesCopy.DataSeries.RemoveAt(removedReturnIndex);
                if (i < numberOfWorstReturns) previousVaR = VaR;
                cVaR += VaR;                
            }
            VaR = VaR * (WorstReturns - (numberOfWorstReturns - 1)) + previousVaR * (numberOfWorstReturns - WorstReturns);
            
            cVaR /= numberOfWorstReturns;
            return VaR;
        }
        protected double CalculateExpWeightedVaR(TimeSeries InputTimeSerie, ref double cVaR)
        {
            double lambda = (double)m_parameters.getParameter("lambda");
            double varThreshold = (double)m_parameters.getParameter("varThreshold");
            TimeSeries InputTimeSerieCopy = new TimeSeries(InputTimeSerie);
            double VaR = 1.0e+10;
            cVaR = 0.0;
            int removedReturnIndex = 0;
            int numberOfWorstReturns = 0;
            double acumulatedProbability = 0.0;
            int origCount = InputTimeSerieCopy.DataSeries.Count;
            List<int> removedIndexes = new List<int>();

            //double sum = 0.0;
            //for (int kk = 0; kk < origCount; kk++) sum += Math.Pow(lambda, kk);
            //double invsum = 1.0 / sum;

            while (acumulatedProbability <= varThreshold & InputTimeSerieCopy.DataSeries.Count > 0)
            {
                VaR = 1.0e+10;
                for (int j = 0; j < InputTimeSerieCopy.DataSeries.Count; j++)
                {
                    if (InputTimeSerieCopy.DataSeries[j].Value < VaR)
                    {
                        VaR = InputTimeSerieCopy.DataSeries[j].Value;
                        removedReturnIndex = j;
                    }
                }
                int origIndex = removedReturnIndex;
                for (int i = removedIndexes.Count - 1; i >= 0; i--)
                {
                    if (origIndex >= removedIndexes[i]) origIndex++;
                }
                removedIndexes.Add(removedReturnIndex);
                double power = (origCount - 1) - origIndex;
                acumulatedProbability += Math.Pow(lambda, power) * (1.0 - lambda);
                //acumulatedProbability += Math.Pow(lambda, power) * invsum;

                InputTimeSerieCopy.DataSeries.RemoveAt(removedReturnIndex);
                numberOfWorstReturns++;
                cVaR += VaR;
            }
            cVaR /= numberOfWorstReturns;
            return VaR;
        }
        protected double CalculateSkewness(int evaluationPeriodDuration, int previousPeriodDuration)
        {
            double averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
            double standardDeviation = CalculateStandardDeviationOnPopulation(EvaluationTimeSeries, averageReturn);

            double tmpSum = 0.0;
            for (int i = 0; i < EvaluationTimeSeries.DataSeries.Count; i++)
            {
                tmpSum += Math.Pow(EvaluationTimeSeries.DataSeries[i].Value - averageReturn, 3.0);
            }
            double Skewness = (evaluationPeriodDuration * tmpSum) / ((evaluationPeriodDuration - 1) * (evaluationPeriodDuration - 2) * Math.Pow(standardDeviation, 3.0));
            return Skewness;
        }
        protected double CalculateKurtosis(int evaluationPeriodDuration, int previousPeriodDuration)
        {
            double averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
            double standardDeviation = CalculateStandardDeviationOnPopulation(EvaluationTimeSeries, averageReturn);

            double tmp = 0.0;
            for (int i = 0; i < EvaluationTimeSeries.DataSeries.Count; i++)
            {
                tmp += Math.Pow(EvaluationTimeSeries.DataSeries[i].Value - averageReturn, 4.0);
            }
            int n = evaluationPeriodDuration;
            double Kurtosis = (n * (n + 1) * tmp) / ((n - 1) * (n - 2) * (n - 3) * Math.Pow(standardDeviation, 4.0)) - 3 * (n - 1) * (n - 1) / ((n - 2) * (n - 3));
            return Kurtosis;
        }
        protected double CalculateSharpIndex(double averageReturn, double stdevp)
        {
            double targetRate = (double)m_parameters.getParameter("targetRate");
            double sharpIndex = (averageReturn - targetRate) / stdevp;
            return sharpIndex;
        }
        protected double CalculateSortino(double averageReturn, double downsideRisk)
        {
            double targetRate = (double)m_parameters.getParameter("targetRate");
            double sortinoIndex = (averageReturn - targetRate) / downsideRisk;
            return sortinoIndex;
        }
        private double GenerateReplicationDifferenceList(int evaluationPeriodDuration, int previousPeriodDuration, TimeSeries ReplicationDifferenceList)
        {
            ReplicationDifferenceList.DataSeries.Clear();
            double averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
            TimeSeries replicationSerie = new TimeSeries();
            replicationSerie = (TimeSeries)m_parameters.getParameter("replicationSerie");

            DayValue dayValue = new DayValue();
            int startIndex = PortfolioTimeSeries.DataSeries.Count - evaluationPeriodDuration;
            averageReturn = 0.0;
            double replicationValue = 0.0;
            try
            {
                 for (int i = 0; i < evaluationPeriodDuration; i++)
                 {
                     dayValue = EvaluationTimeSeries.DataSeries[i];
                     replicationSerie.GetValueOnDay(dayValue.Day, ref replicationValue);
                     double difference = dayValue.Value - replicationValue;

                     DayValue repDayValue = new DayValue(dayValue.Day, difference);
                     ReplicationDifferenceList.DataSeries.Add(repDayValue);
                     averageReturn += difference;
                 }
            }
            catch (Exception ex)
            {
                ex.Source = "GenerateReplicationDifferenceList";
            }

            averageReturn /= evaluationPeriodDuration;
            return averageReturn;
        }
        /// <summary>Calculates Capital Requirements for considered portfolio</summary>
        /// <param name="weights">Shares invested in consideerd portfolio</param>
        /// <param name="backtestingStartDate">Start date of backtesting procedure</param>
        /// <param name="sw">The Stream Writer object for printing backtesting results</param>
        /// <returns> Capital requirements
        /// </returns>
        public double CalculateCapitalRequirementsInR(double[] weights, DecisionVariableType variableType, DateTime CRDate)
        {
            double capitalRequirements;
            int violationsCount;
            int correctionsCount;

            int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
            double VaRThreshold = (double)m_parameters.getParameter("varThreshold");
            string RScriptPath = (string)m_parameters.getParameter("RScriptPath");
            int backtestingPeriod = (int)m_parameters.getParameter("backtestingPeriod");
            AssetSet stressTimeSeriesSet = (AssetSet)m_parameters.getParameter("stressTimeSeriesSet");
            AssetSet timeSeriesSet = (AssetSet)m_parameters.getParameter("CR_TimeSeriesSet");
            List<EvaluationCriteria> evaluationCriteriaList= new List<EvaluationCriteria>();
            EvaluationCriteriaReturnBased EC = new EvaluationCriteriaReturnBased();
            ((EvaluationCriteriaReturnBased)EC).CriteriaType = EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR;
            evaluationCriteriaList.Add(EC);

            RCapitalRequirements CRInR = new RCapitalRequirements(timeSeriesSet, CRDate, evaluationPeriod, backtestingPeriod, ReturnPeriod, variableType, VaRThreshold, RScriptPath, executionType, evaluationCriteriaList, stressTimeSeriesSet);
            CRInR.CalculateCapitalRequirements(weights, out capitalRequirements, out violationsCount, out correctionsCount);

            return capitalRequirements;
        }

        public double[] GetSharesInvested(AssetSet timeSeriesSet, DateTime OnDate, double[] capitalWeights)
        {
            double[] sharesInvested = new double[solution.numberOfVariables()];
            //Assumed portfolio value
            double portfolioValue = 1.0;
            double[] assetsValues = new double[timeSeriesSet.Count];
            //Vrednost holdingsa se racuna na osnovu capital invested weights i vrednosti aseta na dan evaluacije
            if (!timeSeriesSet.GetValues(OnDate, assetsValues)) throw new Exception("Asset values not found for specified date");
            //Odredjivanje holdingsa na poslednji dan evaluacije
            for (int j = 0; j < timeSeriesSet.Count; j++) sharesInvested[j] = (capitalWeights[j] * portfolioValue) / assetsValues[j];
            return sharesInvested;
        }

        /// <summary>Calculates Capital Requirements for considered portfolio</summary>
        /// <param name="sharesInvested">Shares invested in consideerd portfolio</param>
        /// <param name="backtestingStartDate">Start date of backtesting procedure</param>
        /// <param name="sw">The Stream Writer object for printing backtesting results</param>
        /// <returns> Capital requirements
        /// </returns>
        public double CalculateCapitalRequirements(double[] sharesInvested, DateTime CRDate)
        {
            double CapitalRequirements = 0.0;
            int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
            int backtestingPeriod = (int)m_parameters.getParameter("backtestingPeriod");
            AssetSet timeSeriesSet = (AssetSet)m_parameters.getParameter("CR_TimeSeriesSet");
            EvaluationCriteriaReturnBased backtestingEC = new EvaluationCriteriaReturnBased();
            backtestingEC.CriteriaType = EvaluationCriteriaReturnBased.criteriaType.VaR;
            
            Backtesting BacktestingClass = new Backtesting(backtestingEC, CRDate, backtestingPeriod);
            
            BacktestingClass.m_parameters.setParameter("activePortfolio", sharesInvested, true);
            BacktestingClass.m_parameters.setParameter("RebalanceAssetTimeSeriesSet", timeSeriesSet);
            BacktestingClass.m_parameters.setParameter("evaluationPeriod", evaluationPeriod);
            BacktestingClass.m_parameters.setParameter("returnEvaluationPeriod", (int)m_parameters.getParameter("returnEvaluationPeriod"));
            BacktestingClass.m_parameters.setParameter("varThreshold", (double)m_parameters.getParameter("varThreshold"));
            BacktestingClass.m_parameters.setParameter("targetRate", (double)m_parameters.getParameter("targetRate"));
            BacktestingClass.m_parameters.setParameter("lambda", m_parameters.getParameter("lambda"));
            CapitalRequirements = BacktestingClass.Execute();

            return CapitalRequirements;
        }

        public double CalculateDrawdown(int evaluationPeriodDuration)
        {
            int startIndex = PortfolioTimeSeries.DataSeries.Count - evaluationPeriodDuration;//Evaluation period is related to return
            double maxValue = PortfolioTimeSeries.DataSeries[startIndex].Value;
            double DrawDown = double.MaxValue, currentDrawDown, currentValue;
           
            for (int i = 0; i < evaluationPeriodDuration; i++)
            {
                currentValue = PortfolioTimeSeries.DataSeries[startIndex + i].Value;
                if (currentValue > maxValue) maxValue = currentValue;
                                
                currentDrawDown = currentValue / maxValue - 1;
                if (currentDrawDown < DrawDown) DrawDown = currentDrawDown;
            }
            return DrawDown;
        }


        /// <summary>
        /// Calculate yield to maturity of bond portfolio 
        /// </summary>
        /// <returns>yieldToMaturity</returns>
        protected double CalculateYieldToMaturity(Solution inputSolution)
        {
            BondSet bondSet = (BondSet)m_parameters.getParameter("BondSet");

            double yieldToMaturity = 0.0;
            for (int i = 0; i < bondSet.Count; i++)
            {
                Bond bond = bondSet.GetItem(i);
                yieldToMaturity += inputSolution.DecisionVariables[i].getValue() * bond.yieldToMaturity;
            }
            return yieldToMaturity;
        }

        protected double CalculateInterestRateRisk(Solution inputSolution)
        {
            BondSet bondSet = (BondSet)m_parameters.getParameter("BondSet");
            double InterestRateRisk = 0.0;
            //INTEREST RATE RISK
            for (int i = 0; i < bondSet.Count; i++)
            {
                Bond bond = bondSet.GetItem(i);
                InterestRateRisk += inputSolution.DecisionVariables[i].getValue() * (bond.price - bond.stressedPrice) / bond.price;
            }
            //end INTEREST RATE RISK
            
            return InterestRateRisk; 
        }

        protected double CalculateSpreadRisk(Solution inputSolution)
        {
            BondSet bondSet = (BondSet)m_parameters.getParameter("BondSet");
            double SpreadRisk = 0.0;
            //SPREAD RISK
            Dictionary<string, double> SpreadRiskFactor = new Dictionary<string, double>();
            SpreadRiskFactor.Add("AAA", 0.009);
            SpreadRiskFactor.Add("AA", 0.011);
            SpreadRiskFactor.Add("A", 0.014);
            SpreadRiskFactor.Add("BBB", 0.025);
            SpreadRiskFactor.Add("BB", 0.045);
            SpreadRiskFactor.Add("B", 0.075);
            SpreadRiskFactor.Add("Unrated", 0.03);
            double MV, duration;

            //Find bonds with same rating
            Dictionary<string, List<int>> RatingToBondIndexes = new Dictionary<string, List<int>>();
            RatingToBondIndexes.Add("AAA", new List<int>());//contains indexes of bonds with rating AAA
            RatingToBondIndexes.Add("AA", new List<int>());
            RatingToBondIndexes.Add("A", new List<int>());
            RatingToBondIndexes.Add("BBB", new List<int>());
            RatingToBondIndexes.Add("BB", new List<int>());
            RatingToBondIndexes.Add("B", new List<int>());
            RatingToBondIndexes.Add("Unrated", new List<int>());

            for (int i = 0; i < bondSet.Count; i++)
            {
                Bond bond = bondSet.GetItem(i);
                RatingToBondIndexes[bond.rating].Add(i);
            }
            //end 

            for (int i = 0; i < RatingToBondIndexes.Count; i++)
            {
                MV = 0.0;
                duration = 0.0;
                double currentRatingSpreadRisk;

                for (int j = 0; j < RatingToBondIndexes.ElementAt(i).Value.Count; j++)
                {
                    int bondIndex = RatingToBondIndexes.ElementAt(i).Value[j];
                    double weight = (double)inputSolution.DecisionVariables[bondIndex].getValue();
                    MV += weight;
                }

                if (MV > 0.0)
                {
                    for (int j = 0; j < RatingToBondIndexes.ElementAt(i).Value.Count; j++)
                    {
                        int bondIndex = RatingToBondIndexes.ElementAt(i).Value[j];
                        Bond bond = bondSet.GetItem(bondIndex);

                        double weight = (double)inputSolution.DecisionVariables[bondIndex].getValue();
                        duration += (weight * bond.duration) / MV;
                    }

                    currentRatingSpreadRisk = MV * SpreadRiskFactor[RatingToBondIndexes.ElementAt(i).Key] * duration;
                }
                else currentRatingSpreadRisk = 0.0;

                SpreadRisk += currentRatingSpreadRisk;
            }
            //end SPREAD RISK

            return SpreadRisk;
        }

        protected double CalculateConcentrationRisk(Solution inputSolution)
        {
            BondSet bondSet = (BondSet)m_parameters.getParameter("BondSet");
            double ConcentrationRisk = 0.0;

            //CONCENTRATION RISK

            Dictionary<string, double> ConcentrationThreshold = new Dictionary<string, double>();
            ConcentrationThreshold.Add("AAA", 0.03);
            ConcentrationThreshold.Add("AA", 0.03);
            ConcentrationThreshold.Add("A", 0.03);
            ConcentrationThreshold.Add("BBB", 0.015);
            ConcentrationThreshold.Add("BB", 0.015);
            ConcentrationThreshold.Add("B", 0.015);
            ConcentrationThreshold.Add("Unrated", 0.015);

            Dictionary<string, double> GFactor = new Dictionary<string, double>();
            GFactor.Add("AAA", 0.12);
            GFactor.Add("AA", 0.12);
            GFactor.Add("A", 0.21);
            GFactor.Add("BBB", 0.27);
            GFactor.Add("BB", 0.73);
            GFactor.Add("B", 0.73);
            GFactor.Add("Unrated", 0.73);

            //Calculate exposure per issuer
            Dictionary<string, double> IssuerExposure = new Dictionary<string, double>();
            Dictionary<string, string> IssuerRating = new Dictionary<string, string>();
            for (int i = 0; i < bondSet.Count; i++)
            {
                Bond bond = bondSet.GetItem(i);
                if (IssuerExposure.ContainsKey(bond.ticker)) IssuerExposure[bond.ticker] += (double)inputSolution.DecisionVariables[i].getValue();
                else
                {
                    IssuerExposure.Add(bond.ticker, (double)inputSolution.DecisionVariables[i].getValue());
                    IssuerRating.Add(bond.ticker, bond.rating);
                }
            }


            for (int i = 0; i < IssuerExposure.Count; i++)
            {
                double issuerExposure = IssuerExposure.ElementAt(i).Value;
                string issuerRating = IssuerRating[IssuerExposure.ElementAt(i).Key];
                double concentrationThreshold = ConcentrationThreshold[issuerRating];
                double g = GFactor[issuerRating];
                ConcentrationRisk += Math.Pow(Math.Max(0.0, issuerExposure - concentrationThreshold) * g, 2);
            }

            ConcentrationRisk = Math.Pow(ConcentrationRisk, 0.5);

            // END CONCENTRATION RISK

            return ConcentrationRisk;
        }

        protected double CalculateSCR(Solution inputSolution)
        {
            double InterestRateRisk = CalculateInterestRateRisk(inputSolution);
            double SpreadRisk = CalculateSpreadRisk(inputSolution);
            double ConcentrationRisk = CalculateConcentrationRisk(inputSolution);
            double SCR;

            SCR = Math.Pow(InterestRateRisk, 2) + Math.Pow(SpreadRisk, 2) + Math.Pow(ConcentrationRisk, 2) + 0.5 * InterestRateRisk * SpreadRisk;
            SCR = Math.Pow(SCR, 0.5);

            return SCR;
        }

        #endregion

        #region Printing results

        public void PrintResults(StreamWriter sw, JARE.Base.SolutionSet resultPopulation, string inputString = "", bool Headline = true)
        {

            string str = string.Empty;
            string strPonder = string.Empty;
            string headLine = string.Empty;
            string strParameters = string.Empty;
            System.Collections.Generic.Dictionary<string, double> parametersForOptimalPortfolio = new System.Collections.Generic.Dictionary<string, double>();
            bool headLineWritten = false;

            for (int i = 0; i < resultPopulation.size(); i++)
            {
                solution = resultPopulation.getSolution(i);
                GetParametersForOptimalPortfolio(solution, parametersForOptimalPortfolio);
                str = string.Empty;
                //Write head line
                if (Headline == true && !headLineWritten)
                {
                    System.Collections.Generic.Dictionary<string, double>.Enumerator HeadLineEnum = parametersForOptimalPortfolio.GetEnumerator();
                    for (int kk = 0; kk < parametersForOptimalPortfolio.Count; kk++)
                    {
                        HeadLineEnum.MoveNext();
                        headLine += (HeadLineEnum.Current.Key + ",");
                    }
                    HeadLineEnum.Dispose();
                    for (int j = 1; j <= NumberOfVariables; j++) str += "ponder " + j.ToString() + ",";
                    headLine += str;
                    if (inputString != "") headLine = " ," + headLine;
                    sw.WriteLine(headLine);
                    headLineWritten = true;
                }
                //Write parameters
                System.Collections.Generic.Dictionary<string, double>.Enumerator Enum = parametersForOptimalPortfolio.GetEnumerator();
                strParameters = string.Empty;
                for (int kk = 0; kk < parametersForOptimalPortfolio.Count; kk++)
                {
                    Enum.MoveNext();
                    strParameters += (Enum.Current.Value.ToString("F10") + ",");
                }

                strPonder = string.Empty;
                for (int kk = 1; kk <= solution.numberOfVariables(); kk++)
                {
                    strPonder += (solution.DecisionVariables[kk - 1].getValue().ToString("E6") + ", ");
                }

                str = inputString + strParameters + strPonder;
                sw.WriteLine(str);
            }
        }
        public abstract void GetParametersForOptimalPortfolio(Solution solution, Dictionary<string, double> parametersForOptimalPortfolio, bool allParameters = true);
        public virtual void GetReturnBasedParametersForOptimalPortfolio(Solution solution, Dictionary<string, double> parametersForOptimalPortfolio, AssetSet timeSeriesSet, bool allParameters)
        {
            if (allParameters) parametersForOptimalPortfolio.Clear();
            GeneratePortfolioTimeSeries(solution, timeSeriesSet, PortfolioTimeSeries);
            //Calculating average Return
            int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
            int evaluationPeriodDuration = evaluationPeriod;
            int previousPeriodDuration = ReturnPeriod;

            double tmp = 0.0;
            double averageReturn = GenerateReturnList(evaluationPeriodDuration, previousPeriodDuration, EvaluationTimeSeries);
            double actualReturn = EvaluationTimeSeries.DataSeries[EvaluationTimeSeries.DataSeries.Count-1].Value;
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.AverageReturn.ToString(), averageReturn);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, "actualReturn", actualReturn);

            //Calculating standard deviation and downside risk
            double standardDeviation = CalculateStandardDeviationOnPopulation(EvaluationTimeSeries, averageReturn);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.StandardDeviation.ToString(), standardDeviation);

            double downsideRisk = CalculateDownsideRiskOnPopulation(EvaluationTimeSeries);
            double sharpIndex = CalculateSharpIndex(averageReturn, standardDeviation);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.SharpIndex.ToString(), sharpIndex);

            double sortinoIndex = CalculateSortino(averageReturn, downsideRisk);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.Sortino.ToString(), sortinoIndex);
            
            double cVaR = 0.0;
            double VaR = CalculateVaR(EvaluationTimeSeries, ref cVaR);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.VaR.ToString(), -VaR);

            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.cVaR.ToString(), -cVaR);


            double expcVaR = 0.0;
            double expVaR = CalculateExpWeightedVaR(EvaluationTimeSeries, ref expcVaR);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.ExpWeightedVaR.ToString(), -expVaR);

            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.ExpWeightedcVaR.ToString(), -expcVaR);


            tmp = 0.0;
            for (int i = 0; i < EvaluationTimeSeries.DataSeries.Count; i++)
            {
                tmp += Math.Pow(EvaluationTimeSeries.DataSeries[i].Value - averageReturn, 3.0);
            }
            int periodDuration = EvaluationTimeSeries.DataSeries.Count;
            int n = periodDuration;
            double Skewness = (n * tmp) / ((n - 1) * (n - 2) * Math.Pow(standardDeviation, 3.0));
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.Skewness.ToString(), Skewness);


            tmp = 0.0;
            for (int i = 0; i < EvaluationTimeSeries.DataSeries.Count; i++)
            {
                tmp += Math.Pow(EvaluationTimeSeries.DataSeries[i].Value - averageReturn, 4.0);
            }
            double Kurtosis = (n * (n + 1) * tmp) / ((n - 1) * (n - 2) * (n - 3) * Math.Pow(standardDeviation, 4.0)) - 3 * (n - 1) * (n - 1) / ((n - 2) * (n - 3));
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.Kurtosis.ToString(), Kurtosis);

            double DrawDown = CalculateDrawdown(evaluationPeriodDuration);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.DrawDown.ToString(), DrawDown);
        }

        public virtual void GetParametersForOptimalParameterBasedPortfolio(Solution solution, Dictionary<string, double> parametersForOptimalPortfolio, bool allParameters)
        {
            if (allParameters) parametersForOptimalPortfolio.Clear();

            double yieldToMaturity = CalculateYieldToMaturity(solution);
            double SCR = CalculateSCR(solution);
            double InterestRateRisk = CalculateInterestRateRisk(solution);
            double SpreadRisk = CalculateSpreadRisk(solution);
            double ConcetrationRisk = CalculateConcentrationRisk(solution);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaParameterBased.criteriaType.YieldToMaturity.ToString(), yieldToMaturity);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaParameterBased.criteriaType.SCR.ToString(), SCR);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, "InterestRateRisk", InterestRateRisk);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, "SpreadRisk", SpreadRisk);
            AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, "ConcetrationRisk", ConcetrationRisk);
        }

        protected void AddParameterToDictionary(bool allParameters, Dictionary<string, double> parametersForOptimalPortfolio, string parameterName, double parameter)
        {
            if (allParameters) parametersForOptimalPortfolio.Add(parameterName, parameter);
            else if (parametersForOptimalPortfolio.ContainsKey(parameterName))
            {
                parametersForOptimalPortfolio[parameterName] = parameter;
            }
        }

        public string GetCreditExposBasedParametersForOptimalPortfolio(Dictionary<string, double> parametersForOptimalPortfolio, AssetSet assetsTimeSeriesSet, AssetSet creditExposureTimeSeriesSet)
        {
            GeneratePortfolioCreditExposureTimeSeries(assetsTimeSeriesSet, creditExposureTimeSeriesSet, PortfolioCreditExposureTimeSeries);//Ovo je hardcode-ovano
            string str = "";

            //GenerateAbsoluteBasedCreditExposureEvaluationTS();
            GenerateDifferenceBasedCreditExposureEvaluationTS();
            double averageCreditExposure = CalculateAverageOfPopulation(EvaluationTimeSeries);
            str += "averageCreditExposure=" + "," + averageCreditExposure.ToString("F10");
            //parametersForOptimalPortfolio.Add("AvgCreditExposure", averageCreditExposure);
            parametersForOptimalPortfolio.Add(EvaluationCriteriaCreditExposureBased.criteriaType.minimalAverageCreditExposure.ToString(), averageCreditExposure);

            //Calculating standard deviation and downside risk
            GenerateDifferenceBasedCreditExposureEvaluationTS();
            double average = CalculateAverageOfPopulation(EvaluationTimeSeries);
            double standardDeviation = CalculateStandardDeviationOnPopulation(EvaluationTimeSeries, average);
            //parametersForOptimalPortfolio.Add("STDEVCreditExposure", standardDeviation);
            parametersForOptimalPortfolio.Add(EvaluationCriteriaCreditExposureBased.criteriaType.minimalStandardDeviationCreditExposure.ToString(), standardDeviation);
            str += ", STDEVCreditExposure=" + "," + standardDeviation.ToString("F10");
            return str;
        }
        #endregion
        
        
        #region Initial population
         /// <summary> Creates initial population.</summary>
         /// <returns> the initial population.
         /// </returns>
        public override SolutionSet createInitialPopulation(int populationSize)
        {
            SolutionSet population = new SolutionSet(populationSize);
            Solution newSolution;

            if (m_initialSolutions == null) m_initialSolutions = new Dictionary<string, Solution>();

            for (int i = 0; i < populationSize - m_initialSolutions.Count; i++)
            {
                newSolution = new Solution(this);
                population.add(newSolution);
            } //for

            System.Collections.Generic.Dictionary<string, Solution>.Enumerator Enum = m_initialSolutions.GetEnumerator();
            for (int i = 0; i < m_initialSolutions.Count; i++)
            {
                Enum.MoveNext();
                newSolution = new Solution(Enum.Current.Value);
                //newSolution.DecisionVariables = Enum.Current.Value.DecisionVariables;
                population.add(newSolution);
            }

            //StreamWriter writer = new StreamWriter("Initial population.csv", false);
            //for (int k = 0; k < population.m_solutionsList.Count; k++)
            //{
            //    string solution = string.Empty;
            //    for (int j = 0; j < population.getSolution(k).DecisionVariables.Length; j++)
            //    {
            //        solution += population.getSolution(k).DecisionVariables[j].ToString() + ",";
            //    }
            //    //solution += "\n";
            //    writer.WriteLine(solution);
            //}
            //writer.Dispose();

            return population;
        }// createInitialPopulation 
        #endregion

        #region Optimal solutions methods
        public void addOptimalSolution(string name, Solution s)
        {

            if (m_initialSolutions == null)
            {
                m_initialSolutions = new Dictionary<string, Solution>();
            }
            else if (m_initialSolutions.ContainsKey(name))
            {
                m_initialSolutions.Remove(name);
            }
            m_initialSolutions.Add(name, s);

        } // addOptimalSolution

        public void addOptimalSolution(string name, double[] decisionVariables)
        {

            if (m_initialSolutions == null)
            {
                m_initialSolutions = new Dictionary<string, Solution>();
            }
            Solution s = new Solution(this);
            for (int j = 0; j < decisionVariables.Length; j++)
            {
                s.DecisionVariables[j].setValue((double)decisionVariables.GetValue(j));
            }

            m_initialSolutions.Add(name, s);

        } // addOptimalSolution

        public Solution getOptimalSolution(string name)
        {
            try
            {
                return m_initialSolutions[name];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

        } // getOperator   

        public virtual bool containsSolution(string name)
        {
            return m_initialSolutions.ContainsKey(name);
        }
        #endregion

        public abstract bool Init();
        public void CopyGARCHParameters()
        {
            if(m_VaRGARCH != null) m_VaRGARCH.GARCHparameters.CopyParametersTo(ref m_GARCHparameters);
        }

        public void DataValidation(AssetSet timeSeriesSet)
        {
            DateTime optimizationDate = (DateTime)m_parameters.getParameter("optimizationDate");
            timeSeriesSet.DataValidation(optimizationDate);
            int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
            DateTime startDate = GetEvaluationStartDate(optimizationDate, timeSeriesSet, evaluationPeriod, ReturnPeriod);
            timeSeriesSet.DataValidation(startDate);
            timeSeriesSet.Validated = true;
        }

        public virtual void JobResultsToObjectives(EvaluationResult er, Solution s)
        {
        }

        //protected abstract bool IsEvaluationCriteriaType(EvaluationCriteriaReturnBased.criteriaType CT);

        protected string arrayToString<T>(T[] list)
        {
            string strArray = string.Empty;

            foreach (T t in list)
            {
                strArray += t.ToString() + ",";
            }

            return strArray.Substring(0, strArray.Length - 1);
        }

    }
}

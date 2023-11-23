using System;
using System.Collections.Generic;
using System.Text;
using JARE.Base;
using JARE.Base.solutionType;
using JARE.problems.Finance;
using JARE.problems.Finance.DataTypes;
using JARE.EvaluationPool;

namespace JARE.problems.Finance
{
    public class PortfolioOptimizationSO : PortfolioOptimization
    {
         //Criteria of evaluation (optimization)
        EvaluationCriteria evaluationCriteria;

        public PortfolioOptimizationSO(EvaluationCriteria evaluationCriteria, DecisionVariableType variableType, int numberOfVariables, int cardinality, double lowerBound,
            double upperBound, ExecutionType executionType = ExecutionType.singleThread) : base(variableType, numberOfVariables, cardinality, lowerBound, upperBound, executionType)
		{
            this.evaluationCriteria = evaluationCriteria;
            m_numberOfObjectives = 1;
        }
        
        public override void evaluate(Solution inputSolution)
        {
            bool done = false;

            do
            {
                try
                {
                    solution = inputSolution;
                    totalIndividuals++;

                    if (evaluationCriteria.GetType() == RETURN_BASED_CRITERIA || evaluationCriteria.GetType() == CREDIT_BASED_CRITERIA)
                    {
                        AssetSet assetTimeSeriesSet = (AssetSet)m_parameters.getParameter("AssetTimeSeriesSet");

                        if (evaluationCriteria.GetType() == RETURN_BASED_CRITERIA)
                        {
                            GeneratePortfolioTimeSeries(inputSolution, assetTimeSeriesSet, PortfolioTimeSeries);
                        }
                        else if (evaluationCriteria.GetType() == CREDIT_BASED_CRITERIA)
                        {
                            AssetSet creditExposureTimeSeriesSet = (AssetSet)m_parameters.getParameter("CreditExposureTimeSeriesSet");
                            GeneratePortfolioCreditExposureTimeSeries(creditExposureTimeSeriesSet, assetTimeSeriesSet, PortfolioCreditExposureTimeSeries);
                        }
                    }

                    double objectiveValue = EvaluationFunction(evaluationCriteria);
                    inputSolution.Fitness = objectiveValue;
                    inputSolution.setObjective(0, objectiveValue);
                    done = true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Equals("R")) OnREvaluationException(inputSolution, ex);
                    else throw ex;
                }
            } while (!done);
        }

        #region Portfolio parameters
        public override void GetParametersForOptimalPortfolio(Solution solution, Dictionary<string, double> parametersForOptimalPortfolio, bool allParameters = true)
        {
            if (evaluationCriteria.GetType() == RETURN_BASED_CRITERIA)
            {
                AssetSet assetsTimeSeriesSet = (AssetSet)m_parameters.getParameter("AssetTimeSeriesSet");
                GetReturnBasedParametersForOptimalPortfolio(solution, parametersForOptimalPortfolio, assetsTimeSeriesSet, allParameters);
            }
            else if (evaluationCriteria.GetType() == CREDIT_BASED_CRITERIA)
            {
                AssetSet assetsTimeSeriesSet = (AssetSet)m_parameters.getParameter("AssetTimeSeriesSet");
                AssetSet creditExposureTimeSeriesSet = (AssetSet)m_parameters.getParameter("CreditExposureTimeSeriesSet");
                GetReturnBasedParametersForOptimalPortfolio(solution, parametersForOptimalPortfolio, assetsTimeSeriesSet, allParameters);
                GetCreditExposBasedParametersForOptimalPortfolio(parametersForOptimalPortfolio, assetsTimeSeriesSet, creditExposureTimeSeriesSet);
            }
            else throw new Exception("Portfolio optimization.EvaluationFunction: Unknown criteria type");
        }

        public override void GetReturnBasedParametersForOptimalPortfolio(Solution solution, Dictionary<string, double> parametersForOptimalPortfolio, AssetSet timeSeriesSet, bool allParameters)
        {
            base.GetReturnBasedParametersForOptimalPortfolio(solution, parametersForOptimalPortfolio, timeSeriesSet, allParameters);

            if (((EvaluationCriteriaReturnBased)evaluationCriteria).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.VaRGarch && solution.Fitness != 0.0)
            {
                AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.VaRGarch.ToString(), solution.Fitness);
            }
            else
            {
                //VaRGARCH
                if (Init())
                {
                    double VaRGarch = 1;
                    bool done = false;
                    int counter = 0;
                    do
                    {
                        try
                        {
                            VaRGarch = m_VaRGARCH.CalculateVaRGarch(solution);
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            if (!ex.Message.Equals("R")) throw ex;
                            m_GARCHparameters.Clear();
                            Init();
                            counter++;
                        }
                    } while (!done && counter < 2);

                    AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.VaRGarch.ToString(), VaRGarch);
                }
            }
        }
        #endregion

        public override bool Init()
        {
            bool VaRGARCH = false;
                        
            if (evaluationCriteria.GetType() == RETURN_BASED_CRITERIA)
            {
                if (((EvaluationCriteriaReturnBased)evaluationCriteria).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.VaRGarch)
                {
                    /*if (m_VaRGARCH == null)*/
                    List<EvaluationCriteria> EC = new List<EvaluationCriteria>();
                    EC.Add(evaluationCriteria);
                    string RScriptPath = (string)m_parameters.getParameter("RScriptPath");
                    double varThreshold = (double)m_parameters.getParameter("varThreshold");
                    int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
                    DateTime optimizationDate = (DateTime)m_parameters.getParameter("optimizationDate");
                    AssetSet assetsTimeSeriesSet = (AssetSet)m_parameters.getParameter("AssetsTimeSeries");
                    m_VaRGARCH = new VaRGARCH(assetsTimeSeriesSet, EC, optimizationDate, evaluationPeriod, ReturnPeriod, m_VariableType, true, varThreshold, m_GARCHparameters, RScriptPath, executionType);
                    VaRGARCH = true;
                }
                else if (((EvaluationCriteriaReturnBased)evaluationCriteria).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR)
                {
                    List<EvaluationCriteria> EC = new List<EvaluationCriteria>();
                    EC.Add(evaluationCriteria);
                    double VaRThreshold = (double)m_parameters.getParameter("varThreshold");
                    string RScriptPath = (string)m_parameters.getParameter("RScriptPath");
                    int backtestingPeriod = (int)m_parameters.getParameter("backtestingPeriod");
                    AssetSet stressTimeSeriesSet = (AssetSet)m_parameters.getParameter("stressTimeSeriesSet");
                    int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
                    DateTime optimizationDate = (DateTime)m_parameters.getParameter("optimizationDate");
                    AssetSet assetsTimeSeriesSet = (AssetSet)m_parameters.getParameter("AssetsTimeSeries");
                    RCapitalRequirements CRInR = new RCapitalRequirements(assetsTimeSeriesSet, optimizationDate, evaluationPeriod, backtestingPeriod, ReturnPeriod, m_VariableType, VaRThreshold, RScriptPath, executionType, EC, stressTimeSeriesSet);
                }
            }

            return VaRGARCH;
        }

        #region Individuals properties
        public int TotalIndividuals
        {
            get { return totalIndividuals; }
            set { totalIndividuals = value; }
        }

        public int FailedIndividuals
        {
            get { return failedIndividuals; }
            set { failedIndividuals = value; }
        }
        #endregion

       
        public override void JobResultsToObjectives(EvaluationResult er, Solution s)
        {
            s.Fitness = er.Result[0];
        }

    }
}

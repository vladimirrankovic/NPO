using System;
using System.Collections.Generic;
using System.Text;
using JARE.Base;
using JARE.Base.solutionType;
using JARE.problems.Finance;
using JARE.problems.Finance.DataTypes;
using JARE.EvaluationPool;
using System.IO;

namespace JARE.problems
{
    public class PortfolioOptimizationMO : PortfolioOptimization
    {
        //Input time series sets
        //protected AssetSets assetSets;

        //List of evaluation criteria
        public List<EvaluationCriteria> evaluationCriteria;

        public PortfolioOptimizationMO(List<EvaluationCriteria> inputEvaluationCriteria, DecisionVariableType variableType, int numberOfVariables, int cardinality, 
            double lowerBound, double upperBound, ExecutionType executionType = ExecutionType.singleThread) : base(
                                       variableType, numberOfVariables, cardinality, lowerBound, upperBound, executionType)//Proveriti da li setovi vremenskih serija moraju da imaju istu kardinalnost
		{
            evaluationCriteria = inputEvaluationCriteria;
            //assetSets = TSS;
            m_numberOfObjectives = evaluationCriteria.Count;

            //Init();
        }
                
        public override void evaluate(Solution inputSolution)
        {
            bool done = false;

            do
            {
                solution = inputSolution;

                int currentTimeSeriesID = 0;
                int previousTimeSeriesID = 0;
                try
                {
                    for(int i = 0; i < evaluationCriteria.Count; i++)
                    {
                        EvaluationCriteria EC = evaluationCriteria[i];
                        if (EC.GetType() == RETURN_BASED_CRITERIA || EC.GetType() == CREDIT_BASED_CRITERIA)
                        {
                            if (EC.GetType() == RETURN_BASED_CRITERIA)
                            {
                                currentTimeSeriesID = ((EvaluationCriteriaReturnBased)EC).TimeSeriesSetID;
                                if (currentTimeSeriesID != previousTimeSeriesID)
                                {
                                    AssetSet assetTimeSeriesSet = (AssetSet)m_parameters.getParameter("AssetTimeSeriesSet");
                                   GeneratePortfolioTimeSeries(inputSolution, assetTimeSeriesSet, PortfolioTimeSeries);
                                }
                            }
                            else if (EC.GetType() == CREDIT_BASED_CRITERIA)
                            {

                                currentTimeSeriesID = ((EvaluationCriteriaCreditExposureBased)EC).TimeSeriesSetID;
                                if (currentTimeSeriesID != previousTimeSeriesID)
                                {
                                    AssetSet assetTimeSeriesSet = (AssetSet)m_parameters.getParameter("AssetTimeSeriesSet");
                                    AssetSet creditExposureTimeSeriesSet = (AssetSet)m_parameters.getParameter("CreditExposureTimeSeriesSet");
                                    GeneratePortfolioCreditExposureTimeSeries(creditExposureTimeSeriesSet, assetTimeSeriesSet, PortfolioCreditExposureTimeSeries);
                                }
                            }
                            previousTimeSeriesID = currentTimeSeriesID;
                        }

                        if (EC.ExecutionType.Equals(EvaluationCriteria.executionType.singleThread))
                        {
                            double objectiveValue;
                            objectiveValue = EvaluationFunction(EC);
                            inputSolution.setObjective(i, objectiveValue);
                        }
                    }
                    done = true;
                }
                catch (Exception ex)
                {
                    if(ex.Message.Equals("R")) OnREvaluationException(inputSolution, ex);
                    else throw ex;
                }
            } while (!done);
        }

        #region Portfolio parameters
        public override void GetParametersForOptimalPortfolio(Solution solution, Dictionary<string, double> parametersForOptimalPortfolio, bool allParameters = true)
        {
            //parametersForOptimalPortfolio.Clear();
            bool returnBasedCriteriaDone = false;
            bool creditExposureBasedCriteriaDone = false;
            foreach (EvaluationCriteria EC in evaluationCriteria)
            {
                if (EC.GetType() == RETURN_BASED_CRITERIA)
                {
                    if (!returnBasedCriteriaDone)
                    {
                        AssetSet assetTimeSeriesSet = (AssetSet)m_parameters.getParameter("AssetTimeSeriesSet");
                        GetReturnBasedParametersForOptimalPortfolio(solution, parametersForOptimalPortfolio, assetTimeSeriesSet, allParameters);
                        returnBasedCriteriaDone = true;
                    }
                }
                else if (EC.GetType() == CREDIT_BASED_CRITERIA)
                {
                    if (!creditExposureBasedCriteriaDone)
                    {
                        AssetSet assetTimeSeriesSet = (AssetSet)m_parameters.getParameter("AssetTimeSeriesSet");
                        AssetSet creditExposureTimeSeriesSet = (AssetSet)m_parameters.getParameter("CreditExposureTimeSeriesSet");
                        if (!returnBasedCriteriaDone)
                        {
                            GetReturnBasedParametersForOptimalPortfolio(solution, parametersForOptimalPortfolio, assetTimeSeriesSet, allParameters);
                            returnBasedCriteriaDone = true;
                        }
                        GetCreditExposBasedParametersForOptimalPortfolio(parametersForOptimalPortfolio, assetTimeSeriesSet, creditExposureTimeSeriesSet);
                        creditExposureBasedCriteriaDone = true;
                    }
                }
                else if (EC.GetType() == PARAMETER_BASED_CRITERIA)
                {
                    GetParametersForOptimalParameterBasedPortfolio(solution, parametersForOptimalPortfolio, allParameters);
                }
                else throw new Exception("Portfolio optimization.EvaluationFunction: Unknown criteria type");
            }
        }
        public override void GetReturnBasedParametersForOptimalPortfolio(Solution solution, Dictionary<string, double> parametersForOptimalPortfolio, AssetSet timeSeriesSet, bool allParameters)
        {
            base.GetReturnBasedParametersForOptimalPortfolio(solution, parametersForOptimalPortfolio, timeSeriesSet, allParameters);
            int objectiveIndex;

            if (IsEvaluationCriteria(EvaluationCriteriaReturnBased.criteriaType.VaRGarch, out objectiveIndex))
            {
                AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.VaRGarch.ToString(), solution.getObjective(objectiveIndex));
            }
            else if (IsEvaluationCriteria(EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR, out objectiveIndex))
            {
                AddParameterToDictionary(allParameters, parametersForOptimalPortfolio, EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR.ToString(), solution.getObjective(objectiveIndex));
            }            
        }

        public bool IsEvaluationCriteria(EvaluationCriteriaReturnBased.criteriaType criteriaType, out int objectiveIndex)
        {
            bool flag = false;
            objectiveIndex = 0;
            foreach (EvaluationCriteria EC in evaluationCriteria)
            {
                if (EC.GetType() == RETURN_BASED_CRITERIA)
                {
                    if (((EvaluationCriteriaReturnBased)EC).CriteriaType == criteriaType)
                    {
                        flag = true;
                        break;
                    }
                    objectiveIndex++;
                }
            }
            return flag;
        }

        #endregion

        public override bool Init()
        {
            bool VaRGARCH = false;

            foreach (EvaluationCriteria EC in evaluationCriteria)
            {
                if (EC.GetType() == RETURN_BASED_CRITERIA)
                {
                    //AssetSets assetsTimeSeriesSets = (AssetSets)m_parameters.getParameter("AssetsTimeSeriesSets");
                    if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.VaRGarch)
                    {
                        //AssetSet timeSeriesSetVaRGarch = assetsTimeSeriesSets.Get(((EvaluationCriteriaReturnBased)EC).TimeSeriesSetID);
                        AssetSet timeSeriesSetVaRGarch = (AssetSet)m_parameters.getParameter("AssetTimeSeriesSet");
                        string RScriptPath = (string)m_parameters.getParameter("RScriptPath");
                        double varThreshold = (double)m_parameters.getParameter("varThreshold");
                        int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
                        DateTime optimizationDate = (DateTime)m_parameters.getParameter("optimizationDate");
                        m_VaRGARCH = new VaRGARCH(timeSeriesSetVaRGarch, evaluationCriteria, optimizationDate, evaluationPeriod, ReturnPeriod, m_VariableType, true, varThreshold, m_GARCHparameters, RScriptPath, executionType);
                        VaRGARCH = true;
                        break;
                    }
                    else if (((EvaluationCriteriaReturnBased)EC).CriteriaType == EvaluationCriteriaReturnBased.criteriaType.CR_GARCHVaR)
                    {
                        //AssetSet timeSeriesSetVaRGarch = assetsTimeSeriesSets.Get(((EvaluationCriteriaReturnBased)EC).TimeSeriesSetID);
                        AssetSet timeSeriesSetVaRGarch = (AssetSet)m_parameters.getParameter("AssetTimeSeriesSet");
                        double VaRThreshold = (double)m_parameters.getParameter("varThreshold");
                        string RScriptPath = (string)m_parameters.getParameter("RScriptPath");
                        int backtestingPeriod = (int)m_parameters.getParameter("backtestingPeriod");
                        int evaluationPeriod = (int)m_parameters.getParameter("evaluationPeriod");
                        DateTime optimizationDate = (DateTime)m_parameters.getParameter("optimizationDate");
                        AssetSet stressTimeSeriesSet = null;
                        if(m_parameters.ContainsParameter("stressTimeSeriesSet")) stressTimeSeriesSet = (AssetSet)m_parameters.getParameter("stressTimeSeriesSet");
                        else
                        {
                            if(System.Windows.Forms.MessageBox.Show("Time Series needed for stress testing is not defined. Do you want to proceed?", "", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                                return false;
                        }

                        CRInR = new RCapitalRequirements(timeSeriesSetVaRGarch, optimizationDate, evaluationPeriod, backtestingPeriod, ReturnPeriod, m_VariableType, VaRThreshold, RScriptPath, executionType, evaluationCriteria, stressTimeSeriesSet);
                    }
                }
            }

            return VaRGARCH;
        }
        
        //protected void DataValidation(AssetSets timeSeriesSets)
        //{
        //    for (int i = 0; i < timeSeriesSets.GetCount(); i++)
        //    {
        //        AssetSet timeSeriesSet = timeSeriesSets.GetAt(i);
        //        DataValidation(timeSeriesSet);
        //    }
        //} 
        
        public override void JobResultsToObjectives(EvaluationResult er, Solution s)
        {
            s.setObjective(0, er.Result[0]);
            s.setObjective(1, er.Result[1]);

            //int c = 0;
            //int binderCounter = 0;
            //foreach (EvaluationCriteria EC in evaluationCriteria)
            //{
            //    if (EC.ExecutionType.Equals(EvaluationCriteria.executionType.viaBinder))
            //    {
            //        s.setObjective(c, er.Result[binderCounter]);
            //        binderCounter++;
            //    }
            //    c++;
            //}
        }

        public override void PrintGenerationReport(SolutionSet population, SolutionSet bestFront, int genNO)
        {
            //string generationsPrintingPath = (string)m_parameters.getParameter("generationsPrintingPath");
            string generationsPrintingPath = (string)m_parameters.getParameter("PrintDirectory");

            string bestFrontFileName = generationsPrintingPath + "\\BestFrontGeneration-" + genNO.ToString() + ".csv";
            StreamWriter writer = new StreamWriter(bestFrontFileName, false);
            PrintResults(writer, bestFront);
            writer.Dispose();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioGUI.DataTypes;
using JARE.problems.Finance;
using PortfolioGUI.ServiceLayer.Properties;
using System.IO;
using JARE.problems.Finance.DataTypes;

namespace PortfolioGUI.ServiceLayer
{
    public class ParametersService
    {
        //public static Parameters Parameters { get; set; }

        //public static Parameters CreateParameters(int populationSize, SelectionModeEnum selectionMode,
        //    GeneticMethodEnum geneticMethod, int iterations, PortfolioOptimization.EvaluationCriteria fitnessCriteria1,
        //    PortfolioOptimization.EvaluationCriteria fitnessCriteria2, DateTime evaluationEndDate, int weightParamDiscret, int interestRate, int evaluationPeriod,
        //    int varTreshold, int returnPeriod)
        //{
        //    Parameters parameters = new Parameters(populationSize, selectionMode, functionsSet, geneticMethod, iterations, fitnessCriteria1, fitnessCriteria2,
        //        evaluationEndDate, weightParamDiscret, interestRate, evaluationPeriod, varTreshold, returnPeriod);
        //    Parameters = parameters;
            
        //    return parameters;
        //}

        public void LoadParametersFromFile(Parameters parameters, string fileName)
        {
            Dictionary<string, string> parametersDict = new Dictionary<string, string>();
            StreamReader reader = new StreamReader(fileName);
            string line = string.Empty;
            
            try
            {
                // read the data
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    parametersDict.Add(words[0], words[1]);
                }
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                // close file
                if (reader != null)
                    reader.Close();
            }

            try
            {
                if (parametersDict.ContainsKey("Population size")) parameters.PopulationSize = GetIntegerFromString(parametersDict["Population size"]);
                if (parametersDict.ContainsKey("Generations")) parameters.Generations = GetIntegerFromString(parametersDict["Generations"]);
                if (parametersDict.ContainsKey("Mutation probability")) parameters.MutationProbability = GetDoubleFromString(parametersDict["Mutation probability"]);
                if (parametersDict.ContainsKey("Mutation perturbation")) parameters.MutationPerturbation = GetDoubleFromString(parametersDict["Mutation perturbation"]);
                if (parametersDict.ContainsKey("Evaluation period")) parameters.EvaluationPeriod = GetIntegerFromString(parametersDict["Evaluation period"]);
                if (parametersDict.ContainsKey("Return period")) parameters.ReturnPeriod = GetIntegerFromString(parametersDict["Return period"]);
                if (parametersDict.ContainsKey("Interest rate")) parameters.InterestRate = GetDoubleFromString(parametersDict["Interest rate"]);
                if (parametersDict.ContainsKey("VaR treshhold")) parameters.VarTreshold = GetDoubleFromString(parametersDict["VaR treshhold"]);
                if (parametersDict.ContainsKey("Lambda")) parameters.Lambda = GetDoubleFromString(parametersDict["Lambda"]);
                if (parametersDict.ContainsKey("Rebal. period")) parameters.RebalancingDuration = GetIntegerFromString(parametersDict["Rebal. period"]);
                if (parametersDict.ContainsKey("Rebal. buffer")) parameters.RebalancingBuffer = GetDoubleFromString(parametersDict["Rebal. buffer"]);
                if (parametersDict.ContainsKey("Criteria change")) parameters.CriteriaChange = GetDoubleFromString(parametersDict["Criteria change"]);

                if (parametersDict.ContainsKey("Optimization Date")) parameters.OptimizationDate = DateTime.Parse(parametersDict["Optimization Date"]);
                if (parametersDict.ContainsKey("Rebal. Start Date")) parameters.RebalancingStartDate = DateTime.Parse(parametersDict["Rebal. Start Date"]);

                if (parametersDict.ContainsKey("Variable type")) parameters.DecisionVarType = (DecisionVariableType)GetIntegerFromString(parametersDict["Variable type"]);
                if (parametersDict.ContainsKey("Fitness criteria 1")) parameters.OptimizationEvaluationCriteria1.CriteriaType = (EvaluationCriteriaReturnBased.criteriaType)GetIntegerFromString(parametersDict["Fitness criteria 1"]);
                if (parametersDict.ContainsKey("Fitness criteria 2")) parameters.OptimizationEvaluationCriteria2.CriteriaType = (EvaluationCriteriaReturnBased.criteriaType)GetIntegerFromString(parametersDict["Fitness criteria 2"]);
                if (parametersDict.ContainsKey("Fitness criteria 3")) parameters.OptimizationEvaluationCriteria3.CriteriaType = (EvaluationCriteriaCreditExposureBased.criteriaType)GetIntegerFromString(parametersDict["Fitness criteria 3"]);
                if (parametersDict.ContainsKey("Rebal. criteria")) parameters.RebalanceEvaluationCriteria.CriteriaType = (EvaluationCriteriaReturnBased.criteriaType)GetIntegerFromString(parametersDict["Rebal. criteria"]);

                if (parametersDict.ContainsKey("Crossover probability")) parameters.CrossoverProbability = GetDoubleFromString(parametersDict["Crossover probability"]);
                if (parametersDict.ContainsKey("CRType")) parameters.CRType = (PortfolioRebalance.CapitalRequirementsType)GetIntegerFromString(parametersDict["CRType"]);
                if (parametersDict.ContainsKey("CR change")) parameters.CRChange = GetDoubleFromString(parametersDict["CR change"]);
                if (parametersDict.ContainsKey("Backtesting period")) parameters.BacktestingPeriod = GetIntegerFromString(parametersDict["Backtesting period"]);
                if (parametersDict.ContainsKey("TurnOver limit type")) parameters.TOLimitType = (PortfolioRebalance.TurnOverLimitType)GetIntegerFromString(parametersDict["TurnOver limit type"]);

                if (parametersDict.ContainsKey("MultiObjective Tolerance")) parameters.PlateauTolerance = GetDoubleFromString(parametersDict["MultiObjective Tolerance"]);
                if (parametersDict.ContainsKey("Plateau Tolerance")) parameters.PlateauTolerance = GetDoubleFromString(parametersDict["Plateau Tolerance"]);

                if (parametersDict.ContainsKey("MultiObjective Plateau Generations")) parameters.MaxPlateauGenerations = GetIntegerFromString(parametersDict["MultiObjective Plateau Generations"]);
                if (parametersDict.ContainsKey("Max Plateau Generations")) parameters.MaxPlateauGenerations = GetIntegerFromString(parametersDict["Max Plateau Generations"]);
                
                if (parametersDict.ContainsKey("Scheduled Rebalancing")) parameters.ScheduledRebalancing = GetBoolFromString(parametersDict["Scheduled Rebalancing"]);
                if (parametersDict.ContainsKey("Max Allowed Risk")) parameters.MaxAllowedRisk = GetDoubleFromString(parametersDict["Max Allowed Risk"]);

                if (parametersDict.ContainsKey("Cardinality")) parameters.Cardinality = GetIntegerFromString(parametersDict["Cardinality"]);
                if (parametersDict.ContainsKey("Lower Bound")) parameters.LowerBound = GetDoubleFromString(parametersDict["Lower Bound"]);
                if (parametersDict.ContainsKey("Upper Bound")) parameters.UpperBound = GetDoubleFromString(parametersDict["Upper Bound"]);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //PortfolioOptimization.EvaluationCriteria fitnessCriteria1 = GetEnumFromString<PortfolioOptimization.EvaluationCriteria>(properties["fitnessCriteria1"]);
            //PortfolioOptimization.EvaluationCriteria fitnessCriteria2 = GetEnumFromString<PortfolioOptimization.EvaluationCriteria>(properties["fitnessCriteria2"]);
            //SelectionModeEnum selectionMode = GetEnumFromString<SelectionModeEnum>(properties["selectionMode"]);
            //FunctionsSetEnum functionsSet = GetEnumFromString<FunctionsSetEnum>(properties["functionsSet"]);
            //GeneticMethodEnum geneticMethod = GetEnumFromString<GeneticMethodEnum>(properties["geneticMethod"]);
        }

        public void SaveParametersToFile(Parameters parameters, string fileName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName, false);

                sw.WriteLine("Population size" + "," + parameters.PopulationSize.ToString());
                sw.WriteLine("Generations" + "," + parameters.Generations.ToString());
                sw.WriteLine("Mutation probability" + "," + parameters.MutationProbability.ToString());
                sw.WriteLine("Mutation perturbation" + "," + parameters.MutationPerturbation.ToString());
                sw.WriteLine("Evaluation period" + "," + parameters.EvaluationPeriod.ToString());
                sw.WriteLine("Return period" + "," + parameters.ReturnPeriod.ToString());
                sw.WriteLine("Interest rate" + "," + parameters.InterestRate.ToString());
                sw.WriteLine("VaR treshhold" + "," + parameters.VarTreshold.ToString());
                sw.WriteLine("Lambda" + "," + parameters.Lambda.ToString());
                sw.WriteLine("Rebal. period" + "," + parameters.RebalancingDuration.ToString());
                sw.WriteLine("Rebal. buffer" + "," + parameters.RebalancingBuffer.ToString());
                sw.WriteLine("Criteria change" + "," + parameters.CriteriaChange.ToString());
                sw.WriteLine("Optimization Date" + "," + parameters.OptimizationDate.ToString());
                sw.WriteLine("Rebal. Start Date" + "," + parameters.RebalancingStartDate.ToString());
                sw.WriteLine("Variable type" + "," + ((int)parameters.DecisionVarType).ToString());
                sw.WriteLine("Fitness criteria 1" + "," + ((int)parameters.OptimizationEvaluationCriteria1.CriteriaType).ToString());
                sw.WriteLine("Fitness criteria 2" + "," + ((int)parameters.OptimizationEvaluationCriteria2.CriteriaType).ToString());
                sw.WriteLine("Fitness criteria 3" + "," + ((int)parameters.OptimizationEvaluationCriteria3.CriteriaType).ToString());
                sw.WriteLine("Rebal. criteria" + "," + ((int)parameters.RebalanceEvaluationCriteria.CriteriaType).ToString());
                sw.WriteLine("Crossover probability" + "," + parameters.CrossoverProbability.ToString());
                sw.WriteLine("CRType" + "," + ((int)parameters.CRType).ToString());
                sw.WriteLine("CR change" + "," + parameters.CRChange.ToString());
                sw.WriteLine("Backtesting period" + "," + parameters.BacktestingPeriod.ToString());
                sw.WriteLine("TurnOver limit type" + "," + ((int)parameters.TOLimitType).ToString());
                sw.WriteLine("Plateau Tolerance" + "," + parameters.PlateauTolerance.ToString());
                sw.WriteLine("Max Plateau Generations" + "," + parameters.MaxPlateauGenerations.ToString());
                sw.WriteLine("Scheduled Rebalancing" + "," + parameters.ScheduledRebalancing.ToString());
                sw.WriteLine("Max Allowed Risk" + "," + parameters.MaxAllowedRisk.ToString());
                sw.WriteLine("Cardinality" + "," + parameters.Cardinality.ToString());
                sw.WriteLine("Lower Bound" + "," + parameters.LowerBound.ToString());
                sw.WriteLine("Upper Bound" + "," + parameters.UpperBound.ToString());

                sw.Dispose();
            }
            catch (Exception)
            {
                return;
            }

        }

        private static int GetIntegerFromString(string s)
        {
            int variable;
            if (!int.TryParse(s, out variable)) throw new InvalidDataException("Parametar: " + s + " nije u ispravnom formatu.");
            return variable;
        }
        private static bool GetBoolFromString(string s)
        {
            bool variable;
            if (!bool.TryParse(s, out variable)) throw new InvalidDataException("Parametar: " + s + " nije u ispravnom formatu.");
            return variable;
        }

        private static double GetDoubleFromString(string s)
        {
            double variable;
            if (!double.TryParse(s, out variable)) throw new InvalidDataException("Parametar: " + s + " nije u ispravnom formatu.");
            return variable;
        }

        private static T GetEnumFromString<T>(string s) where T : struct, IConvertible
        {
            T t = (T) Enum.Parse(typeof(T), s, true);
            return t;
        }
    }
}

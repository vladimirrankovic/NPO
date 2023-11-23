using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POUI.DataTypes;
using JARE.problems.Finance;
using POUI.ServiceLayer.Properties;
using System.IO;

namespace POUI.ServiceLayer
{
    public class ParametersService
    {
        public static Parameters Parameters { get; set; }

        public static Parameters CreateParameters(int populationSize, SelectionModeEnum selectionMode,
            FunctionsSetEnum functionsSet, GeneticMethodEnum geneticMethod, int iterations, PortfolioOptimization.EvaluationCriteria fitnessCriteria1,
            PortfolioOptimization.EvaluationCriteria fitnessCriteria2, DateTime evaluationEndDate, int weightParamDiscret, int interestRate, int evaluationPeriod,
            int varTreshold, int returnPeriod)
        {
            Parameters parameters = new Parameters(populationSize, selectionMode, functionsSet, geneticMethod, iterations, fitnessCriteria1, fitnessCriteria2,
                evaluationEndDate, weightParamDiscret, interestRate, evaluationPeriod, varTreshold, returnPeriod);
            Parameters = parameters;
            
            return parameters;
        }

        public static Parameters LoadParametersFromFile(string fileName)
        {
            Dictionary<string, string> properties = PropertiesService.PropertiesFromFile(fileName);
            int populationSize = GetIntegerFromString(properties["populationSize"]);
            SelectionModeEnum selectionMode = GetEnumFromString<SelectionModeEnum>(properties["selectionMode"]);
            FunctionsSetEnum functionsSet = GetEnumFromString<FunctionsSetEnum>(properties["functionsSet"]);
            GeneticMethodEnum geneticMethod = GetEnumFromString<GeneticMethodEnum>(properties["geneticMethod"]);
            int iterations = GetIntegerFromString(properties["iterations"]);
            PortfolioOptimization.EvaluationCriteria fitnessCriteria1 = GetEnumFromString<PortfolioOptimization.EvaluationCriteria>(properties["fitnessCriteria1"]);
            PortfolioOptimization.EvaluationCriteria fitnessCriteria2 = GetEnumFromString<PortfolioOptimization.EvaluationCriteria>(properties["fitnessCriteria2"]);
            DateTime evaluationEndDate = DateTime.Parse(properties["evaluationEndDate"]);
            int interestRate = GetIntegerFromString(properties["interestRate"]);
            int evaluationPeriod = GetIntegerFromString(properties["evaluationPeriod"]);
            int weightParamDiscret = GetIntegerFromString(properties["weightParamDiscret"]);
            int varTreshold = GetIntegerFromString(properties["varTreshold"]);
            int returnPeriod = GetIntegerFromString(properties["returnPeriod"]);

            Parameters parameters = new Parameters(populationSize, selectionMode, functionsSet, geneticMethod, iterations, fitnessCriteria1, fitnessCriteria2,
                evaluationEndDate, weightParamDiscret, interestRate, evaluationPeriod, varTreshold, returnPeriod);
            Parameters = parameters;

            return parameters;
        }

        private static int GetIntegerFromString(string s)
        {
            int variable;
            if (!int.TryParse(s, out variable)) throw new InvalidDataException("Parametar: " + s + " nije u ispravnom formatu.");
            return variable;
        }

        private static T GetEnumFromString<T>(string s) where T : struct, IConvertible
        {
            T t = (T) Enum.Parse(typeof(T), s, true);
            return t;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SignalFilteringGUI.Util;
using System.IO;
using JARE.problems.Finance.DataTypes;

namespace SignalFilteringGUI.Util.ServiceLayer
{
    public class ParametersService
    {
          public void LoadParametersFromFile(OptimizationParameters parameters, string fileName)
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
                if (parametersDict.ContainsKey("Crossover probability")) parameters.CrossoverProbability = GetDoubleFromString(parametersDict["Crossover probability"]);

                if (parametersDict.ContainsKey("Plateau Tolerance")) parameters.PlateauTolerance = GetDoubleFromString(parametersDict["Plateau Tolerance"]);

                if (parametersDict.ContainsKey("Max Plateau Generations")) parameters.MaxPlateauGenerations = GetIntegerFromString(parametersDict["Max Plateau Generations"]);
                

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

        public void SaveParametersToFile(OptimizationParameters parameters, string fileName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName, false);

                sw.WriteLine("Population size" + "," + parameters.PopulationSize.ToString());
                sw.WriteLine("Generations" + "," + parameters.Generations.ToString());
                sw.WriteLine("Mutation probability" + "," + parameters.MutationProbability.ToString());
                sw.WriteLine("Mutation perturbation" + "," + parameters.MutationPerturbation.ToString());
                sw.WriteLine("Crossover probability" + "," + parameters.CrossoverProbability.ToString());
                sw.WriteLine("Plateau Tolerance" + "," + parameters.PlateauTolerance.ToString());
                sw.WriteLine("Max Plateau Generations" + "," + parameters.MaxPlateauGenerations.ToString());

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

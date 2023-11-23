using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using JARE.Base;
using JARE.Base.solutionType;
using JARE.problems.Finance;
using JARE.problems.Finance.DataTypes;
using PortfolioOptimizationGA = JARE.metaheuristics.singleObjective.geneticAlgorithm.PortfolioOptimizationGA;

namespace JARE.problems.Finance
{
    public class VaRExpVaRComparison
    {
        PortfolioOptimizationSO po;

        AssetSet Assets;
        DateTime evaluationEndDate;
        int evaluationPeriod;
        int ReturnPeriod;
        double varThreshold;
        DecisionVariableType variableType;
        int testingPeriod;

        public delegate void IterationCounterChangedHandler(int iterationCount);
        public event IterationCounterChangedHandler IterationCounterChanged;

        public VaRExpVaRComparison(AssetSet timeSeriesSet, 
                                     DateTime evaluationEndDate, int evaluationPeriod, int ReturnPeriod,
                                     double varThreshold, DecisionVariableType variableType, int testingPeriod)
		{
            this.Assets = timeSeriesSet;
            this.evaluationEndDate = evaluationEndDate;
            this.evaluationPeriod = evaluationPeriod;
            this.ReturnPeriod = ReturnPeriod;
            this.varThreshold = varThreshold;
            this.variableType = variableType;
            this.testingPeriod = testingPeriod;

        }

        public void Execute(StreamWriter sw)
        {
            double[] weightParameter = new double[2]{1.0, 0.0};
            //double[] tmp = new double[2] { 1.0, 1.0 };
            //double[,] tmp = new double[2, 2] { { 1.0, 0.0 }, { 1.0, 0.0 } };
            double[,] tmp = new double[2, 2] { { -1.0, 0.0 }, { -1.0, 0.0 } };
            EvaluationCriteriaReturnBased[] EC = new EvaluationCriteriaReturnBased[1] { new EvaluationCriteriaReturnBased(EvaluationCriteriaReturnBased.criteriaType.VaR, EvaluationCriteria.executionType.singleThread) };

            JARE.Base.SolutionSet s = new SolutionSet();
            DateTime EndDateTmp = evaluationEndDate;
            Solution StaticPortfolio = new Solution();

            string strParameters = "date,averageReturn,standardDeviation,sharpIndex,sortinoIndex,VaR,cVaR,expVaR,expcVaR,Skewness,Kurtosis,Return";
            sw.WriteLine(strParameters);
            int counter = 0;
            int i = 0;

            //sw.WriteLine("Dynamic portfolio - Var");
            //while (counter < testingPeriod)
            //{
            //    EndDateTmp = evaluationEndDate.AddDays(i);
            //    if (EndDateTmp.DayOfWeek == DayOfWeek.Saturday || EndDateTmp.DayOfWeek == DayOfWeek.Sunday)
            //    {
            //        i++;
            //        continue;
            //    }

            //    po = new PortfolioOptimizationSO(1, Assets, EndDateTmp, evaluationPeriod, ReturnPeriod, 0, varThreshold, EC, weightParameter, tmp, PortfolioOptimization.DecisionVariableType.capitalInvestedProportionType, this.Assets.Count);
            //    AlgorithmExecution(po, ref s);

            //    JARE.Base.Solution bestSolution = s.getSolution(s.size() - 1);
            //    po.evaluate(bestSolution);
            //    PrintResults(po, sw);
            //    if (counter == 0)
            //    {
            //        StaticPortfolio = bestSolution;
            //    }

            //    EndDateTmp = EndDateTmp.AddDays(1);
            //    while (EndDateTmp.DayOfWeek == DayOfWeek.Saturday || EndDateTmp.DayOfWeek == DayOfWeek.Sunday)
            //    {
            //        EndDateTmp = EndDateTmp.AddDays(1);
            //    }
            //    po = new PortfolioOptimizationSO(1, Assets, EndDateTmp, evaluationPeriod, ReturnPeriod, 0, varThreshold, EC, weightParameter, tmp, PortfolioOptimization.DecisionVariableType.capitalInvestedProportionType, this.Assets.Count);
            //    po.evaluate(bestSolution);
            //    PrintResults(po, sw);

            //    counter++;
            //    i++;
            //}

            //sw.WriteLine("Static portfolio - Var");
            //counter = 0;
            //i = 0;
            //while (counter < testingPeriod)
            //{
            //    EndDateTmp = evaluationEndDate.AddDays(i);
            //    if (EndDateTmp.DayOfWeek == DayOfWeek.Saturday || EndDateTmp.DayOfWeek == DayOfWeek.Sunday)
            //    {
            //        i++;
            //        continue;
            //    }

            //    po = new PortfolioOptimizationSO(1, Assets, EndDateTmp, evaluationPeriod, ReturnPeriod, 0, varThreshold, EC, weightParameter, tmp, PortfolioOptimization.DecisionVariableType.capitalInvestedProportionType, this.Assets.Count);
            //    po.evaluate(StaticPortfolio);
            //    PrintResults(po, sw);
            //    counter++;
            //    i++;
            //}


            EC[0].CriteriaType = EvaluationCriteriaReturnBased.criteriaType.ExpWeightedVaR;

            counter = 0;
            i = 0;
            sw.WriteLine("Dynamic portfolio - expVar");
            while (counter < testingPeriod)
            {
                EndDateTmp = evaluationEndDate.AddDays(i);
                if (EndDateTmp.DayOfWeek == DayOfWeek.Saturday || EndDateTmp.DayOfWeek == DayOfWeek.Sunday)
                {
                    i++;
                    continue;
                }

                po = new PortfolioOptimizationSO(1, Assets, EndDateTmp, evaluationPeriod, ReturnPeriod, EC[0], /*weightParameter, tmp, */DecisionVariableType.capitalInvested, this.Assets.Count, 0.0, 1.0);
                po.m_parameters.setParameter("varThreshold", varThreshold);
                po.Init();
                AlgorithmExecution(po, ref s); // Generational GA with fitness

                JARE.Base.Solution bestSolution = s.getSolution(s.size() - 1);
                po.evaluate(bestSolution);
                PrintResults(po, sw);
                if (counter == 0)
                {
                    StaticPortfolio = bestSolution;
                }

                EndDateTmp = EndDateTmp.AddDays(1);
                while (EndDateTmp.DayOfWeek == DayOfWeek.Saturday || EndDateTmp.DayOfWeek == DayOfWeek.Sunday)
                {
                    EndDateTmp = EndDateTmp.AddDays(1);
                }
                po = new PortfolioOptimizationSO(1, Assets, EndDateTmp, evaluationPeriod, ReturnPeriod, EC[0], /*weightParameter, tmp, */DecisionVariableType.capitalInvested, Assets.Count, 0.0, 1.0);
                po.m_parameters.setParameter("varThreshold", varThreshold);
                po.Init();
                po.evaluate(bestSolution);
                PrintResults(po, sw);
                 
                counter++;
                i++;
                // raise event
                OnIterationCounterChanged(i);

            }

            sw.WriteLine("Static portfolio - expVar");
            counter = 0;
            i = 0;
            while (counter < testingPeriod)
            {
                EndDateTmp = evaluationEndDate.AddDays(i);
                if (EndDateTmp.DayOfWeek == DayOfWeek.Saturday || EndDateTmp.DayOfWeek == DayOfWeek.Sunday)
                {
                    i++;
                    continue;
                }

                po = new PortfolioOptimizationSO(1, Assets, EndDateTmp, evaluationPeriod, ReturnPeriod, EC[0], /*weightParameter, tmp, */DecisionVariableType.capitalInvested, Assets.Count, 0.0, 1.0);
                po.m_parameters.setParameter("varThreshold", varThreshold);
                po.Init();
                po.evaluate(StaticPortfolio);
                PrintResults(po, sw);
                counter++;
                i++;
                // raise event
                OnIterationCounterChanged(i);

            }


            sw.Dispose();
        }

        private void AlgorithmExecution(PortfolioOptimizationSO po, ref JARE.Base.SolutionSet s)
        {
            PortfolioOptimizationGA algorithm; // The algorithm to use
            Operator crossover; // Crossover operator
            Operator mutation; // Mutation operator
            Operator selection; // Selection operator

            // Mutation and Crossover for Real codification 
            crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WeightsSinglePointCrossover");
            crossover.setParameter("probability", 1.0);
            crossover.setParameter("distributionIndex", 20.0);

            mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("WeightsUniformMutation");
            mutation.setParameter("probability", 0.5);
            mutation.setParameter("perturbationIndex", 0.5);

            /* Selection Operator */
            selection = JARE.Base.operators.selection.SelectionFactory.getSelectionOperator("BestSolutionSelection");
            JARE.Base.operators.comparator.FitnessComparator comparator = new JARE.Base.operators.comparator.FitnessComparator();
            selection.setParameter("comparator", comparator);
            Operator selectionPopulation = JARE.Base.operators.selection.SelectionFactory.getSelectionOperator("RouletteWheelSelection");
            int populationSize = Assets.Count * 5;
            selectionPopulation.setParameter("size", populationSize - 1);

            algorithm = new PortfolioOptimizationGA(po); // Generational GA with fitness

            /* Algorithm parameters*/
            algorithm.setInputParameter("populationSize", populationSize);
            algorithm.setInputParameter("maxEvaluations", 200);
            /* Add the operators to the algorithm*/
            algorithm.addOperator("crossover", crossover);
            algorithm.addOperator("mutation", mutation);
            algorithm.addOperator("selection", selection);
            algorithm.addOperator("selectionPopulation", selectionPopulation);
            ((PortfolioOptimizationGA)algorithm).Init();

            s = algorithm.execute();
        }

        void PrintResults(PortfolioOptimizationSO PortfolioOptimization, StreamWriter sw)
        {
            string strParameters;
            Dictionary<string, double> parametersForOptimalPortfolio = new Dictionary<string, double>();

            PortfolioOptimization.GetParametersForOptimalPortfolio(PortfolioOptimization.solution, parametersForOptimalPortfolio);
            strParameters = string.Empty;
            DateTime optimizationDate = (DateTime)PortfolioOptimization.m_parameters.getParameter("optimizationDate");

            strParameters += optimizationDate.Date.ToShortDateString() + ",";

            Dictionary<string, double>.Enumerator Enum = parametersForOptimalPortfolio.GetEnumerator();
            for (int kk = 0; kk < parametersForOptimalPortfolio.Count; kk++)
            {
                Enum.MoveNext();
                strParameters += (Enum.Current.Value.ToString("F10") + ",");
            }

            TimeSeries ReturnList = new TimeSeries();
            PortfolioOptimization.GenerateReturnList(evaluationPeriod, ReturnPeriod, ReturnList);
            strParameters += ReturnList.DataSeries[ReturnList.DataSeries.Count - 1].Value.ToString("F10") + ",";

            for (int kk = 0; kk < PortfolioOptimization.solution.numberOfVariables(); kk++)
            {
                strParameters += (PortfolioOptimization.solution.DecisionVariables[kk].getValue().ToString("F10") + ",");
            }
            //strParameters += "\n";
            sw.WriteLine(strParameters);
        }

        public void OnIterationCounterChanged(int iterationCount)
        {
            if (IterationCounterChanged != null) IterationCounterChanged(iterationCount);
        }

        
    }
}

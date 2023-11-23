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


namespace JARE.problems.Finance
{
    public class PortfolioRebalance : PortfolioOptimizationSO
    {
        public enum TurnOverLimitType
        {
            GreaterThan = 0,
            LessThan = 1,
            CumulativeLessThan = 2,
            WithinLimit_Ver2 = 3 //Version 2: 
        }
        public enum CapitalRequirementsType
        {
            None = 0,
            CR_Historical_VaR = 1,
            CR_GARCH_VaR = 2,
        }



        TurnOverLimitType TOLimitType;
        CapitalRequirementsType CRType;

        public PortfolioOptimizationGA algorithm; // The algorithm to use
        protected internal ParameterCollection printingData;
        Dictionary<string, double> portfolioParameters;
 
        DateTime rebalancingStartDate;
        int rebalancingPeriod;
        double turnoverTreshold;
        double criteriaChangeTreshold;
        EvaluationCriteria EC;
        double CRChangeTreshold;
        bool Canceled = false;
        bool scheduledRebalancing = false;
        //bool cumulativeBuffer;

        public delegate void RebalanceDayChangedHandler(int rebalanceDayCounter);
        public event RebalanceDayChangedHandler RebalanceDayChanged;
        public delegate void GenerationChangedHandler(string generationCounter);
        public event GenerationChangedHandler GenerationChanged;

 		/// <summary> Constructor</summary>
        /// <param name="timeSeriesSet">Set of assests time series </param>
        /// <param name="EC">Evaluation (Rebalance) criteria </param>
        /// <param name="rebalancingStartDate"> Start date of rebalancing procedure</param>
        /// <param name="rebalancingPeriod">Period of rebalancing procedure (number of days)</param>
        /// <param name="turnoverTreshold">Turnover treshold (amount of turnover that triggers the rebalance)</param>
        /// <param name="evaluationPeriod">Period (number of days) used for the evaluation of evaluation criteria (for optimization problems only)</param>
        /// <param name="ReturnPeriod">Period (number of days) for return calculation</param>
        /// <param name="cardinality">Maximum alowed number of assets comprising the portfolio</param>
        /// <param name="CRcalculation">Indicator whether calculation of Capital Requirements is included</param>
        /// <param name="varTreshold">VaR treshold</param>
        public PortfolioRebalance(EvaluationCriteria EC, DateTime rebalancingStartDate, int rebalancingPeriod, double turnoverTreshold, 
            int numberOfVariables, int cardinality, double lowerBound, double upperBound, CapitalRequirementsType CRType, TurnOverLimitType TOlimitType, bool scheduledRebalancing)
            : base(EC, DecisionVariableType.capitalInvested, numberOfVariables, cardinality, lowerBound, upperBound)
		{
            RealWeightsSolutionType m_solutionType = (RealWeightsSolutionType)new RealWeightsSolutionType(this);
            this.rebalancingStartDate = rebalancingStartDate;
            this.rebalancingPeriod = rebalancingPeriod;
            this.turnoverTreshold = turnoverTreshold*0.01;//percentage
            this.EC = EC;
            printingData = new ParameterCollection();
            //Ovo treba da se omoguci kroz intefejs da se zadaje 
            portfolioParameters = new Dictionary<string, double>();
            portfolioParameters.Add(EvaluationCriteriaReturnBased.criteriaType.StandardDeviation.ToString(), 0.0);
            portfolioParameters.Add(EvaluationCriteriaReturnBased.criteriaType.SharpIndex.ToString(), 0.0);
            portfolioParameters.Add(EvaluationCriteriaReturnBased.criteriaType.Skewness.ToString(), 0.0);
            portfolioParameters.Add(EvaluationCriteriaReturnBased.criteriaType.Kurtosis.ToString(), 0.0);
            portfolioParameters.Add(EvaluationCriteriaReturnBased.criteriaType.VaR.ToString(), 0.0);
            this.CRType = CRType;
            TOLimitType = TOlimitType;
            m_initialSolutions = new Dictionary<string, Solution>();
            algorithm = new PortfolioOptimizationGA(this); // Generational GA with fitness
            algorithm.IterationCounterChanged += new PortfolioOptimizationGA.IterationCounterChangedHandler(OnGenerationChanged);
            this.scheduledRebalancing = scheduledRebalancing;
        }

		/// <summary>Executes rebalancing procedure</summary>
        /// <param name="sw">The Stream Writer object for printing results</param>
        public void Execute(StreamWriter sw)
        {
            AssetSet timeSeriesSet = (AssetSet)m_parameters.getParameter("RebalanceAssetTimeSeriesSet");
            m_parameters.setParameter("AssetTimeSeriesSet", timeSeriesSet);//potrebno za PortfolioOptimizationSO
            JARE.Base.SolutionSet s = new SolutionSet();
            DateTime currentDate = rebalancingStartDate;
            Solution activePortfolio = new Solution();
            activePortfolio = null;
            double turnOver = 0.0;
            double activePortfolioValue = 1.0;
            double[] sharesInvestedActivePortfolio = null;
            
            //Read active portfolio (If resuming rebalancing) - Ucitavanje aktivnog portfolija (Samo u slucaju nastavka proracuna)
            if(m_parameters.ContainsParameter("activePortfolio"))
            { 
                double[] tmp1 = (double[])m_parameters.getParameter("activePortfolio");
                sharesInvestedActivePortfolio = new double[timeSeriesSet.Count];
                tmp1.CopyTo(sharesInvestedActivePortfolio, 0);
            }

            double[] sharesInvestedCurrentPortfolio = new double[timeSeriesSet.Count];
            double[] capitalInvestedWeightCurrentPortfolio = new double[timeSeriesSet.Count];

            //Open files for printing backtesting results
            StreamWriter SWCurrentPortfolio = null;
            StreamWriter SWActivePortfolio = null;
            if (CRType != CapitalRequirementsType.None)
            {
                string path = ((FileStream)sw.BaseStream).Name;
                string directoryName = Path.GetDirectoryName(path);
                SWCurrentPortfolio = new StreamWriter(directoryName + "\\CurrentPortfolioBacktesting.csv", false);
                SWActivePortfolio = new StreamWriter(directoryName + "\\ActivePortfolioBacktesting.csv", false);
            }//Open files for printing backtesting results

            //Print head line - Stampanje naslovne linije
            PrintHeadLine(sw);
            int counter = 0;
            int i = 0;
            int currentMonth = rebalancingStartDate.Month;
            bool scheduledRebalancingDone = false;

            double currentPortfolioCapitalRequirements = 0.0;
            double activePortfolioCapitalRequirements = 0.0;
            JARE.Base.Solution previousPortfolio = null;

            //Rebalancing - Rebalans portfolija
            while (counter < rebalancingPeriod)
            {
                if (Canceled) break;
                currentDate = rebalancingStartDate.AddDays(i);


                //Get current assets values - Ucitavanje trenutnih (na tekuci dan) vrednosti aseta
                double[] currentAssetsValues = new double[timeSeriesSet.Count];
                if (!timeSeriesSet.GetValues(currentDate, currentAssetsValues))
                {
                    i++;
                    continue;
                }//Get current assets values
                
                //
                if (scheduledRebalancing)
                {
                    if (currentDate.Month != currentMonth)
                    {
                        currentMonth = currentDate.Month;
                        scheduledRebalancingDone = false;
                    }
                    if (scheduledRebalancingDone || currentDate.DayOfWeek != DayOfWeek.Monday)
                    {
                        counter++;
                        i++;
                        continue;
                    }
                    else scheduledRebalancingDone = true;
                }

                //Clear data collection for printing
                printingData.Clear();

                //Determing the index value of current date in time series of assets - Odredjivanje indeksa aktuelnog datuma u vremenskoj seriji aseta
                int currentIndex;
                if (!timeSeriesSet.GetItemIndex(currentDate, out currentIndex)) throw new Exception("Input Date is not valid.");

                //Determing the start date for optimization - Odredjivanje pocetnog datuma vremenskih serija za optimizaciju 
                DateTime optimizationDate = (DateTime)m_parameters.getParameter("optimizationDate");
                optimizationDate = timeSeriesSet.GetDay(currentIndex);

                //Initializing base class - Inicijalizacija bazne klase
                InitPortfolioOptimization();

                JARE.Base.Solution currentPortfolio = new Solution(this);
                
                //Calculate current value of active portfolio - Odredjivanje trenutne vrednosti aktivnog portfolija
                if (activePortfolio == null && sharesInvestedActivePortfolio == null)//Start of the rebalancing - Pocetak rebalans procedure
                {
                    sharesInvestedActivePortfolio = new double[timeSeriesSet.Count];
                    //Starting value of active portfolio is set to 1 (If starting rebalancing) - Ukoliko je pocetak rebalans procedure (jos uvek ne postoji aktivni portfolio) vrednost aktivnog portfolija je 1
                    activePortfolioValue = 1.0;
                }
                else
                {
                    //Determing the value of active portflio
                    activePortfolioValue = 0.0;
                    for (int j = 0; j < sharesInvestedActivePortfolio.Length; j++) activePortfolioValue += sharesInvestedActivePortfolio[j] * currentAssetsValues[j];
                    
                    if (activePortfolio == null && sharesInvestedActivePortfolio != null)//Resuming calculation - Nastavak rebalans procedure
                    {
                        activePortfolio = new Solution(this);
                    }
                    //Updating capital invested weights - Azuriranje pondera shodno trenutnim vrednostima aseta
                    for (int j = 0; j < sharesInvestedActivePortfolio.Length; j++) activePortfolio.DecisionVariables[j].setValue(sharesInvestedActivePortfolio[j] * currentAssetsValues[j] / activePortfolioValue);
                }
                m_parameters.setParameter("portfolioValue", activePortfolioValue, true);
                //////////////////////////////////////////////////////////////////
                
                //Determining the current portfolio - Odredjivanje aktuelnog portfolija
                if (((EvaluationCriteriaReturnBased)EC).CriteriaType.Equals(EvaluationCriteriaReturnBased.criteriaType.none))// 1/N portfolio
                {
                    double capitalInvestedWeight = 1.0 / timeSeriesSet.Count;
                    for (int j = 0; j < timeSeriesSet.Count; j++)
                    {
                        sharesInvestedCurrentPortfolio[j] = (capitalInvestedWeight * activePortfolioValue) / currentAssetsValues[j];
                    }
                    for (int j = 0; j < timeSeriesSet.Count; j++)
                    {
                        currentPortfolio.DecisionVariables[j].setValue(capitalInvestedWeight);
                    }
                }
                else 
                {

                    //Add active portfolio in initial population for optimization
                    if (activePortfolio != null) addOptimalSolution("active portfolio", activePortfolio);

                    //Add current portfolio in initial population for optimization
                    if (previousPortfolio != null) addOptimalSolution("previous portfolio", previousPortfolio);

                    //Perform execution of optimization algorithm - Izvrsavanje optimizacionog algoritma za zadatim kriterijumom optimizacije
                    AlgorithmExecution(ref s);

                    //Get optimal portfolio (current) - Ucitavanje optimalnog portfolija
                    currentPortfolio = s.getSolution(s.size() - 1);

                    //Calculate shares invested in current (optimal) portfolio - Odredjivanje broja aseta investiranih u optimalni portofolio
                    CalculateSharesInvested(sharesInvestedCurrentPortfolio, currentPortfolio, currentAssetsValues, activePortfolioValue);
                }
                
                //Calculate turnover
                if (activePortfolio != null)
                {
                    turnOver = CalculateTurnOver(sharesInvestedCurrentPortfolio, sharesInvestedActivePortfolio, currentAssetsValues, activePortfolioValue);
                }//Calculate turnover

                //Calculate criteria change - Odredjivanje relativne promene vrednosti kriterijuma optimizacije
                double currentPortfolioFitness = currentPortfolio.Fitness;
                double activePortfolioFitness = 0.0;

                double criteriaChange = 1.0;
                if (activePortfolio != null && !((EvaluationCriteriaReturnBased)EC).CriteriaType.Equals(EvaluationCriteriaReturnBased.criteriaType.none))
                {
                    if (((EvaluationCriteriaReturnBased)EC).CriteriaType.Equals(EvaluationCriteriaReturnBased.criteriaType.VaRGarch))
                    {
                        //Ovo je ubaceno jer kad se radi GARCH estimacija VaRa moze da se dogodi da za neki solution R ne moze da odredi VaR pa se solution menja sa drugim
                        //treba promeniti code ispod da se nadje bolje resenje
                        Solution tmpSolution = new Solution(activePortfolio);
                        evaluate(tmpSolution);
                        if(tmpSolution.DecisionVariables[0].getValue() == activePortfolio.DecisionVariables[0].getValue()) activePortfolio.Fitness = tmpSolution.Fitness;
                        else activePortfolio.Fitness = 1.0;
                    }
                    else evaluate(activePortfolio);

                    activePortfolioFitness = activePortfolio.Fitness;
                    criteriaChange = (currentPortfolioFitness - activePortfolioFitness)/Math.Abs(activePortfolioFitness);
                }//Calculate criteria change

                //Calculate relative change of Capital Requirements - Odredjivanje relativne promene vrednosti Capital Requirements
                double CRChange = 1.0;
                if (CRType != CapitalRequirementsType.None)
                {
                    int backtestingPeriod = (int)m_parameters.getParameter("backtestingPeriod") + 1 ;
                    int backtestingStartDateIndex = currentIndex - backtestingPeriod;
                    DateTime backtestingStartDate;
                    //Determining the start date of backtesting procedure - Odredjivanje pocekta bektesting procedure
                    try
                    {
                        backtestingStartDate = timeSeriesSet.GetDay(backtestingStartDateIndex);
                    }
                    catch (Exception ex) { throw new Exception("Input time series has not length for backtesting procedure", ex); }
                    
                    string strCurrentDate = currentDate.ToShortDateString();
                    SWCurrentPortfolio.WriteLine(strCurrentDate);
                    SWActivePortfolio.WriteLine(strCurrentDate);

                    if (CRType == CapitalRequirementsType.CR_GARCH_VaR)
                    {
                        currentPortfolioCapitalRequirements = CalculateCapitalRequirementsInR(sharesInvestedCurrentPortfolio, DecisionVariableType.sharesInvestedRealValues, currentDate);
                        activePortfolioCapitalRequirements = CalculateCapitalRequirementsInR(sharesInvestedActivePortfolio, DecisionVariableType.sharesInvestedRealValues, currentDate);
                    }
                    else if (CRType == CapitalRequirementsType.CR_Historical_VaR)
                    {
                        currentPortfolioCapitalRequirements = CalculateCapitalRequirements(sharesInvestedCurrentPortfolio, currentDate);
                        activePortfolioCapitalRequirements = CalculateCapitalRequirements(sharesInvestedActivePortfolio, currentDate);
                    }

                    printingData.setParameter("currentPortfolioCR", currentPortfolioCapitalRequirements);
                    printingData.setParameter("activePortfolioCR", activePortfolioCapitalRequirements);
                    CRChange = (currentPortfolioCapitalRequirements / activePortfolioCapitalRequirements) - 1.0;
                } //Calculate relative change of Capital Requirements

                int rebalanced = 0;
                criteriaChangeTreshold = -Math.Abs((double)m_parameters.getParameter("criteriaChange") * 0.01);
                CRChangeTreshold = -Math.Abs((double)m_parameters.getParameter("CRChange") * 0.01);
                if (activePortfolio == null ||
                   (((TOLimitType == TurnOverLimitType.GreaterThan && turnOver > turnoverTreshold) ||
                   ((TOLimitType == TurnOverLimitType.LessThan || TOLimitType == TurnOverLimitType.CumulativeLessThan || TOLimitType == TurnOverLimitType.WithinLimit_Ver2) && turnOver <= turnoverTreshold)) && 
                   (((EvaluationCriteriaReturnBased)EC).CriteriaType.Equals(EvaluationCriteriaReturnBased.criteriaType.none) ? true: criteriaChange < criteriaChangeTreshold) && 
                   (CRType == CapitalRequirementsType.None || CRChange < CRChangeTreshold)))
                {
                    rebalanced = 1;
                    activePortfolio = currentPortfolio;
                    sharesInvestedCurrentPortfolio.CopyTo(sharesInvestedActivePortfolio, 0);
                    if (TOLimitType == TurnOverLimitType.CumulativeLessThan) turnoverTreshold -= turnOver;
                }//Perform rebalancing

                //if (((EvaluationCriteriaReturnBased)EC).CriteriaType.Equals(EvaluationCriteriaReturnBased.criteriaType.none))
                //{
                //    evaluate(activePortfolio);
                //}

                //Get performance parameters for active portfolio
                GetParametersForOptimalPortfolio(activePortfolio, portfolioParameters, false);
                
                //Print results
                PrintResults(sw, sharesInvestedActivePortfolio, currentAssetsValues, currentDate, turnOver, rebalanced, activePortfolioFitness, currentPortfolioFitness);
                
                //Previous portfolio becomes current portfolio
                if(previousPortfolio == null) previousPortfolio = new Solution();
                previousPortfolio = currentPortfolio;
                 
                counter++;
                i++;
                // raise event
                OnRebalanceDayChanged(counter);
            }

            if(SWCurrentPortfolio != null) SWCurrentPortfolio.Dispose();
            if(SWActivePortfolio != null) SWActivePortfolio.Dispose();
        }

        /// <summary>Initializes the base class (PortfolioOptimization)</summary>
        protected void InitPortfolioOptimization()
        {
            AssetSet timeSeriesSet = (AssetSet)m_parameters.getParameter("AssetsTimeSeries");
            DataValidation(timeSeriesSet);
            Init();
        }

        /// <summary>Performs execution of optimization algorithm</summary>
        /// <param name="s">Solution set</param>
        protected void AlgorithmExecution(ref JARE.Base.SolutionSet s)
        {
            Operator crossover; // Crossover operator
            Operator mutation; // Mutation operator
            Operator selection; // Selection operator

            //algorithm = new PortfolioOptimizationGA(this); // Generational GA with fitness

            int newIndividualsCount = 0;

            // Mutation and Crossover for Real codification
            if (containsSolution("active portfolio") && TOLimitType == TurnOverLimitType.WithinLimit_Ver2)
            {
                //Adding the active portfolio needed for generation of initial population
                //////////////////////////////////////////////////////////
                Solution activePortfolio = getOptimalSolution("active portfolio");
                ((RealWeightsSolutionType)m_solutionType).ReferentSolution = activePortfolio;
                double tresholdTolerance = 0.999;
                ((RealWeightsSolutionType)m_solutionType).AbsoluteDeviationFromReferentSolution = turnoverTreshold * tresholdTolerance;
                //////////////////////////////////////////////////////////

                crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WholeArithmeticCrossover");
                crossover.setParameter("probability", (double)m_parameters.getParameter("crossoverProbability"));
                crossover.setParameter("distributionIndex", 20.0);

                mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("WeightsUniformMutation");
                mutation.setParameter("probability", 0.0);
                mutation.setParameter("perturbationIndex", (double)m_parameters.getParameter("mutationPerturbation"));
                newIndividualsCount = 10;
            }
            else
            {
                //crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WeightsSinglePointCrossover");
                crossover = JARE.Base.operators.crossover.CrossoverFactory.getCrossoverOperator("WeightsUniformCrossover");
                crossover.setParameter("probability", (double)m_parameters.getParameter("crossoverProbability"));
                crossover.setParameter("distributionIndex", 20.0);

                mutation = JARE.Base.operators.mutation.MutationFactory.getMutationOperator("WeightsUniformMutation");
                mutation.setParameter("probability", (double)m_parameters.getParameter("mutationProbability"));
                mutation.setParameter("perturbationIndex", (double)m_parameters.getParameter("mutationPerturbation"));
                newIndividualsCount = 0;
            }

            /* Selection Operator */
            int populationSize = (int)m_parameters.getParameter("populationSize");
            selection = JARE.Base.operators.selection.SelectionFactory.getSelectionOperator("BinaryTournament");

            /* Add the operators to the algorithm*/
            algorithm.deleteOperators();
            algorithm.addOperator("crossover", crossover);
            algorithm.addOperator("mutation", mutation);
            algorithm.addOperator("selection", selection);
            //algorithm.addOperator("selectionPopulation", selectionPopulation);

            /* Algorithm parameters*/
            algorithm.deleteInputParameters();
            algorithm.setInputParameter("populationSize", populationSize);
            algorithm.setInputParameter("maxGenerations", (int)m_parameters.getParameter("maxGenerations"));
            algorithm.setInputParameter("newIndividualsCount", newIndividualsCount);
            algorithm.setInputParameter("plateauTolerance", (double)m_parameters.getParameter("plateauTolerance"));
            algorithm.setInputParameter("maxPlateauGenerations", (int)m_parameters.getParameter("maxPlateauGenerations"));

            //((PortfolioOptimizationGA)algorithm).Init();
            //algorithm.IterationCounterChanged += new PortfolioOptimizationGA.IterationCounterChangedHandler(OnGenerationChanged);

            s = algorithm.execute();
        } //Performs execution of optimization algorithm


        //Calculates turnover of portfolio rebalance
        /// <summary>Calculates turnover induced by portfolio rebalance</summary>
        /// <param name="sharesInvestedCurrentPortfolio">Array of shares of current (optimized) portfolio</param>
        /// <param name="sharesInvestedActivePortfolio">Array of shares of active portfolio</param>
        /// <param name="assetsValues">Array of current assets values</param>
        /// <param name="activePortfolioValue">The value of active portfolio</param>
        /// <returns> Turnover value
        /// </returns>
        protected double CalculateTurnOver(double[] sharesInvestedCurrentPortfolio, double[] sharesInvestedActivePortfolio, double[] assetsValues, double activePortfolioValue)
        {
            double turnOver = 0.0;
            double tmp = 0.0;
            
            //Turnover = SUM(ABS(W(i,t)-W(i,t-1))) = SUM(ABS(n(i,t)-n(i,t-1))*p(i,t)/P(i))
            for (int i = 0; i < sharesInvestedCurrentPortfolio.Length; i++)
            {
                tmp += Math.Abs((sharesInvestedCurrentPortfolio[i] - sharesInvestedActivePortfolio[i]) * assetsValues[i]);
                turnOver = tmp / activePortfolioValue;
            }

            return turnOver;
        }//Calculates turnover of portfolio rebalance

        /// <summary>Calculates shares invested in portfolio using portfolio value and assets values</summary>
        /// <param name="sharesInvested">Array of shares invested in portfolio (return variable)</param>
        /// <param name="Portfolio">Solution representing considered portfolio</param>
        /// <param name="assetsValues">Array of assets values</param>
        /// <param name="portfolioValue">Value of considered portfolio</param>
        protected void CalculateSharesInvested(double[] sharesInvested, Solution Portfolio, double[] assetsValues, double portfolioValue)
        {
            //Calculate shares Invested for current portfolio
            for (int j = 0; j < Portfolio.DecisionVariables.Length; j++) sharesInvested[j] = (Portfolio.DecisionVariables[j].getValue() * portfolioValue) / assetsValues[j];
        }//Calculates shares invested in portfolio using portfolio value and assets values

        /// <summary>Prints head line in file with results</summary>
        /// <param name="sw">The Stream Writer object for printing results</param>
        protected void PrintHeadLine(StreamWriter sw)
        {
            AssetSet timeSeriesSet = (AssetSet)m_parameters.getParameter("AssetsTimeSeries");
            //Print head line - Stampanje naslovne linije
            string strParameters = "date, portfolio value, turn over, rebalanced, ";
            strParameters += ("active" + ((EvaluationCriteriaReturnBased)EC).CriteriaType.ToString() + ",");
            strParameters += ("optimized" + ((EvaluationCriteriaReturnBased)EC).CriteriaType.ToString() + ",");

            System.Collections.Generic.Dictionary<string, double>.Enumerator HeadLineEnum = portfolioParameters.GetEnumerator();
            for (int kk = 0; kk < portfolioParameters.Count; kk++)
            {
                HeadLineEnum.MoveNext();
                strParameters += (HeadLineEnum.Current.Key + ",");
            }
            HeadLineEnum.Dispose();

            if (CRType != CapitalRequirementsType.None)
            {
                strParameters += ("activePortfolioCR,optimizedPortfolioCR,");
            }
            for (int j = 0; j < timeSeriesSet.Count; j++) strParameters += (timeSeriesSet.assetList[j].name + " - capital_invested,");
            for (int j = 0; j < timeSeriesSet.Count; j++) strParameters += (timeSeriesSet.assetList[j].name + " - shares_invested,");
            sw.WriteLine(strParameters);
        }//Prints head line in file with results

        /// <summary>Prints results to *.csv file</summary>
        /// <param name="sw">The Stream Writer object for printing results</param>
        /// <param name="sharesInvestedActivePortfolio">Array of shares invested in active portfolio</param>
        /// <param name="assetsValues">Array of assets values</param>
        /// <param name="currentDate">Current date</param>
        /// <param name="turnOver">Turnover value</param>
        /// <param name="rebalanced">Indicator whether the portfolio is rebalanced</param>
        /// <param name="activePortfolioOptimizationCriteriaValue">Optimization criteria value of active portfolio</param>
        /// <param name="currentPortfolioOptimizationCriteriaValue">Optimization criteria value of current portfolio</param>
        void PrintResults(StreamWriter sw, double[] sharesInvestedActivePortfolio, double[] assetsValues, DateTime currentDate, double turnOver, int rebalanced, double activePortfolioOptimizationCriteriaValue, double currentPortfolioOptimizationCriteriaValue)
        {
            string strParameters = string.Empty;
            string str = string.Empty;

            strParameters += currentDate.Date.ToShortDateString() + ",";

            double capitalInvestedWeight = 0.0;

            double portfolioValue = 0.0;
            for (int i = 0; i < sharesInvestedActivePortfolio.Length; i++)
            {
                portfolioValue += sharesInvestedActivePortfolio[i] * assetsValues[i];
            }

            strParameters += (portfolioValue.ToString("F10") + ",");
            strParameters += (turnOver.ToString("F10") + ",");
            strParameters += (rebalanced.ToString("d") + ",");
            strParameters += (activePortfolioOptimizationCriteriaValue.ToString("F10") + ",");
            strParameters += (currentPortfolioOptimizationCriteriaValue.ToString("F10") + ",");
           
            System.Collections.Generic.Dictionary<string, double>.Enumerator HeadLineEnum = portfolioParameters.GetEnumerator();
            for (int kk = 0; kk < portfolioParameters.Count; kk++)
            {
                HeadLineEnum.MoveNext();
                strParameters += (HeadLineEnum.Current.Value + ",");
            }
            HeadLineEnum.Dispose();

            if(printingData.ContainsParameter("activePortfolioCR")) strParameters += ((double)printingData.getParameter("activePortfolioCR")).ToString("F10") + ",";
            if (printingData.ContainsParameter("currentPortfolioCR")) strParameters += ((double)printingData.getParameter("currentPortfolioCR")).ToString("F10") + ",";

            for (int i = 0; i < sharesInvestedActivePortfolio.Length; i++)
            {
                capitalInvestedWeight = (sharesInvestedActivePortfolio[i] * assetsValues[i]) / portfolioValue;
                strParameters += (capitalInvestedWeight.ToString("F10") + ",");
            }
            
            for (int i = 0; i < sharesInvestedActivePortfolio.Length; i++)
            {
                strParameters += (sharesInvestedActivePortfolio[i].ToString("F10") + ",");
            }

            sw.WriteLine(strParameters);
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

        /// <summary> Creates initial population.</summary>
        /// <returns> the initial population.
        /// </returns>
        //public override SolutionSet createInitialPopulation(int populationSize)
        //{
        //    if (containsSolution("active portfolio") && newVersion)
        //    {
        //        Solution activePortfolio = getOptimalSolution("active portfolio");
        //        ((RealWeightsSolutionType)m_solutionType).ReferentSolution = activePortfolio;
        //        ((RealWeightsSolutionType)m_solutionType).AbsoluteDeviationFromReferentSolution = turnoverTreshold;

        //        SolutionSet population = new SolutionSet(populationSize);
        //        Solution newSolution;

        //        for (int i = 0; i < populationSize - m_optimalSolutions.Count; i++)
        //        {
        //            newSolution = new Solution(this);
        //            population.add(newSolution);
        //        } //for

        //        System.Collections.Generic.Dictionary<string, Solution>.Enumerator Enum = m_optimalSolutions.GetEnumerator();
        //        for (int i = 0; i < m_optimalSolutions.Count; i++)
        //        {
        //            newSolution = new Solution(this);
        //            Enum.MoveNext();
        //            newSolution.DecisionVariables = Enum.Current.Value.DecisionVariables;
        //            population.add(newSolution);
        //        }

        //        StreamWriter writer = new StreamWriter("Initial population.csv", false);
        //        for (int k = 0; k < population.m_solutionsList.Count; k++)
        //        {
        //            string solution = string.Empty;
        //            for (int j = 0; j < population.getSolution(k).DecisionVariables.Length; j++)
        //            {
        //                solution += population.getSolution(k).DecisionVariables[j].ToString() + ",";
        //            }
        //            //solution += "\n";
        //            writer.WriteLine(solution);
        //        }
        //        writer.Dispose();

        //        return population;
        //    }
        //    else return base.createInitialPopulation(populationSize);
        //} // createInitialPopulation        

    }
}

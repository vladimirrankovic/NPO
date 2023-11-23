
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.problems.Finance;
using JARE.problems.Finance.DataTypes;


namespace PortfolioGUI.DataTypes
{
    #region Enums
    public enum SelectionModeEnum
        {
            None,
            Elite,
            Rank,
            Roulette
        }

        public enum GeneticMethodEnum
        {
            None,
            GP,
            GEP
        }

        /*
        public enum FitnessCriteriaEnum
        {
            None,
            MaxReturn,
            MinSTDEVP,
            MaxSharpIndex,
            MaxSortino,
            MaxVaR,
            MaxcVaR,
            MaxExpWeightedVaR,
            MaxExpWeightedcVaR,
            MaxSkew,
            MaxKurtosis,
            MaxVaRGarch
        }
         * */
        #endregion

    public class Parameters
    {

        private int populationSize = 50;
        private int generationsNumber = 5;
        private int selectionMethod = 0;

        //private int evaluationPeriod = 756;
        private int evaluationPeriod;// = 1000;
        private int returnPeriod;// = 1;
        //private DateTime evaluationEndDate = new DateTime(2010, 12, 23);//za rad expsys, Comp Economics
        private DateTime optimizationDate;// = new DateTime(1999, 7, 8);//
        private DateTime rebalancingStartDate;// = new DateTime(2010, 11, 3);
        //private DateTime evaluationEndDate = new DateTime(2011, 2, 18);//S&P100
        private double interestRate;// = 0.0;
        private double varThreshold;// = 5.0;
        private double lambda;// = 0.99;
        private int rebalancingDuration;// = 100;
        private double rebalancingBuffer;// = 5;
        private double criteriaChange;// = 1;
        private double mutationProbability;// = 0.2;
        private double mutationPerturbation;// = 0.1;
        private double crossoverProbability;// = 1.0;
        private int cardinality;
        private double lowerBound;
        private double upperBound;
        private DecisionVariableType decisionVariableType;
        private EvaluationCriteriaReturnBased rebalanceEvaluationCriteria;//CriteriaType = (EvaluationCriteriaReturnBased.criteriaType)fitnessRebalCriteriaBox.SelectedIndex;
        private EvaluationCriteriaReturnBased optimizationEvaluationCriteria1;//.CriteriaType = (EvaluationCriteriaReturnBased.criteriaType)fitnessCriteria1Box.SelectedIndex;
        private EvaluationCriteriaReturnBased optimizationEvaluationCriteria2;//.CriteriaType = (EvaluationCriteriaReturnBased.criteriaType)fitnessCriteria2Box.SelectedIndex;
        private EvaluationCriteriaCreditExposureBased optimizationEvaluationCriteria3;//.CriteriaType = (EvaluationCriteriaCreditExposureBased.criteriaType)fitnessCriteria3Box.SelectedIndex;
        private EvaluationCriteriaParameterBased optimizationEvaluationCriteria4;//.CriteriaType = (EvaluationCriteriaParameterBased.criteriaType)bondsFitnessCriteriaComboBox1.SelectedIndex;
        private EvaluationCriteriaParameterBased optimizationEvaluationCriteria5;//.CriteriaType = (EvaluationCriteriaParameterBased.criteriaType)bondsFitnessCriteriaComboBox2.SelectedIndex;

        private PortfolioRebalance.CapitalRequirementsType CapitalRequirementsType;
        private double CRchange;
        private int backtestingPeriod;
        private PortfolioRebalance.TurnOverLimitType TOlimitType;
        private int maxPlateauGenerations; //Maximal number of successive generations without quality improvement
        private double plateauTolerance; //Minimal improvement of quality indicator
        private bool scheduledRebalancing;
        private double maxAllowedRisk;//Maximum allowed risk of managed portfolio. If portfolio risk exceeds this value rebalancing is needed.

        //------------------------------
        //private PortfolioOptimization.EvaluationCriteria _fitnessCriteria1;
        //private PortfolioOptimization.EvaluationCriteria _fitnessCriteria2;
        //private SelectionModeEnum _selectionMode;
        //private GeneticMethodEnum _geneticMethod;
        //------------------------------

        public int PopulationSize { get { return populationSize; } set { populationSize = value; } }
        public int Generations { get { return generationsNumber; } set { generationsNumber = value; } }
        //public PortfolioOptimization.EvaluationCriteria FitnessCriteria1 { get { return _fitnessCriteria1; } }
        //public PortfolioOptimization.EvaluationCriteria FitnessCriteria2 { get { return _fitnessCriteria2; } }
        public DateTime OptimizationDate { get { return optimizationDate; } set { optimizationDate = value; } }
        public DateTime RebalancingStartDate { get { return rebalancingStartDate; } set { rebalancingStartDate = value; } }
        public double InterestRate { get { return interestRate; } set { interestRate = value; } }
        public int EvaluationPeriod { get { return evaluationPeriod; } set { evaluationPeriod = value; } }
        public double VarTreshold { get { return varThreshold; } set { varThreshold = value; } }
        public double Lambda { get { return lambda; } set { lambda = value; } }
        public int ReturnPeriod { get { return returnPeriod; } set { returnPeriod = value; } }
        public int RebalancingDuration { get { return rebalancingDuration; } set { rebalancingDuration = value; } }
        public double RebalancingBuffer { get { return rebalancingBuffer; } set { rebalancingBuffer = value; } }
        public double CriteriaChange { get { return criteriaChange; } set { criteriaChange = value; } }
        public double MutationProbability { get { return mutationProbability; } set { mutationProbability = value; } }
        public double MutationPerturbation { get { return mutationPerturbation; } set { mutationPerturbation = value; } }
        public double CrossoverProbability { get { return crossoverProbability; } set { crossoverProbability = value; } }
        public int Cardinality { get { return cardinality; } set { cardinality = value; } }
        public double LowerBound { get { return lowerBound; } set { lowerBound = value; } }
        public double UpperBound { get { return upperBound; } set { upperBound = value; } }
        public DecisionVariableType DecisionVarType { get { return decisionVariableType; } set { decisionVariableType = value; } }
        public EvaluationCriteriaReturnBased RebalanceEvaluationCriteria { get { return rebalanceEvaluationCriteria; } set { rebalanceEvaluationCriteria = value; } }
        public EvaluationCriteriaReturnBased OptimizationEvaluationCriteria1 { get { return optimizationEvaluationCriteria1; } set { optimizationEvaluationCriteria1 = value; } }
        public EvaluationCriteriaReturnBased OptimizationEvaluationCriteria2 { get { return optimizationEvaluationCriteria2; } set { optimizationEvaluationCriteria2 = value; } }
        public EvaluationCriteriaCreditExposureBased OptimizationEvaluationCriteria3 { get { return optimizationEvaluationCriteria3; } set { optimizationEvaluationCriteria3 = value; } }
        public EvaluationCriteriaParameterBased OptimizationEvaluationCriteria4 { get { return optimizationEvaluationCriteria4; } set { optimizationEvaluationCriteria4 = value; } }
        public EvaluationCriteriaParameterBased OptimizationEvaluationCriteria5 { get { return optimizationEvaluationCriteria5; } set { optimizationEvaluationCriteria5 = value; } }
        public PortfolioRebalance.CapitalRequirementsType CRType { get { return CapitalRequirementsType; } set { CapitalRequirementsType = value; } }
        public double CRChange { get { return CRchange; } set { CRchange = value; } }
        public int BacktestingPeriod { get { return backtestingPeriod; } set { backtestingPeriod = value; } }
        public PortfolioRebalance.TurnOverLimitType TOLimitType { get { return TOlimitType; } set { TOlimitType = value; } }
        public int MaxPlateauGenerations { get { return maxPlateauGenerations; } set { maxPlateauGenerations = value; } }
        public double PlateauTolerance { get { return plateauTolerance; } set { plateauTolerance = value; } }
        public bool ScheduledRebalancing { get { return scheduledRebalancing; } set { scheduledRebalancing = value; } }
        public double MaxAllowedRisk { get { return maxAllowedRisk; } set { maxAllowedRisk = value; } }


        //public SelectionModeEnum SelectionMode { get { return _selectionMode; } }
        //public FunctionsSetEnum FunctionSet { get { return _functionsSet; } }
        //public GeneticMethodEnum GeneticMethod { get { return _geneticMethod; } }

        public Parameters(int populationSize, SelectionModeEnum selectionMode, int iterations, DateTime optimizationDate, 
            DateTime rebalancingStartDate, double interestRate, int evaluationPeriod, double varTreshold, int returnPeriod, 
            double lambda, int rebalancingPeriod, double rebalancingBuffer, double criteriaChange, double mutationProbability,
            double mutationPerturbation, double crossoverProbability, int cardinality, DecisionVariableType decisionVariableType, EvaluationCriteriaReturnBased rebalanceEvaluationCriteria,
            EvaluationCriteriaReturnBased optimizationEvaluationCriteria1, EvaluationCriteriaReturnBased optimizationEvaluationCriteria2,
            EvaluationCriteriaCreditExposureBased optimizationEvaluationCriteria3,
            EvaluationCriteriaParameterBased optimizationEvaluationCriteria4, EvaluationCriteriaParameterBased optimizationEvaluationCriteria5,
            PortfolioRebalance.CapitalRequirementsType CRType, double CRchange, int backtestingPeriod,
            PortfolioRebalance.TurnOverLimitType TOlimitType, int maxPlateauGenerations, double plateauTolerance, bool rebalanceScheduled, double maxAllowedRisk,
            double inpupLowerBound, double inputUpperBound)
        {
            PopulationSize = populationSize;
            Generations = iterations;
            //_fitnessCriteria1 = fitnessCriteria1;
            //_fitnessCriteria2 = fitnessCriteria2;
            OptimizationDate = optimizationDate;
            RebalancingStartDate = rebalancingStartDate;
            InterestRate = interestRate;
            EvaluationPeriod = evaluationPeriod;
            VarTreshold = varTreshold;
            Lambda = lambda;
            ReturnPeriod = returnPeriod;
            RebalancingDuration = rebalancingPeriod;
            RebalancingBuffer = rebalancingBuffer;
            CriteriaChange = criteriaChange;
            MutationProbability = mutationProbability;
            MutationPerturbation = mutationProbability;
            CrossoverProbability = crossoverProbability;
            Cardinality = cardinality;
            DecisionVarType = decisionVariableType;
            RebalanceEvaluationCriteria = rebalanceEvaluationCriteria;
            OptimizationEvaluationCriteria1 = optimizationEvaluationCriteria1;
            OptimizationEvaluationCriteria2 = optimizationEvaluationCriteria2;
            OptimizationEvaluationCriteria3 = optimizationEvaluationCriteria3;
            OptimizationEvaluationCriteria4 = optimizationEvaluationCriteria4;
            OptimizationEvaluationCriteria5 = optimizationEvaluationCriteria5;
            this.CRType = CRType;
            CRChange = CRchange;
            BacktestingPeriod = backtestingPeriod;
            TOLimitType = TOlimitType;
            MaxPlateauGenerations = maxPlateauGenerations;
            PlateauTolerance = plateauTolerance;
            ScheduledRebalancing = rebalanceScheduled;
            MaxAllowedRisk = maxAllowedRisk;
            LowerBound = inpupLowerBound;
            UpperBound = inputUpperBound;
        }
        public Parameters()
        {
            Init();
        }
        void Init()
        {
            
            PopulationSize = 50;
            Generations = 10;
            //_fitnessCriteria1 = fitnessCriteria1;
            //_fitnessCriteria2 = fitnessCriteria2;
            //EvaluationEndDate = new DateTime(1999, 7, 8);
            //EvaluationEndDate = new DateTime(2010, 12, 23);
            OptimizationDate = new DateTime(2008, 1, 31);
            RebalancingStartDate = new DateTime(2010, 11, 3); 
            InterestRate = 0.0;
            EvaluationPeriod = 1000;
            VarTreshold = 5.0;
            Lambda = 0.99;
            ReturnPeriod = 1;
            RebalancingDuration = 100;
            RebalancingBuffer = 5.0;
            CriteriaChange = -1.0;
            MutationProbability = 0.05;
            MutationPerturbation = 0.1;
            CrossoverProbability = 0.9;
            Cardinality = 0;
            LowerBound = 0.0;
            UpperBound = 1.0;
            decisionVariableType = DecisionVariableType.capitalInvested;
            RebalanceEvaluationCriteria = new EvaluationCriteriaReturnBased(EvaluationCriteriaReturnBased.criteriaType.VaRGarch, EvaluationCriteriaReturnBased.executionType.singleThread);
            OptimizationEvaluationCriteria1 = new EvaluationCriteriaReturnBased(EvaluationCriteriaReturnBased.criteriaType.AverageReturn, EvaluationCriteriaReturnBased.executionType.singleThread);
            OptimizationEvaluationCriteria2 = new EvaluationCriteriaReturnBased(EvaluationCriteriaReturnBased.criteriaType.StandardDeviation, EvaluationCriteriaReturnBased.executionType.singleThread);
            OptimizationEvaluationCriteria3 = new EvaluationCriteriaCreditExposureBased(EvaluationCriteriaCreditExposureBased.criteriaType.none, EvaluationCriteriaCreditExposureBased.executionType.singleThread);
            OptimizationEvaluationCriteria4 = new EvaluationCriteriaParameterBased(EvaluationCriteriaParameterBased.criteriaType.YieldToMaturity, EvaluationCriteriaParameterBased.executionType.singleThread);
            OptimizationEvaluationCriteria5 = new EvaluationCriteriaParameterBased(EvaluationCriteriaParameterBased.criteriaType.SCR, EvaluationCriteriaParameterBased.executionType.singleThread);
            CRType = PortfolioRebalance.CapitalRequirementsType.None;
            CRChange = -1.0;
            BacktestingPeriod = 250;
            TOLimitType = PortfolioRebalance.TurnOverLimitType.WithinLimit_Ver2;
            MaxPlateauGenerations = 10;
            PlateauTolerance = 0.0005;
            ScheduledRebalancing = false;
            MaxAllowedRisk = 100;
        }
    }
}

/// <summary> RealWeightsSolutionType
/// 
/// </summary>
/// <author>  Vladimir Rankovic
/// </author>
/// <version>  1.0
/// 
/// Class representing the solution type of solutions composed of an Real weights 
/// variable 
/// </version>
using System;
using Problem = JARE.Base.Problem;
using SolutionType = JARE.Base.SolutionType;
using Variable = JARE.Base.Variable;
using Real = JARE.Base.variable.Real;
using System.Collections.Generic;

namespace JARE.Base.solutionType
{

    public class RealWeightsSolutionType : SolutionType
	{
		
        private int cardinality;
        private Solution referentSolution = null;
        private double absoluteDeviationFromReferentSolution;
        private Random randomGenerator;
        int sleepTime;
 		
        /// <summary> Constructor</summary>
		/// <param name="problem">
		/// </param>
		/// <throws>  ClassNotFoundException  </throws>
        /// 
        public RealWeightsSolutionType(Problem problem) : base(problem)
		{
            randomGenerator = new Random();
            problem.m_variableType = new System.Type[problem.NumberOfVariables];
            problem.SolutionType = this;

            // Initializing the types of the variables
            for (int i = 0; i < problem.NumberOfVariables; i++)
            {
                problem.m_variableType[i] = System.Type.GetType("JARE.Base.variable.Real");
            } // for  
            cardinality = 0;
		}

        public int Cardinality
        {
            get { return cardinality; }
            set { cardinality = value; }
        }

        public Solution ReferentSolution
        {
            get { return referentSolution; }
            set { referentSolution = new Solution(value); }
        }

        public double AbsoluteDeviationFromReferentSolution
        {
            get { return absoluteDeviationFromReferentSolution; }
            set { absoluteDeviationFromReferentSolution = value; }
        }
 		/// <summary> Creates the variables of the solution</summary>
		/// <param name="decisionVariables">
		/// </param>
		public override Variable[] createVariables()
		{
            sleepTime = (int)randomGenerator.Next(1, 20);
            System.Threading.Thread.Sleep(sleepTime);

            Variable[] variables = new Variable[m_problem.NumberOfVariables];
            if (referentSolution == null)
            {
                variables = createVariablesBasic();//
            }
            else
            {
                if (cardinality != 0 && cardinality != m_problem.NumberOfVariables) throw new Exception("Cardinality constraint is not supported if referent solution is defined.");
                variables = createVariablesRelativeToReferentSolution(referentSolution);
            }         

            return variables;
        } // createVariables

        protected Variable[] createVariablesBasic()
        {
            Variable[] variables = new Variable[m_problem.NumberOfVariables];

            for (int var = 0; var < m_problem.NumberOfVariables; var++)
                variables[var] = new Real(m_problem.getLowerLimit(var), m_problem.getUpperLimit(var));

            //////////////////////////////////////////////////////////////////////////////////////////
            if (cardinality == 0)
            {
                cardinality = variables.Length;
            }

            int[] choosenVariables = new int[variables.Length];
            for (int i = 0; i < choosenVariables.Length; i++) choosenVariables[i] = 0;
            int counter = 0;
            int sample;
            double valueLimit = 1.0;
            bool correction = false;
            double correctionValue = 0.0;
            List<int> chosenVariables = new List<int>();
            int variableIndex = -1;

            for (int i = 0; i < variables.Length; i++)
            {
                variables[i].setValue(0.0);
                //    variables[i].setLowerBound(0.0);
                //    variables[i].setUpperBound(1.0);
            }
            while (counter < cardinality - 1)
            {
                sample = randomGenerator.Next(variables.Length);
                if (choosenVariables[sample] == 0)
                {
                    //variables[sample].setValue(randomGenerator.NextDouble() * valueLimit);
                    
                    double upperBound = variables[sample].getUpperBound() < valueLimit ? variables[sample].getUpperBound() : valueLimit;
                    double value;
                    if (variables[sample].getLowerBound() < valueLimit)
                    {
                        double valueRange = upperBound - variables[sample].getLowerBound();
                        value = variables[sample].getLowerBound() + randomGenerator.NextDouble() * valueRange;
                    }
                    else value = valueLimit;

                    variables[sample].setValue(value);
                    valueLimit = valueLimit - value;
                    counter++;
                    choosenVariables[sample] = 1;
                    chosenVariables.Add(sample);                    
                }
            }

            if (valueLimit > 0)
            {
                if (cardinality == variables.Length)
                {
                    for (int i = 0; i < variables.Length; i++)
                    {
                        if (choosenVariables[i] == 0)
                        {
                            if (valueLimit > variables[i].getUpperBound() || valueLimit < variables[i].getLowerBound())
                            {
                                correction = true;
                                if (valueLimit > variables[i].getUpperBound()) correctionValue = valueLimit - variables[i].getUpperBound();
                                if (valueLimit < variables[i].getLowerBound()) correctionValue = valueLimit - variables[i].getLowerBound();
                                variableIndex = i;
                            }
                            variables[i].setValue(valueLimit);
                            chosenVariables.Add(i);

                            break;
                        }
                    }
                }
                else
                {
                    do
                    {
                        sample = randomGenerator.Next(variables.Length);
                    }
                    while (choosenVariables[sample] == 1);
                    if (valueLimit > variables[sample].getUpperBound() || valueLimit < variables[sample].getLowerBound())
                    {
                        correction = true;
                        if (valueLimit > variables[sample].getUpperBound()) correctionValue = valueLimit - variables[sample].getUpperBound();
                        if (valueLimit < variables[sample].getLowerBound()) correctionValue = valueLimit - variables[sample].getLowerBound();
                        variableIndex = sample;
                    }
                    chosenVariables.Add(sample);
                    variables[sample].setValue(valueLimit);
                }
            }

            if (correction)
            {
                while (correctionValue != 0.0)
                {
                    double value = 0.0;
                    sample = randomGenerator.Next(chosenVariables.Count);
                    int index = chosenVariables[sample];
                    if (index != variableIndex)
                    {
                        if (correctionValue > 0.0)
                        {
                            if (correctionValue > (variables[index].getUpperBound() - variables[index].getValue()))
                            {
                                value = variables[index].getUpperBound() - variables[index].getValue();
                            }
                            else value = correctionValue;
                            variables[index].setValue(variables[index].getValue() + value);
                            variables[variableIndex].setValue(variables[variableIndex].getValue() - value);
                            correctionValue -= value;
                        }
                        else
                        {
                            double absValue = Math.Abs(correctionValue);
                            if (absValue > (variables[index].getValue() - variables[index].getLowerBound()))
                            {
                                value = variables[index].getValue() - variables[index].getLowerBound();
                            }
                            else value = absValue;
                            variables[index].setValue(variables[index].getValue() - value);
                            variables[variableIndex].setValue(variables[variableIndex].getValue() + value);
                            correctionValue += value;
                        }
                    }
                }
            }

            //System.IO.StreamWriter writer = new System.IO.StreamWriter("solution.csv", true);
            //string tmp = "";
            //for (int i = 0; i < variables.Length; i++)
            //{
            //    tmp += variables[i].ToString() + ",";
            //}
            //tmp += sleepTime.ToString();
            //writer.WriteLine(tmp);
            //writer.Close();

            return variables;
        }

        #region Creation of variables relatively to the referent solution

        /// <summary> Creates the variables of the solution relatively to the referent solution</summary>
        /// <param name="referentSolution">
        /// </param>
        public Variable[] createVariablesRelativeToReferentSolution(Solution referentSolution)
        {
            Solution newSolution = new Solution(referentSolution);
            Variable[] variables = new Variable[m_problem.NumberOfVariables];
            newSolution.DecisionVariables.CopyTo(variables, 0);

            //Mark whether weight is changed: positive for increased value, negative for decreased value
            //If weight is increased it can not be decreased later and vice versa
            int[] markerArray = new int[variables.Length];
            //At the beginning all marks are equal to 0
            for (int i = 0; i < variables.Length; i++) markerArray[i] = 0;
            ///weights that can be increased by considered value
            List<int> weightsToIncrease = new List<int>();
            ///weights that can be decreased by considered value
            List<int> weightsToDecrease = new List<int>();
            //Indexes of weights that can be decreased the most 
            List<int> highestPossibleChangeIndexes = new List<int>();
             
            double positiveTurnOver = absoluteDeviationFromReferentSolution * 0.5;
            double turnoverTolerance = 0.001 * positiveTurnOver;
            double highestPossibleIncrease = 0;
            double highestPossibleDecrease = 0;

            double randomValue = randomGenerator.NextDouble();
            if (randomValue < 1.5)
            {

                ///Version 1. Restruction of referent portfolio by selling of random chosen assets and buying random chosen assets
                while (positiveTurnOver > turnoverTolerance)
                {
                    //Generate selling/buying value(between 0 and turnover value)
                    double value = 0.0;
                    while (value == 0.0) value = randomGenerator.NextDouble() * positiveTurnOver;

                    weightsToIncrease.Clear();
                    weightsToDecrease.Clear();
                    bool breakLoop = false;
                    //Determine which assets can be sold/bought in amount equal to given value
                    while (weightsToIncrease.Count == 0 || weightsToDecrease.Count == 0)
                    {
                        highestPossibleIncrease = 0;
                        highestPossibleDecrease = 0;
                        //Get weights indexes that can be increased by generated value
                        GetWeightsAbleToChange(variables, value, weightsToIncrease, 1, markerArray, ref highestPossibleIncrease, highestPossibleChangeIndexes);
                        //Get weights indexes that can be decreased by generated value
                        GetWeightsAbleToChange(variables, value, weightsToDecrease, -1, markerArray, ref highestPossibleDecrease, highestPossibleChangeIndexes);

                        //If there is no assets which can be sold/bought in amount equal to given value change value to be equal to highest possible chabge
                        if (weightsToIncrease.Count == 0 || weightsToDecrease.Count == 0)
                        {
                            value = (highestPossibleIncrease <= highestPossibleDecrease ? highestPossibleIncrease : highestPossibleDecrease);
                            if (value == 0.0 || value > positiveTurnOver)
                            {
                                //If highest possible change is equal to zero stop restruction
                                breakLoop = true;
                                break;
                            }
                        }
                    }
                    if (breakLoop) break;


                    int weightToIncrease = -1;
                    int weightToDecrease = -1;

                    bool found = false;
                    //Determine weight which will be increased
                    while (!found)
                    {
                        weightToIncrease = randomGenerator.Next((int)weightsToIncrease.Count);
                        if (markerArray[weightsToIncrease[weightToIncrease]] >= 0) found = true;
                        markerArray[weightsToIncrease[weightToIncrease]]++;
                    }
                    found = false;
                    //Determine weight which will be decreased
                    while (!found)
                    {
                        weightToDecrease = randomGenerator.Next((int)weightsToDecrease.Count);
                        if (markerArray[weightsToDecrease[weightToDecrease]] <= 0) found = true;
                        markerArray[weightsToDecrease[weightToDecrease]]--;
                    }
                    //Increase weight by given value
                    variables[weightsToIncrease[weightToIncrease]].setValue(variables[weightsToIncrease[weightToIncrease]].getValue() + value);
                    //Decrease weight by given value
                    variables[weightsToDecrease[weightToDecrease]].setValue(variables[weightsToDecrease[weightToDecrease]].getValue() - value);

                    //Decrease turnover value
                    positiveTurnOver -= value;
                }
            }
            else
            {//Version 2. Restruction of referent portfolio with aim to produce the highest possible turn over

                //Check if there is any weight which can be increased by given turnover value
                GetWeightsAbleToChange(variables, positiveTurnOver, weightsToIncrease, 1, markerArray, ref highestPossibleIncrease, highestPossibleChangeIndexes);
                //If there is no weights that can be increased by given turnover value consider weights that can be increased by highest value
                if (weightsToIncrease.Count == 0) weightsToIncrease = highestPossibleChangeIndexes;

                //Chose randomly weight that will be increased (increased weight)
                int tmpIndex = (int)(randomGenerator.NextDouble() * weightsToIncrease.Count);
                int highestPossibleChangeIndex = weightsToIncrease[tmpIndex];

                double realizedTurnOver;
                //If given turnover is bigger than the highest possible increase allowed turnover is set to be equal to the highest possible increase, otherwise we adopt given turnover value
                if (positiveTurnOver > highestPossibleIncrease) realizedTurnOver = highestPossibleIncrease;
                else realizedTurnOver = positiveTurnOver;

                int counter = 1;
                while (realizedTurnOver > turnoverTolerance && counter < variables.Length)
                {
                    bool found = false;
                    int weight = 0;
                    //Chose randomly weight which will be decreased
                    while (!found)
                    {
                        weight = randomGenerator.Next((int)variables.Length);
                        if (markerArray[weight] == 0 && weight != highestPossibleChangeIndex)
                        {
                            found = true;
                            markerArray[weight]--;
                        }
                    }

                    //Determine decrease value according to lower bound value of chosen weight
                    double decreaseValue;
                    if (variables[weight].getValue() <= realizedTurnOver) decreaseValue = variables[weight].getValue();
                    else if ((variables[weight].getValue() - variables[weight].getLowerBound()) <= realizedTurnOver) decreaseValue = variables[weight].getValue() - variables[weight].getLowerBound();
                    else decreaseValue = realizedTurnOver;

                    //Decrease chosen weight by given value
                    variables[weight].setValue(variables[weight].getValue() - decreaseValue);
                    //Increase increased weight by the same value
                    variables[highestPossibleChangeIndex].setValue(variables[highestPossibleChangeIndex].getValue() + decreaseValue);
                    //Decrease total turnover
                    realizedTurnOver -= decreaseValue;
                    counter++;
                }
            }

            System.IO.StreamWriter writer = new System.IO.StreamWriter("solution.csv", true);
            string tmp = "";
            for (int i = 0; i < variables.Length; i++)
            {
                tmp += variables[i].ToString() + ",";
            }
            //tmp += sleepTime.ToString();
            writer.WriteLine(tmp);
            writer.Close();
        
            return variables;
        }

        void GetWeightsAbleToChange(Variable[] solutionVariables, double value, List<int> weights, int changeMarker, int[] markerArray, ref double highestPossibleChange, List<int> highestPossibleChangeIndexes)//changeMarker=+1 to increase, -1 to decrease
        {
            weights.Clear();
            highestPossibleChangeIndexes.Clear();
            for (int i = 0; i < solutionVariables.Length; i++)
            {
                if (changeMarker == 1 && markerArray[i] >= 0)
                {
                    if ((m_problem.m_upperLimit[i] - solutionVariables[i].getValue()) > highestPossibleChange)
                    {
                        highestPossibleChange = m_problem.m_upperLimit[i] - solutionVariables[i].getValue();
                        highestPossibleChangeIndexes.Clear();
                        highestPossibleChangeIndexes.Add(i);
                    }
                    else if ((m_problem.m_upperLimit[i] - solutionVariables[i].getValue()) == highestPossibleChange)
                    {
                        highestPossibleChangeIndexes.Add(i);
                    }
                    if ((solutionVariables[i].getValue() + value) <= m_problem.m_upperLimit[i] && (solutionVariables[i].getValue() + value) >= m_problem.m_lowerLimit[i]) weights.Add(i);
                }
                else if(changeMarker == -1 && markerArray[i] <= 0)
                {
                    //if ((referentVariables[i].getValue() - m_problem.m_lowerLimit[i]) > highestPossibleChange)
                    if ((solutionVariables[i].getValue() - 0.0) > highestPossibleChange)
                    {
                        //highestPossibleChange = referentVariables[i].getValue() - m_problem.m_lowerLimit[i];
                        highestPossibleChange = solutionVariables[i].getValue() - 0.0;
                        highestPossibleChangeIndexes.Clear();
                        highestPossibleChangeIndexes.Add(i);
                    }
                    //else if ((referentVariables[i].getValue() - m_problem.m_lowerLimit[i]) == highestPossibleChange)
                    else if ((solutionVariables[i].getValue() - 0.0) == highestPossibleChange)
                    {
                        highestPossibleChangeIndexes.Add(i);
                    }
                    if ((solutionVariables[i].getValue() - value) >= m_problem.m_lowerLimit[i]) weights.Add(i);
                    else if ((solutionVariables[i].getValue() - value) == 0.0) weights.Add(i);
                }
            }
        }

        void GetWeightWithClosestValue(Variable[] Variables, double value, ref int weight)//changeMarker=+1 to increase, -1 to decrease
        {
            weight = -1;
            double difference = double.MaxValue;
            for (int i = 0; i < Variables.Length; i++)
            {
                if (Variables[i].getValue() != 0.0)
                {
                    double currentDifference = Math.Abs(Variables[i].getValue() - value);
                    if (currentDifference < difference)
                    {
                        weight = i;
                        difference = currentDifference;
                    }
                }
            }
        }

        public void doCorrection(Solution solution)
        {
            //Normalization
            double totalSum = 0.0;
            for (int i = 0; i < solution.DecisionVariables.Length; i++) totalSum += solution.DecisionVariables[i].getValue();
            for (int i = 0; i < solution.DecisionVariables.Length; i++) solution.DecisionVariables[i].setValue(solution.DecisionVariables[i].getValue() / totalSum);

            //Due to normalization weights could be out of allowed range
            try
            {
                correctBoundaries(solution);
                if (referentSolution != null) correctTurnOver(solution);
            }
            catch (Exception ex) 
            { 
                throw new Exception("Correction failed: "+ex.Message); 
            }
            //System.IO.StreamWriter writer = new System.IO.StreamWriter("solution.csv", true);
            //string tmp = "";
            //double turnover = 0;
            //for (int i = 0; i < solution.DecisionVariables.Length; i++)
            //{
            //    tmp += solution.DecisionVariables[i].ToString() + ",";
            //    turnover += Math.Abs(solution.DecisionVariables[i].getValue() - referentSolution.DecisionVariables[i].getValue());
            //}
            //tmp += turnover.ToString();
            //writer.WriteLine(tmp);
            //writer.Close();

        }

        /// <summary> Perform correction of weights in order to be within given boundaries</summary>
        protected void correctBoundaries(Solution solution)
        {
            try
            {
                double positiveCorrection = 0.0;
                double negativeCorrection = 0.0;
                for (int i = 0; i < solution.DecisionVariables.Length; i++)
                {
                    if (solution.DecisionVariables[i].getValue() < solution.DecisionVariables[i].getLowerBound())
                    {
                        if (solution.DecisionVariables[i].getValue() > (solution.DecisionVariables[i].getLowerBound() * 0.5))
                        {
                            positiveCorrection += solution.DecisionVariables[i].getLowerBound() - solution.DecisionVariables[i].getValue();
                            solution.DecisionVariables[i].setValue(solution.DecisionVariables[i].getLowerBound());
                        }
                        else
                        {
                            negativeCorrection += solution.DecisionVariables[i].getValue();
                            solution.DecisionVariables[i].setValue(0.0);
                        }
                        //negativeLimitDeviations.Add(i, solution.DecisionVariables[i].getLowerBound() - solution.DecisionVariables[i].getValue());
                    }
                    else if (solution.DecisionVariables[i].getValue() > solution.DecisionVariables[i].getUpperBound())
                    {
                        //positiveLimitDeviations.Add(i, solution.DecisionVariables[i].getValue() - solution.DecisionVariables[i].getUpperBound());
                        negativeCorrection += solution.DecisionVariables[i].getValue() - solution.DecisionVariables[i].getUpperBound();
                        solution.DecisionVariables[i].setValue(solution.DecisionVariables[i].getUpperBound());
                    }
                }
                double correctionDifference = positiveCorrection - negativeCorrection;

                List<int> weightsToChange = new List<int>();
                List<int> highestPossibleChangeIndexes = new List<int>();
                int[] markerArray = new int[solution.DecisionVariables.Length];
                //At the beginning all marks are equal to 0
                for (int i = 0; i < solution.DecisionVariables.Length; i++) markerArray[i] = 0;
                double highestPossibleChange = 0.0;

                int flag = 0;
                if (correctionDifference < 0.0) flag = 1;
                else if (correctionDifference > 0.0) flag = -1;

                if (flag != 0)
                {
                    while (correctionDifference != 0.0)
                    {
                        double value = Math.Abs(correctionDifference);
                        int weight;
                        int index = -1;
                        GetWeightsAbleToChange(solution.DecisionVariables, value, weightsToChange, flag, markerArray, ref highestPossibleChange, highestPossibleChangeIndexes);
                        if (weightsToChange.Count != 0)
                        {
                            weight = randomGenerator.Next(weightsToChange.Count);
                            index = weightsToChange[weight];
                        }
                        else
                        {
                            if (flag == 1)
                            {
                                weight = randomGenerator.Next(highestPossibleChangeIndexes.Count);
                                index = highestPossibleChangeIndexes[weight];
                                value = highestPossibleChange;
                            }
                            else if (flag == -1)
                            {
                                GetWeightWithClosestValue(solution.DecisionVariables, value, ref index);
                                value = solution.DecisionVariables[index].getValue();
                            }
                        }

                        solution.DecisionVariables[index].setValue(solution.DecisionVariables[index].getValue() + flag * value);
                        correctionDifference += flag * value;
                        if (correctionDifference < 0.0) flag = 1;
                        else if (correctionDifference > 0.0) flag = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Correction of boundaries failed: " + ex.Message);
            }

        }
        protected void correctTurnOver(Solution solution)
        {
            double buyingTurnover = 0.0;
            double sellingTurnOver = 0.0;
            double solutionWeight, referentSolutionWeight, difference;
            double totalIncreaseableAmount = 0;
            double totalDecreaseableAmount = 0;
            //Dictionary<int, double> changeableWeightsWithNegativeTurnOver = new Dictionary<int, double>();
            //Dictionary<int, double> changeableWeightsWithPositiveTurnOver = new Dictionary<int, double>();
            List<int> changeableWeightsWithNegativeTurnOver = new List<int>();
            List<int> changeableWeightsWithPositiveTurnOver = new List<int>();
            List<int> zeroWeights = new List<int>();
            List<double> negativeTurnOverPerWeight = new List<double>();
            List<double> positiveTurnOverPerWeight = new List<double>();
            List<double> negativeTurnOverCorrectionPerWeight = new List<double>();
            List<double> positiveTurnOverCorrestionPerWeight = new List<double>();

            try
            {
                for (int i = 0; i < solution.DecisionVariables.Length; i++)
                {
                    solutionWeight = solution.DecisionVariables[i].getValue();
                    referentSolutionWeight = referentSolution.DecisionVariables[i].getValue();

                    if (solutionWeight < referentSolutionWeight)
                    {
                        difference = referentSolutionWeight - solutionWeight;
                        sellingTurnOver += difference;
                        if (solutionWeight >= solution.DecisionVariables[i].getLowerBound()) 
                        {
                            changeableWeightsWithNegativeTurnOver.Add(i);//These are weights wich could be increased in order to decrease negative turonover. 
                            negativeTurnOverPerWeight.Add(difference);//Except weights lower than LowerBoundary (i.e. equal to zero).
                            totalIncreaseableAmount += difference;
                        }
                        else zeroWeights.Add(i);//Completely sold weights
                    }
                    else if (solutionWeight > referentSolutionWeight) 
                    {
                        difference = solutionWeight - referentSolutionWeight;
                        buyingTurnover += difference;
                        changeableWeightsWithPositiveTurnOver.Add(i);
                        positiveTurnOverPerWeight.Add(difference);
                        totalDecreaseableAmount += difference;
                    }
                }
                //double totalTurnOver = Math.Min(buyingTurnover, sellingTurnOver);
                double scaleFactor;
                double buyingTurnoverLimit = absoluteDeviationFromReferentSolution * 0.5;

                if (sellingTurnOver > absoluteDeviationFromReferentSolution * 0.5)
                {
                    //Increasing weights by value proportional to their distances from referent porfolio weights
                    //totalIncreasiableAmount = 0.0;
                    //for (int i = 0; i < changeableWeightsWithNegativeTurnOver.Count; i++)
                    //{
                    //    solutionWeight = solution.DecisionVariables[changeableWeightsWithNegativeTurnOver[i]].getValue();
                    //    referentSolutionWeight = referentSolution.DecisionVariables[changeableWeightsWithNegativeTurnOver[i]].getValue();
                    //    totalIncreasiableAmount += referentSolutionWeight - solutionWeight;
                    //}
                    double correctedSellingTurnover = sellingTurnOver;
                    if (correctedSellingTurnover > totalIncreaseableAmount)
                    {
                        //throw new Exception("totalTurnOver is larger than totalIncreasiableAmount!");//This exception should be handled (e.g. generate new solution)
                        int start = (int)randomGenerator.Next(0, zeroWeights.Count - 1);
                        for(int i = start; i < (zeroWeights.Count + start); i++)//Loop over zero weights starts from arbitrary(random) index
                        {
                            int counter = i % zeroWeights.Count;
                            double lowerBound = solution.DecisionVariables[zeroWeights[counter]].getLowerBound();
                            if (lowerBound < correctedSellingTurnover)
                            {
                                solution.DecisionVariables[zeroWeights[counter]].setValue(lowerBound);//Icrease zero weight to be equal to lower bound
                                correctedSellingTurnover -= lowerBound;//Decrease selling turnover by weight lower bound
                                referentSolutionWeight = referentSolution.DecisionVariables[zeroWeights[counter]].getValue();
                                difference = referentSolutionWeight - lowerBound;//Difference between referent solution weight and weight lower bound
                                totalIncreaseableAmount += difference;//Increase selling amount that can be changed
                                changeableWeightsWithNegativeTurnOver.Add(zeroWeights[counter]);//Add this weight in list of changeable weights
                                negativeTurnOverPerWeight.Add(difference);//Add amount that can be used in correction
                            }
                            if (correctedSellingTurnover <= totalIncreaseableAmount) break;
                        }
                    }

                    //Check if corrected selling Turnover is still higher than constrained TurnOver
                    if (correctedSellingTurnover > absoluteDeviationFromReferentSolution * 0.5)
                    {
                        scaleFactor = (correctedSellingTurnover - absoluteDeviationFromReferentSolution * 0.5) / totalIncreaseableAmount;

                        for (int i = 0; i < changeableWeightsWithNegativeTurnOver.Count; i++)
                        {
                            solutionWeight = solution.DecisionVariables[changeableWeightsWithNegativeTurnOver[i]].getValue();
                            referentSolutionWeight = referentSolution.DecisionVariables[changeableWeightsWithNegativeTurnOver[i]].getValue();
                            //Increase solution weight for scaleValue
                            solution.DecisionVariables[changeableWeightsWithNegativeTurnOver[i]].setValue(solutionWeight + (referentSolutionWeight - solutionWeight) * scaleFactor);
                        }
                    }
                    else
                    {
                        //Selling Turnover is lower than constrained TurnOver so buying Turnover has to be corrected to be equal to selling turnover
                        buyingTurnoverLimit = correctedSellingTurnover;
                    }

                    //Decreasing weights by value proportional to their distances from referent porfolio weights
                    //totalDecreaseableAmount = 0.0;
                    //for (int i = 0; i < changeableWeightsWithPositiveTurnOver.Count; i++)
                    //{
                    //    solutionWeight = solution.DecisionVariables[changeableWeightsWithPositiveTurnOver[i]].getValue();
                    //    referentSolutionWeight = referentSolution.DecisionVariables[changeableWeightsWithPositiveTurnOver[i]].getValue();
                    //    totalDecreaseableAmount += solutionWeight - referentSolutionWeight;
                    //}
                    if (buyingTurnover > totalDecreaseableAmount) throw new Exception("totalTurnOver is larger than totalDecreaseableAmount!");//This exception should be handled (e.g. generate new solution)
                    scaleFactor = (buyingTurnover - buyingTurnoverLimit) / totalDecreaseableAmount;
                    for (int i = 0; i < changeableWeightsWithPositiveTurnOver.Count; i++)
                    {
                        solutionWeight = solution.DecisionVariables[changeableWeightsWithPositiveTurnOver[i]].getValue();
                        referentSolutionWeight = referentSolution.DecisionVariables[changeableWeightsWithPositiveTurnOver[i]].getValue();
                        //Decrease solution weight for scaleValue
                        solution.DecisionVariables[changeableWeightsWithPositiveTurnOver[i]].setValue(solutionWeight - (solutionWeight - referentSolutionWeight) * scaleFactor);
                    }
                }

                double turnover = 0;
                for (int i = 0; i < solution.DecisionVariables.Length; i++)
                {
                    turnover += Math.Abs(solution.DecisionVariables[i].getValue() - referentSolution.DecisionVariables[i].getValue());
                }
                double a = turnover;



            }
            catch (Exception ex)
            {
                throw new Exception("Correction of Turnover failed: " + ex.Message);
            }


        }
        #endregion

        #region Previous versions of create variables functions
        /// <summary> Creates the variables of the solution (basic method)</summary>
        protected Variable[] createVariablesBasicVersion1()
        {
            Variable[] variables = new Variable[m_problem.NumberOfVariables];

            for (int var = 0; var < m_problem.NumberOfVariables; var++)
                variables[var] = new Real(m_problem.getLowerLimit(var), m_problem.getUpperLimit(var));

            //////////////////////////////////////////////////////////////////////////////////////////
            if (cardinality == 0)
            {
                cardinality = variables.Length;
            }

            int[] choosenVariables = new int[variables.Length];
            int counter = 0;
            int sample;
            double valueLimit = 1.0;

            for (int i = 0; i < variables.Length; i++)
            {
                variables[i].setValue(0.0);
                variables[i].setLowerBound(0.0);
                variables[i].setUpperBound(1.0);
            }
            while (counter < cardinality - 1)
            {
                sample = randomGenerator.Next(variables.Length);
                if (choosenVariables[sample] == 0)
                {
                    variables[sample].setValue(randomGenerator.NextDouble() * valueLimit);
                    valueLimit = valueLimit - variables[sample].getValue();
                    counter++;
                    choosenVariables[sample] = 1;
                }
            }

            if (cardinality == variables.Length)
            {
                for (int i = 0; i < variables.Length; i++)
                {
                    if (variables[i].getValue() == 0.0)
                    {
                        variables[i].setValue(valueLimit);
                        break;
                    }
                }
            }
            else
            {
                do
                {
                    sample = randomGenerator.Next(variables.Length);
                }
                while (choosenVariables[sample] == 1);
                variables[sample].setValue(valueLimit);
            }
            return variables;
        }

        #endregion


    } // RealWeightsSolutionType
}
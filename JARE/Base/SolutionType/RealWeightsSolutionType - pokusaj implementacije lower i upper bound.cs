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
                variables = createVariablesRelativeToReferentSolution(referentSolution);
            }         

            return variables;
        } // createVariables

        /// <summary> Creates the variables of the solution (basic method)</summary>
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
                    double valueRange = variables[sample].getUpperBound() - variables[sample].getLowerBound();
                    variables[sample].setValue(variables[sample].getLowerBound() + randomGenerator.NextDouble() * valueRange);
                    valueLimit = valueLimit - variables[sample].getValue();
                    counter++;
                    choosenVariables[sample] = 1;
                    chosenVariables.Add(sample);
                }
            }

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
                            if (valueLimit < variables[i].getLowerBound()) correctionValue = variables[i].getLowerBound() - valueLimit;
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
                    if (valueLimit < variables[sample].getLowerBound()) correctionValue = variables[sample].getLowerBound() - valueLimit;
                    variableIndex = sample;
                }
                chosenVariables.Add(sample);
                variables[sample].setValue(valueLimit);
            }

            if (correction)
            {
                while (correctionValue != 0.0)
                {
                    double value = 0.0;
                    sample = randomGenerator.Next(chosenVariables.Count);
                    int index = chosenVariables[sample];
                    if(index != variableIndex)
                    {
                        if (correctionValue > 0.0)
                        {
                            if (correctionValue > (variables[sample].getUpperBound() - variables[sample].getValue()))
                            {
                                value = variables[sample].getUpperBound() - variables[sample].getValue();
                            }
                            else value = correctionValue;
                            variables[sample].setValue(variables[sample].getValue() + value);
                            variables[variableIndex].setValue(variables[variableIndex].getValue() - value);
                            correctionValue -= value;
                        }
                        else
                        {
                            double absValue = Math.Abs(correctionValue);
                            if (absValue > (variables[sample].getValue() - variables[sample].getLowerBound()))
                            {
                                value = variables[sample].getValue() - variables[sample].getLowerBound();
                            }
                            else value = absValue;
                            variables[sample].setValue(variables[sample].getValue() - value);
                            variables[variableIndex].setValue(variables[variableIndex].getValue() + value);
                            correctionValue += value;
                        }                        
                    }
                }
            }
            
            System.IO.StreamWriter writer = new System.IO.StreamWriter("solution.csv", true);
            string tmp = "";
            for (int i = 0; i < variables.Length; i++)
            {
                tmp += variables[i].ToString() + ",";
            }
            tmp += sleepTime.ToString();
            writer.WriteLine(tmp);
            writer.Close();

            return variables;
        }

        private void correctVariables()
        {
 
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
            if (randomValue < 0.7)
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
                            if (value == 0.0)
                            {
                                //If highest possible chabge is equal to zero stop restruction
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
            {//Version 1. Restruction of referent portfolio with aim to produce the highest possible turn over

                //Generate selling/buying value(between 0 and turnover value)
                double value = 0.0;
                while (value == 0.0) value = randomGenerator.NextDouble() * positiveTurnOver;
                //Get weights indexes that can be increased by generated value
                GetWeightsAbleToChange(variables, value, weightsToIncrease, 1, markerArray, ref highestPossibleIncrease, highestPossibleChangeIndexes);

                //Chose randomly weight that will be increased (increased weight)
                //int tmpIndex = variablePicker.Next(0, highestPossibleChangeIndexes.Count);
                int tmpIndex = (int)(randomGenerator.NextDouble() * highestPossibleChangeIndexes.Count);
                int highestPossibleChangeIndex = highestPossibleChangeIndexes[tmpIndex];

                double realizedTurnOver;
                //If given turnover is bigger than highest possible increase allowed turnover is set to be equal to highest possible increase, otherwise we adopt given turnover value
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
                    double decreaseValue = variables[weight].getValue() - variables[weight].getLowerBound();
                    //If decrease value is higher than allowed turnover decrease value is equal to allowed turnover 
                    if(decreaseValue > realizedTurnOver) decreaseValue = realizedTurnOver;
                    //Decrease chosen weight by given value
                    variables[weight].setValue(variables[weight].getValue() - decreaseValue);
                    //Increase increased weight by the same value
                    variables[highestPossibleChangeIndex].setValue(variables[highestPossibleChangeIndex].getValue() + decreaseValue);
                    //Decrease total turnover
                    realizedTurnOver -= decreaseValue;
                    counter++;
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
            }
            //System.IO.StreamWriter writer = new System.IO.StreamWriter("solution.csv", true);
            //string tmp = "";
            //for (int i = 0; i < variables.Length; i++)
            //{
            //    tmp += variables[i].ToString() + ",";
            //}
            //tmp += sleepTime.ToString();
            //writer.WriteLine(tmp);
            //writer.WriteLine(randomValue.ToString());
            //writer.Close();
           
            return variables;
        }

        void GetWeightsAbleToChange(Variable[] referentVariables, double value, List<int> weights, int changeMarker, int[] markerArray, ref double highestPossibleChange, List<int> highestPossibleChangeIndexes)//changeMarker=+1 to increase, -1 to decrease
        {
            weights.Clear();
            for (int i = 0; i < referentVariables.Length; i++)
            {
                if (changeMarker == 1 && markerArray[i] >= 0)
                {
                    if ((m_problem.m_upperLimit[i] - referentVariables[i].getValue()) > highestPossibleChange)
                    {
                        highestPossibleChange = m_problem.m_upperLimit[i] - referentVariables[i].getValue();
                        highestPossibleChangeIndexes.Clear();
                        highestPossibleChangeIndexes.Add(i);
                    }
                    else if ((m_problem.m_upperLimit[i] - referentVariables[i].getValue()) == highestPossibleChange)
                    {
                        highestPossibleChangeIndexes.Add(i);
                    }
                    if ((referentVariables[i].getValue() + value) <= m_problem.m_upperLimit[i]) weights.Add(i);
                }
                else if(changeMarker == -1 && markerArray[i] <= 0)
                {
                    if ((referentVariables[i].getValue() - m_problem.m_lowerLimit[i]) > highestPossibleChange)
                    {
                        highestPossibleChange = referentVariables[i].getValue() - m_problem.m_lowerLimit[i];
                        highestPossibleChangeIndexes.Clear();
                        highestPossibleChangeIndexes.Add(i);
                    }
                    else if ((referentVariables[i].getValue() - m_problem.m_lowerLimit[i]) == highestPossibleChange)
                    {
                        highestPossibleChangeIndexes.Add(i);
                    }
                    if ((referentVariables[i].getValue() - value) >= m_problem.m_lowerLimit[i]) weights.Add(i);
                }
            }
        }
        #endregion


    } // RealWeightsSolutionType
}
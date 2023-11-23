using System;
using JARE.Base;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using JARE.util;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using JARE.Base.variable;


namespace JARE.metaheuristics.nsgaII
{
    public class NSGAIIFViaBinder : NSGAIIF
    {

        EvaluationPool.EvaluationPool epw;
        string startingPopulation = "";

        /// <summary> Constructor</summary>
        /// <param name="problem">Problem to solve</param>
        /// <param name="script">Script for running WorkBinder client</param>
        /// <param name="dir">Working directory for the script</param>
        /// <param name="varFile">Path to directory where files with parames to be evaluated will be written</param>
        /// <param name="LogFilesDir">Path to directory where log files will be written</param>
        /// <param name="procTimeout">Max time for evaluation process</param>
        public NSGAIIFViaBinder(Problem problem, string script, string dir,string varFile, string LogFilesDir, int procTimeout) : base(problem, LogFilesDir)
        {
            EvaluationPool.IWBEvaluation p;
            p = problem as EvaluationPool.IWBEvaluation;

            if (p != null)
            {
                epw = new JARE.EvaluationPool.EvaluationPoolForBinderTPL(p, script, dir, varFile, procTimeout,varFile);
            }
            else
            {
                throw new Exception("This problem can not be solved via Binder");
            }
        } // NSGAII


        public NSGAIIFViaBinder(Problem problem, string script, string dir, string varFile, string LogFilesDir, int procTimeout, string startingPopulation)
            : this(problem, script, dir, varFile, LogFilesDir, procTimeout)
        {
            this.startingPopulation = startingPopulation;
        }

        public override void evaluatePopulationBasic(SolutionSet population)
        {
            for (int i = 0; i < population.size(); i++)
            {
                epw.SendSolution(population.m_solutionsList[i]);
                evaluations++;
            } //for 
            epw.Wait();
        } 

        protected override void initExecution() 
        {
            base.initExecution();

            epw.Start();
        }

        protected override void closeExecution()
        {
            base.closeExecution();

            epw.Stop();
        }

        /// <summary> Runs the NSGA-II algorithm.</summary>
        /// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
        /// as a result of the algorithm execution
        /// </returns>
        ///// <throws>  SMException  </throws>
        //public override SolutionSet execute()
        //{
        //    int populationSize;
        //    double previousQualityValue = 0;

        //    QualityIndicator indicators; // QualityIndicator object

        //    SolutionSet population = new SolutionSet(); //Slavisa dodao
        //    SolutionSet offspringPopulation;
        //    SolutionSet union;

        //    Operator mutationOperator;
        //    Operator crossoverOperator;
        //    Operator selectionOperator;

        //    Distance distance = new Distance();

        //    //Read the parameters
        //    populationSize = ((System.Int32) getInputParameter("populationSize"));
        //    maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
        //    indicators = (QualityIndicator) getInputParameter("indicators");

        //    //Read the operators
        //    mutationOperator = m_operators["mutation"];
        //    crossoverOperator = m_operators["crossover"];
        //    selectionOperator = m_operators["selection"];

        //    //Init execution parameters
        //    initExecution();

        //    // Create the initial solutionSet
        //    population = createInitialPopulation(populationSize);

        //    evaluatePopulation(population); 

        //    while (evaluations < maxEvaluations)
        //    {
        //        if (Canceled) break;
        //        generations++;

        //        offspringPopulation = new SolutionSet(populationSize);
        //        Solution[] parents = new Solution[2];
        //        for (int i = 0; i < (populationSize / 2); i++)
        //        {
        //            parents[0] = (Solution)selectionOperator.execute(population);
        //            parents[1] = (Solution)selectionOperator.execute(population);
        //            Solution[] offSpring = (Solution[])crossoverOperator.execute(parents);
        //            mutationOperator.execute(offSpring[0]);
        //            mutationOperator.execute(offSpring[1]);

        //            offspringPopulation.add(offSpring[0]);
        //            offspringPopulation.add(offSpring[1]);
        //        } // for
               
        //        evaluatePopulation(offspringPopulation); 

        //        union = ((SolutionSet)population).union(offspringPopulation);

        //        // Ranking the union
        //        Ranking ranking = new Ranking(union);

        //        int remain = populationSize;
        //        int index = 0;
        //        SolutionSet front = null;
        //        population.clear();

        //        // Obtain the next front
        //        front = ranking.getSubfront(index);

        //        while ((remain > 0) && (remain >= front.size()))
        //        {
        //            //Assign crowding distance to individuals
        //            distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
        //            //Add the individuals of this front
        //            for (int k = 0; k < front.size(); k++)
        //            {
        //                population.add(front.getSolution(k));
        //            } // for

        //            //Decrement remain
        //            remain = remain - front.size();

        //            //Obtain the next front
        //            index++;
        //            if (remain > 0)
        //            {
        //                front = ranking.getSubfront(index);
        //            } // if        
        //        } // while

        //        // Remain is less than front(index).size, insert only the best one
        //        if (remain > 0)
        //        {
        //            // front contains individuals to insert                        
        //            distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
        //            front.sort(new JARE.Base.operators.comparator.CrowdingComparator());
        //            for (int k = 0; k < remain; k++)
        //            {
        //                population.add(front.getSolution(k));
        //            } // for

        //            remain = 0;
        //        } // if                               

        //        //Calculate required evaluations
        //        determineRequiredEvaluations(population, indicators);

        //        Ranking pRanking = new Ranking(population);

        //        SolutionSet s1 = pRanking.getSubfront(0);

        //        printReportForGeneration(population, s1, generations);

        //        if (isQualityTerminationConditionSatisfied(s1, ref previousQualityValue)) break;

        //    } // while
           
        //    // Return as output parameter the required evaluations
        //    setOutputParameter("evaluations", requiredEvaluations);

        //    // Return the first non-dominated front
        //    Ranking ranking2 = new Ranking(population);

        //    closeExecution();

        //    return ranking2.getSubfront(0);
        //}


    } 
}

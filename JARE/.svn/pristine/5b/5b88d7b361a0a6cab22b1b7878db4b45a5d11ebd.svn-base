using System;
using SharpMetal.Base;
using QualityIndicator = SharpMetal.qualityIndicator.QualityIndicator;
using SharpMetal.util;
using System.Threading;
using System.Collections.Generic;
namespace SharpMetal.metaheuristics.nsgaII
{
    /// <summary> This class implements the NSGA-II algorithm. </summary>
    [Serializable]
    public class NSGAIIEviaManager : Algorithm
    {
        /// <summary> stores the problem  to solve</summary>
        private Problem m_problem;
        DateTime NOW;
        EvaluationPool.EvaluationPoolToManager epw;

        //VISNJA: UPISIVANJE NAJBOLJIH IZ SVAKE POPULACIJE U FAJL
        System.IO.StreamWriter sw;

        String timeFile;
        String[] timeData;

        /// <summary> Constructor</summary>
        /// <param name="problem">Problem to solve
        /// </param>
        public NSGAIIEviaManager(Problem problem, WCFManagerClient manager)
        {

            this.m_problem = problem;
            EvaluationPool.IWSEvaluation p = problem as EvaluationPool.IWSEvaluation;
            epw = new SharpMetal.EvaluationPool.EvaluationPoolToManager(p, manager);
            timeFile = String.Format("{0}\\times_{1}_{2}_{3}_{4}_{5}.txt", SharpMetal.Properties.Settings.Default.LogFilesLocation, DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);

        //    string timesFile = String.Format("{0}\\times_{1}_{2}_{3}_{4}_{5}.txt", SharpMetal.Properties.Settings.Default.LogFilesLocation, DateTime.Now.Year,
        //            DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
            sw = new System.IO.StreamWriter(String.Format("{0}\\times_{1}_{2}_{3}_{4}_{5}.txt", SharpMetal.Properties.Settings.Default.LogFilesLocation, DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute));


        } // NSGAII

        /// <summary> Runs the NSGA-II algorithm.</summary>
        /// <returns> a <code>SolutionSet</code> that is a set of non dominated solutions
        /// as a result of the algorithm execution
        /// </returns>
        /// <throws>  SMException  </throws>
        public override SolutionSet execute()
        {
            int populationSize;
            int maxEvaluations;
            int evaluations;

            QualityIndicator indicators; // QualityIndicator object
            int requiredEvaluations; // Use in the example of use of the
            // indicators object (see below)

            SolutionSet population;
            SolutionSet offspringPopulation;
            SolutionSet union;

            Operator mutationOperator;
            Operator crossoverOperator;
            Operator selectionOperator;

            Distance distance = new Distance();

            //Read the parameters
            populationSize = ((System.Int32)getInputParameter("populationSize"));
            maxEvaluations = ((System.Int32)getInputParameter("maxEvaluations"));
            indicators = (QualityIndicator)getInputParameter("indicators");

            //Initialize the variables
            population = new SolutionSet(populationSize);
            evaluations = 0;

            requiredEvaluations = 0;

            //Read the operators
            mutationOperator = m_operators["mutation"];
            crossoverOperator = m_operators["crossover"];
            selectionOperator = m_operators["selection"];


            //Kreiranje niza za pakovanje podataka o prolaznim vrmenima

            timeData = new string[maxEvaluations]; 

            // Create the initial solutionSet
            Solution newSolution;
            for (int i = 0; i < populationSize; i++)
            {
                newSolution = new Solution(m_problem);
                population.add(newSolution);
            } //for 

            epw.Start();

            double avgTime;
            double sumTime = 0;
            double totalTime = 0;
            double currentTime;
            DateTime totalStartTime = DateTime.Now;
            DateTime t0 = totalStartTime;
            DateTime NOW;

            //  sw.Write("JAAAAAA"); 
            sw.WriteLine(TimeUtil.TimeToString(totalStartTime)+ "Evaluating initial population...");
            //   timeData[]
            
            #region Evaluacija_prve_populacije
            
            Console.WriteLine("Evaluating initial population...");
            
            //VISNJA Prvo sve doda u populaciju, pa onda evaluira
            for (int i = 0; i < populationSize; @i++)
            {
                epw.SendSolution(population.m_solutionsList[i]);
                evaluations++;
            } //for 
            epw.Wait();
            Console.WriteLine("Initial population evaluated.");

            #endregion
        
            NOW = DateTime.Now;
            sw.WriteLine(TimeUtil.TimeToString(NOW) + " Initial population evaluated.");
            currentTime = ((TimeSpan)(NOW - t0)).TotalSeconds;
            totalTime += currentTime;

            // Generations ...
            int generations = 0;

            while (evaluations < maxEvaluations)
            {
                generations++;

                // Create the offSpring solutionSet  
                Console.WriteLine("Creating offspring of " + generations.ToString() + ". generation...");

                offspringPopulation = new SolutionSet(populationSize);
                Solution[] parents = new Solution[2];
                for (int i = 0; i < (populationSize / 2); i++)
                {
                //    Console.WriteLine("PROLAZ "+i);
                    parents[0] = (Solution)selectionOperator.execute(population);
                    parents[1] = (Solution)selectionOperator.execute(population);
                    Solution[] offSpring = (Solution[])crossoverOperator.execute(parents);
                    mutationOperator.execute(offSpring[0]);
                    mutationOperator.execute(offSpring[1]);

                    offspringPopulation.add(offSpring[0]);
                    offspringPopulation.add(offSpring[1]);
                } // for


                t0 = DateTime.Now;
                sw.WriteLine(TimeUtil.TimeToString(t0) + " Evaluating offspring of " + generations.ToString() + ". generation...");

                #region Evaluating offspring
                Console.WriteLine("Evaluating offspring of " + generations.ToString() + ". generation...");
                for (int i = 0; i < offspringPopulation.size(); i++)
                {
                    if (evaluations < maxEvaluations)
                    {
                        //obtain parents
                        epw.SendSolution(offspringPopulation.m_solutionsList[i]);
                        evaluations++;
                    } // if                            
                } // for

                epw.Wait();
                Console.WriteLine("Offspring of " + generations.ToString() + ". generation evaluated.");
                #endregion

                NOW = DateTime.Now;
                sw.WriteLine(TimeUtil.TimeToString(NOW) + " Offspring of " + generations.ToString() + ". generation evaluated.");
                currentTime = ((TimeSpan)(NOW - t0)).TotalSeconds;
                totalTime += currentTime;
                sw.WriteLine("_______ evalTime ______ " + currentTime);
                Console.WriteLine("_______ evalTime ______ " + currentTime);
                
                // Create the solutionSet union of solutionSet and offSpring
                union = ((SolutionSet)population).union(offspringPopulation);

                // Ranking the union
                Console.WriteLine("Ranking " + generations.ToString() + ". generation...");
                Ranking ranking = new Ranking(union);

                int remain = populationSize;
                int index = 0;
                SolutionSet front = null;
                population.clear();

                // Obtain the next front
                front = ranking.getSubfront(index);

                while ((remain > 0) && (remain >= front.size()))
                {
                    //Assign crowding distance to individuals
                    distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
                    //Add the individuals of this front
                    for (int k = 0; k < front.size(); k++)
                    {
                        population.add(front.getSolution(k));
                    } // for

                    //Decrement remain
                    remain = remain - front.size();

                    //Obtain the next front
                    index++;
                    if (remain > 0)
                    {
                        front = ranking.getSubfront(index);
                    } // if        
                } // while

                // Remain is less than front(index).size, insert only the best one
                if (remain > 0)
                {
                    // front contains individuals to insert                        
                    distance.crowdingDistanceAssignment(front, m_problem.NumberOfObjectives);
                    front.sort(new SharpMetal.Base.operators.comparator.CrowdingComparator());
                    for (int k = 0; k < remain; k++)
                    {
                        population.add(front.getSolution(k));
                    } // for

                    remain = 0;
                } // if                               

                // This piece of code shows how to use the indicator object into the code
                // of NSGA-II. In particular, it finds the number of evaluations required
                // by the algorithm to obtain a Pareto front with a hypervolume higher
                // than the hypervolume of the true Pareto front.
                if ((indicators != null) && (requiredEvaluations == 0))
                {
                    double HV = indicators.getHypervolume(population);
                    if (HV >= (0.98 * indicators.TrueParetoFrontHypervolume))
                    {
                        requiredEvaluations = evaluations;
                    } // if
                } // if

                Ranking pRanking = new Ranking(population);

                SolutionSet s1 = pRanking.getSubfront(0);
                Console.WriteLine("Writing report for " + generations.ToString() + ". generation...");

         
                m_problem.PrintGenerationReport(population, s1, generations);
            } // while

            //VISNJA
            sw.WriteLine(TimeUtil.TimeToString(DateTime.Now) + " DONE");
            Console.WriteLine("DONE!!!");
            epw.Stop();
            // Return as output parameter the required evaluations

            setOutputParameter("evaluations", requiredEvaluations);
            // Write population
            int solutionIndex = 0;
            

            // Return the first non-dominated front
            Ranking ranking2 = new Ranking(population);
            SolutionSet s = ranking2.getSubfront(0);
            
            NOW = DateTime.Now;
            Console.WriteLine("TOTAL TIME " + ((TimeSpan)(NOW - totalStartTime)).TotalSeconds);
            sw.WriteLine("TOTAL TIME " + ((TimeSpan)(NOW - totalStartTime)).TotalSeconds);
            avgTime = totalTime / (generations + 1);
            Console.WriteLine("AVGtime" + avgTime);
            sw.WriteLine("AVGtime" + avgTime);
            sw.Close();
            
            return ranking2.getSubfront(0);
            //eps.sw.Close();
        } // execute


        
    } // NSGA-II
}
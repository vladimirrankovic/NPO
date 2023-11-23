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
    public class NSGAIIViaBinder : Algorithm
    {
        /// <summary> stores the problem  to solve</summary>
        private Problem m_problem;
        DateTime beggining = DateTime.Now;
        DateTime begginingOfGenerationEvaluation;
        double t;
        EvaluationPool.EvaluationPool epw;
        bool startFromNthPopulation = false;
        string startingPopulation = "", info="";


        //Slavisa
        String infoFromConsole = "";

        //VISNJA: UPISIVANJE vremena
        System.IO.StreamWriter sw;

        /// <summary> Constructor</summary>
        /// <param name="problem">Problem to solve</param>
        /// <param name="script">Script for running WorkBinder client</param>
        /// <param name="dir">Working directory for the script</param>
        /// <param name="varFile">Path to directory where files with parames to be evaluated will be written</param>
        /// <param name="LogFilesDir">Path to directory where log files will be written</param>
        /// <param name="procTimeout">Max time for evaluation process</param>

        public NSGAIIViaBinder(Problem problem, string script, string dir,string varFile, string LogFilesDir, int procTimeout)
        {
            this.m_problem = problem;
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

            sw = new System.IO.StreamWriter(String.Format("{0}\\times_{1}_{2}_{3}_{4}_{5}.txt", LogFilesDir, DateTime.Now.Year,
            DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute));
        } // NSGAII


        public NSGAIIViaBinder(Problem problem, string script, string dir, string varFile, string LogFilesDir, int procTimeout, string startingPopulation)
            : this(problem, script, dir, varFile, LogFilesDir, procTimeout)
        {
            startFromNthPopulation = true;
            this.startingPopulation = startingPopulation;
        }


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
            int evaluationsInOnePopulation;

            QualityIndicator indicators; // QualityIndicator object
            int requiredEvaluations; // Use in the example of use of the
            // indicators object (see below)

            SolutionSet population = new SolutionSet(); //Slavisa dodao
            SolutionSet offspringPopulation;
            SolutionSet union;

            Operator mutationOperator;
            Operator crossoverOperator;
            Operator selectionOperator;

            Distance distance = new Distance();

            //Read the parameters
            populationSize = ((System.Int32) getInputParameter("populationSize"));
            maxEvaluations = ((System.Int32) getInputParameter("maxEvaluations"));
            indicators = (QualityIndicator) getInputParameter("indicators");

            //Initialize the variables
            evaluations = 0;
            evaluationsInOnePopulation = 0;

            requiredEvaluations = 0;

            //Read the operators
            mutationOperator = m_operators["mutation"];
            crossoverOperator = m_operators["crossover"];
            selectionOperator = m_operators["selection"];

            DateTime totalStartTime = DateTime.Now;
            DateTime t0 = totalStartTime;


            if (!startFromNthPopulation)
            {
                populationSize = ((System.Int32)getInputParameter("populationSize"));
                population = new SolutionSet(populationSize);

                // Create the initial solutionSet
                Solution newSolution;
                for (int i = 0; i < populationSize; i++)
                {
                    newSolution = new Solution(m_problem);
                    population.add(newSolution);
                } //for 

            }
            else
            {
                List<double[]> initialPopulation = ReadInitialPopulation(this.startingPopulation);
                //populationSize = initialPopulation.Count;
                population = new SolutionSet(populationSize);

                Solution newSolution;
                Variable[] variables;
                Real r;

                for (int i = 0; i < initialPopulation.Count; i++)
                {
                    variables = new Variable[m_problem.NumberOfVariables];

                    for (int var = 0; var < m_problem.NumberOfVariables; var++)
                    {
                        r = new Real();
                        r.setValue(initialPopulation[i][var]);
                        r.setLowerBound(m_problem.m_lowerLimit[var]);
                        r.setUpperBound(m_problem.m_upperLimit[var]);
                        variables[var] = r;
                    }
                    newSolution = new Solution(m_problem, variables);
                    population.add(newSolution);
                }
                for (int i = initialPopulation.Count; i < populationSize; i++)
                {
                    newSolution = new Solution(m_problem);
                    population.add(newSolution);
                }
            }

                epw.Start();
                //Slavisa
                //infoFromConsole = " Evaluating initial population... \n";
                //m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                
                infoFromConsole = "Evaluating initial population...  \n";
                m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                
                Console.WriteLine("Evaluating initial population...");
                begginingOfGenerationEvaluation = DateTime.Now;

                //Evaluate initial solution set
                for (int i = 0; i < populationSize; i++)
                {
                    epw.SendSolution(population.m_solutionsList[i]);
                    evaluations++;
                    evaluationsInOnePopulation++;
                } //for 

                epw.Wait();
                infoFromConsole = String.Format("Initial population evaluated in {0} s. Total time: {1} s \n", (DateTime.Now - begginingOfGenerationEvaluation).TotalSeconds, (DateTime.Now - beggining).TotalSeconds);
                m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));

                Console.WriteLine(infoFromConsole);

                //Slavisa
                infoFromConsole = "Initial population evaluated. \n";
                m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));

                //sw.WriteLine("Initial population evaluated.");
            
            // Generations ...
            int generations = 0;

            while (evaluations < maxEvaluations)
            {
                generations++;

                // Create the offSpring solutionSet  
                //Slavisa

                offspringPopulation = new SolutionSet(populationSize);
                Solution[] parents = new Solution[2];
                for (int i = 0; i < (populationSize / 2); i++)
                {
                    parents[0] = (Solution)selectionOperator.execute(population);
                    parents[1] = (Solution)selectionOperator.execute(population);
                    Solution[] offSpring = (Solution[])crossoverOperator.execute(parents);
                    mutationOperator.execute(offSpring[0]);
                    mutationOperator.execute(offSpring[1]);

                    offspringPopulation.add(offSpring[0]);
                    offspringPopulation.add(offSpring[1]);
                } // for

                //Slavisa
                infoFromConsole = "Evaluating offspring of " + generations.ToString() + ". generation... \n";
                m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                Console.WriteLine("Evaluating offspring of " + generations.ToString() + ". generation...");
                infoFromConsole = "";

                t0 = DateTime.Now;
                begginingOfGenerationEvaluation = DateTime.Now;
                
                for (int i = 0; i < offspringPopulation.size(); i++)
                {
                    if (evaluations < maxEvaluations)
                    {
                        epw.SendSolution(offspringPopulation.m_solutionsList[i]);
                        evaluations++;
                        evaluationsInOnePopulation++;
                    } // if                            
                } // for

                epw.Wait();

                t = (DateTime.Now - begginingOfGenerationEvaluation).TotalSeconds;
                info = String.Format("Offspring of " + generations.ToString() + ". generation evaluated in {0} s. Total time: {1} s",
                t, (DateTime.Now - beggining).TotalSeconds);
                sw.WriteLine(t);
                infoFromConsole = info;
                m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                //infoFromConsole1 += String.Format("Offspring of " + generations.ToString() + ". generation evaluated in {0} s. Total time: {1} s -------------- {2}",
                //t, (DateTime.Now - beggining).TotalSeconds, evaluationsInOnePopulation);
                //infoFromConsole = info;
                //m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                //infoFromConsole1 += String.Format("Offspring of " + generations.ToString() + ". generation evaluated in {0} s. Total time: {1} s -------------- {2}",
                //t, (DateTime.Now - beggining).TotalSeconds, evaluationsInOnePopulation);
                Console.WriteLine(info);

                infoFromConsole = String.Format("Begining time: {0}. Estimated end time: {1}", beggining, DateTime.Now.AddSeconds(t * (maxEvaluations / populationSize - generations - 1)));
                m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                Console.WriteLine("Begining time: {0}. Estimated end time: {1}", beggining, DateTime.Now.AddSeconds(t * (maxEvaluations / populationSize - generations - 1)));

                // Create the solutionSet union of solutionSet and offSpring

                union = ((SolutionSet)population).union(offspringPopulation);

                // Ranking the union
                //Slavisa
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
                    front.sort(new JARE.Base.operators.comparator.CrowdingComparator());
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
                //Slavisa
                infoFromConsole = "Writing report for " + generations.ToString() + ". generation... \n";
                m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                m_problem.CreateAlgorithmInfoNew(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
                Console.WriteLine("Writing report for " + generations.ToString() + ". generation...");
                m_problem.PrintGenerationReport(population, s1, generations);
                m_problem.CreateAlgorithmInfo(new AlgorithmInfo(s1, "", m_problem.m_numberOfVariables));
                //m_problem.CreateAlgorithmInfo(new AlgorithmInfo(s1, infoFromConsole, m_problem.m_numberOfVariables));
                //evaluationsInOnePopulation = 0;
                sw.Flush();
                //infoFromConsole = "";
            } // while

           
            epw.Stop();

            //sw.WriteLine(TimeUtil.TimeToString(DateTime.Now) + " DONE");

            // Return as output parameter the required evaluations
            setOutputParameter("evaluations", requiredEvaluations);

            // Return the first non-dominated front
            Ranking ranking2 = new Ranking(population);
            //infoFromConsole = "";
            //m_problem.CreateAlgorithmInfo(new AlgorithmInfo(null, infoFromConsole, m_problem.m_numberOfVariables));
            sw.Close();

            return ranking2.getSubfront(0);
        }

        private List<double[]> ReadInitialPopulation(string file)
        {
            StreamReader str = new StreamReader(file);
            string line = ""; string[] splittedLine;
            List<double> individual;
            List<double[]> initialPopulation = new List<double[]>();

            //citaj header
            line = str.ReadLine();

            while (!str.EndOfStream)
            {
                line = str.ReadLine();
                splittedLine = line.Split(',');

                individual = new List<double>();
                for (int i = m_problem.NumberOfObjectives + 1; i < splittedLine.Length; i++)
                {
                    individual.Add(Convert.ToDouble(splittedLine[i]));
                }

                initialPopulation.Add(individual.ToArray());
            }

            return initialPopulation;
        }

    } 
}

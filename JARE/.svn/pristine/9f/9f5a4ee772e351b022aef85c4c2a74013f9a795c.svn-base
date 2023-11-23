using System;
using JARE.Base;
using QualityIndicator = JARE.qualityIndicator.QualityIndicator;
using JARE.util;
using System.Threading;
namespace JARE.metaheuristics.nsgaII
{
    /// <summary> This class implements the NSGA-II algorithm. </summary>
    [Serializable]
    public class NSGAIIEvalPoolMultithread : Algorithm
    {
        /// <summary> stores the problem  to solve</summary>
        private Problem m_problem;

        EvaluationPool.EvaluationPoolMultithread eps;

        //VISNJA: UPISIVANJE NAJBOLJIH IZ SVAKE POPULACIJE U FAJL
        System.IO.StreamWriter sw;

        /// <summary> Constructor</summary>
        /// <param name="problem">Problem to solve
        /// </param>
        public NSGAIIEvalPoolMultithread(Problem problem)
        {

            this.m_problem = problem;
            eps = new JARE.EvaluationPool.EvaluationPoolMultithread(m_problem, 3);

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

            int brojPoslatih = 0;
            eps.Start();

            // Create the initial solutionSet
            Solution newSolution;
            for (int i = 0; i < populationSize; i++)
            {
                newSolution = new Solution(m_problem);
                population.add(newSolution);
            } //for 


            //VISNJA Prvo sve doda u populaciju, pa onda evaluira
            for (int i = 0; i < populationSize; i++)
            {
                eps.SendSolution(population.m_solutionsList[i]);
                //brojPoslatih++;
            } //for 
            eps.Wait();
            
            Console.WriteLine("napravljena pocetna populacija");
            // Generations ...
            //VISNJA
           // Console.WriteLine(eps.BrojEvaluiranih);
            int generations = 0;

            while (evaluations < maxEvaluations)
            {

                // Create the offSpring solutionSet      
                offspringPopulation = new SolutionSet(populationSize);
                Solution[] parents = new Solution[2];
                for (int i = 0; i < (populationSize / 2); i++)
                {
                    //obtain parents
                    parents[0] = (Solution)selectionOperator.execute(population);
                    parents[1] = (Solution)selectionOperator.execute(population);
                    Solution[] offSpring = (Solution[])crossoverOperator.execute(parents);
                    mutationOperator.execute(offSpring[0]);
                    mutationOperator.execute(offSpring[1]);

                    offspringPopulation.add(offSpring[0]);
                    offspringPopulation.add(offSpring[1]);
                } // for

                for (int i = 0; i < offspringPopulation.size(); i++)
                {
                    if (evaluations < maxEvaluations)
                    {
                        //VISNJA
                        eps.SendSolution(offspringPopulation.m_solutionsList[i]);
                        //brojPoslatih++;
                    } // if                            
                } // for
                
                eps.Wait();

                // Create the solutionSet union of solutionSet and offSpring
                union = ((SolutionSet)population).union(offspringPopulation);

                // Ranking the union
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

                //VISNJA
                Ranking myranking = new Ranking(population);

                //VISNJA
                Console.WriteLine("GEN No. {0}", generations);

                SolutionSet s = myranking.getSubfront(0);
                m_problem.PrintGenerationReport(population, s, generations++);
            } // while

            // Return as output parameter the required evaluations
            setOutputParameter("evaluations", requiredEvaluations);

            eps.Stop();
            // Return the first non-dominated front
            Ranking ranking2 = new Ranking(population);
            Console.WriteLine("Poslato je " + brojPoslatih.ToString());
            return ranking2.getSubfront(0);
            //eps.sw.Close();
        } // execute


    } // NSGA-II
}
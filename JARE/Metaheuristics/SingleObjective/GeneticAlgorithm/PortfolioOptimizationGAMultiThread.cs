using System;
using System.Collections.Generic;
using System.Text;
using JARE.Base;
using JARE.Base.operators.comparator;
using Algorithm = JARE.Base.Algorithm;
using XReal = JARE.util.wrapper.XReal;
using JARE.util;
using JARE.problems.Finance;

namespace JARE.metaheuristics.singleObjective.geneticAlgorithm
{
    public class PortfolioOptimizationGAMultiThread : PortfolioOptimizationGA
    {

        //Execution via Binder
        EvaluationPool.EvaluationPoolMultithread EPM;
        System.IO.StreamWriter sw;
		
		/// <summary> 
		/// Constructor
        /// Create a new PortfolioOptimizationGAMultiThread instance.
		/// </summary>
		/// <param name="problem">Problem to solve.
		/// </param>

        public PortfolioOptimizationGAMultiThread(Problem problem)
            : base(problem)
		{
            EPM = new EvaluationPool.EvaluationPoolMultithread(problem, problem.NumberOfVariables);
		} // PortfolioOptimizationGAMultiThread
		
		/// <summary> Execute the GGA algorithm</summary>
		/// <throws>  SMException  </throws>

        public override void Init()
        {
            //InitEvaluationPoolForBinder();
            base.Init();
        }
        
        public override SolutionSet execute()
        {
            EPM.Start();
            SolutionSet population = base.execute();

            if (EPM != null) EPM.Stop();

            return population;
        }

        public override void Evaluate(SolutionSet population)
        {
            for (int i = 0; i < population.size(); i++)
            {
                EPM.SendSolution(population.m_solutionsList[i]);
            } //for
            EPM.Wait();
        }
    }
}

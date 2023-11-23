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
    public class PortfolioOptimizationGAViaBinder : PortfolioOptimizationGA
    {

        //Execution via Binder
        EvaluationPool.EvaluationPoolForBinderTPL epw;
        System.IO.StreamWriter sw;
		
		/// <summary> 
		/// Constructor
		/// Create a new GA instance.
		/// </summary>
		/// <param name="problem">Problem to solve.
		/// </param>

        public PortfolioOptimizationGAViaBinder(Problem problem):base(problem)
		{
			//this.m_problem = problem;
		} // GGA
		
		/// <summary> Execute the GGA algorithm</summary>
		/// <throws>  SMException  </throws>

        public override void Init()
        {
            base.Init();
        }
        
        public override SolutionSet execute()
        {
            InitEvaluationPoolForBinder();

            epw.Start();

            SolutionSet population = base.execute();

            if (epw != null) epw.Stop();

            return population;
        }

        public override void Evaluate(SolutionSet population)
        {
            for (int i = 0; i < population.size(); i++)
            {
                epw.SendSolution(population.m_solutionsList[i]);
            } //for   
            epw.Wait();
        }

        void InitEvaluationPoolForBinder()
        {
            EvaluationPool.IWBEvaluation p;
            p = m_problem as EvaluationPool.IWBEvaluation;

            string script = (string)getInputParameter("binderScript");
            string dir = (string)getInputParameter("binderDir");
            string varFile = (string)getInputParameter("binderVarFile");
            int procTimeout = (int)getInputParameter("binderProcTimeout");

            string LogFilesDir = (string)getInputParameter("binderLogFilesDir");

            if (p != null)
            {
                epw = new JARE.EvaluationPool.EvaluationPoolForBinderTPL(p, script, dir, varFile, procTimeout, varFile);
            }
            else
            {
                throw new Exception("This problem can not be solved via Binder");
            }

            sw = new System.IO.StreamWriter(String.Format("{0}\\times_{1}_{2}_{3}_{4}_{5}.txt", LogFilesDir, DateTime.Now.Year,
            DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute));
        }

    }
}

/// <summary> Poloni.java
/// 
/// </summary>
/// <author>  Antonio J. Nebro
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using BinaryRealSolutionType = JARE.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = JARE.Base.solutionType.RealSolutionType;
using SMException = JARE.util.SMException;


namespace JARE.problems
{
	/// <summary> Class representing problem Poloni. This problem has two objectives to be
	/// MAXIMIZED. As JARE always minimizes, the rule Max(f(x)) = -Min(f(-x)) must
	/// be applied.
	/// </summary>
	[Serializable]
	public class Poloni:Problem
	{
		
		/// <summary> Constructor.
		/// Creates a default instance of the Poloni problem
		/// </summary>
		/// <param name="solutionType">The solution type must "Real" or "BinaryReal".
		/// </param>
		public Poloni(System.String solutionType)
		{
			m_numberOfVariables = 2;
			m_numberOfObjectives = 2;
			m_numberOfConstraints = 0;
			m_problemName = "Poloni";
			
			m_lowerLimit = new double[m_numberOfVariables];
			m_upperLimit = new double[m_numberOfVariables];
			for (int var = 0; var < m_numberOfVariables; var++)
			{
				m_lowerLimit[var] = (- 1) * System.Math.PI;
				m_upperLimit[var] = System.Math.PI;
			} //for
			
			if (String.CompareOrdinal(solutionType, "BinaryReal") == 0)
				m_solutionType = new BinaryRealSolutionType(this);
			else if (String.CompareOrdinal(solutionType, "Real") == 0)
				m_solutionType = new RealSolutionType(this);
			else
			{
				System.Console.Out.WriteLine("Error: solution type " + solutionType + " invalid");
				System.Environment.Exit(- 1);
			}
		} //Poloni

        // evaluacija se obavlja koriscenjem web sevisa
        //public override void evaluate(Solution solution)
        //{

        //    Variable[] decisionVariables = solution.DecisionVariables;

        //    double[] x = new double[m_numberOfVariables];
        //    double[] f = new double[m_numberOfObjectives];

        //    x[0] = decisionVariables[0].getValue();
        //    x[1] = decisionVariables[1].getValue();

        //    f = WCFServiceClient.Client.RunWCFService(x[0], x[1]);

        //    solution.setObjective(0, (-1) * f[0]);
        //    solution.setObjective(1, (-1) * f[1]);


        //} // evaluate

        //evaluacija se obavlja pozivanjem spoljnog programa 
        //public override void evaluate(Solution solution)
        //{
        //    System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\Temp\\In.txt");
            
        //    Variable[] decisionVariables = solution.DecisionVariables;

        //    double[] x = new double[m_numberOfVariables];
        //    double[] f = new double[m_numberOfObjectives];

        //    x[0] = decisionVariables[0].getValue();
        //    x[1] = decisionVariables[1].getValue();

        //    sw.WriteLine(x[0]);
        //    sw.WriteLine(x[1]);
        //    sw.Close();

        //    System.Diagnostics.Process p = new System.Diagnostics.Process();
        //    p.StartInfo.FileName = "D:\\Temp\\PoloniTestFunction.exe";
           
           
        //    p.Start();
        //    p.WaitForExit();

          
        //    System.IO.StreamReader sr = new System.IO.StreamReader("D:\\Temp\\Out.txt");

        //    f[0] = double.Parse(sr.ReadLine());
        //    f[1] = double.Parse(sr.ReadLine());

        //    // The two objectives to be minimized. According to Max(f(x)) = -Min(f(-x)), 
        //    // they must be multiplied by -1. Consequently, the obtained solutions must
        //    // be also multiplied by -1 

        //    solution.setObjective(0, (-1) * f[0]);
        //    solution.setObjective(1, (-1) * f[1]);

            
        //    sr.Close();
        //} // evaluate

		/// <summary> Evaluates a solution </summary>
		/// <param name="solution">The solution to evaluate
		/// </param>
		/// <throws>  SMException  </throws>
        public override void evaluate(Solution solution)
        {
            //UPGRADE_NOTE: Final was removed from the declaration of 'A1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            double A1 = 0.5 * System.Math.Sin(1.0) - 2.0 * System.Math.Cos(1.0) + System.Math.Sin(2.0) - 1.5 * System.Math.Cos(2.0); //!< Constant A1
            //UPGRADE_NOTE: Final was removed from the declaration of 'A2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            double A2 = 1.5 * System.Math.Sin(1.0) - System.Math.Cos(1.0) + 2.0 * System.Math.Sin(2.0) - 0.5 * System.Math.Cos(2.0); //!< Constant A2

            Variable[] decisionVariables = solution.DecisionVariables;

            double[] x = new double[m_numberOfVariables];
            double[] f = new double[m_numberOfObjectives];

            x[0] = decisionVariables[0].getValue();
            x[1] = decisionVariables[1].getValue();

            double B1 = 0.5 * System.Math.Sin(x[0]) - 2.0 * System.Math.Cos(x[0]) + System.Math.Sin(x[1]) - 1.5 * System.Math.Cos(x[1]);
            double B2 = 1.5 * System.Math.Sin(x[0]) - System.Math.Cos(x[0]) + 2.0 * System.Math.Sin(x[1]) - 0.5 * System.Math.Cos(x[1]);

            f[0] = -(1 + System.Math.Pow(A1 - B1, 2) + System.Math.Pow(A2 - B2, 2));
            f[1] = -(System.Math.Pow(x[0] + 3, 2) + System.Math.Pow(x[1] + 1, 2));

            // The two objectives to be minimized. According to Max(f(x)) = -Min(f(-x)), 
            // they must be multiplied by -1. Consequently, the obtained solutions must
            // be also multiplied by -1 

            solution.setObjective(0, (-1) * f[0]);
            solution.setObjective(1, (-1) * f[1]);
        } // evaluate
	} // Poloni
}
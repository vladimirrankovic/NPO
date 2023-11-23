using System;
using System.Collections.Generic;
using System.Text;
using SharpMetal.Base;
using System.Configuration;

using BinaryRealSolutionType = SharpMetal.Base.solutionType.BinaryRealSolutionType;
using RealSolutionType = SharpMetal.Base.solutionType.RealSolutionType;

namespace SharpMetal.problems
{
    public class ExternalProgram : External
    {
        public ExternalProgram(Problem problem):base(problem)
        {
            
        }

        public override double[] Execute(double[] x, int numberOfObjectives, string appPath)
        {
            string filesLocation = System.Configuration.ConfigurationManager.AppSettings.GetValues("LokacijaFileova")[0];
            string inputFile = String.Format(filesLocation + "In.txt");
            string outputFile = String.Format(filesLocation + "Out.txt");

            System.IO.StreamWriter sw = new System.IO.StreamWriter(inputFile);

            for (int i = 0; i < x.Length; i++)
            {
                sw.WriteLine(x[i]);
            }
            sw.Close();
            //Console.WriteLine("Start");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = appPath;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.WaitForExit();

            System.IO.StreamReader sr = new System.IO.StreamReader(outputFile);

            double[] f = new double[numberOfObjectives];

            for (int i = 0; i < numberOfObjectives; i++)
            {
                f[i] = double.Parse(sr.ReadLine());
            }

            sr.Close();

            return f;
        }
    }
}

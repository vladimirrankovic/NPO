using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMetal.EvaluationPool
{
    public class JobParameters
    {
        double[] par;

        public JobParameters(int numOfParams)
        {
            par = new double[numOfParams];
        }

        public void ToJobParameters(double[] p)
        {
            p.CopyTo(this.par, 0);
        }
    }
}

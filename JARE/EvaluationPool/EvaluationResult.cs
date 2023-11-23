using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JARE.EvaluationPool
{
    public enum EvaluationStatus
    {
        DONE,
        FAILED,
        NO_READY_JOBS
    }

    public class EvaluationResult
    {
        private string MessageField;
        private double[] ResultField;
        public EvaluationStatus Status { get; set; } 

        public string Message
        {
            get
            {
                return this.MessageField;
            }
            set
            {
                this.MessageField = value;
            }
        }

        public double[] Result
        {
            get
            {
                return this.ResultField;
            }
            set
            {
                this.ResultField = value;
            }
        }
    }
}

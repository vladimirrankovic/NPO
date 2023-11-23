using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JARE.util
{
    public class TSValue
    {
        public static readonly double NO_DATA_VALUE = -9999.0;

        public bool HasValue { get; private set; }

        public double Value { get; private set; }

        public double Accuracy { get; private set; }

        public DateTime Time { get; private set; }

        public TSValue(DateTime dt, double value, double accuracy)
        {
            Time = dt;
            Value = value;
            Accuracy = accuracy;
            HasValue = true;
        }

        public TSValue(DateTime dt)
        {
            Time = dt;
            HasValue = false;
        }
    }
}

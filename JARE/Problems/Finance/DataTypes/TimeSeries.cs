using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.problems.Finance;

namespace JARE.problems.Finance.DataTypes
{
    public class DayValue
    {
        public DateTime Day;
        public double Value;
        public DayValue() { }
        public DayValue(DateTime day, double value)
        {
            Day = day;
            Value = value;
        }
    }
    public class TimeSeries
    {
        public string name = "";


        public System.Collections.Generic.List<DayValue> DataSeries = new System.Collections.Generic.List<DayValue>();
        public TimeSeries(TimeSeries other)
        {
            name = other.name;
            foreach (DayValue tmp in other.DataSeries) DataSeries.Add(tmp);
        }
        public TimeSeries()
        {
        }
        public bool GetValueOnDay(DateTime Day, ref double value)
        {
            bool found = false;
            //double value = 0.0;
            //value = DataSerie.Where(t => t.Day == Day).ElementAt(0).Value;

            foreach (DayValue tmp in DataSeries)
            {
                if (tmp.Day.Equals(Day))
                {
                    value = tmp.Value;
                    found = true;
                    break;
                }
            }
            return found;
        }
    }
}

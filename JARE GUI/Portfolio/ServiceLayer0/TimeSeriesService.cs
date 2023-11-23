using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using POUI.DataTypes;
using JARE.problems.Finance;
using JARE.problems.Finance.DataTypes;

namespace POUI.ServiceLayer
{
    public class TimeSeriesService
    {
        public static TimeSeriesSet LoadTimeSeriesFromCsv(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string line = String.Empty;
            bool hasHeaders = false;
            char splitter = ';';
            int cardinality = 0;
            string[] words = null;
            
            // init local variables
            if ((line = reader.ReadLine()) != null)
            {
                words = line.Split(splitter);
                if (words.Length == 1)
                {
                    splitter = ',';
                    words = line.Split(splitter);
                }

                double d;
                hasHeaders = !double.TryParse(words[1], out d);
                cardinality = words.Length - 1;
            }

            // create timeSeries list based on cardinality
            TimeSeriesSet timeSeriesSet = new TimeSeriesSet(cardinality, hasHeaders, words[0]);

            // if hasHeaders then set names, else set points to time series
            if (hasHeaders)
            {
                for (int i = 0; i < timeSeriesSet.TimeSeriesList.Count; i++)
                {
                    timeSeriesSet.TimeSeriesList[i].name = words[i + 1];
                }
            }
            else
            {
                AddPoints(words, timeSeriesSet);
            }

            // read rest of data
            while ((line = reader.ReadLine()) != null)
            {
                words = line.Split(splitter);
                AddPoints(words, timeSeriesSet);
            }

            reader.Close();

            return timeSeriesSet;
        }

        public static void DumpTimeSeriesToCsv(TimeSeriesSet timeSeriesSet, string fileName, int startIndex, int evaluationPeriod, bool writeHeaders, char splitter)
        {
            StreamWriter writer = new StreamWriter(fileName);

            /*
            JARE.problems.TimeSerie.DayValue dayValue = (timeSeriesSet.TimeSeriesList[0].DataSerie.Where(t => t.Day == startDate)).ElementAt(0);
            int indexOf = timeSeriesSet.TimeSeriesList[0].DataSerie.IndexOf(dayValue);
            int startIndex = indexOf - evaluationPeriod + 1;
            */

            if (writeHeaders)
            {
                string headers = timeSeriesSet.DateColumnName;
                for (int i = 0; i < timeSeriesSet.TimeSeriesList.Count; i++)
                {
                    headers = headers + splitter.ToString() + timeSeriesSet.TimeSeriesList[i].name;
                }

                writer.WriteLine(headers);
            }

            for (int i = 0; i < evaluationPeriod; i++)
            {
                string line = timeSeriesSet.TimeSeriesList[0].DataSerie[startIndex + i].Day.ToString("d");

                for (int j = 0; j < timeSeriesSet.TimeSeriesList.Count; j++)
                {
                    line = line + splitter.ToString() + timeSeriesSet.TimeSeriesList[j].DataSerie[startIndex + i].Value.ToString();
                }

                writer.WriteLine(line);
            }

            writer.Close();
        }

        private static void AddPoints(string[] words, TimeSeriesSet timeSeriesSet)
        {
            for(int i = 0; i < timeSeriesSet.TimeSeriesList.Count; i++)
            {
                JARE.problems.Finance.DataTypes.TimeSerie.DayValue dayValue = new TimeSerie.DayValue();
                dayValue.Day = DateTime.Parse(words[0]);
                dayValue.Value = double.Parse(words[i + 1]);
                timeSeriesSet.TimeSeriesList[i].DataSerie.Add(dayValue);
            }
        }
    }
}

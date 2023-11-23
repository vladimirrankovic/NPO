using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JARE.util
{
    public class TimeSeries
    {
        public enum TimeUnit
        {
            HOUR,
            DAY,
            MONTH,
        }

        private List<TSValue> m_Values = new List<TSValue>();

        

        public TimeSeries()
        {

        }

        public void AddValue(DateTime dt)
        {
            m_Values.Add(new TSValue(dt));
        }

        public void AddValue(DateTime dt, double value, double accuracy)
        {
            m_Values.Add(new TSValue(dt, value, accuracy));
        }

        internal TSValue GetValue(int index)
        {
            return m_Values[index];
        }

        public void Clear()
        {
            m_Values.Clear();
        }

        public double GetPointValue(int index)
        {
            return m_Values[index].Value;
        }

        public DateTime GetPointTime(int index)
        {
            return m_Values[index].Time;
        }

        public int Count()
        {
            return m_Values.Count;
        }

        public bool GetY(System.DateTime time, out double y)
        {
            int newIndex = 0;
            return GetY(time, out y, 0, out newIndex);
        }

        // Calculates Y corresponding to specified X
        public bool GetY(System.DateTime time, out double y, int index, out int newIndex)
        {
            int i;
            int nPointCount;
            TSValue p1, p2;

            y = 0.0;

            nPointCount = Count();
            newIndex = 0;

            if (nPointCount > 1)
            {
                //if (time <= GetValue(0).Time)
                //{
                //    if (Extrapolation == ExtrapolationMethod.Constant)
                //    {
                //        y = GetValue(0).Y.Value;
                //        return true;
                //    }
                //    else if (Extrapolation == ExtrapolationMethod.Tangent)
                //    {
                //        if (nPointCount > 1)
                //        {
                //            p1 = GetValue(0);
                //            p2 = GetValue(1);
                //            y = (time.Ticks - p1.X.Value.Ticks) * (p2.Y.Value - p1.Y.Value) / (p2.X.Value.Ticks - p1.X.Value.Ticks) + p1.Y.Value;
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                //}

                if (time >= GetValue(nPointCount - 1).Time)
                {
                    //if (Extrapolation == ExtrapolationMethod.Constant)
                    //{
                    //    y = GetValue(nPointCount - 1).Value;
                    //    return true;
                    //}
                    //else if (Extrapolation == ExtrapolationMethod.Tangent)
                    //{
                        if (nPointCount > 1)
                        {
                            p1 = GetValue(nPointCount - 2);
                            p2 = GetValue(nPointCount - 1);
                            y = (time.Ticks - p1.Time.Ticks) * (p2.Value - p1.Value) / (p2.Time.Ticks - p1.Time.Ticks) + p1.Value;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    //}
                }

                p1 = GetValue(index);
                for (i = index + 1; i < nPointCount; i++)
                {
                    //Worker.inCount++;

                    p2 = GetValue(i);
                    if ((time >= p1.Time && time <= p2.Time) || (time >= p2.Time && time <= p1.Time))
                    {
                        //if (Interpolation == InterpolationMethod.Linear)
                        //{

                        y = Convert.ToDouble(time.Ticks - p1.Time.Ticks) * (p2.Value - p1.Value) / (p2.Time.Ticks - p1.Time.Ticks) + p1.Value;

                        //}
                        //else
                        //{
                        //    y = p1.Y.Value;
                        //}

                        if (i > 0)
                            newIndex = i - 1;
                        else
                            newIndex = 0;

                        return true;
                    }

                    p1 = p2;
                }
            }

            y = GetValue(0).Value;
            newIndex = 0;

            return false;

        }

        public void toFEQ(String path)
        {
            String tabID = Path.GetFileNameWithoutExtension(path);
            toFEQ(path, tabID);
        }

        public void toFEQ(String path, String tabID)
        {
            StreamWriter sw = new StreamWriter(path);

            sw.WriteLine("TABID=" + tabID);
            sw.WriteLine("TYPE=   -7");
            sw.WriteLine("REFL=0.0        FAC=1.0");
            sw.WriteLine("YEAR MN DY      HOUR     LEVEL  INFLOW HYDROGRAPH AT UPSTREAM END");

            DateTime dt;

            for (int i = 0; i < this.Count(); i++)
            {
                dt = this.GetValue(i).Time;

                double hour = dt.Hour + (dt.Minute / 60.0);

                sw.Write(String.Format("{0,4}", dt.Year));
                sw.Write(String.Format("{0,3}", dt.Month));
                sw.Write(String.Format("{0,3}", dt.Day));
                sw.Write(" {0,9}", String.Format("{0:0.000000}", hour));
                sw.WriteLine(" {0,9}", String.Format("{0:0.00}", this.GetValue(i).Value));
            }

            dt = this.GetValue(this.Count() - 1).Time;
            sw.Write("{0,4}", dt.Year);
            sw.Write("{0,3}", dt.Month);
            sw.Write("{0,3}", dt.Day);
            sw.Write("{0,10}", String.Format("{0:0.00000}", dt.Hour + (dt.Minute / 60.0)));

            sw.Close();
        }

        public static TimeSeries fromFEQ(String path)
        {
            TimeSeries ts = new TimeSeries();
            String line = "";
            try
            {
                StreamReader sr = new StreamReader(path);

                //for (int i = 0; i < 3; i++)
                //    sr.ReadLine();

                line = sr.ReadLine();

                while (!Char.IsNumber(line[0]))
                    line = sr.ReadLine();

                DateTime lastDate = new DateTime(1, 1, 1);

                while (line != null)
                {
                    String[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    int year = Int32.Parse(parts[0]);
                    int month = Int32.Parse(parts[1]);
                    int day = Int32.Parse(parts[2]);

                    string temps = parts[3];
                    double hourMin = Double.Parse(parts[3]);
                    int hour = (int)(Math.Floor(hourMin));
                    int min = (int)(Math.Floor(60 * (hourMin - hour) + 0.5));

                    //int year = Int32.Parse(line.Substring(0, 4));
                    //int month = Int32.Parse(line.Substring(4, 3));
                    //int day = Int32.Parse(line.Substring(8, 2));

                    //string temps = line.Substring(10, 10);
                    //double hourMin = Double.Parse(line.Substring(10, 10));
                    //int hour = (int)(Math.Floor(hourMin));
                    //int min = (int)(Math.Floor(60 * (hourMin - hour) + 0.5));

                    DateTime dt;

                    if (hour < 0)
                        dt = new DateTime(year, month, day, 0, min, 0).AddHours(-1);
                    else
                        dt = new DateTime(year, month, day, hour, min, 0);

                    if (dt < lastDate || line.Count() - 21 < 0)
                        break;

                    //double value = Double.Parse(line.Substring(20, line.Count() - 21));
                    double value = Double.Parse(parts[4]);

                    lastDate = dt;

                    ts.AddValue(dt, value, 0.0);

                    line = sr.ReadLine();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "_____ loadPrescribedValues(String path) ____ " + path + " LINE: " + line + " ");
            }

            return ts;
        }

        public void fromCSV(String path)
        {
            String line = "";
            try
            {
                StreamReader sr = new StreamReader(path);
                string header = sr.ReadLine();

                while ((line = sr.ReadLine()) != null)
                {
                    String[] parts = line.Split(',');

                    DateTime dt = DateTime.Parse(parts[0]);
                    double value = double.Parse(parts[1]);

                    AddValue(dt, value, 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "_____ loadPrescribedValues(String path) ____ " + path + " LINE: " + line + " ");
            }
        }

        public void toCSV(String path)
        {
            String line = "";
            try
            {
                StreamWriter sw = new StreamWriter(path);


                string header = "Date, Value";
                sw.WriteLine(header);

                for(int i = 0; i < Count(); i++)
                {
                    string str = GetPointTime(i).ToString() + "," + GetPointValue(i).ToString();
                    sw.WriteLine(str);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "_____ Print to CSV (String path) ____ " + path + " LINE: " + line + " ");
            }
        }

        public static TimeSeries loadFromFEQBinary(String path)
        {
            TimeSeries ts = new TimeSeries();

            if (!File.Exists(path))
                return null;

            System.IO.BinaryReader reader = new System.IO.BinaryReader(System.IO.File.Open(path, System.IO.FileMode.Open));

            List<System.DateTime> dates = new List<DateTime>();
            dates.Add(new System.DateTime());
            dates.Add(new System.DateTime());

            float a;
            System.Int32 nType;
            double dTime;
            float fValue;
            int i = 0;

            a = reader.ReadSingle();
            nType = reader.ReadInt32();

            a = reader.ReadSingle();

            //TimeSpan dateDiff = new System.DateTime(1899, 12, 30, 0, 0, 0) - new System.DateTime(1858, 11, 17, 0, 0, 0);
            //int daysDiff = dateDiff.Days;

            int daysDiff = 15018;

            try
            {
                for (;;)
                {
                    a = reader.ReadSingle();

                    dTime = reader.ReadDouble() - daysDiff;
                    fValue = reader.ReadSingle();

                    if (i == 0)
                    {
                        dates[0] = System.DateTime.FromOADate(dTime);
                    }

                    DateTime tmmm = System.DateTime.FromOADate(dTime);

                    if (tmmm > dates[1])
                    {
                        dates[1] = tmmm;
                        ts.AddValue(tmmm, fValue, 1);
                    }

                    i++;
                    a = reader.ReadSingle();
                }
            }
            catch (System.IO.EndOfStreamException)
            {
                return ts;
                //throw new Exception(e.Message + "______loadFromBinary(" + path + ")");
            }
            finally
            {
                reader.Close();
            }
        }
    }
}

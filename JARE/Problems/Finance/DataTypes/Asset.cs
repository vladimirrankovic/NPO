using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JARE.problems.Finance;
using System.IO;


namespace JARE.problems.Finance.DataTypes
{

    public class Asset
    {
        public string name;
        public TimeSeries timeSeries;

        public Asset()
        {
            name = "None";
            timeSeries = new TimeSeries();
        }

        public Asset(string otherName, TimeSeries otherTimeSeries)
        {
            name = otherName;
            timeSeries = new TimeSeries(otherTimeSeries);
        }

        public Asset(Asset other)
        {
            name = other.name;
            timeSeries = new TimeSeries(other.timeSeries);
        }
    }

    public class Bond : Asset
    {
        public string ticker;
        public double price;
        public double stressedPrice;
        public double duration;
        public string rating;
        public double yieldToMaturity;

        public Bond()
            : base()
        {
        }

        public Bond(string Name, string Ticker, double Price, double StressedPrice, double Duration, string Rating, double YieldToMaturity)
            : base()
        {
            this.name = Name;
            ticker = Ticker;
            price = Price;
            stressedPrice = StressedPrice;
            duration = Duration;
            rating = Rating;
            yieldToMaturity = YieldToMaturity;
        }
        public Bond(Bond other)
            : base()
        {
            name = other.name;
            ticker = other.ticker;
            price = other.price;
            stressedPrice = other.stressedPrice;
            duration = other.duration;
            rating = other.rating;
            yieldToMaturity = other.yieldToMaturity;
        }

    }

    public class BondSet
    {
        private List<Bond> BondList;
        private int m_ID;


        public BondSet(int ID, List<Bond> Bonds)
        {
            m_ID = ID;
            BondList = new List<Bond>(Bonds.Count);
            for (int i = 0; i < Bonds.Count; i++)
            {
                BondList.Add(new Bond(Bonds[i]));
            }
        }

        public Bond GetItem(int index)
        {
            return BondList[index];
        }

        public int ID
        {
            set { m_ID = value; }
            get { return m_ID; }
        }
        
        public int Count
        {
            get { return BondList.Count; }
        }


    }

    public class AssetSet
    {
        private List<Asset> AssetList;
        //private bool hasHeaders;
        //private string dateColumnName;
        private int m_ID;
        private bool validated;

        public int ID
        {
            set { m_ID = value; }
            get { return m_ID; }
        }

        public bool Validated
        {
            set { validated = value; }
            get { return validated; }
        }

        public List<Asset> assetList
        {
            get { return AssetList; }
        }

        //public bool HasHeaders
        //{
        //    get { return hasHeaders; }
        //}

        //public string DateColumnName
        //{
        //    get { return dateColumnName; }
        //}

        public int Count
        {
            get { return AssetList.Count; }
        }

        public DayValue GetDayValue(int timeSerieIndex, int index)
        {
            return AssetList[timeSerieIndex].timeSeries.DataSeries[index];
        }

        public DateTime GetDay(int index)
        {
            return GetDayValue(0, index).Day;
        }

        public TimeSeries GetAssetTimeSeries(int assetIndex)
        {
            return AssetList[assetIndex].timeSeries;
        }

        public AssetSet(int ID, int cardinality/*, bool hasHeaders, string dateColumnName*/)
        {
            m_ID = ID;
            AssetList = new List<Asset>(cardinality);
            for (int i = 0; i < cardinality; i++)
            {
                AssetList.Add(new Asset());
            }
            //this.hasHeaders = hasHeaders;
            //this.dateColumnName = dateColumnName;
            validated = false;
        }

        public AssetSet( int ID, List<Asset> Assets/*, bool hasHeaders, string dateColumnName*/)
        {
            m_ID = ID;
            AssetList = new List<Asset>(Assets.Count);
            for (int i = 0; i < Assets.Count; i++)
            {
                AssetList.Add(new Asset(Assets[i]));
            }
            //this.hasHeaders = hasHeaders;
            //this.dateColumnName = dateColumnName;
            validated = false;
        }
        public void DumpToCsv(string fileName, DateTime Date, int numberOfRows, bool writeHeaders, char splitter)
        {
            int DateIndex;
            GetItemIndex(Date, out DateIndex);

            StreamWriter writer = new StreamWriter(fileName);

            if (writeHeaders)
            {
                //string headers = dateColumnName;
                string headers = string.Empty;
                for (int i = 0; i < AssetList.Count; i++)
                {
                    headers = headers + splitter.ToString() + assetList[i].name;
                }

                writer.WriteLine(headers);
            }

            for (int i = 1; i <= numberOfRows; i++)
            {
                string line = assetList[0].timeSeries.DataSeries[DateIndex - numberOfRows + i].Day.ToString("d");

                for (int j = 0; j < assetList.Count; j++)
                {
                    line = line + splitter.ToString() + assetList[j].timeSeries.DataSeries[DateIndex - numberOfRows + i].Value.ToString();
                }

                writer.WriteLine(line);
            }

            writer.Close();
        }
        //public void DumpToCsv(string fileName, int startIndex, int numberOfRows, bool writeHeaders, char splitter)
        //{
        //    StreamWriter writer = new StreamWriter(fileName);

        //    if (writeHeaders)
        //    {
        //        string headers = dateColumnName;
        //        for (int i = 0; i < timeSeriesList.Count; i++)
        //        {
        //            headers = headers + splitter.ToString() + TimeSeriesList[i].name;
        //        }

        //        writer.WriteLine(headers);
        //    }

        //    for (int i = 0; i < numberOfRows; i++)
        //    {
        //        string line = TimeSeriesList[0].DataSerie[startIndex + i].Day.ToString("d");

        //        for (int j = 0; j < TimeSeriesList.Count; j++)
        //        {
        //            line = line + splitter.ToString() + TimeSeriesList[j].DataSerie[startIndex + i].Value.ToString();
        //        }

        //        writer.WriteLine(line);
        //    }

        //    writer.Close();
        //}

        public bool GetItemIndex(DateTime Date, out int index)
        {
            index = -1;
            bool found = false;
            for (int i = 0; i < assetList[0].timeSeries.DataSeries.Count; i++)
            {
                if (Date == assetList[0].timeSeries.DataSeries[i].Day)
                {
                    index = i;
                    found = true;
                    break;
                }
            }
            return found;
        }

        public bool GetValues(DateTime Date, double[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (!assetList[i].timeSeries.GetValueOnDay(Date, ref values[i])) return false;
            }
            return true;
        }

        public void DataValidation(DateTime Date)
        {
            int DateIndex;
            if (!GetItemIndex(Date, out DateIndex)) throw new Exception("Asset values not found for specified date");
        }
 
    }

    public class AssetSets
    {
        private Dictionary<int, AssetSet> AssetSetMap;

        public AssetSets()
        {
            AssetSetMap = new Dictionary<int, AssetSet>();
        }

        public void Add(AssetSet timeSeriesSet)
        {
            if (IsExist(timeSeriesSet.ID)) AssetSetMap.Remove(timeSeriesSet.ID);
            AssetSetMap.Add(timeSeriesSet.ID, timeSeriesSet);
        }

        public AssetSet GetAt(int Index)
        {
            return AssetSetMap.ElementAt(Index).Value;
        }

        public bool IsExist(int ID)
        {
            return AssetSetMap.ContainsKey(ID);
        }

        public AssetSet Get(int ID)
        {
            AssetSet tSS;
            try
            {
                AssetSetMap.TryGetValue(ID, out tSS);
                return tSS;
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("There is no TimeSeriesSet with specified ID");
            }
        }

        public int GetCount()
        {
            return AssetSetMap.Count;
        }
    }

}

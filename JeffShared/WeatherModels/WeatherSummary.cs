using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.WeatherModels
{
    public class WeatherSummary
    {
        public string City { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
        public double AvgTemp { get; set; }
        public double MaxWind { get; set; }
        public double MinWind { get; set; }
        public double AvgWind { get; set; }
        public List<DailySummary> DailySummaries { get; set; }
    }

    public class DailySummary
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }
        public double AvgTemp { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
    }  
}

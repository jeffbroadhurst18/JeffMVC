using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.WeatherModels
{
    public class MinMaxTemps
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public virtual Cities City { get; set; }
    }
}

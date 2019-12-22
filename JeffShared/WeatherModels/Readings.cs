using System;
using System.Collections.Generic;

namespace JeffShared.WeatherModels
{
    public partial class Readings
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public DateTime CurrentTime { get; set; }
        public string MainWeather { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public double Wind { get; set; }
        public string Direction { get; set; }
        public string Icon { get; set; }

        public virtual Cities City { get; set; }
    }
}

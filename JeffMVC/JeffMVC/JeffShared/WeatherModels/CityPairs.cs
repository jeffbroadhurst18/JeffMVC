using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.WeatherModels
{
    public class CityPairs
    {
        public int Id { get; set; }

        public int City1Id { get; set; }
        public string City1Name { get; set; }
        public int City2Id { get; set; }
        public string City2Name { get; set; }
    }
}

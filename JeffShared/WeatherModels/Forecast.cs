using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.WeatherModels
{
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Rain
    {
        [JsonProperty("1h")]
        public double _1h { get; set; }
    }

    public class Hourly
    {
        public int dt { get; set; }
        public DateTime dtDate {get; set;}
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public List<MiniForecast> weather { get; set; }
        public double pop { get; set; }
        public Rain rain { get; set; }
    }

    public class MiniForecast
    {
        public int id { get; set; }
        public string main { get; set; }
        public string  description { get; set; }
        public string icon { get; set; }
    }

    public class Forecast
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public List<Hourly> hourly { get; set; }
    }
}
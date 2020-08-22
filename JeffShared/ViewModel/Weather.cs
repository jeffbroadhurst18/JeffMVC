using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.ViewModel
{
	public class Weather
	{
		public string Name { get; set; }
		public string MainWeather { get; set; }
		public string Description { get; set; }
		public double Temperature { get; set; }
		public double Wind { get; set; }
		public string Direction { get; set; }
		public string Icon { get; set; }
		public DateTime Sunrise { get; set; }
		public DateTime Sunset { get; set; }
		public DateTime CurrentTime { get; set; }
		public WeatherParameters Query { get; set; }
		public TemperatureDay AnnualMin { get; set; }
		public TemperatureDay AnnualMax { get; set; }
		public TemperatureDay MonthlyMin { get; set; }
		public TemperatureDay MonthlyMax { get; set; }

	}

	public class TemperatureDay
	{
		public DateTime TempDate { get; set; }
		public double TempVal { get; set; }
	}

	public class WeatherParameters
	{
		public string Name { get; set; }
		public string Country { get; set; }
		public int TimeLag { get; set; }
	}
}

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
		public string Query { get; set; }
	}
}

using JeffShared.ViewModel;
using JeffShared.WeatherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeffShared
{
	public class WeatherService : IWeatherService
	{

		private IWeatherApiClient _client;

		public WeatherService(IWeatherApiClient client)
		{
			_client = client;
		}

		public async Task<WeatherRootObject> GetCurrent(string id)
		{
			return await _client.GetWeatherAsync(id);
		}

		public async Task<FormattedForecast> GetForecastAsync(double lat, double lon)
        {
			var raw = await _client.GetForecastAsync(lat,lon);
			var formatted = new FormattedForecast
			{
				lat = raw.lat,
				lon = raw.lon,
				timezone = raw.timezone,
				timezone_offset = raw.timezone_offset,
				hourly = GetFormattedHourly(raw.hourly)
			};
			return formatted;
        }

        private List<FormattedHourly> GetFormattedHourly(List<Hourly> hourly)
        {
			var outputList = new List<FormattedHourly>();
			hourly.ForEach(h =>
			{
				var nicer = new FormattedHourly
				{
					 dtDate = h.dtDate,
					 feels_like = h.feels_like,
					 temp = h.temp,
					 weather = h.weather.FirstOrDefault(),
					 direction = GetDirectionText(h.wind_deg),
					 wind = h.wind_speed 
				};
				outputList.Add(nicer);
			});
			return outputList;
        }

        private string GetDirectionText(double deg)
		{
			var textArray = new string[] { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
			var fig = deg % 360;
			var index = Math.Round(fig / 22.5, 0) + 1;
			if (index > 16) { index = 16; }
			return textArray[(int)index];
		}
	}
}

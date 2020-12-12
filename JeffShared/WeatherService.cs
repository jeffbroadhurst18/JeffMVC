using JeffShared.ViewModel;
using JeffShared.WeatherModels;
using System;
using System.Collections.Generic;
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

		public async Task<Forecast> GetForecastAsync(double lat, double lon)
        {
			return await _client.GetForecastAsync(lat,lon);
        }
	}
}

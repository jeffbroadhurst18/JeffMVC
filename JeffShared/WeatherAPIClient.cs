using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using JeffShared.ViewModel;
using JeffShared.WeatherModels;

namespace JeffShared
{
	public class WeatherApiClient : IWeatherApiClient
	{
		private readonly HttpClient _client;
		private IConfiguration _configuration;
		private string _baseUrl;
		private string _baseForecastUrl;
		private string _key;

		public WeatherApiClient(HttpClient client, IConfiguration configuration)
		{
			_client = client;
			_configuration = configuration;
			_baseUrl = _configuration.GetSection("Weather").GetSection("BaseURL").Value;
			_baseForecastUrl = _configuration.GetSection("Weather").GetSection("BaseForecastURL").Value;
			_key = _configuration.GetSection("Weather").GetSection("Key").Value;
		}

        public async Task<Forecast> GetForecastAsync(double lat,double lon)
        {
			var queryString = $"lat={lat}&lon={lon}&exclude=current,minutely,daily,alerts";
			var res = await _client.GetStringAsync($"{_baseForecastUrl}{queryString}&appid={_key}");
			var forecast = JsonConvert.DeserializeObject<Forecast>(res);
			return ConvertTheDate(forecast);
		}

        public async Task<WeatherRootObject> GetWeatherAsync(string id)
		{
			var encoded = WebUtility.UrlEncode(id);
			var forecast = JsonConvert.DeserializeObject<WeatherRootObject>(await _client.GetStringAsync($"{_baseUrl}{encoded}&appid={_key}"));
			return forecast;
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		public static Forecast ConvertTheDate(Forecast forecast)
        {
			foreach(var h in forecast.hourly)
            {
				h.dtDate = UnixTimeStampToDateTime(h.dt);
            }
			return forecast;
        }
	}

}

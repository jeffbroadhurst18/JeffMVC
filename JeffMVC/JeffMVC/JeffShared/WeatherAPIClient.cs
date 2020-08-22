using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace JeffShared
{
	public class WeatherApiClient : IWeatherApiClient
	{
		private readonly HttpClient _client;
		private IConfiguration _configuration;
		private string _baseUrl;
		private string _key;

		public WeatherApiClient(HttpClient client, IConfiguration configuration)
		{
			_client = client;
			_configuration = configuration;
			_baseUrl = _configuration.GetSection("Weather").GetSection("BaseURL").Value;
			_key = _configuration.GetSection("Weather").GetSection("Key").Value;
		}

		public async Task<WeatherRootObject> GetWeatherAsync(string id)
		{
			var encoded = WebUtility.UrlEncode(id);
			var forecast = JsonConvert.DeserializeObject<WeatherRootObject>(await _client.GetStringAsync($"{_baseUrl}{encoded}&appid={_key}"));
			return forecast;
		}
	}

}

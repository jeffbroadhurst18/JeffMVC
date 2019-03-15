using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JeffShared
{
	public class CountriesAPIClient : ICountriesAPIClient
	{
		private readonly HttpClient _client;
		private IConfiguration _configuration;
		private string _baseUrl;

		public CountriesAPIClient(HttpClient client, IConfiguration configuration)
		{
			_client = client;
			_configuration = configuration;
			_baseUrl = _configuration.GetSection("Countries").GetSection("BaseURL").Value;
		}

		public async Task<BaseCountry[]> GetCountries(string id)
		{
			var results = await _client.GetStringAsync($"{_baseUrl}/region/{id}");
			return JsonConvert.DeserializeObject<BaseCountry[]>(results);
		}

		public async Task<CapitalRaw[]> GetCapital(string id)
		{
			return JsonConvert.DeserializeObject<CapitalRaw[]>(await _client.GetStringAsync($"{_baseUrl}/capital/{id}"));
		}
	}

	public interface ICountriesAPIClient
	{
		Task<BaseCountry[]> GetCountries(string id);
		Task<CapitalRaw[]> GetCapital(string id);
	}
}

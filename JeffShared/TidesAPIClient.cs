using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JeffShared
{
	public class TidesApiClient : ITidesApiClient
	{
		private readonly HttpClient _client;
		private IConfiguration _configuration;
		private string _baseUrl;

		public TidesApiClient(HttpClient client, IConfiguration configuration)
		{
			_client = client;
			_configuration = configuration;
			_baseUrl = _configuration.GetSection("Admiralty").GetSection("BaseURL").Value;
		}

		public async Task<IndividualStation> GetStationAsync(string id)
		{
			var individualStation = JsonConvert.DeserializeObject<IndividualStation>(await _client.GetStringAsync($"{_baseUrl}/{id}"));
			return individualStation;
		}

		public async Task<StationData> GetStations()
		{
			return JsonConvert.DeserializeObject<StationData>(await _client.GetStringAsync(_baseUrl));
		}

		public Task<TideData[]> GetTideData(string id)
		{
			throw new NotImplementedException();
		}

		
	}

	public interface ITidesApiClient
	{
		Task<StationData> GetStations();
		Task<TideData[]> GetTideData(string id);
		Task<IndividualStation> GetStationAsync(string id);
	}
}

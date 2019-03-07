using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public async Task<Feature> GetStationAsync(string id)
		{
			var individualStation = JsonConvert.DeserializeObject<Feature>(await _client.GetStringAsync($"{_baseUrl}/{id}"));
			return individualStation;
		}

		public async Task<StationData> GetStations()
		{
			return JsonConvert.DeserializeObject<StationData>(await _client.GetStringAsync(_baseUrl));
		}

		public async Task<TideData[]> GetTideData(string id)
		{
			if (id == "0")
			{
				List<TideData> tideList = new List<TideData>
				{
					new TideData { EventType = "No data" }
				};

				return tideList.ToArray();
			}

			var url = $"{_baseUrl}/{id}/TidalEvents";
			var json = _client.GetStringAsync(url);
			var allTides = JsonConvert.DeserializeObject<TideData[]>(await json);
			return allTides.Skip(4).Take(8).ToArray();
		}

		
	}

	public interface ITidesApiClient
	{
		Task<StationData> GetStations();
		Task<TideData[]> GetTideData(string id);
		Task<Feature> GetStationAsync(string id);
	}
}

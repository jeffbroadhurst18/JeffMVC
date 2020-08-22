using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JeffShared
{
	public class TidesService : ITidesService
	{
		private ITidesApiClient _client;

		public TidesService(ITidesApiClient client)
		{
			_client = client;
		}

		public async Task<Feature> GetStation(string id)
		{
			return await _client.GetStationAsync(id);
		}

		public async Task<StationData> GetStations()
		{
			 return await _client.GetStations();
		}

		public async Task<TideData[]> GetTideData(string id)
		{
			return await _client.GetTideData(id);
		}

		
	}
}

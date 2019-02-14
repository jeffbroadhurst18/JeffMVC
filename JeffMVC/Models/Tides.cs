using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JeffMVC.Models
{
	public class TidesContext
	{
		private static readonly HttpClient client = new HttpClient();

		//private IEnumerable<TideData> _tideData = null;

		//public IEnumerable<TideData> TideData
		//{
		//	get => _tideData ?? (_tideData = GetTideData("0462").Result);
		//}

		private async Task<TideData[]> GetTideData(string loc)
		{
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "fbade2ef0cb5465f9311c724a5e75e01");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var url = $"https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/{loc}/TidalEvents";
			var json = client.GetStringAsync(url);
			return JsonConvert.DeserializeObject<TideData[]>(await json);
		}

		public async Task<IEnumerable<TideData>> GetTides(string id)
		{
			return await GetTideData(id);
		}
	}

	public class Geometry
	{
		public string type { get; set; }
		public List<double> coordinates { get; set; }
	}

	public class Properties
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }
		public bool ContinuousHeightsAvailable { get; set; }
		public string Footnote { get; set; }
	}

	public class Feature
	{
		public string type { get; set; }
		public Geometry geometry { get; set; }
		public Properties properties { get; set; }
	}

	public class Tide
	{
		public string type { get; set; }
		public List<Feature> features { get; set; }
	}

	public class TideData
	{
		public string EventType { get; set; }
		public DateTime DateTime { get; set; }
		public bool IsApproximateTime { get; set; }
		public bool IsApproximateHeight { get; set; }
		public bool Filtered { get; set; }
	}
}

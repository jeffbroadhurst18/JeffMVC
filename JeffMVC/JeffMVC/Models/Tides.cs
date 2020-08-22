using JeffMVC.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
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

		private IEnumerable<SelectListItem> _stationlist = null;
		private IEnumerable<TideData> _tides = null;
		private string _selectedstation = null;
		private List<double> _coords = null;
		public string selectedstation { get => _selectedstation ?? (_selectedstation = "0"); set => _selectedstation = value; }

		public List<double> coords
		{
			get => _coords ?? (_coords = GetCoords(selectedstation).Result.ToList());
		}

		public IEnumerable<SelectListItem> stationlist
		{
			get => _stationlist ?? (_stationlist = GetStations().Result);
		}

		public IEnumerable<TideData> tides
		{
			get => _tides ?? (_tides = GetTides(selectedstation).Result);
		}

		private async Task<TideData[]> GetTideData(string loc)
		{
			if (loc == "0") {
				List<TideData> tideList = new List<TideData>
				{
					new TideData { EventType = "No data" }
				};

				return tideList.ToArray();
			}

			SetHeader();

			var url = $"https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/{loc}/TidalEvents";
			var json = client.GetStringAsync(url);
			var allTides = JsonConvert.DeserializeObject<TideData[]>(await json);
			return allTides.Skip(4).Take(8).ToArray();
		}

		public async Task<IEnumerable<TideData>> GetTides(string id)
		{
			id = id ?? "0";
			var tideData = await GetTideData(id);
			foreach(var item in tideData)
			{
				item.EventType = item.EventType == "HighWater" ? "High Water" : "Low Water";
			}
			return tideData;
		}

		private async Task<IEnumerable<SelectListItem>> GetStations()
		{
			string selectedId = string.Empty;
			var stations = await GetStationData();
			var dictionary = new Dictionary<string, string>();

			foreach(var station in stations.features)
			{
				dictionary.Add(station.properties.Id, station.properties.Name);
			}

			return dictionary.ToSelectListItems(selectedId).OrderBy(p => p.Text);
			
		}

		private async Task<IEnumerable<double>> GetCoords(string selstat)
		{
			if (selstat == "0")
			{
				return new List<double>
				{
					0.00
				};
			}

			SetHeader();

			var stationData = JsonConvert.DeserializeObject<IndividualStation>(await client.GetStringAsync($"https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/{selstat}"));
			return stationData.geometry.coordinates;
		}

		private static void SetHeader()
		{
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "fbade2ef0cb5465f9311c724a5e75e01");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		private async Task<StationData> GetStationData()
		{
			SetHeader();

			return JsonConvert.DeserializeObject<StationData>(await client.GetStringAsync("https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations"));
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

	
	public class StationData
	{
		public string type { get; set; }
		public List<Feature> features { get; set; }
	}

	public class IndividualStation
	{
		public string type { get; set; }
		public Geometry geometry { get; set; }
		public Properties properties { get; set; }
	}
}


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TestHttp
{
	class Program
	{
		private static readonly HttpClient client = new HttpClient();

		static async Task Main(string[] args)
		{


			//var answer = await ProcessRepositories();
			var data = await GetSeaData();
			var tides = await GetTideData("0462");
		}

		private static async Task<RootObject> GetSeaData()
		{
			client.DefaultRequestHeaders.Accept.Clear();
			//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "fbade2ef0cb5465f9311c724a5e75e01");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


			var json = client.GetStringAsync("https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations");
			var y = await json;
			var x = JsonConvert.DeserializeObject<RootObject>(y);
			return x;
		}

		

			private static async Task<TideData[]> GetTideData(string loc)
		{
			client.DefaultRequestHeaders.Accept.Clear();
			//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "fbade2ef0cb5465f9311c724a5e75e01");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var url = $"https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/{loc}/TidalEvents";
			var json = client.GetStringAsync(url);
			var y = await json;
			var x = JsonConvert.DeserializeObject<TideData[]>(y);
			return x;
		}

		//private static async Task<List<Forecast[]>> ProcessRepositories()
		//{
		//	client.DefaultRequestHeaders.Accept.Clear();
		//	client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
		//	client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

		//	var stringTask = client.GetStringAsync("http://datapoint.metoffice.gov.uk/public/data/txt/wxfcs/regionalforecast/json/507?key=fb4c0ad5-6ba6-4068-bbcd-c5a2788e8265");

		//	var msg = await stringTask;
		//	Console.Write(msg);
		//	var x = JsonConvert.DeserializeObject<RootObject>(msg);

		//	var listperiods = x.RegionalFcst.FcstPeriods.Period;
		//	Forecast[] forecasts = new Forecast[] { new Forecast { f = string.Empty, title = string.Empty } };
		//	List<Forecast[]> forecastList = new List<Forecast[]>();

		//	var l = listperiods[0];
		//	Console.WriteLine(listperiods[0].Paragraph);
		//	var y = l.Paragraph.ToString().Replace('$', 'f');
		//	forecasts = JsonConvert.DeserializeObject<Forecast[]>(y);
		//	forecastList.Add(forecasts);

		//	return forecastList;
		//}

		public class Period
		{
			public string id { get; set; }
			public object Paragraph { get; set; }
		}

		public class FcstPeriods
		{
			public List<Period> Period { get; set; }
		}

		public class RegionalFcst
		{
			public DateTime createdOn { get; set; }
			public DateTime issuedAt { get; set; }
			public string regionId { get; set; }
			public FcstPeriods FcstPeriods { get; set; }
		}

		//public class RootObject
		//{
		//	public RegionalFcst RegionalFcst { get; set; }
		//}

		public class Forecast
		{
			public string title { get; set; }
			public string f { get; set; }
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

		public class RootObject
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


}


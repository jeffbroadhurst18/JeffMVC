
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
			//var data = await GetSeaData("0324");
			//var tides = await GetTideData("0462");
			var x = await GetWeatherForecast();
			Console.WriteLine(JsonConvert.SerializeObject(x));
			Console.ReadLine();
		}

		private static async Task<RootObject> GetSeaData(string loc)
		{
			client.DefaultRequestHeaders.Accept.Clear();
			//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "fbade2ef0cb5465f9311c724a5e75e01");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


			var json = client.GetStringAsync($"https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/{loc}");
			var y = await json;
			var x = JsonConvert.DeserializeObject<RootObject>(y);
			return x;
		}

		//private static async Task<WeatherRoot> GetWeather()
		//{
		//	client.DefaultRequestHeaders.Accept.Clear();
		//	//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
		//	client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "community-open-weather-map.p.rapidapi.com");
		//	client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "G7ZSpJuH8GmshvKucWNG8JS371tep1HfaAyjsntKv9zuCNVpfr");
		//	client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		//	var json = client.GetStringAsync($"https://community-open-weather-map.p.rapidapi.com/weather?id=2172797&units=%22metric%22+or+%22imperial%22&mode=xml%2C+html&q=London%2Cuk");
		//	//var y = await json;
		//	return JsonConvert.DeserializeObject<WeatherRoot>(await json);
		//	//var x = JsonConvert.DeserializeObject<RootObject>(y);
		//}

		private static async Task<ForecastRootObject> GetWeatherForecast()
		{
			client.DefaultRequestHeaders.Accept.Clear();
			//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
			client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "community-open-weather-map.p.rapidapi.com");
			client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "G7ZSpJuH8GmshvKucWNG8JS371tep1HfaAyjsntKv9zuCNVpfr");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var json = client.GetStringAsync($"https://community-open-weather-map.p.rapidapi.com/forecast?q=london%2Cuk");
			//var y = await json;
			return JsonConvert.DeserializeObject<ForecastRootObject>(await json);
			//var x = JsonConvert.DeserializeObject<RootObject>(y);
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

		//public class Coord
		//{
		//	public double lon { get; set; }
		//	public double lat { get; set; }
		//}

		//public class Weather
		//{
		//	public int id { get; set; }
		//	public string main { get; set; }
		//	public string description { get; set; }
		//	public string icon { get; set; }
		//}

		//public class MainWeather
		//{
		//	public double temp { get; set; }
		//	public int pressure { get; set; }
		//	public int humidity { get; set; }
		//	public double temp_min { get; set; }
		//	public double temp_max { get; set; }
		//}

		//public class Wind
		//{
		//	public double speed { get; set; }
		//	public int deg { get; set; }
		//}

		//public class Clouds
		//{
		//	public int all { get; set; }
		//}

		//public class Sys
		//{
		//	public int type { get; set; }
		//	public int id { get; set; }
		//	public double message { get; set; }
		//	public string country { get; set; }
		//	public int sunrise { get; set; }
		//	public int sunset { get; set; }
		//}

		//public class WeatherRoot
		//{
		//	public Coord coord { get; set; }
		//	public List<Weather> weather { get; set; }
		//	public string @base { get; set; }
		//	public MainWeather main { get; set; }
		//	public int visibility { get; set; }
		//	public Wind wind { get; set; }
		//	public Clouds clouds { get; set; }
		//	public int dt { get; set; }
		//	public Sys sys { get; set; }
		//	public int id { get; set; }
		//	public string name { get; set; }
		//	public int cod { get; set; }
		//}

		public class ForecastMain
		{
			public double temp { get; set; }
			public double temp_min { get; set; }
			public double temp_max { get; set; }
			public double pressure { get; set; }
			public double sea_level { get; set; }
			public double grnd_level { get; set; }
			public int humidity { get; set; }
			public double temp_kf { get; set; }
		}

		public class Weather
		{
			public int id { get; set; }
			public string main { get; set; }
			public string description { get; set; }
			public string icon { get; set; }
		}

		public class Clouds
		{
			public int all { get; set; }
		}

		public class Wind
		{
			public double speed { get; set; }
			public double deg { get; set; }
		}

		public class Rain
		{
			public double __invalid_name__3h { get; set; }
		}

		public class Sys
		{
			public string pod { get; set; }
		}

		public class List
		{
			public int dt { get; set; }
			public ForecastMain main { get; set; }
			public List<Weather> weather { get; set; }
			public Clouds clouds { get; set; }
			public Wind wind { get; set; }
			public Rain rain { get; set; }
			public Sys sys { get; set; }
			public string dt_txt { get; set; }
		} 

		public class Coord
		{
			public double lat { get; set; }
			public double lon { get; set; }
		}

		public class City
		{
			public int id { get; set; }
			public string name { get; set; }
			public Coord coord { get; set; }
			public string country { get; set; }
			public int population { get; set; }
		}

		public class ForecastRootObject
		{
			public string cod { get; set; }
			public double message { get; set; }
			public int cnt { get; set; }
			public List<List> list { get; set; }
			public City city { get; set; }
		} 
	}


}


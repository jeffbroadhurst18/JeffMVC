
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
			////http://datapoint.metoffice.gov.uk/public/data/txt/wxfcs/regionalforecast/json/sitelist?key=fb4c0ad5-6ba6-4068-bbcd-c5a2788e8265'

			var answer = await ProcessRepositories();

		}

		private static async Task<List<Forecast[]>> ProcessRepositories()
		{
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
			client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

			var stringTask = client.GetStringAsync("http://datapoint.metoffice.gov.uk/public/data/txt/wxfcs/regionalforecast/json/507?key=fb4c0ad5-6ba6-4068-bbcd-c5a2788e8265");

			var msg = await stringTask;
			Console.Write(msg);
			var x = JsonConvert.DeserializeObject<RootObject>(msg);

			var listperiods = x.RegionalFcst.FcstPeriods.Period;
			Forecast[] forecasts = new Forecast[] { new Forecast { f = string.Empty, title = string.Empty } };
			List<Forecast[]> forecastList = new List<Forecast[]>();

			var l = listperiods[0];
			Console.WriteLine(listperiods[0].Paragraph);
			var y = l.Paragraph.ToString().Replace('$', 'f');
			forecasts = JsonConvert.DeserializeObject<Forecast[]>(y);
			forecastList.Add(forecasts);
			
			return forecastList;
		}

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

		public class RootObject
		{
			public RegionalFcst RegionalFcst { get; set; }
		}

		public class Forecast
		{
			public string title { get; set; }
			public string f { get; set; }
		}

	}


}


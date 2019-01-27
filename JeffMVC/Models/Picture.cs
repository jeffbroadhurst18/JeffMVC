using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JeffMVC.Models
{
	public class PicturesAndWeatherContext
	{
		private static readonly HttpClient client = new HttpClient();

		private IEnumerable<Picture> GetPictures()
		{
			var p = new List<Picture>();
			p.Add(GetPicture(0));
			p.Add(GetPicture(1));
			p.Add(GetPicture(2));
			return p;
		}

		private IEnumerable<Picture> _pictures = null;
		public IEnumerable<Picture> Pictures
		{
			get => _pictures ?? (_pictures = GetPictures().ToList());
		}

		private IEnumerable<Forecast> _forecasts = null;
		public IEnumerable<Forecast> Forecasts
		{
			get => _forecasts ?? (_forecasts = GetForecast().Result);
		}

		private static async Task<List<Forecast>> GetForecast()
		{
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
			client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

			var stringTask = client.GetStringAsync("http://datapoint.metoffice.gov.uk/public/data/txt/wxfcs/regionalforecast/json/507?key=fb4c0ad5-6ba6-4068-bbcd-c5a2788e8265");

			var msg = await stringTask;
			var x = JsonConvert.DeserializeObject<RootObject>(msg);
			var listperiods = x.RegionalFcst.FcstPeriods.Period;
			Forecast[] forecasts = new Forecast[] { new Forecast { f = string.Empty, title = string.Empty } };

			var l = listperiods[0];
			var y = l.Paragraph.ToString().Replace('$', 'f');
			forecasts = JsonConvert.DeserializeObject<Forecast[]>(y);

			return forecasts.ToList();
		}

		private Picture GetPicture(int id)
		{
			Picture pictures;
			string[] photos;

			switch (id)
			{
				case 0:
					photos = new string[] { "~/images/newyork/newyork1.jpg", "~/images/newyork/newyork2.jpg", "~/images/newyork/newyork3.jpg", "~/images/newyork/newyork4.jpg" };
					pictures = new Picture
					{
						Name = "New York",
						Year = 2018,
						Path = photos.ToList()
					};
					break;
				case 1:
					photos = new string[] { "~/images/roma/roma1.jpg", "~/images/roma/roma2.jpg", "~/images/roma/roma3.jpg", "~/images/roma/roma4.jpg" };
					pictures = new Picture
					{
						Name = "Rome",
						Year = 2018,
						Path = photos.ToList()
					};
					break;
				case 2:
					photos = new string[] { "~/images/budapest/budapest1.jpg", "~/images/budapest/budapest2.jpg", "~/images/budapest/budapest3.jpg", "~/images/budapest/budapest4.jpg" };
					pictures = new Picture
					{
						Name = "Budapest",
						Year = 2017,
						Path = photos.ToList()
					};
					break;
				default:
					photos = new string[] { "~/images/newyork/newyork1.jpg", "~/images/newyork/newyork2.jpg", "~/images/newyork/newyork3.jpg", "~/images/newyork/newyork4.jpg" };
					pictures = new Picture
					{
						Name = "New York",
						Year = 2018,
						Path = photos.ToList(),
					};
					break;
			}
			
			return getTwoPictures(pictures);
		}

		private Picture getTwoPictures(Picture pictures)
		{
			string temp;
			Random rnd = new Random();
			pictures.Path.RemoveAt(rnd.Next(0, 4));
			pictures.Path.RemoveAt(rnd.Next(0, 3));
			if (rnd.Next(0, 2) == 0)
			{
				temp = pictures.Path[0];
				pictures.Path[0] = pictures.Path[1];
				pictures.Path[1] = temp;
			}
			return pictures;
		}

	}


	public class Picture
	{
		

		public string Name { get; set; }
		public int Year { get; set; }
		public List<string> Path { get; set; }
		

		
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

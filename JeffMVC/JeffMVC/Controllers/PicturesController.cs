using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JeffMVC.Filters;
using JeffMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JeffMVC.Controllers
{
	public class PicturesController : Controller
	{
		PicturesAndWeatherContext _ctx;

		public PicturesController(PicturesAndWeatherContext ctx)
		{
			_ctx = ctx;
		}

		[Logging]
		public IActionResult City(int id)
		{
			var city = _ctx.GetPicture(id);
			ViewBag.Title = city.Name;
			return View(city);
		}

		public IActionResult GetWeather()
		{
			ViewBag.EventsTitle = "Weather";
			return PartialView(_ctx);
		}


	}


}
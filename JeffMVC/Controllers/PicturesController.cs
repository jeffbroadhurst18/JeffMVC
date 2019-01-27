using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JeffMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JeffMVC.Controllers
{
	public class PicturesController : Controller
	{
		PicturesAndWeatherContext ctx;

		public PicturesController()
		{
			ctx = new PicturesAndWeatherContext();
		}

		public IActionResult NewYork()
		{
			return View(ctx);
		}

		public IActionResult Roma()
		{
			return View(ctx);
		}

		public IActionResult Budapest()
		{
			return View(ctx);
		}

		public IActionResult GetWeather()
		{
			ViewBag.EventsTitle = "Weather";
			return PartialView(ctx);
		}


	}


}
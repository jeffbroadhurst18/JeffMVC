using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeffMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace JeffMVC.Controllers
{
    public class TidesController : Controller
    {
		TidesContext _ctx;

		public TidesController(TidesContext ctx)
		{
			_ctx = ctx;
		}

        public IActionResult Index()
        {
            return View();
        }

		public IActionResult Tides(string id)
		{
			var tide = _ctx.GetTides(id).Result;
			return View(tide);
		}
    }
}
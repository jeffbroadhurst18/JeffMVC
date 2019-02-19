using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeffMVC.Filters;
using JeffMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JeffMVC.Controllers
{
    public class TidesController : Controller
    {
		TidesContext _ctx;
		//IEnumerable<SelectListItem> stations;

		public TidesController(TidesContext ctx)
		{
			_ctx = ctx;
		}

        public IActionResult Index()
        {
            return View();
        }

		[Logging]
		public IActionResult Get(string selectedstation = null)
		{
			if (selectedstation == null) { return View(_ctx); }
			_ctx.selectedstation = selectedstation;
		
			return View(_ctx);
		}
	}
}
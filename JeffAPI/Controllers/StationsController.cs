using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeffShared;
using Microsoft.AspNetCore.Mvc;

namespace JeffAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StationsController : ControllerBase
	{
		private ITidesService _tidesService;
		public StationsController(ITidesService tidesService)
		{
			_tidesService = tidesService;
		}


		// GET api/values
		[HttpGet(Name ="Stations")]
		public ActionResult<StationData> Stations()
		{
			return _tidesService.GetStations().Result;
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<IndividualStation> Station(string id)
		{
			return _tidesService.GetStation(id).Result;
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}

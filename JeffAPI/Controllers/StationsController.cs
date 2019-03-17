using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using JeffShared;
using JeffShared.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace JeffAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StationsController : ControllerBase
	{
		private ITidesService _tidesService;
		private readonly IMapper _mapper;
		public StationsController(ITidesService tidesService, IMapper mapper)
		{
			_tidesService = tidesService;
			_mapper = mapper;
		}

		// GET api/values
		[EnableCors("AnyGET")]
		[HttpGet(Name = "Stations")]
		public ActionResult<List<Station>> Stations()
		{
			var stationData = _tidesService.GetStations().Result;
			var stations = _mapper.Map<List<Station>>(stationData).OrderBy(o => o.Name);
			return Ok(stations);
		}

		// GET api/values/5
		[EnableCors("AnyGET")]
		[HttpGet("{id}")]
		public ActionResult<Station> Station(string id)
		{
			try
			{
				Regex regex = new Regex(@"^\d+[A-Z]?$");
				if (!regex.IsMatch(id))
				{
					return BadRequest("Station must be numeric only");
				}

				if (id == null) { return BadRequest("No Station"); }
				var feature = _tidesService.GetStation(id).Result;
				var station = _mapper.Map<Station>(feature);
				if (station == null) { return BadRequest("Unknown Station"); }
				return Ok(station);
			}
			catch (Exception ex) {
				return BadRequest("Whoops");
			}
			
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

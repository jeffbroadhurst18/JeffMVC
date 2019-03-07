using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JeffShared;
using JeffShared.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace JeffAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TidesController : ControllerBase
	{
		private ITidesService _tidesService;
		private readonly IMapper _mapper;
		public TidesController(ITidesService tidesService, IMapper mapper)
		{
			_tidesService = tidesService;
			_mapper = mapper;
		}

		// GET api/values
		[HttpGet("{id}")]
		public ActionResult<List<Tide>> Tides(string id)
		{
			if (id == null) { return BadRequest("No Station entered"); }
			var tideData = _tidesService.GetTideData(id).Result.ToList();
			var tides = _mapper.Map<List<Tide>>(tideData);
			if (tides == null) { return BadRequest("Unknown Station"); }
			return Ok(tides);
		}
	}
}
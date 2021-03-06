﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using JeffShared;
using JeffShared.ViewModel;
using Microsoft.AspNetCore.Cors;
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
		[EnableCors("AnyGET")]
		public ActionResult<List<Tide>> Tides(string id)
		{
			try
			{
				if (id == null) { return BadRequest("No Station entered"); }
				Regex regex = new Regex(@"^\d+[A-Z]?$");
				if (!regex.IsMatch(id))
				{
					return BadRequest("Station must be numeric only");
				}

				var tideData = _tidesService.GetTideData(id).Result.ToList();
				var tides = _mapper.Map<List<Tide>>(tideData);
				if (tides == null) { return BadRequest("Unknown Station"); }
				return Ok(tides);
			}
			catch (Exception ex)
			{
				return BadRequest("Error. Oh No!!");
			}
		}
	}
}
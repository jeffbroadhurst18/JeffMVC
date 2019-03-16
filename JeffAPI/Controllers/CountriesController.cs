using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using JeffShared;
using JeffShared.ViewModel;
using Microsoft.AspNetCore.Cors;

namespace JeffAPI.Controllers
{
	[Produces("application/json")]
	[Route("api/countries")]
	[ApiController]
	public class CountriesController : ControllerBase
	{
		private readonly ICountryService _countryService;
		private readonly IMapper _mapper;

		public CountriesController(ICountryService countryService, IMapper mapper)
		{
			_countryService = countryService;
			_mapper = mapper;
		}

		[HttpGet("{name}")]
		[EnableCors("AnyGET")]
		public async Task<IActionResult> Get(string name)
		{
			try
			{
				var countries = await _countryService.GetCountries(name);

				if (countries == null) return NotFound($"Country {name} was not found");

				var mapped = _mapper.Map<IEnumerable<Country>>(countries).OrderBy(y => y.Name);
				return Ok(mapped);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		[HttpGet("")]
		[Route("capital/{name}")]
		[EnableCors("AnyGET")]
		public async Task<IActionResult> GetCapital(string name)
		{
			try
			{
				var capital = await _countryService.GetCapital(name);

				if (capital == null) { return NotFound($"Capital {name} was not found"); }

				var mapped = _mapper.Map<Capital>(capital.FirstOrDefault());

				return Ok(mapped);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
	}
}
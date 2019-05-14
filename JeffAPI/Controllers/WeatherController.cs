using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JeffShared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeffAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
		private IWeatherService _weatherService;
		private readonly IMapper _mapper;
		public WeatherController(IWeatherService weatherService, IMapper mapper)
		{
			_weatherService = weatherService;
			_mapper = mapper;
		}

		// GET: api/Weather
		[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

		// GET: api/Weather/5
		[HttpGet("{id}")]
		[EnableCors("AnyGET")]
        public async Task<ActionResult<JeffShared.ViewModel.Weather>> Get(string id)
        {
			//id += ",uk";
			var p = id.Split(',');
			var weatherData = await _weatherService.GetForecast($"{ p[0]},{p[1]}");
			var weather = _mapper.Map<JeffShared.ViewModel.Weather>(weatherData);
			weather.Query = id;
			return weather;
		}

        // POST: api/Weather
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Weather/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

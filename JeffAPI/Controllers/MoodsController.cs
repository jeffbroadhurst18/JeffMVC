using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JeffShared.WeatherModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeffAPI.Controllers
{
    [Route("api/[controller]")]
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class MoodsController : ControllerBase
    {
        private readonly IWeatherRepository _weatherRepository;

        public MoodsController(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        // GET: api/Profiles
        [HttpGet]
        [EnableCors("AnyGET")]
        [Route("{name}/{month}")]
        public async Task<ActionResult<IList<Moods>>> GetMoods(string name, int month)
        {
            return await _weatherRepository.GetMoods(name, month);
        }

        [EnableCors("AnyGET")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostMoods([FromBody] Moods mood)
        {
            var result = await _weatherRepository.PostMoods(mood);

            if (result > 0) { return Ok(); }
            return StatusCode(500);
        }
    }
}

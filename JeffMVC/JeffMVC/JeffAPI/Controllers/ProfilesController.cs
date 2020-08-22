using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JeffShared.WeatherModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace JeffAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IWeatherRepository _weatherRepository;

        public ProfilesController(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        // GET: api/Profiles
        [HttpGet]
        [EnableCors("AnyGET")]
        [Route("{name}")]
        public async Task<ActionResult<IEnumerable<Profiles>>> GetProfiles(string name)
        {
            return await _weatherRepository.GetProfiles(name);
        }

        // PUT: api/Profiles/5
        [EnableCors("AnyGET")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutProfiles(int id, [FromBody]Profiles profile)
        {
            if (id != profile.Id)
            {
                return BadRequest();
            }

            var result = await _weatherRepository.UpdateProfile(profile);

            if (result) { return Ok(); }
            return StatusCode(500);
        }


    }
}

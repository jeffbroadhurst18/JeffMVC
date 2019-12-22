using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JeffShared;
using JeffShared.ViewModel;
using JeffShared.WeatherModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JeffAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
		private IWeatherService _weatherService;
		private readonly IMapper _mapper;
		private readonly IMemoryCache _cache;
		private readonly WeatherDBContext _weatherDBContext;
		private List<WeatherParameters> _cities;
		MemoryCacheEntryOptions _cacheExpirationOptions;

		public WeatherController(IWeatherService weatherService, IMapper mapper, IMemoryCache cache,
			WeatherDBContext weatherDBContext)
		{
			_weatherService = weatherService;
			_mapper = mapper;
			_cache = cache;
			_weatherDBContext = weatherDBContext;
			_cities = GetCities();
			_cacheExpirationOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(30)
			};

		}

		// GET: api/Weather/5
		[EnableCors("AnyGET")]
		public async Task<ActionResult<JeffShared.ViewModel.Weather>> Get([FromQuery] WeatherParameters data)
		{
			var cacheKey = $"FORECAST_{data.Name}";
			var weatherParameters = _cities.FirstOrDefault(c => c.Name.ToLower() == data.Name.ToLower());

			if (!_cache.TryGetValue(cacheKey, out JeffShared.ViewModel.Weather weather))
			{
				var weatherData = await _weatherService.GetForecast($"{ weatherParameters.Name},{weatherParameters.Country}");
				weather = _mapper.Map<JeffShared.ViewModel.Weather>(weatherData);
				weather.Query = weatherParameters;
				// Save data in cache.
				_cache.Set(cacheKey, weather, _cacheExpirationOptions);
			}
					   
			return weather;
		}

		[EnableCors("AnyGET")]
		[Route("cities")]
		public ActionResult<List<WeatherParameters>> Cities(bool uk)
		{


			return uk ? _cities.Where(c => c.Country == "uk").ToList() : _cities.Where(c => c.Country != "uk").ToList();
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

		List<WeatherParameters> GetCities()
		{
			List<WeatherParameters> dbCities; 
			using (var context = new WeatherDBContext())
			{


				dbCities = context.Cities.Select(c => new WeatherParameters {

					
						Name = c.Name,
						Country = c.Country,
						TimeLag = c.TimeLag
					}
				).ToList();
			}
			return dbCities;
			//var cities = new List<WeatherParameters>();
			//cities.Add(new WeatherParameters { Name = "Northwich", Country = "uk", TimeLag = 0 });
			//cities.Add(new WeatherParameters { Name = "Durham", Country = "uk", TimeLag = 0 });
			//cities.Add(new WeatherParameters { Name = "London", Country = "uk", TimeLag = 0 });
			//cities.Add(new WeatherParameters { Name = "Bournemouth", Country = "uk", TimeLag = 0 });
			//cities.Add(new WeatherParameters { Name = "Edinburgh", Country = "uk", TimeLag = 0 });
			//cities.Add(new WeatherParameters { Name = "Birmingham", Country = "uk", TimeLag = 0 });

			//cities.Add(new WeatherParameters { Name = "Stockholm", Country = "se", TimeLag = 1 });
			//cities.Add(new WeatherParameters { Name = "Copenhagen", Country = "dk", TimeLag = 1 });
			//cities.Add(new WeatherParameters { Name = "Roma", Country = "it", TimeLag = 1 });
			//cities.Add(new WeatherParameters { Name = "New York", Country = "us", TimeLag = -5 });
			//cities.Add(new WeatherParameters { Name = "Paris", Country = "fr", TimeLag = 1 });
			//cities.Add(new WeatherParameters { Name = "Perth", Country = "au", TimeLag = 7 });

			//return cities;
		}

		
	}

	
}

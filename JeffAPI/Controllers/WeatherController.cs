﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JeffAPI.Hubs;
using JeffShared;
using JeffShared.ViewModel;
using JeffShared.WeatherModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JeffAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private IWeatherService _weatherService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IWeatherRepository _weatherRepository;
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly ILogger<WeatherController> _logger;
        private List<WeatherParameters> _cities;
        MemoryCacheEntryOptions _cacheExpirationOptions;
        IHubContext<SignalHub, ITypedHubClient> _messageHubContext;

        public WeatherController(IWeatherService weatherService, IMapper mapper, IMemoryCache cache,
            IWeatherRepository weatherRepository, IHubContext<SignalHub> hubContext, ILogger<WeatherController> logger, IHubContext<SignalHub, ITypedHubClient> messageHubContext)
        {
            _weatherService = weatherService;
            _mapper = mapper;
            _cache = cache;
            _weatherRepository = weatherRepository;
            _hubContext = hubContext;
            _messageHubContext = messageHubContext;
            _logger = logger;
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
            try
            {
                var cacheKey = $"FORECAST_{data.Name}";
                var weatherParameters = _cities.FirstOrDefault(c => c.Name.ToLower() == data.Name.ToLower());

                if (!_cache.TryGetValue(cacheKey, out JeffShared.ViewModel.Weather weather))
                {
                    var weatherData = await _weatherService.GetCurrent($"{ weatherParameters.Name},{weatherParameters.Country}");
                    weather = _mapper.Map<JeffShared.ViewModel.Weather>(weatherData);
                    weather.AnnualMax = _weatherRepository.GetAnnualMax(data.Name, data.Year);
                    weather.AnnualMin = _weatherRepository.GetAnnualMin(data.Name, data.Year);
                    weather.MonthlyMax = _weatherRepository.GetMonthlyMax(data.Name, DateTime.Now.Month, data.Year);
                    weather.MonthlyMin = _weatherRepository.GetMonthlyMin(data.Name, DateTime.Now.Month, data.Year);
                    weather.Query = weatherParameters;
                    // Save data in cache.
                    //	_cache.Set(cacheKey, weather, _cacheExpirationOptions);

                    var converted = JsonConvert.SerializeObject(weather);
                    _logger.LogInformation($"Sending SignalMessageReceived for {weather.Name}");
                    _logger.LogInformation($"Content {JsonConvert.SerializeObject(weather)}");

                    await _messageHubContext.Clients.All.SendMessageToClient(converted);
                    _logger.LogInformation("Sent info");
                    _logger.LogInformation("--------");
                }

                return weather;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message + " " + ex.StackTrace);
                throw;
            }
        }

        [EnableCors("AnyGET")]
        [Route("cities")]
        public ActionResult<List<WeatherParameters>> Cities(bool uk)
        {
            return uk ? _cities.Where(c => c.Country == "uk").ToList() : _cities.Where(c => c.Country != "uk").ToList();
        }

        [EnableCors("AnyGET")]
        [Route("history/{name}/{hours}")]
        public ActionResult<List<Readings>> GetHistory(string name, int hours)
        {
            try
            {
                return _weatherRepository.GetHistory(name, hours);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message + " " + ex.StackTrace);
                throw;
            }
        }

        [EnableCors("AnyGET")]
        [Route("history/{name}")]
        public ActionResult<List<Readings>> GetHistory(string name)
        {
            return _weatherRepository.GetHistory(name);
        }

        [EnableCors("AnyGET")]
        [Route("monthly/{city}/{month}/{year}")]
        public ActionResult<WeatherSummary> GetMonthlyData(string city, int month, int year)
        {
            try
            {
                if (year == 0) { year = DateTime.Now.Year; }
                return _weatherRepository.GetMonthlyData(city, month, year);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message + " " + ex.StackTrace);
                throw;
            }
        }

        [EnableCors("AnyGET")]
        [Route("monthly/{city}")]
        public ActionResult<List<WeatherSummary>> GetMonthlyData(string city, int year)
        {
            try
            {
                if (year == 0) { year = DateTime.Now.Year; }
                return _weatherRepository.GetMonthlyData(city, year);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: " + ex.Message + " " + ex.StackTrace);
                throw;
            }
        }
        // POST: api/Weather
        [EnableCors("AnyGET")]
        [Route("citypair")]
        [HttpPost]
        public void Post([FromBody] CityPairs cityPairs)
        {
            _weatherRepository.SetCityPairs(cityPairs);
        }

        [EnableCors("AnyGET")]
        [Route("citypairs/{id}")]
        [HttpDelete]
        public void DeleteCityPair(int id)
        {
            _weatherRepository.DeleteCityPairs(id);
        }

        [EnableCors("AnyGET")]
        [Route("citypair")]
        [HttpGet]
        public ActionResult<List<CityPairs>> GetCityPairs()
        {
            return _weatherRepository.RetrieveCityPairs();
        }

        [EnableCors("AnyGET")]
        [Route("forecast")]
        [HttpGet]
        public async Task<ActionResult<FormattedForecast>> GetForecast(double lat, double lon)
        {
            return await _weatherService.GetForecastAsync(lat,lon);
        }

        // PUT: api/Weather/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        List<WeatherParameters> GetCities()
        {
            return _weatherRepository.GetCities();
        }
    }


}

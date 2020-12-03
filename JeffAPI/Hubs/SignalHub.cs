using System;
using System.Threading.Tasks;
using AutoMapper;
using JeffShared;
using JeffShared.ViewModel;
using JeffShared.WeatherModels;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace JeffAPI.Hubs
{
    public class SignalHub : Hub<ITypedHubClient>
    {
        private readonly IWeatherService _weatherService;
        private readonly IMapper _mapper;
        private readonly IWeatherRepository _weatherRepository;

        public SignalHub(IWeatherService weatherService, IMapper mapper, IWeatherRepository weatherRepository)
        {
            _weatherService = weatherService;
            _mapper = mapper;
            _weatherRepository = weatherRepository;
        }
        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendMessageToClient("Connected successfully!");
        }

        public Task SendMessage(string message)
        {
            return Clients.All.SendMessageToClient(message);
        }

        public async Task<Task> SendManual(string name, string country)
        {
            var year = DateTime.Now.Year;
            var weatherData = await _weatherService.GetCurrent($"{name},{country}");
            var weather = _mapper.Map<JeffShared.ViewModel.Weather>(weatherData);
            weather.AnnualMax = _weatherRepository.GetAnnualMax(name, year);
            weather.AnnualMin = _weatherRepository.GetAnnualMin(name, year);
            weather.MonthlyMax = _weatherRepository.GetMonthlyMax(name, DateTime.Now.Month, year);
            weather.MonthlyMin = _weatherRepository.GetMonthlyMin(name, DateTime.Now.Month, year);
            weather.Query = new WeatherParameters
            {
                Name = name,
                Country = country,
                TimeLag = 0,
                Year = year
            };
            var jString = JsonConvert.SerializeObject(weather);
            return Clients.All.SendMessageToClient(jString);
        }
    }

    public interface ITypedHubClient
    {
        Task SendMessageToClient(string message);
    }
}

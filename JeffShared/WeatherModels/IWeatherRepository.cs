using System.Collections.Generic;
using System.Threading.Tasks;
using JeffShared.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace JeffShared.WeatherModels
{
	public interface IWeatherRepository
	{
		List<WeatherParameters> GetCities();
		List<Readings> GetHistory(string name, int hours);
		List<Readings> GetHistory(string name);
        WeatherSummary GetMonthlyData(string city,int month, int year);
		List<WeatherSummary> GetMonthlyData(string city, int year);
		TemperatureDay GetAnnualMax(string name, int year);
		TemperatureDay GetAnnualMin(string name, int year);
		TemperatureDay GetMonthlyMax(string name, int month, int year);
		TemperatureDay GetMonthlyMin(string name, int month, int year);
		void SetCityPairs(CityPairs cityPairs);
		void DeleteCityPairs(int cityPairId);
		List<CityPairs> RetrieveCityPairs();
		Task<List<Profiles>> GetProfiles(string name);
		Task<bool> UpdateProfile(Profiles profile);
        Task<List<Moods>> GetMoods(string name, int month);
        Task<int> PostMoods(Moods mood);
        Task<ActionResult<IList<Moods>>> GetMoods(string name);
    }
}
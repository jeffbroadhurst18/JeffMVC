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
        WeatherSummary GetMonthlyData(string city,int month);
		List<WeatherSummary> GetMonthlyData(string city);
		TemperatureDay GetAnnualMax(string name);
		TemperatureDay GetAnnualMin(string name);
		TemperatureDay GetMonthlyMax(string name, int month);
		TemperatureDay GetMonthlyMin(string name, int month);
		void SetCityPairs(CityPairs cityPairs);
		void DeleteCityPairs(int cityPairId);
		List<CityPairs> RetrieveCityPairs();
		Task<List<Profiles>> GetProfiles();
		Task<bool> UpdateProfile(Profiles profile);
		
	}
}
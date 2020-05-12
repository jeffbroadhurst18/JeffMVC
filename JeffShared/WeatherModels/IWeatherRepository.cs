using System.Collections.Generic;
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
	}
}
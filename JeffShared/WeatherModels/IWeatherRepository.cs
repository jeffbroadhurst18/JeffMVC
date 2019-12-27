using System.Collections.Generic;
using JeffShared.ViewModel;

namespace JeffShared.WeatherModels
{
	public interface IWeatherRepository
	{
		List<WeatherParameters> GetCities();
		List<Readings> GetHistory(string name);
	}
}
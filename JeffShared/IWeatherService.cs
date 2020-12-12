using JeffShared.ViewModel;
using JeffShared.WeatherModels;
using System.Threading.Tasks;

namespace JeffShared
{
	public interface IWeatherService
	{
		Task<WeatherRootObject> GetCurrent(string id);
		Task<FormattedForecast> GetForecastAsync(double lat, double lon);
	}
}
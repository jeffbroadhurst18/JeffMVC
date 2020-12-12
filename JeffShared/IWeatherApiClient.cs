using JeffShared.ViewModel;
using JeffShared.WeatherModels;
using System.Threading.Tasks;

namespace JeffShared
{
	public interface IWeatherApiClient
	{
		Task<WeatherRootObject> GetWeatherAsync(string id);
        Task<Forecast> GetForecastAsync(double lat, double lon);
    }
}
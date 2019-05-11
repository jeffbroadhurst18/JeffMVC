using System.Threading.Tasks;

namespace JeffShared
{
	public interface IWeatherService
	{
		Task<WeatherRootObject> GetForecast(string id);
	}
}
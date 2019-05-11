using System.Threading.Tasks;

namespace JeffShared
{
	public interface IWeatherApiClient
	{
		Task<WeatherRootObject> GetWeatherAsync(string id);
	}
}
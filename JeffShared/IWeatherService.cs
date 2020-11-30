using System.Threading.Tasks;

namespace JeffShared
{
	public interface IWeatherService
	{
		Task<WeatherRootObject> GetCurrent(string id);
	}
}
using Microsoft.EntityFrameworkCore;

namespace JeffShared.WeatherModels
{
	public interface IWeatherDBContext
	{
		DbSet<Cities> Cities { get; set; }
		DbSet<Readings> Readings { get; set; }
		DbSet<Profiles> Profiles { get; set; }
		DbSet<Moods> Moods { get; set; }
	}
}
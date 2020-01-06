using JeffShared.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace JeffShared.WeatherModels
{
	public class WeatherRepository : IWeatherRepository
	{
		private readonly WeatherDBContext _context;

		public WeatherRepository(WeatherDBContext context)
		{
			_context = context;
		}

		public List<WeatherParameters> GetCities()
		{
			List<WeatherParameters> dbCities;

			dbCities = _context.Cities.Select(c => new WeatherParameters
			{
				Name = c.Name,
				Country = c.Country,
				TimeLag = c.TimeLag
			}).ToList();

			return dbCities;
		}

		public List<Readings> GetHistory(string name,int hours)
		{
			List<Readings> readings = _context.Readings.Include(c => c.City).Where(r => r.City.Name == name).
				OrderByDescending(d => d.CurrentTime).Take(hours).ToList();
			return readings;
		}

		public List<Readings> GetHistory(string name)
		{
			List<Readings> readings = _context.Readings.Include(c => c.City).Where(r => r.City.Name == name).
				OrderByDescending(d => d.CurrentTime).ToList();
			return readings;
		}
	}

}

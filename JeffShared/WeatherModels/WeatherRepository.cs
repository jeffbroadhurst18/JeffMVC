using JeffShared.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public List<Readings> GetHistory(string name, int hours)
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

        public WeatherSummary GetMonthlyData(string city, int month)
        {
            var summary = _context.Readings.Include(c => c.City)
                                            .Where(r => r.City.Name == city && r.CurrentTime.Month == month)
                                            .GroupBy(x =>
                                            new
                                            {
                                                x.City.Name,
                                                x.CurrentTime.Month
                                            },
            (key, group) => new WeatherSummary
            {
                City = key.Name,
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(key.Month),
                MaxTemp = Truncate(group.Max(t => t.Temperature),1),
                MinTemp = Truncate(group.Min(t => t.Temperature), 1),
                AvgTemp = Truncate(group.Average(t => t.Temperature), 1),
                MaxWind = Truncate(group.Max(t => t.Wind), 1),
                MinWind = Truncate(group.Min(t => t.Wind), 1),
                AvgWind = Truncate(group.Average(t => t.Wind), 1),
            }).First();

            List<DailySummary> dsList = _context.Readings.Include(c => c.City)
                                .Where(r => r.City.Name == city && r.CurrentTime.Month == month)
                                .GroupBy(x =>
                               new
                               {
                                   x.City.Name,
                                   x.CurrentTime.Month,
                                   x.CurrentTime.Day
                               },
                                (key, group) => new DailySummary
                                {
                                    City = key.Name,
                                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(key.Month),
                                    Day = key.Day,
                                    AvgTemp = Truncate(group.Average(t => t.Temperature), 1)
                                }).ToList();

             summary.DailySummaries = dsList;
             return summary;
        }

        private static double Truncate(double value, int digits)
        {
            double mult = System.Math.Pow(10.0, digits);
            return System.Math.Truncate(value * mult) / mult;
        }
    }

}

using JeffShared.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            List<Readings> readings = _context.Readings.Where(r => r.City.Name == name && r.CurrentTime > DateTime.Now.AddDays(-3)).Include(c => c.City).OrderByDescending(d => d.CurrentTime).Take(hours).ToList();
            return readings;
        }

        public List<Readings> GetHistory(string name)
        {
            List<Readings> readings = _context.Readings.Include(c => c.City).Where(r => r.City.Name == name && r.CurrentTime > DateTime.Now.AddDays(-3)).
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
                MaxTemp = Truncate(group.Max(t => t.Temperature), 1),
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
                                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(key.Month),
                                    Day = key.Day,
                                    AvgTemp = Truncate(group.Average(t => t.Temperature), 1),
                                    MinTemp = Truncate(group.Min(t => t.Temperature), 1),
                                    MaxTemp = Truncate(group.Max(t => t.Temperature), 1)
                                }).OrderBy(o => o.Day).ToList();

            summary.DailySummaries = dsList;
            return summary;
        }

        public List<WeatherSummary> GetMonthlyData(string city)
        {
            var monthlyList = new List<WeatherSummary>();
            var currentMonth = DateTime.Now.Month;

            for (var i = 0; i < currentMonth; i++)
            {
                monthlyList.Add(GetMonthlyData(city, i+1));
            }
            return monthlyList;
        }

        private static double Truncate(double value, int digits)
        {
            double mult = System.Math.Pow(10.0, digits);
            return System.Math.Truncate(value * mult) / mult;
        }

        public TemperatureDay GetAnnualMax(string name)
        {
            return _context.Readings.Include(c => c.City)
                                .Where(r => r.City.Name == name).Select(td => new TemperatureDay()
                                {
                                    TempDate = new DateTime(td.CurrentTime.Year, td.CurrentTime.Month, td.CurrentTime.Day),
                                    TempVal = td.Temperature
                                }).OrderByDescending(z => z.TempVal).First();
        }

        public TemperatureDay GetAnnualMin(string name)
        {
            return _context.Readings.Include(c => c.City)
                                .Where(r => r.City.Name == name).Select(td => new TemperatureDay()
                                {
                                    TempDate = new DateTime(td.CurrentTime.Year, td.CurrentTime.Month, td.CurrentTime.Day),
                                    TempVal = td.Temperature
                                }).OrderBy(z => z.TempVal).First();
        }

        public TemperatureDay GetMonthlyMax(string name, int month)
        {
            return _context.Readings.Include(c => c.City)
                                .Where(r => r.City.Name == name && r.CurrentTime.Month == month).Select(td => new TemperatureDay()
                                {
                                    TempDate = new DateTime(td.CurrentTime.Year, td.CurrentTime.Month, td.CurrentTime.Day),
                                    TempVal = td.Temperature
                                }).OrderByDescending(z => z.TempVal).First();
        }

        public TemperatureDay GetMonthlyMin(string name, int month)
        {
            return _context.Readings.Include(c => c.City)
                                .Where(r => r.City.Name == name && r.CurrentTime.Month == month).Select(td => new TemperatureDay()
                                {
                                    TempDate = new DateTime(td.CurrentTime.Year, td.CurrentTime.Month, td.CurrentTime.Day),
                                    TempVal = td.Temperature
                                }).OrderBy(z => z.TempVal).First();
        }

        public void SetCityPairs(CityPairs cityPairs)
        {
            _context.Add(cityPairs);
            _context.SaveChanges();
        }

        public void DeleteCityPairs(int cityPairId)
        {
            var cityPair = _context.CityPairs.Where(k => k.Id == cityPairId).First();
            _context.Remove(cityPair);
            _context.SaveChanges();
        }

        public List<CityPairs> RetrieveCityPairs()
        {
            return _context.CityPairs.ToList();
        }

        public async Task<List<Profiles>> GetProfiles()
        {
            return await _context.Profiles.OrderBy(t => t.Id).ToListAsync();
        }

        public async Task<bool> UpdateProfile(Profiles profile)
        {
            _context.Update(profile);
            var recs = await _context.SaveChangesAsync();
            return recs > 0; ;
        }
    }

}

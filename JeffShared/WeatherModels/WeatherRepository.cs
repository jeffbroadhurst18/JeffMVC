﻿using JeffShared.ViewModel;
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
                TimeLag = c.TimeLag,
                Lat = c.Lat,
                Lon = c.Lon
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

        public WeatherSummary GetMonthlyData(string city, int month, int year)
        {
            //var summary = _context.Readings.Include(c => c.City)
            var summary = _context.Readings.Where(r => r.City.Name == city && r.CurrentTime.Month == month && r.CurrentTime.Year == year)
                                            .GroupBy(x =>
                                            new
                                            {
                                                x.City.Name,
                                                x.CurrentTime.Month,
                                                x.CurrentTime.Year
                                            },
            (key, group) => new WeatherSummary
            {
                City = key.Name,
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(key.Month),
                Year = key.Year,
                MaxTemp = Truncate(group.Max(t => t.Temperature), 1),
                MinTemp = Truncate(group.Min(t => t.Temperature), 1),
                AvgTemp = Truncate(group.Average(t => t.Temperature), 1),
                MaxWind = Truncate(group.Max(t => t.Wind), 1),
                MinWind = Truncate(group.Min(t => t.Wind), 1),
                AvgWind = Truncate(group.Average(t => t.Wind), 1),
            }).First();

            List<DailySummary> dsList = _context.Readings.Include(c => c.City)
                                .Where(r => r.City.Name == city && r.CurrentTime.Month == month && r.CurrentTime.Year == year)
                                .GroupBy(x =>
                               new
                               {
                                   x.City.Name,
                                   x.CurrentTime.Year,
                                   x.CurrentTime.Month,
                                   x.CurrentTime.Day
                               },
                                (key, group) => new DailySummary
                                {
                                    Year = key.Year,
                                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(key.Month),
                                    Day = key.Day,
                                    AvgTemp = Truncate(group.Average(t => t.Temperature), 1),
                                    MinTemp = Truncate(group.Min(t => t.Temperature), 1),
                                    MaxTemp = Truncate(group.Max(t => t.Temperature), 1)
                                }).OrderBy(o => o.Day).ToList();

            summary.DailySummaries = dsList;
            return summary;
        }

        public List<WeatherSummary> GetMonthlyData(string city, int year)
        {
            var monthlyList = new List<WeatherSummary>();
            var currentMonth = DateTime.Now.Month;

            for (var i = 0; i < currentMonth; i++)
            {
                monthlyList.Add(GetMonthlyData(city, i + 1, year));
            }
            return monthlyList;
        }

        private static double Truncate(double value, int digits)
        {
            double mult = System.Math.Pow(10.0, digits);
            return System.Math.Truncate(value * mult) / mult;
        }

        public TemperatureDay GetAnnualMax(string name, int year)
        {
            return _context.MinMaxTemps.Include(c => c.City)
                .Where(c => c.City.Name == name && c.Year == year)
                .Select(g => new TemperatureDay()
                {
                    TempDate = g.MaxDate,
                    TempVal = g.MaxTemperature
                }).OrderByDescending(p => p.TempVal).First();
        }

        public TemperatureDay GetAnnualMin(string name, int year)
        {
            return _context.MinMaxTemps.Include(c => c.City)
                .Where(c => c.City.Name == name && c.Year == year)
                .Select(g => new TemperatureDay()
                {
                    TempDate = g.MinDate,
                    TempVal = g.MinTemperature
                }).OrderBy(p => p.TempVal).First();
        }

        public TemperatureDay GetMonthlyMax(string name, int month, int year)
        {
            return _context.MinMaxTemps.Include(c => c.City)
                    .Where(c => c.City.Name == name && c.Month == month && c.Year == year)
                    .Select(r => new TemperatureDay()
                    {
                        TempDate = r.MaxDate,
                        TempVal = r.MaxTemperature
                    }).FirstOrDefault();
        }

        public TemperatureDay GetMonthlyMin(string name, int month, int year)
        {
            return _context.MinMaxTemps.Include(c => c.City)
                    .Where(c => c.City.Name == name && c.Month == month && c.Year == year)
                    .Select(r => new TemperatureDay()
                    {
                        TempDate = r.MinDate,
                        TempVal = r.MinTemperature
                    }).FirstOrDefault();
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

        public async Task<List<Profiles>> GetProfiles(string name)
        {
            var result = await _context.Profiles.Where(z => z.UserName == name).OrderBy(t => t.Id).ToListAsync();
            result.ForEach(g =>
            {
                if (g.LastUpdated.Date < DateTime.Now.Date)
                {
                    g.Active = 0;
                    g.PicDisabled = 0;
                }
            });
            return result;
        }

        public async Task<bool> UpdateProfile(Profiles profile)
        {
            profile.LastUpdated = DateTime.Now;
            _context.Update(profile);
            var recs = await _context.SaveChangesAsync();
            return recs > 0; ;
        }

        public async Task<List<Moods>> GetMoods(string name, int month)
        {
            int year = DateTime.Now.Year;
            int lastDay = DateTime.DaysInMonth(year, month);
            DateTime startDate = new DateTime(year, month, 1).Date;
            DateTime endDate = new DateTime(year, month, lastDay).Date;

            var moods = await _context.Moods.Where(x => x.UserName == name && x.MoodDate >= startDate && x.MoodDate <= endDate)
                .OrderBy(c => c.MoodDate).ToListAsync();

            return moods;
        }

        public async Task<int> PostMoods(Moods mood)
        {
            //var existing = _context.Moods.Where(m => m.UserName == mood.UserName && m.MoodDate == mood.MoodDate);
            //await existing.ForEachAsync(async p => { 
            //    _context.Remove(p);
            //    await _context.SaveChangesAsync();
            //});

            _context.Update(mood);
            return await _context.SaveChangesAsync();

        }

        public async Task<ActionResult<IList<Moods>>> GetMoods(string name)
        {
            int year = DateTime.Now.Year;
            int lastDay = DateTime.DaysInMonth(year, 12);
            DateTime startDate = new DateTime(year, 1, 1).Date;
            DateTime endDate = new DateTime(year, 12, lastDay).Date;

            var moods = await _context.Moods.Where(x => x.UserName == name && x.MoodDate >= startDate && x.MoodDate <= endDate)
                .OrderBy(c => c.MoodDate).ToListAsync();

            moods.ForEach(l =>
            {
                l.MoodMonth = l.MoodDate.Month;
            });

            return moods;
        }
    }

}

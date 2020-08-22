using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using JeffShared.ViewModel;

namespace JeffShared
{
	public class WeatherMappingProfile : Profile
	{
		public WeatherMappingProfile()
		{
			CreateMap<WeatherRootObject, ViewModel.Weather>()
				.ForMember(w => w.Name, opt => opt.MapFrom(x => x.name)) 
				.ForMember(w => w.MainWeather, opt => opt.MapFrom(x => x.weather[0].main))
				.ForMember(w => w.Description, opt => opt.MapFrom(x => x.weather[0].description))
				.ForMember(w => w.Sunrise, opt => opt.MapFrom(x => UnixTimeStampToDateTime(x.sys.sunrise)))
				.ForMember(w => w.Sunset, opt => opt.MapFrom(x => UnixTimeStampToDateTime(x.sys.sunset)))
				.ForMember(w => w.CurrentTime, opt => opt.MapFrom(x => DateTime.Now))
				.ForMember(w => w.Icon, opt => opt.MapFrom(x => x.weather[0].icon))
				.ForMember(w => w.Temperature, opt => opt.MapFrom(x => Math.Round(x.main.temp - 273.15,1)))
				.ForMember(w => w.Wind, opt => opt.MapFrom(x => Math.Round(x.wind.speed * 3600/1609,1)))
				.ForMember(w => w.Direction, opt => opt.MapFrom(x => GetDirectionText(x.wind.deg)));
		}

		private string GetDirectionText(double deg)
		{
			var textArray = new string[] { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
			var fig = deg % 360;
			var index = Math.Round(fig / 22.5, 0) + 1;
			if (index > 16) { index = 16; }
			return textArray[(int)index];
		}

		public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}
	}

	
}

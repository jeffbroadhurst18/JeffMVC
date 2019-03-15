using AutoMapper.Configuration;
using JeffShared.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;

namespace JeffShared
{
	public class CountriesMappingProfile : Profile
	{
		public CountriesMappingProfile()
		{

			CreateMap<BaseCountry, Country>()
				.ForMember(p => p.Name, opt => opt.MapFrom(s => s.name))
				.ForMember(p => p.Capital, opt => opt.MapFrom(s => s.capital))
				.ForMember(p => p.Code, opt => opt.MapFrom(s => s.alpha3Code));
				//.ConvertUsing(source => source. .CountryList.Select(p => new Country
				//	{
				//		Name = p.name,
				//		Capital = p.capital,
				//		Code = p.alpha3Code
				//	}).ToList());

			CreateMap<CapitalRaw, Capital>()
				.ForMember(p => p.Name, opt => opt.MapFrom(s => s.capital))
				.ForMember(p => p.Country, opt => opt.MapFrom(s => s.name))
				.ForMember(p => p.Flag, opt => opt.MapFrom(s => s.flag))
				.ForMember(p => p.LocationPoints, opt => opt.MapFrom(s => s.latlng));
		}


	}
	
}

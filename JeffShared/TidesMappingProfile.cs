using AutoMapper;
using JeffShared;
using JeffShared.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JeffAPI
{
	public class TidesMappingProfile : Profile
	{
		public TidesMappingProfile()
		{
			CreateMap<StationData, List<Station>>()
				.ConvertUsing(source => source.features.Select(p => new Station {
					Id = p.properties.Id,
					Name = p.properties.Name,
					Country = p.properties.Country,
					LocationPoints = p.geometry.coordinates
				}).ToList());

			CreateMap<Feature, Station>()
				.ForMember(s => s.Id, opt => opt.MapFrom(x => x.properties.Id))
				.ForMember(s => s.Name, opt => opt.MapFrom(x => x.properties.Name))
				.ForMember(s => s.Country, opt => opt.MapFrom(x => x.properties.Country))
				.ForMember(s => s.LocationPoints, opt => opt.MapFrom(x => x.geometry.coordinates));

			CreateMap<TideData, Tide>()
				.ForMember(s => s.TideTime, opt => opt.MapFrom(x => x.DateTime.ToString("dd/MM/yyyy HH:mm")))
				.ForMember(s => s.TideType, opt => opt.MapFrom(x => x.EventType == "HighWater" ? "High Water" : "Low Water"));
		}
	}
}

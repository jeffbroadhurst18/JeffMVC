using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JeffShared
{
	public interface ITidesService
	{
		Task<StationData> GetStations();
		Task<TideData[]> GetTideData(string id);
		Task<IndividualStation> GetStation(string id);
	}
}

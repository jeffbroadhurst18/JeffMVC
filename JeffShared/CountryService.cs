using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JeffShared
{
	public class CountryService : ICountryService
	{
		private ICountriesAPIClient _client;

		public CountryService(ICountriesAPIClient client)
		{
			_client = client;
		}

		public async Task<CapitalRaw[]> GetCapital(string id)
		{
			return await _client.GetCapital(id);
		}

		public async Task<BaseCountry[]> GetCountries(string id)
		{
			return await _client.GetCountries(id);
		}
	}
}

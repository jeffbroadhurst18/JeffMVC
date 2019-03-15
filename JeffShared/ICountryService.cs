using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JeffShared
{
	public interface ICountryService
	{
		Task<BaseCountry[]> GetCountries(string id);
		Task<CapitalRaw[]> GetCapital(string id);
	}
}

using System.Collections.Generic;

namespace JeffShared.ViewModel
{
	public class Country
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public string Capital { get; set; }
	}

	public class Capital
	{
		public string Name { get; set; }
		public List<double> LocationPoints { get; set; }
		public string Flag { get; set; }
		public string Country { get; set; }
	}
}

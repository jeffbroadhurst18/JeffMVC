using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.ViewModel
{
	public class Station
	{
		public string Name { get; set; }
		public string Country { get; set; }
		public string Id { get; set; }
		public List<double> LocationPoints { get; set; }
	}
}

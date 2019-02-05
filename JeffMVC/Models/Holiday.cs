using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JeffMVC.Models
{
	public class Holiday
	{
		public int Id { get; set; }

		public string City { get; set; }

		public int Year { get; set; }

		public int Score { get; set; }
	}

	public class SortOption
	{
		public int Id { get; set; }
		public string optionName { get; set; }
	}

	public enum SortOptions
	{
		Year = 1, Score = 2
	}
}

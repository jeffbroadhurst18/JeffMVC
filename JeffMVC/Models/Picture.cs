using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JeffMVC.Models
{
	public class Picture
	{
		public string Name { get; set; }
		public int Year { get; set; }
		public List<string> Path { get; set; }
	}
}

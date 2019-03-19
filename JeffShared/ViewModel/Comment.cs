using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.ViewModel
{
	public class Comment
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime DateRecorded { get; set; }
		public bool Important { get; set; }
	}
}

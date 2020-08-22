using System;

namespace JeffShared.ViewModel
{
	public class Comment
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime DateRecorded { get; set; }
		public bool Important { get; set; }
		public Category Category { get; set; }
	}

	public enum Category
	{
		Work = 0,
		Home = 1,
		Education = 2
	}
}

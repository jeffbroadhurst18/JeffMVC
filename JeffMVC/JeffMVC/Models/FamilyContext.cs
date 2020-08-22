using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JeffMVC.Models
{
	public class FamilyContext
	{
		private IEnumerable<FamilyMember> _familymembers = null;

		public IEnumerable<FamilyMember> FamilyMembers
		{
			get => _familymembers ?? (_familymembers = GetFamilyMembers());
		}

		private IEnumerable<FamilyMember> GetFamilyMembers()
		{
			return new List<FamilyMember>
			{
				new FamilyMember{ Name = "Jeff Broadhurst", DoB = new DateTime(1968,12,18)},
				new FamilyMember{ Name = "Wendy Broadhurst", DoB = new DateTime(1969,7,5)},
				new FamilyMember{ Name = "Rebecca Broadhurst", DoB = new DateTime(2000,4,15)},
				new FamilyMember{ Name = "Tom Broadhurst", DoB = new DateTime(2003,2,12)}
			};
		}
	}

	public class FamilyMember
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DoB { get; set; }
	}
}

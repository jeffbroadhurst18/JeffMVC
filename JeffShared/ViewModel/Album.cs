using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.ViewModel
{
	public class Album
	{
		public int Id { get; set; }
		public string Band { get; set; }
		public string Name { get; set; }
		public int Year { get; set; }
		public string Review { get; set; }
		public string FileName { get; set; }
	}

	public class Member
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class MemberAlbum
	{
		public int Id { get; set; }
		public int AlbumId { get; set; }
		public Album Album { get; set; }
		public int MemberId { get; set; }
		public Member Member { get; set; }
	}

	public class DisplayAlbum
	{
		public int Id { get; set; }
		public string Band { get; set; }
		public string Name { get; set; }
		public int Year { get; set; }
		public string Review { get; set; }
		public string FileName { get; set; }
		public IEnumerable<Member> Members { get; set; }
	}
}
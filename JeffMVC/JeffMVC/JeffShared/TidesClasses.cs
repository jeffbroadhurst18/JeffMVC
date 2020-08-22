using System;
using System.Collections.Generic;

namespace JeffShared
{
	public class Geometry
	{
		public string type { get; set; }
		public List<double> coordinates { get; set; }
	}

	public class Properties
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }
		public bool ContinuousHeightsAvailable { get; set; }
		public string Footnote { get; set; }
	}

	public class Feature
	{
		public string type { get; set; }
		public Geometry geometry { get; set; }
		public Properties properties { get; set; }
	}

	public class TideData
	{
		public string EventType { get; set; }
		public DateTime DateTime { get; set; }
		public bool IsApproximateTime { get; set; }
		public bool IsApproximateHeight { get; set; }
		public bool Filtered { get; set; }
	}


	public class StationData
	{
		public string type { get; set; }
		public List<Feature> features { get; set; }
	}
}

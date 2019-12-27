using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JeffShared.WeatherModels
{
    public partial class Cities
    {
        public Cities()
        {
            Readings = new HashSet<Readings>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public int TimeLag { get; set; }

		[JsonIgnore]
		[IgnoreDataMember]
		public virtual ICollection<Readings> Readings { get; set; }
    }
}

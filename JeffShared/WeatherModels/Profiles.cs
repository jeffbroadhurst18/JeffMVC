using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.WeatherModels
{
    public class Profiles
    {
        public int Id { get; set; }
        public string PicPath { get; set; }
        public string PicName { get; set; }
        public int PicVotesUp { get; set; }
        public int PicVotesDown { get; set; }
        public int Active { get; set; }
        public int PicDisabled { get; set; }
    }
}

using JeffShared.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace JeffShared.WeatherModels
{
    public class Moods
    {
        public int Id { get; set; }
        public DateTime MoodDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public int MoodMonth { get; set; }
    }
}

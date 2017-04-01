using System;
using System.Collections.Generic;

namespace Vanyofy.Models
{
    public class AlarmSetting
    {         
        public string PlaylistUrl { get; set; }
         
        public List<DayOfWeek> Days { get; set; }
         
        public DateTime Time { get; set; }
         
        public bool IncrementVolume { get; set; }

        public int IncrementSeconds { get; set; }

        public AlarmSetting()
        {
            this.PlaylistUrl = string.Empty;
            this.Days = new List<DayOfWeek>();
            this.Time = new DateTime();
            this.IncrementVolume = false;
            this.IncrementSeconds = 0;
    }
    }
}

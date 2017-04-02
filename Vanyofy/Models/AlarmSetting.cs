using System;
using System.Collections.Generic;

namespace Vanyofy.Models
{
    public class AlarmSetting
    {         
        public string PlaylistUrl { get; set; }
         
        public List<DayOfWeek> Days { get; set; }
         
        public int Hour { get; set; }

        public int Minutes { get; set; }

        public bool isSingle { get; set; }

        public DateTime startDate { get; set; }
        
        public bool IncrementVolume { get; set; }

        public int IncrementSeconds { get; set; }

        public AlarmSetting()
        {
            this.PlaylistUrl = string.Empty;
            this.Days = new List<DayOfWeek>();
            this.Hour = 8;
            this.Minutes = 40;
            this.IncrementVolume = false;
            this.IncrementSeconds = 0;

            //TODO if single alarm
            this.startDate = DateTime.Now;
            this.isSingle = false;
    }
    }
}

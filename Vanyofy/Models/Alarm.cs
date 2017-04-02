using System;

namespace Vanyofy.Models
{
    public class Alarm
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public bool NotActive { get; set; }

        public AlarmSetting Settings { get; set; }

        public DateTime DateCreated { get; set; }

        public Alarm()
        {
            this.Id = null;
            this.Name = "Wake me up";
            this.Active = false;
            this.NotActive = !this.Active;
            this.Settings = new AlarmSetting();
            this.DateCreated = DateTime.Now;
        }
    }
}

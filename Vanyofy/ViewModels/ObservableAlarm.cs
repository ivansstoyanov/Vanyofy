using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Vanyofy.Models;

namespace Vanyofy.ViewModels
{
    public class ObservableAlarm : INotifyPropertyChanged
    {
        private Guid id { get; set; }

        private string name { get; set; }

        private bool active { get; set; }

        private bool notActive { get; set; }

        private string playlistUrl { get; set; }

        private List<DayOfWeek> days { get; set; }

        private DateTime time { get; set; }

        private bool incrementVolume { get; set; }

        private int incrementSeconds { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableAlarm()
        {
            this.id = Guid.NewGuid();
            this.name = "New Alarm";
            this.active = false;
            this.notActive = !this.active;

            this.playlistUrl = "";
            this.days = new List<DayOfWeek>();
            this.time = DateTime.Now;
            this.incrementVolume = false;
            this.incrementSeconds = 0;
        }

        public void SetAlarm(Alarm alarm)
        {
            this.id = alarm.Id;
            this.name = alarm.Name;
            this.active = alarm.Active;
            this.notActive = alarm.NotActive;

            this.playlistUrl = alarm.Settings.PlaylistUrl;
            this.days = alarm.Settings.Days;
            this.time = alarm.Settings.Time;
            this.incrementVolume = alarm.Settings.IncrementVolume;
            this.incrementSeconds = alarm.Settings.IncrementSeconds;
        }

        public Guid ID
        {
            get
            {
                return this.id;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Active
        {
            get
            {
                return this.active;
            }

            set
            {
                if (value != this.active)
                {
                    this.active = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool NotActive
        {
            get
            {
                return this.notActive;
            }

            set
            {
                if (value != this.notActive)
                {
                    this.notActive = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string PlaylistUrl
        {
            get
            {
                return this.playlistUrl;
            }

            set
            {
                if (value != this.playlistUrl)
                {
                    this.playlistUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IncrementVolume
        {
            get
            {
                return this.incrementVolume;
            }

            set
            {
                if (value != this.incrementVolume)
                {
                    this.incrementVolume = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int IncrementSeconds
        {
            get
            {
                return this.incrementSeconds;
            }

            set
            {
                if (value != this.incrementSeconds)
                {
                    this.incrementSeconds = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}

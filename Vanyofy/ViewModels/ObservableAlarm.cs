using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<bool> days { get; set; }

        private int timeHours { get; set; }

        private int timeMinutes { get; set; }

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

            this.playlistUrl = string.Empty;
            this.days = new ObservableCollection<bool>();
            for (int i = 0; i < 7; i++)
            {
                this.days.Add(true);
            }

            this.timeHours = 8;
            this.timeMinutes = 40;
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
            this.days = new ObservableCollection<bool>();
            for (int i = 0; i < 7; i++)
            {
                this.days.Add(true);
            }

            for (int i = 0; i < alarm.Settings.Days.Count; i++)
            {
                switch(alarm.Settings.Days[i])
                {
                    case DayOfWeek.Monday: this.days[0] = false; break;
                    case DayOfWeek.Tuesday: this.days[1] = false; break;
                    case DayOfWeek.Wednesday: this.days[2] = false; break;
                    case DayOfWeek.Thursday: this.days[3] = false; break;
                    case DayOfWeek.Friday: this.days[4] = false; break;
                    case DayOfWeek.Saturday: this.days[5] = false; break;
                    case DayOfWeek.Sunday: this.days[6] = false; break;
                }
            }

            this.timeHours = alarm.Settings.Time.Hour;
            this.timeMinutes = alarm.Settings.Time.Minute;

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

        public ObservableCollection<bool> Days
        {
            get
            {
                return this.days;
            }

            set
            {
                if (value != this.days)
                {
                    this.days = value;
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

        public int TimeHours
        {
            get
            {
                return this.timeHours;
            }

            set
            {
                if (value != this.timeHours)
                {
                    this.timeHours = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int TimeMinutes
        {
            get
            {
                return this.timeMinutes;
            }

            set
            {
                if (value != this.timeMinutes)
                {
                    this.timeMinutes = value;
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

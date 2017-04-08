using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Vanyofy.Models;

namespace Vanyofy.ViewModels
{
    public class ObservableAlarm : INotifyPropertyChanged
    {
        private Guid? id { get; set; }

        private string name { get; set; }

        private bool active { get; set; }

        private bool notActive { get; set; }

        private string playlistUrl { get; set; }

        private ObservableCollection<bool> days { get; set; }

        private int timeHours { get; set; }

        private int timeMinutes { get; set; }

        private bool incrementVolume { get; set; }

        private int incrementSeconds { get; set; }

        private string visualTimer { get; set; }

        private string asd { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        DispatcherTimer t;
        public ObservableAlarm()
        {
            this.id = null;
            this.name = "Wake me up";
            this.active = true;
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


            this.start = DateTime.Now;
            t = new DispatcherTimer(DispatcherPriority.Background, Dispatcher.CurrentDispatcher);
            t.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            t.Tick += (sender, e) => T_Tick(sender, e, this);
            t.IsEnabled = true;
        }

        DateTime start;
        private void T_Tick(object sender, EventArgs e, ObservableAlarm a)
        {
            var now = DateTime.Now;
            var next = GetNextAlarmDate();

            var val = next - now;

            var daysint = (int)(val.TotalDays);
            string daysString = string.Empty;

            if (daysint != 0)
            {
                daysString = string.Format("{0}d", daysint);
            }

            this.VisualTimer = this.active ? string.Format("{0} {1}:{2}:{3}", daysString, val.Hours, val.Minutes, val.Seconds) : string.Empty;
        }

        public DateTime GetNextAlarmDate()
        {
            DateTime now = DateTime.Now;
            int day = ((int)DateTime.Now.DayOfWeek == 0) ? 7 : (int)DateTime.Now.DayOfWeek;//1 to 7
            day = day - 1;

            var execAlarm = DateTime.Now;
            for (int i = day; i < this.days.Count; i++)
            {
                if (this.days[i] == false)
                {
                    TimeSpan ts = new TimeSpan(this.timeHours, this.timeMinutes, 0);
                    execAlarm = execAlarm.Date + ts;

                    if (execAlarm > now)
                    {
                        return execAlarm;
                    }
                }
                execAlarm = execAlarm.AddDays(1);
            }
            for (int i = 0; i < day; i++)
            {
                if (this.days[i] == false)
                {
                    TimeSpan ts = new TimeSpan(this.timeHours, this.timeMinutes, 0);
                    execAlarm = execAlarm.Date + ts;

                    if (execAlarm > now)
                    {
                        return execAlarm;
                    }
                }
                execAlarm = execAlarm.AddDays(1);
            }

            return now;
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

            this.timeHours = alarm.Settings.Hour;
            this.timeMinutes = alarm.Settings.Minutes;

            this.incrementVolume = alarm.Settings.IncrementVolume;
            this.incrementSeconds = alarm.Settings.IncrementSeconds;
        }

        public Alarm GetCurrentAlarm()
        {
            var newAlarm = new Alarm();

            newAlarm.Id = this.ID;
            newAlarm.Name = this.Name;
            newAlarm.Active = this.Active;
            newAlarm.NotActive = this.NotActive;
            newAlarm.DateCreated = DateTime.Now;

            newAlarm.Settings = new AlarmSetting();
            newAlarm.Settings.PlaylistUrl = this.PlaylistUrl;

            newAlarm.Settings.IncrementVolume = this.IncrementVolume;
            newAlarm.Settings.IncrementSeconds = this.IncrementSeconds;

            newAlarm.Settings.Hour = this.TimeHours;
            newAlarm.Settings.Minutes = this.TimeMinutes;

            newAlarm.Settings.Days = new List<DayOfWeek>();
            for (int i = 0; i < this.Days.Count; i++)
            {
                if (this.Days[i] == false)
                {
                    switch (i)
                    {
                        case 0: newAlarm.Settings.Days.Add(DayOfWeek.Monday); break;
                        case 1: newAlarm.Settings.Days.Add(DayOfWeek.Tuesday); break;
                        case 2: newAlarm.Settings.Days.Add(DayOfWeek.Wednesday); break;
                        case 3: newAlarm.Settings.Days.Add(DayOfWeek.Thursday); break;
                        case 4: newAlarm.Settings.Days.Add(DayOfWeek.Friday); break;
                        case 5: newAlarm.Settings.Days.Add(DayOfWeek.Saturday); break;
                        case 6: newAlarm.Settings.Days.Add(DayOfWeek.Sunday); break;
                    }
                }
            }

            return newAlarm;
        }

        public Guid? ID
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

        public string VisualTimer
        {
            get
            {
                return this.visualTimer;
            }

            set
            {
                if (value != this.visualTimer)
                {
                    this.visualTimer = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}

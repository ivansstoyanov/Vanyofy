using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Vanyofy.Models;

namespace Vanyofy.AlarmScheduler
{
    public class AlarmScheduler
    {
        Dictionary<Guid, List<CancellationTokenSource>> cancellationTokensDictionary = null;

        public AlarmScheduler()
        {
            this.cancellationTokensDictionary = new Dictionary<Guid, List<CancellationTokenSource>>();
        }

        public void ScheduleAllAlarms(ObservableCollection<Alarm> alarms)
        {
            foreach (var alarm in alarms)
            {
                this.ScheduleAlarm(alarm);
            }
        }

        public void ScheduleAlarm(Alarm alarm)
        {
            if (alarm.NotActive)
            {
                return;
            }

            DateTime alarmStart = DateTime.Now;
            TimeSpan ts = new TimeSpan(alarm.Settings.Hour, alarm.Settings.Minutes, 0);
            alarmStart = alarmStart.Date + ts;

            if (alarmStart < DateTime.Now)
            {
                alarmStart = alarmStart.AddDays(1);
            }

            var startInterval = alarmStart - DateTime.Now;
            var interval = TimeSpan.FromDays(1);

            var newCancelSource = CheckForCancellationToken(alarm.Id.Value);
            RunPeriodicAsync(() => AlarmExecute(alarm), startInterval, interval, newCancelSource.Token);
        }

        private static async Task RunPeriodicAsync(Action alarmExecute, TimeSpan startAfter, TimeSpan interval, CancellationToken token)
        {
            if (startAfter > TimeSpan.Zero)
            {
                await Task.Delay(startAfter, token);
            }

            while (!token.IsCancellationRequested)
            {
                alarmExecute?.Invoke();

                if (interval > TimeSpan.Zero)
                {
                    await Task.Delay(interval, token);
                }
            }
        }

        private void AlarmExecute(Alarm currentAlarm)
        {
            if (currentAlarm.NotActive)
            {
                return;
            }

            DayOfWeek today = DateTime.Now.DayOfWeek;
            if (!currentAlarm.Settings.Days.Contains(today))
            {
                return;
            }

            this.StartAlarm(currentAlarm);
        }

        private void StartAlarm(Alarm alarm)
        {
            SpotifyConnector.SpotifyConnector sc = new SpotifyConnector.SpotifyConnector();

            try
            {
                sc.StartSpotify();
                sc.StartPlaylist(alarm.Settings.PlaylistUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public CancellationTokenSource CheckForCancellationToken(Guid id)
        {
            var newCancelSource = new CancellationTokenSource();

            if (cancellationTokensDictionary.ContainsKey(id))
            {
                foreach (var token in cancellationTokensDictionary[id])
                {
                    token.Cancel();
                }

                cancellationTokensDictionary[id].Add(newCancelSource);
            }
            else
            {
                cancellationTokensDictionary.Add(id, new List<CancellationTokenSource>() { newCancelSource });
            }

            return newCancelSource;
        }

        public void CancelAllTokens()
        {
            foreach (var key in this.cancellationTokensDictionary.Keys)
            {
                foreach (var token in cancellationTokensDictionary[key])
                {
                    token.Cancel();
                }
            }
        }
    }
}

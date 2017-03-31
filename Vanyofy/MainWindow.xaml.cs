using SpotifyAPI.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vanyofy.Models;

namespace Vanyofy
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Alarm> AlarmsObservableList = null;
        public MainWindow()
        {
            InitializeComponent();
            
            this.AlarmsObservableList = new ObservableCollection<Alarm>();
            this.AlarmsObservableList.Add(new Alarm() {
                Id = Guid.NewGuid(),
                Name = "Alarm 1Alarm 1Alarm 1Alarm 1Alarm 1Alarm 1Alarm 1Alarm 1Alarm 1Alarm 1",
                Active = true,
                NotActive = false,
                Settings = new AlarmSetting()
            });
            this.AlarmsObservableList.Add(new Alarm()
            {
                Id = Guid.NewGuid(),
                Name = "Alarm 2",
                Active = false,
                NotActive = true,
                Settings = new AlarmSetting()
            });

            AlarmsList.ItemsSource = this.AlarmsObservableList;

            Alarm testA = this.AlarmsObservableList[0];


            DateTime alarmStart = DateTime.Now.AddSeconds(20); // new DateTime(2017, 3, 29, 19, 14, 0);

            var startinterval = alarmStart.AddSeconds(-7) - DateTime.Now;
            var interval = TimeSpan.FromSeconds(10);//TimeSpan.FromDays(1); // 86400

            // TODO: Add a CancellationTokenSource and supply the token here instead of None.
            //RunPeriodicAsync(() => AlarmExecute(testA), startinterval, interval, CancellationToken.None, testA);
            



            var a = Properties.Settings.Default.Alarms;

            Properties.Settings.Default.Alarms.Add("added in code");
            Properties.Settings.Default.Save();

            Properties.Settings.Default.Reset();




            //var json = new JavaScriptSerializer().Serialize(obj);
            //var objj = new JavaScriptSerializer().Deserialize<Alarm>("");
        }


        private static async Task RunPeriodicAsync(Action alarmExecute, TimeSpan startAfter, TimeSpan interval, CancellationToken token, Alarm currentAlarm)
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

            //start alarm
            this.StartAlarm(currentAlarm);
        }

        private void StartAlarm(Alarm alarm)
        {
            //SpotifyLocalAPI spotifyLocalAPI = new SpotifyLocalAPI(50);
            //if (!spotifyLocalAPI.Connect())
            //{
            //    //Program.FailureHandling(inputArguments, null, true);
            //}
            //spotifyLocalAPI.PlayURL("https://open.spotify.com/user/doublejmusicltd/playlist/6ChXRsZP7VVQd0LpTcca6P", "");
        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Application.Current.MainWindow.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            //this.WindowState = WindowState.Minimized;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //clean up notifyicon (would otherwise stay open until application finishes)
            MyNotifyIcon.Dispose();

            base.OnClosing(e);
        }

        private void MyNotifyIcon_TrayContextMenuOpen(object sender, System.Windows.RoutedEventArgs e)
        {
            //OpenEventCounter.Text = (int.Parse(OpenEventCounter.Text) + 1).ToString();
        }

        private void MyNotifyIcon_PreviewTrayContextMenuOpen(object sender, System.Windows.RoutedEventArgs e)
        {
            //marking the event as handled suppresses the context menu
            //e.Handled = (bool)SuppressContextMenu.IsChecked;

            //PreviewOpenEventCounter.Text = (int.Parse(PreviewOpenEventCounter.Text) + 1).ToString();
        }

        private void AddNewAlarm(object sender, RoutedEventArgs e)
        {
            this.AlarmsObservableList.Add(new Alarm());
        }

        private void ActivateAlarm(object sender, RoutedEventArgs e)
        {
            var parent = ((Grid)((Button)sender).Parent).Tag;
            var currentAlarm = this.AlarmsObservableList.FirstOrDefault(x => x.Id.ToString() == parent.ToString());

            var myValue = ((Button)sender).Tag;
            if (myValue.ToString() == "true")
            {
                currentAlarm.Active = true;
                currentAlarm.NotActive = false;

                var activeButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetActive"));
                activeButt.Visibility = Visibility.Visible;
                var notActiveButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetNotActive"));
                notActiveButt.Visibility = Visibility.Hidden;
            }
            else
            {
                currentAlarm.Active = false;
                currentAlarm.NotActive = true;

                var activeButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetActive"));
                activeButt.Visibility = Visibility.Hidden;
                var notActiveButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetNotActive"));
                notActiveButt.Visibility = Visibility.Visible;
            }


        }

        private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    
}

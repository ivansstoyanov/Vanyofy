using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Vanyofy.Animations;
using Vanyofy.Models;

namespace Vanyofy
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Alarm> AlarmsObservableList = null;
        public MainWindow()
        {
            InitializeComponent();
            this.AppAlarmSettingsRow.Height = new GridLength(0);


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



            //todo N.B.
            //if now time is bigger, add one day;

            //DateTime alarmStart = DateTime.Now.AddSeconds(20); // new DateTime(2017, 3, 29, 19, 14, 0);

            //var startinterval = alarmStart.AddSeconds(-7) - DateTime.Now;
            //var interval = TimeSpan.FromSeconds(30);//TimeSpan.FromDays(1); // 86400

            //// TODO: Add a CancellationTokenSource and supply the token here instead of None.
            //RunPeriodicAsync(() => AlarmExecute(testA), startinterval, interval, CancellationToken.None, testA);



            ////////morning test
            ////DateTime alarmStart = DateTime.Now; // new DateTime(2017, 3, 29, 19, 14, 0);
            ////TimeSpan ts = new TimeSpan(9, 20, 0);
            ////alarmStart = alarmStart.Date + ts;
            //////todo
            //////if now time is bigger, add one day;
            
            ////var startinterval = alarmStart - DateTime.Now;
            ////var interval = TimeSpan.FromDays(1);//TimeSpan.FromDays(1); // 86400

            ////// TODO: Add a CancellationTokenSource and supply the token here instead of None.
            ////RunPeriodicAsync(() => AlarmExecute(testA), startinterval, interval, CancellationToken.None, testA);
        }


        protected override void OnGiveFeedback(System.Windows.GiveFeedbackEventArgs e)
        {
            Mouse.SetCursor(Cursors.Hand);
            e.Handled = true;
        }

        void s_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mouse = Mouse.DirectlyOver;
            var currentObj = mouse as Line;

            if (mouse is Line && currentObj.Tag != null && currentObj.Tag.ToString() == "draggable" && sender is ListBoxItem)
            {
                Mouse.OverrideCursor = Cursors.SizeAll;

                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        void listbox1_Drop(object sender, DragEventArgs e)
        {
            Alarm droppedData = e.Data.GetData(typeof(Alarm)) as Alarm;
            Alarm target = ((ListBoxItem)(sender)).DataContext as Alarm;

            int removedIdx = this.AlarmsList.Items.IndexOf(droppedData);
            int targetIdx = this.AlarmsList.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                AlarmsObservableList.Insert(targetIdx + 1, droppedData);
                AlarmsObservableList.RemoveAt(removedIdx);
                this.AlarmsList.SelectedIndex = targetIdx;
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (AlarmsObservableList.Count + 1 > remIdx)
                {
                    AlarmsObservableList.Insert(targetIdx, droppedData);
                    AlarmsObservableList.RemoveAt(remIdx);
                    this.AlarmsList.SelectedIndex = targetIdx;
                }
            }

            Mouse.OverrideCursor = Cursors.Arrow;
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
                //return;
            }

            //start alarm
            this.StartAlarm(currentAlarm);
        }

        private void StartAlarm(Alarm alarm)
        {
            SpotifyConnector.SpotifyConnector sc = new SpotifyConnector.SpotifyConnector();

            try
            {
                sc.StartSpotify();
                sc.StartPlaylist();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.AlarmsList.SelectedItem = null;
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
            //this.AlarmsObservableList.Add(new Alarm());

            //this.AppAlarmSettingsRow.Height = new GridLength(100);

            if(this.AppAlarmSettingsRow.Height.Value == 0)
            {
                var anim = new DoubleAnimation(100, (Duration)TimeSpan.FromSeconds(0.25));
                //anim.Completed += (s, _) => Expanded = false;
                AppAlarmSettingsRow.BeginAnimation(AnimatedGridRowBehavior.AnimatedHeightProperty, anim);
            }
            else
            {
                var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.25));
                //anim.Completed += (s, _) => Expanded = false;
                AppAlarmSettingsRow.BeginAnimation(AnimatedGridRowBehavior.AnimatedHeightProperty, anim);
            }
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
    }
}

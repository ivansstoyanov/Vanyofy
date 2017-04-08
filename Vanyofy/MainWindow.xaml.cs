using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Vanyofy.Animations;
using Vanyofy.Models;
using Vanyofy.Settings;
using Vanyofy.ViewModels;

namespace Vanyofy
{
    public partial class MainWindow : Window
    {        
        public BindingList<ObservableAlarm> AlarmsObservableList = null;
        AlarmScheduler.AlarmScheduler AlarmScheduler = null;

        public MainWindow()
        {
            //TODO remove style when hover or selected listbox

            InitializeComponent();
            this.AppAlarmSettingsRow.Height = new GridLength(0);
            this.AlarmWizard.WizardCompleted += new EventHandler(Handler_WizardCompleted);

            this.AlarmScheduler = new AlarmScheduler.AlarmScheduler();

            RefreshAlarmsData();

            this.AlarmScheduler.ScheduleAllAlarms(this.AlarmsObservableList);
        }

        public void RefreshAlarmsData()
        {
            AlarmsProvider ap = new AlarmsProvider();
            var alarms = ap.GetAll();

            this.AlarmsObservableList = new BindingList<ObservableAlarm>();
            foreach (var a in alarms)
            {
                var oa = new ObservableAlarm();
                oa.SetAlarm(a);
                this.AlarmsObservableList.Add(oa);
            }

            AlarmsList.ItemsSource = this.AlarmsObservableList;
            this.AlarmsList.SelectedItem = null;
            this.CreateNewButtonOpened.Visibility = Visibility.Visible;
            this.CreateNewButtonClosed.Visibility = Visibility.Collapsed;
        }

        public void Handler_WizardCompleted(object sender, EventArgs e)
        {
            if (this.AlarmsObservableList.Count >= 10)
            {
                MessageBox.Show("You can only add 10 alarms in this version.");

                ShowAlarmWizard();
                RefreshAlarmsData();

                return;
            }

            var wizard = (NewAlarmWizard)sender;
            var newAlarm = wizard.WizardAlarm.GetCurrentAlarm();

            if (newAlarm.Id != null)
            {
                this.AlarmScheduler.CheckForCancellationToken(newAlarm.Id.Value);
            }

            AlarmsProvider ap = new AlarmsProvider();
            var na = ap.Add(newAlarm);
            wizard.WizardAlarm.ID = na.Id;

            ShowAlarmWizard();

            RefreshAlarmsData();

            this.AlarmScheduler.ScheduleAlarm(wizard.WizardAlarm);
        }



        /// <summary>
        /// Buttons Area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewAlarm(object sender, RoutedEventArgs e)
        {
            ShowAlarmWizard();
        }

        private void ShowAlarmWizard(Alarm editAlarm = null)
        {
            if (this.AppAlarmSettingsRow.Height.Value == 0)
            {
                this.CreateNewButtonOpened.Visibility = Visibility.Collapsed;
                this.CreateNewButtonClosed.Visibility = Visibility.Visible;

                AlarmWizard.StartOver(editAlarm);
                var anim = new DoubleAnimation(90, (Duration)TimeSpan.FromSeconds(0.25));
                AppAlarmSettingsRow.BeginAnimation(AnimatedGridRowBehavior.AnimatedHeightProperty, anim);
            }
            else
            {
                this.CreateNewButtonOpened.Visibility = Visibility.Visible;
                this.CreateNewButtonClosed.Visibility = Visibility.Collapsed;

                var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.25));
                AppAlarmSettingsRow.BeginAnimation(AnimatedGridRowBehavior.AnimatedHeightProperty, anim);
            }
        }

        private void ActivateAlarmClick(object sender, RoutedEventArgs e)
        {
            var parent = ((Grid)((Button)sender).Parent).Tag;
            var currentAlarm = this.AlarmsObservableList.FirstOrDefault(x => x.ID.ToString() == parent.ToString());
            var currentAlarmIndex = this.AlarmsObservableList.IndexOf(currentAlarm);

            var myValue = ((Button)sender).Tag;
            if (myValue.ToString() == "true")
            {
                currentAlarm.Active = true;
                currentAlarm.NotActive = false;

                var activeButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetActive"));
                activeButt.Visibility = Visibility.Visible;
                var notActiveButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetNotActive"));
                notActiveButt.Visibility = Visibility.Hidden;

                AlarmsProvider ap = new AlarmsProvider();
                ap.SetActive(currentAlarm.GetCurrentAlarm());

                var alarms = ap.GetAll();
                this.AlarmsObservableList = new BindingList<ObservableAlarm>();
                foreach (var a in alarms)
                {
                    var oa = new ObservableAlarm();
                    oa.SetAlarm(a);
                    this.AlarmsObservableList.Add(oa);
                }
                AlarmsList.ItemsSource = this.AlarmsObservableList;
                this.AlarmsList.SelectedItem = null;

                this.AlarmScheduler.ScheduleAlarm(currentAlarm);
            }
            else
            {
                currentAlarm.Active = false;
                currentAlarm.NotActive = true;
                AlarmsList.ItemsSource = this.AlarmsObservableList;

                var activeButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetActive"));
                activeButt.Visibility = Visibility.Hidden;
                var notActiveButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetNotActive"));
                notActiveButt.Visibility = Visibility.Visible;

                AlarmsProvider ap = new AlarmsProvider();
                ap.SetActive(currentAlarm.GetCurrentAlarm());

                var alarms = ap.GetAll();
                this.AlarmsObservableList = new BindingList<ObservableAlarm>();
                foreach (var a in alarms)
                {
                    var oa = new ObservableAlarm();
                    oa.SetAlarm(a);
                    this.AlarmsObservableList.Add(oa);
                }
                AlarmsList.ItemsSource = this.AlarmsObservableList;
                this.AlarmsList.SelectedItem = null;

                this.AlarmScheduler.CheckForCancellationToken(currentAlarm.ID.Value);
            }
        }

        private void EditAlarmClick(object sender, RoutedEventArgs e)
        {
            var parent = ((Grid)((Button)sender).Parent).Tag;
            var currentAlarm = this.AlarmsObservableList.FirstOrDefault(x => x.ID.ToString() == parent.ToString());

            ShowAlarmWizard(currentAlarm.GetCurrentAlarm());
        }

        private void DeleteAlarmClick(object sender, RoutedEventArgs e)
        {
            var parent = ((Grid)((Button)sender).Parent).Tag;
            var currentAlarm = this.AlarmsObservableList.FirstOrDefault(x => x.ID.ToString() == parent.ToString());

            this.AlarmScheduler.CheckForCancellationToken(currentAlarm.ID.Value);

            AlarmsProvider ap = new AlarmsProvider();
            ap.Delete(currentAlarm.ID.Value);

            RefreshAlarmsData();
        }



        /// <summary>
        /// Window behavior Area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            this.WindowState = WindowState.Minimized;
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
            MyNotifyIcon.Dispose();

            base.OnClosing(e);
        }



        /// <summary>
        /// Draggable area
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGiveFeedback(System.Windows.GiveFeedbackEventArgs e)
        {
            Mouse.SetCursor(Cursors.Hand);
            e.Handled = true;
        }

        void s_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mouse = Mouse.DirectlyOver;
            var currentObj = mouse as Image;

            if (currentObj != null && currentObj.Source.ToString().EndsWith("activity-feed-48.png"))
            {
                Mouse.OverrideCursor = Cursors.SizeAll;

                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        void listbox1_Drop(object sender, DragEventArgs e)
        {
            ObservableAlarm droppedData = e.Data.GetData(typeof(ObservableAlarm)) as ObservableAlarm;
            ObservableAlarm target = ((ListBoxItem)(sender)).DataContext as ObservableAlarm;

            int removedIdx = this.AlarmsList.Items.IndexOf(droppedData);
            int targetIdx = this.AlarmsList.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                AlarmsObservableList.Insert(targetIdx + 1, droppedData);
                AlarmsObservableList.RemoveAt(removedIdx);
                this.AlarmsList.SelectedIndex = targetIdx;

                SaveNewOrder();
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (AlarmsObservableList.Count + 1 > remIdx)
                {
                    AlarmsObservableList.Insert(targetIdx, droppedData);
                    AlarmsObservableList.RemoveAt(remIdx);
                    this.AlarmsList.SelectedIndex = targetIdx;

                    SaveNewOrder();
                }
            }

            Mouse.OverrideCursor = Cursors.Arrow;

            //TODO try deselecting after drag
        }

        private void SaveNewOrder()
        {
            List<Guid> newOrder = new List<Guid>();

            foreach (var listItem in this.AlarmsObservableList)
            {
                newOrder.Add(listItem.ID.Value);
            }

            AlarmsProvider ap = new AlarmsProvider();
            ap.UpdateOrder(newOrder);
        }
    }
}

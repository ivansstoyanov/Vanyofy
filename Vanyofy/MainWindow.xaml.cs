using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Vanyofy.Animations;
using Vanyofy.Models;
using Vanyofy.Settings;

namespace Vanyofy
{
    public partial class MainWindow : Window
    {        
        public ObservableCollection<Alarm> AlarmsObservableList = null;
        AlarmScheduler.AlarmScheduler AlarmScheduler = null;

        public MainWindow()
        {
            //TODO remove style when hover or selected listbox

            //TODO on icon double click show Vanyofy

            //TODO style ON / OFF button and listbox row
            //TODO when on set remaining timer visible

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

            this.AlarmsObservableList = new ObservableCollection<Alarm>();
            foreach (var a in alarms)
            {
                this.AlarmsObservableList.Add(a);
            }

            AlarmsList.ItemsSource = this.AlarmsObservableList;
            this.AlarmsList.SelectedItem = null;

            //TODO animation when refreshing
        }

        public void Handler_WizardCompleted(object sender, EventArgs e)
        {
            var wizard = (NewAlarmWizard)sender;
            var newAlarm = wizard.WizardAlarm.GetCurrentAlarm();

            if (newAlarm.Id != null)
            {
                this.AlarmScheduler.CheckForCancellationToken(newAlarm.Id.Value);
            }

            AlarmsProvider ap = new AlarmsProvider();
            ap.Add(newAlarm);

            ShowAlarmWizard();

            RefreshAlarmsData();
        }



        /// <summary>
        /// Buttons Area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewAlarm(object sender, RoutedEventArgs e)
        {
            //TODO change button style /\

            ShowAlarmWizard();
        }

        private void ShowAlarmWizard(Alarm editAlarm = null)
        {
            if (this.AppAlarmSettingsRow.Height.Value == 0)
            {
                var noAlarmsTemplate = AlarmsList.Template.FindName("NoAlarmsTemplate", AlarmsList) as StackPanel;
                var addAlarmTitle = AlarmsList.Template.FindName("AddAlarmTooltip", AlarmsList) as TextBlock;
                if (addAlarmTitle != null && addAlarmTitle != null)
                {
                    noAlarmsTemplate.Height = 50;
                    addAlarmTitle.Visibility = Visibility.Collapsed;
                }

                AlarmWizard.StartOver(editAlarm);
                var anim = new DoubleAnimation(100, (Duration)TimeSpan.FromSeconds(0.25));
                AppAlarmSettingsRow.BeginAnimation(AnimatedGridRowBehavior.AnimatedHeightProperty, anim);
            }
            else
            {
                var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.25));
                anim.Completed += (s, _) =>
                {
                    var noAlarmsTemplate = AlarmsList.Template.FindName("NoAlarmsTemplate", AlarmsList) as StackPanel;
                    var addAlarmTitle = AlarmsList.Template.FindName("AddAlarmTooltip", AlarmsList) as TextBlock;
                    if (addAlarmTitle != null && addAlarmTitle != null)
                    {
                        noAlarmsTemplate.Height = 100;
                        addAlarmTitle.Visibility = Visibility.Visible;
                    }
                };
                AppAlarmSettingsRow.BeginAnimation(AnimatedGridRowBehavior.AnimatedHeightProperty, anim);
            }
        }

        private void ActivateAlarmClick(object sender, RoutedEventArgs e)
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

                AlarmsProvider ap = new AlarmsProvider();
                ap.SetActive(currentAlarm);

                this.AlarmScheduler.ScheduleAlarm(currentAlarm);
            }
            else
            {
                currentAlarm.Active = false;
                currentAlarm.NotActive = true;

                var activeButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetActive"));
                activeButt.Visibility = Visibility.Hidden;
                var notActiveButt = (Button)(((Grid)((Button)sender).Parent).FindName("SetNotActive"));
                notActiveButt.Visibility = Visibility.Visible;

                AlarmsProvider ap = new AlarmsProvider();
                ap.SetActive(currentAlarm);

                this.AlarmScheduler.CheckForCancellationToken(currentAlarm.Id.Value);
            }
        }

        private void EditAlarmClick(object sender, RoutedEventArgs e)
        {
            var parent = ((Grid)((Button)sender).Parent).Tag;
            var currentAlarm = this.AlarmsObservableList.FirstOrDefault(x => x.Id.ToString() == parent.ToString());

            ShowAlarmWizard(currentAlarm);
        }

        private void DeleteAlarmClick(object sender, RoutedEventArgs e)
        {
            var parent = ((Grid)((Button)sender).Parent).Tag;
            var currentAlarm = this.AlarmsObservableList.FirstOrDefault(x => x.Id.ToString() == parent.ToString());

            this.AlarmScheduler.CheckForCancellationToken(currentAlarm.Id.Value);

            AlarmsProvider ap = new AlarmsProvider();
            ap.Delete(currentAlarm.Id.Value);

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
            Application.Current.Shutdown();
            //TODO on release set this
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
                newOrder.Add(listItem.Id.Value);
            }

            AlarmsProvider ap = new AlarmsProvider();
            ap.UpdateOrder(newOrder);
        }
    }
}

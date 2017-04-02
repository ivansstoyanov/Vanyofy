using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Vanyofy.Animations;
using Vanyofy.Models;
using Vanyofy.ViewModels;

namespace Vanyofy
{
    public partial class NewAlarmWizard : UserControl
    {
        private int CurrentStep = 0;
        private List<StackPanel> WizardSteps = new List<StackPanel>();

        public ObservableAlarm WizardAlarm = new ObservableAlarm();

        public NewAlarmWizard()
        {
            InitializeComponent();

            WizardSteps.Add(step1);
            WizardSteps.Add(step2);
            WizardSteps.Add(step3);
            WizardSteps.Add(step4);
            //WizardSteps.Add(step5);
        }

        public void StartOver(Alarm alarm = null)
        {
            WizardAlarm = new ObservableAlarm();
            
            if (alarm != null)
            {
                WizardAlarm.SetAlarm(alarm);
            }
            WizardWrapper.DataContext = WizardAlarm;

            CurrentStep = 0;
            ShowFinalStep(false);
            SetStep(CurrentStep);
        }

        private void SetStep(int index)
        {
            var lastStep = index == WizardSteps.Count - 1;

            if (index == 0)
            {
                foreach (var step in WizardSteps)
                {
                    step.Width = 0;
                }
                WizardSteps[0].Width = 405;
            }
            else
            {
                ShowStep(WizardSteps[index - 1], WizardSteps[index], lastStep);
            }
        }

        private void ShowStep(StackPanel from, StackPanel to, bool lastStep)
        {
            var anim = new DoubleAnimation(50, (Duration)TimeSpan.FromSeconds(0.2));
            anim.Completed += (s, _) =>
            {
                from.Width = 0;
                to.Width = 405;
                ShowFinalStep(lastStep);

                var anim1 = new DoubleAnimation(455, (Duration)TimeSpan.FromSeconds(0.25));
                WizardWrapper.BeginAnimation(AnimatedGridColumnBehavior.AnimatedWidthProperty, anim1);
            };

            WizardWrapper.BeginAnimation(AnimatedGridColumnBehavior.AnimatedWidthProperty, anim);
        }

        private void ShowFinalStep(bool show)
        {
            if (show)
            {
                NextButton.Visibility = Visibility.Collapsed;
                CompleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                NextButton.Visibility = Visibility.Visible;
                CompleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            CurrentStep++;
            this.SetStep(CurrentStep);
        }

        private void Complate_Click(object sender, RoutedEventArgs e)
        {
            RaiseCompleteEvent();
        }

        public event EventHandler WizardCompleted;
        private void RaiseCompleteEvent()
        {
            if (this.WizardCompleted != null)
            {
                this.WizardCompleted(this, new EventArgs());
            }
        }










        int minvalue = 0;
        int maxvalue = 59;
        int startvalue = 0;

        private void NUDButtonUP_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text);
            else number = 0;
            if (number < maxvalue)
                NUDTextBox.Text = Convert.ToString(number + 1);
        }

        private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text);
            else number = 0;
            if (number > minvalue)
                NUDTextBox.Text = Convert.ToString(number - 1);
        }

        private void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up)
            {
                NUDButtonUP.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { true });
            }


            if (e.Key == Key.Down)
            {
                NUDButtonDown.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { true });
            }
        }

        private void NUDTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { false });
        }

        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (NUDTextBox.Text != "")
                if (!int.TryParse(NUDTextBox.Text, out number)) NUDTextBox.Text = startvalue.ToString();
            if (number > maxvalue) NUDTextBox.Text = maxvalue.ToString();
            if (number < minvalue) NUDTextBox.Text = minvalue.ToString();

            if (NUDTextBox.Text.Length == 0)
            {
                NUDTextBox.Text = "00";
            }
            else if (NUDTextBox.Text.Length == 1)
            {
                NUDTextBox.Text = "0" + NUDTextBox.Text;
            }

            NUDTextBox.SelectionStart = NUDTextBox.Text.Length;
            BindingOperations.GetBindingExpression(NUDTextBox, TextBox.TextProperty).UpdateSource();
        }




        int minvalueh = 0;
        int maxvalueh = 23;
        int startvalueh = 0;

        private void NUDButtonUPh_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBoxh.Text != "") number = Convert.ToInt32(NUDTextBoxh.Text);
            else number = 0;
            if (number < maxvalueh)
                NUDTextBoxh.Text = Convert.ToString(number + 1);
        }

        private void NUDButtonDownh_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBoxh.Text != "") number = Convert.ToInt32(NUDTextBoxh.Text);
            else number = 0;
            if (number > minvalueh)
                NUDTextBoxh.Text = Convert.ToString(number - 1);
        }

        private void NUDTextBoxh_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up)
            {
                NUDButtonUPh.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUPh, new object[] { true });
            }


            if (e.Key == Key.Down)
            {
                NUDButtonDownh.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDownh, new object[] { true });
            }
        }

        private void NUDTextBoxh_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUPh, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDownh, new object[] { false });
        }

        private void NUDTextBoxh_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (NUDTextBoxh.Text != "")
                if (!int.TryParse(NUDTextBoxh.Text, out number)) NUDTextBoxh.Text = startvalueh.ToString();
            if (number > maxvalueh) NUDTextBoxh.Text = maxvalueh.ToString();
            if (number < minvalueh) NUDTextBoxh.Text = minvalueh.ToString();

            if (NUDTextBoxh.Text.Length == 0)
            {
                NUDTextBoxh.Text = "00";
            }
            else if (NUDTextBoxh.Text.Length == 1)
            {
                NUDTextBoxh.Text = "0" + NUDTextBoxh.Text;
            }

            NUDTextBoxh.SelectionStart = NUDTextBoxh.Text.Length;
            BindingOperations.GetBindingExpression(NUDTextBoxh, TextBox.TextProperty).UpdateSource();
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Vanyofy.Animations;
using Vanyofy.Models;
using Vanyofy.ViewModels;

namespace Vanyofy
{
    public partial class NewAlarmWizard : UserControl
    {
        public ObservableAlarm WizardAlarm = new ObservableAlarm();

        public NewAlarmWizard()
        {
            InitializeComponent();
        }

        public void StartOver(Alarm alarm = null)
        {
            //set to step one
            //set all with width 0
            InitializeComponent();

            WizardAlarm = new ObservableAlarm();
            step1.DataContext = WizardAlarm;
            if (alarm != null)
            {
                WizardAlarm.SetAlarm(alarm);
            }

            step1.Width = 405;
            step2.Width = 0;
        }

        private void Next_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //don't allow click if still animating

            //get visible step

            //show next step


            //final step different button


            
            var anim = new DoubleAnimation(50, (Duration)TimeSpan.FromSeconds(0.2));
            anim.Completed += (s, _) =>
            {
                step1.Width = 0;
                step2.Width = 405;
                var anim1 = new DoubleAnimation(455, (Duration)TimeSpan.FromSeconds(0.25));
                anim1.Completed += (ss, __) =>
                {
                    //finished allow click
                };
                WizardWrapper.BeginAnimation(AnimatedGridColumnBehavior.AnimatedWidthProperty, anim1);
            };
            WizardWrapper.BeginAnimation(AnimatedGridColumnBehavior.AnimatedWidthProperty, anim);
            
        }
    }
}

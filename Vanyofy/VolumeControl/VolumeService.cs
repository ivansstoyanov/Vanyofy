using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Windows.Threading;

namespace Vanyofy.VolumeControl
{
    public class VolumeService
    {
        DispatcherTimer t;
        CoreAudioDevice defaultPlaybackDevice;
        public VolumeService()
        {
            this.defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            this.t = new DispatcherTimer(DispatcherPriority.Background, Dispatcher.CurrentDispatcher);
        }

        public void IncreaseVolume(int milliSecondsToSleep)
        {
            this.defaultPlaybackDevice.Volume = 0;

            this.t.Interval = new TimeSpan(0, 0, 0, 0, milliSecondsToSleep);
            this.t.Tick += (sender, e) => T_Tick(sender, e);
            this.t.IsEnabled = true;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            defaultPlaybackDevice.Volume = defaultPlaybackDevice.Volume + 4;

            if (defaultPlaybackDevice.Volume >= 100)
            {
                this.t.IsEnabled = false;
            }
        }
    }
}

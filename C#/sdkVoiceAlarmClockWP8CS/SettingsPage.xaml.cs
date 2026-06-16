using System;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Audio;
using Windows.Phone.Speech.Recognition;

namespace MathClock
{
    public partial class Settings : PhoneApplicationPage
    {
        

        


        public Settings()
        {
            InitializeComponent();

            
        }

        // When the settings page is no longer the active page, store the settings variables according to the ToggleSwitches.
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Stop the sound and vibration.
            //this.timer.Stop();
            //this.alarmSound.Stop();

            //Settings.is24Hr.Value = AlarmPage.hourFmtToggleSwitch.IsChecked.Value;
            //Settings.showSeconds.Value = AlarmPage.secFmtToggleSwitch.IsChecked.Value;
            //Settings.enableVibration.Value = AlarmPage.vibrationToggleSwitch.IsChecked.Value;
            //Settings.enableSpeech.Value = AlarmPage.speechToggleSwitch.IsChecked.Value;
        }

        // When the settings page becomes the active page, initialize the ToggleSwitch status according to the settings.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //this.hourFmtToggleSwitch.IsChecked = Settings.is24Hr.Value;
            //this.secFmtToggleSwitch.IsChecked = Settings.showSeconds.Value;
            //this.vibrationToggleSwitch.IsChecked = Settings.enableVibration.Value;
            //this.speechToggleSwitch.IsChecked = Settings.enableSpeech.Value;

            //this.hourFmtToggleSwitch.Content = Settings.is24Hr.Value ?
            //    "24 Hour Clock: ON" : "24 Hour Clock: OFF";
            //this.secFmtToggleSwitch.Content = Settings.showSeconds.Value ?
            //    "Show Seconds: ON" : "Show Seconds: OFF";
            //this.vibrationToggleSwitch.Content = Settings.enableVibration.Value ?
            //    "Enable Vibration: ON" : "Enable Vibration: OFF";
            //this.speechToggleSwitch.Content = Settings.enableSpeech.Value ?
            //    "Enable Speech: ON" : "Enable Speech: OFF";
        }

        private async void VoicePwdButton_Clicked(object sender, RoutedEventArgs e)
        {
            
        }

        private async void TestVoicePwdButton_Clicked(object sender, RoutedEventArgs e)
        {
            
        }

        //private async void TestVolumeButton_Checked(object sender, RoutedEventArgs e)
        //{
        //    // Vibrate only if it is enabled.
        //    //if (Settings.enableVibration.Value)
        //    //    this.timer.Start();

        //    //// Play the sound.
        //    //this.alarmSound.Play();


        //}

        //private void TestVolumeButton_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    // Stop the sound and vibration.
        //   // this.timer.Stop();
        //    //this.alarmSound.Stop();
        //}

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Vibrate for half of a second.
            VibrateController.Default.Start(TimeSpan.FromSeconds(.5));
        }

        

    }
}

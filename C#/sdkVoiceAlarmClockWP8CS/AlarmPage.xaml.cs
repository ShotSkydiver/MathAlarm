using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Phone.Controls;

namespace MathClock
{
    public partial class AlarmPage : PhoneApplicationPage
    {


        public AlarmPage()
        {
            InitializeComponent();

        }

        //private string alarmTimeString
        //{
        //    get
        //    {
        //        int hour = Settings.alarmTime.Value.Hours;
        //        int minute = Settings.alarmTime.Value.Minutes;
        //        DateTime alarmDateTime = new DateTime(1, 1, 1, hour, minute, 0);

        //        return "alarm time: " + alarmDateTime.ToString("h:mmtt");
        //    }
        //}

        //protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    base.OnNavigatedFrom(e);
        //}

        //protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    this.alarmToggleSwitch.IsChecked = Settings.alarmSet.Value;
        //    this.alarmToggleSwitch.Content = Settings.alarmSet.Value ? "Alarm ON" : "Alarm OFF";
        //    this.timePicker.Value = new DateTime(1, 1, 1,
        //                                         Settings.alarmTime.Value.Hours,
        //                                         Settings.alarmTime.Value.Minutes,
        //                                         0
        //                                         );

        //    this.alarmTimeTextBlock.Text = alarmTimeString;
        //}

        //// Navigates back to the main page with the alarm on.
        //private void SetAlarmButton_Click(object sender, EventArgs e)
        //{
        //    this.NavigationService.Navigate(new Uri("/MainPage.xaml",
        //        UriKind.Relative));
        //}

        //// Navigates back to the main page with no changes saved and alarm off.
        //private void Cancel_Click(object sender, EventArgs e)
        //{
        //    this.NavigationService.Navigate(new Uri("/MainPage.xaml",
        //        UriKind.Relative));
        //}

        //private void alarmToggleSwitch_ValueChange(object sender, RoutedEventArgs e)
        //{
        //    Settings.alarmSet.Value = this.alarmToggleSwitch.IsChecked.Value;
        //    this.alarmToggleSwitch.Content = this.alarmToggleSwitch.IsChecked.Value ? "Alarm ON" : "Alarm OFF";
        //}
        
        //private void ToggleSwitch_UnChecked(object sender, RoutedEventArgs e)
        //{
        //    ToggleSwitch senderToggleSwitch = sender as ToggleSwitch;
        //    string toggleSwitchString = senderToggleSwitch.Content as string;
        //    senderToggleSwitch.Content = toggleSwitchString.Substring(0, toggleSwitchString.Length - "ON".Length) + "OFF";
        //}

        //private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        //{
        //    ToggleSwitch senderToggleSwitch = sender as ToggleSwitch;
        //    string toggleSwitchString = senderToggleSwitch.Content as string;
        //    senderToggleSwitch.Content = toggleSwitchString.Substring(0, toggleSwitchString.Length - "OFF".Length) + "ON";
        //}


        //private void timePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        //{
        //    // Update the alarm time.
        //    Settings.alarmTime.Value = new TimeSpan(this.timePicker.Value.Value.Hour, 
        //                                            this.timePicker.Value.Value.Minute, 
        //                                            0
        //                                            );

        //    // Update the alarm's time settings.
        //    Clock.alarm.BeginTime = this.timePicker.Value.Value;

        //    // Update the string at the top of the screen.
        //    this.alarmTimeTextBlock.Text = alarmTimeString;

        }

    }


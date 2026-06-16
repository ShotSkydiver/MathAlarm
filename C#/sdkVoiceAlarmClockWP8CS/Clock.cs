#region using
using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Devices;
using Microsoft.Phone.Scheduler;
using Microsoft.Xna.Framework.Audio;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Navigation;
#endregion

namespace MathClock
{
    public class Clock
    {
        private DispatcherTimer dispatcherTimer;
        private SoundEffectInstance alarmSound;
        private TimeSpan timeToAlarm;
        private Dictionary<DayOfWeek, TextBlock> dayOfWeekTextBlock;
        private MainPage mainPage;
        public static Alarm alarm;
        public CustomMessageBox messageBoxAlarm;
        

        public Clock(MainPage mainPage)
        {
            this.mainPage = mainPage;

            this.alarmSound = SoundEffects.Alarm.CreateInstance();
            this.alarmSound.IsLooped = true;

            #region days of week
            this.dayOfWeekTextBlock = new Dictionary<DayOfWeek, TextBlock>();
            this.dayOfWeekTextBlock[DayOfWeek.Monday] = mainPage.monTextBlock;
            this.dayOfWeekTextBlock[DayOfWeek.Tuesday] = mainPage.tueTextBlock;
            this.dayOfWeekTextBlock[DayOfWeek.Wednesday] = mainPage.wedTextBlock;
            this.dayOfWeekTextBlock[DayOfWeek.Thursday] = mainPage.thuTextBlock;
            this.dayOfWeekTextBlock[DayOfWeek.Friday] = mainPage.friTextBlock;
            this.dayOfWeekTextBlock[DayOfWeek.Saturday] = mainPage.satTextBlock;
            this.dayOfWeekTextBlock[DayOfWeek.Sunday] = mainPage.sunTextBlock;
            #endregion

            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += dispatcherTimer_Tick;
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            this.dispatcherTimer.Start();

            Clock.alarm = new Alarm("alarm");
            Clock.alarm.ExpirationTime = DateTime.MaxValue;
            Clock.alarm.RecurrenceType = RecurrenceInterval.Daily;
            Clock.alarm.Sound = new Uri("Audio/alarm.wav", UriKind.Relative);
            Clock.alarm.Content = "Alarm ringing!";

           
           //messagePrompt.ActionPopUpButtons.Add(btnComplete);
            
            
          /*
           this.messageBoxAlarm = new CustomMessageBox()
           {
               Caption = "The alarm is ringing",
               Message =
                   ("Solve the math problem below to turn off the alarm, otherwise, hit snooze." 
                   + ("\nrandom 1: " + mainPage.num1 + " random 2: " + mainPage.num2 + " added: " + mainPage.answer)),
               Content = userAnswer,
               LeftButtonContent = "Snooze",
               RightButtonContent = "Solve",
           };
           messageBoxAlarm.Dismissed += (s1, e1) =>
           {
               switch (e1.Result)
               {
                   case CustomMessageBoxResult.LeftButton:

                       this.alarmSound.Stop();

                       break;
                   case CustomMessageBoxResult.RightButton:
                        
                       int userAns = Convert.ToInt32(userAnswer.Text);

                       checkAnswer(userAns);

                       //if (userAns != mainPage.answer)
                       //{
                       //    this.messageBoxAlarm.Caption = "Wrong answer";

                           //if (Convert.ToInt32(userAnswer.Text) != userAns)
                           //{
                           //    userAns = Convert.ToInt32(userAnswer.Text);
                          // }
                            
                            
                       //}
                           this.alarmSound.Stop();
                           MainPage.alarmSet.Value = false;

                           MainPage.alarmTime.Value = new TimeSpan(mainPage.timePicker.Value.Value.Hour,
                                                                   mainPage.timePicker.Value.Value.Minute,
                                                                   0
                                                                   );

                           //int hour = MainPage.alarmTime.Value.Hours;
                           //int minute = MainPage.alarmTime.Value.Minutes;
                           //DateTime alarmDateTime = new DateTime(1, 1, 1, hour, minute, 0);

                           Clock.alarm.BeginTime = mainPage.timePicker.Value.Value;

                           this.mainPage.alarmTimeText.Text = alarmTimeString;

                           mainPage.alarmToggleSwitch.IsChecked = MainPage.alarmSet.Value;

                           this.mainPage.alarmTimeText.Text = "alarm off";

                           messageBoxAlarm.Dismiss();

                       break;
                   case CustomMessageBoxResult.None:
                       // Do something.
                       break;
                   default:
                       break;
               }
           };*/
        }

        private string alarmTimeString
        {
            get
            {
                int hour = MainPage.alarmTime.Value.Hours;
                int minute = MainPage.alarmTime.Value.Minutes;
                DateTime alarmDateTime = new DateTime(1, 1, 1, hour, minute, 0);

                return alarmDateTime.ToString("h:mmtt");
            }
        }

        
        
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Brush accent = Application.Current.Resources["PhoneAccentBrush"] as Brush;

            DateTime now = DateTime.Now;
            int month = DateTime.Now.Month;
            int date = DateTime.Now.Day;
            
            TimeSpan currentTime = now.TimeOfDay;

            // Update the electronic clock.
            //this.mainPage.TimeBlock.Text = GetFormattedDateTimeString(now);

            this.mainPage.hourText.Text = now.ToString("hh:");
            this.mainPage.minuteText.Text = now.ToString("mm");
            this.mainPage.secondsText.Text = now.ToString("ss");
            this.mainPage.amPM.Text = now.ToString("tt");

            this.mainPage.dateNumber.Text = Convert.ToString(date);

            #region month numbers to names

            if (month == 1)
            {
                this.mainPage.month.Text = "JANUARY";
            }
            else if (month == 2)
            {
                this.mainPage.month.Text = "FEBRUARY";
            }
            else if (month == 3)
            {
                this.mainPage.month.Text = "MARCH";
            }
            else if (month == 4)
            {
                this.mainPage.month.Text = "APRIL";
            }
            else if (month == 5)
            {
                this.mainPage.month.Text = "MAY";
            }
            else if (month == 6)
            {
                this.mainPage.month.Text = "JUNE";
            }
            else if (month == 7)
            {
                this.mainPage.month.Text = "JULY";
            }
            else if (month == 8)
            {
                this.mainPage.month.Text = "AUGUST";
            }
            else if (month == 9)
            {
                this.mainPage.month.Text = "SEPTEMBER";
            }
            else if (month == 10)
            {
                this.mainPage.month.Text = "OCTOBER";
            }
            else if (month == 11)
            {
                this.mainPage.month.Text = "NOVEMBER";
            }
            else if (month == 12)
            {
                this.mainPage.month.Text = "DECEMBER";
            }
            this.mainPage.month.Foreground = accent;

            #endregion

            if (MainPage.alarmSet.Value == true)
            {
                this.mainPage.alarmTimeText.Text = alarmTimeString;
            }
            else if (MainPage.alarmSet.Value == false)
            {
                this.mainPage.alarmTimeText.Text = "alarm off";
            }


            // Update the mechanical clock.
            /*((CompositeTransform)mainPage.hour.RenderTransform).Rotation
                  = (currentTime.Hours % 12) * 30 + currentTime.Minutes * 0.5 - 90;
            ((CompositeTransform)mainPage.minute.RenderTransform).Rotation
                = currentTime.Minutes * 6 - 90;
            ((CompositeTransform)mainPage.second.RenderTransform).Rotation
                = currentTime.Seconds * 6 - 90;  */

            foreach (TextBlock textblock in dayOfWeekTextBlock.Values)
            {
                textblock.Opacity = 0.3;
            }

            
            this.dayOfWeekTextBlock[DateTime.Today.DayOfWeek].Opacity = 1;
            this.dayOfWeekTextBlock[DateTime.Today.DayOfWeek].Foreground = accent;


            // Alarm functions
            if (MainPage.alarmSet.Value)
            {
                this.timeToAlarm = MainPage.alarmTime.Value - DateTime.Now.TimeOfDay;
                if (this.timeToAlarm.TotalSeconds <= 0 && this.timeToAlarm.TotalSeconds >= -60)
                {
                    // Vibrate only if the vibration setting is enabled.
                    if (MainPage.enableVibration.Value)
                    {
                        VibrateController.Default.Start(TimeSpan.FromSeconds(.5));
                    }

                    

                    this.alarmSound.Play();

                    //messageBoxAlarm.Show();
                    //mainPage.msgPrompt.Show();
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/AlarmRinging.xaml", UriKind.Relative));
                }
                else if (alarmSound.State == SoundState.Playing)
                {
                    this.alarmSound.Stop();
                }
            }
            else if (alarmSound.State == SoundState.Playing)
            {
                this.alarmSound.Stop();
            }
        }

        private string GetFormattedDateTimeString(DateTime dateTime)
        {
            if (MainPage.is24Hr.Value)
            {
               return dateTime.ToString("H:mm:ss");
            }
            else
            {
               return dateTime.ToString("h:mm:sstt");
            }
        }

        
    }
}

#region using
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Linq;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using System.Windows.Threading;
using Microsoft.Devices;
using Coding4Fun.Toolkit.Controls;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
//using Telerik.Windows.Controls;

#endregion

namespace MathClock
{
    public partial class MainPage : PhoneApplicationPage
    {
        public IEnumerable Difficulty { get; set; }
        public IEnumerable Tones { get; set; }

        //private RadResizeHeightAnimation slideAnimation;

        private Clock clock;
        private RoutedEventHandler onLoadedEventHandler;

        public SoundEffectInstance alarmSound;
        DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        //Global variables for random number generator
        

        public PhoneTextBox userAnswerTextBox;
        public MessagePrompt msgPrompt;
        

        public MediaElement mediaElement;

        //private List<Telerik.Windows.Controls.RadWindow> windows = new List<Telerik.Windows.Controls.RadWindow>();

        

        #region settings booleans
        public static readonly StoredItem<bool> canAutoLock = new StoredItem<bool>("canAutoLock", true);
        public static readonly StoredItem<bool> is24Hr = new StoredItem<bool>("is24Hr", true);
        public static readonly StoredItem<bool> showSeconds = new StoredItem<bool>("showSeconds", true);
        public static readonly StoredItem<bool> enableVibration = new StoredItem<bool>("enableVibration", true);
        public static readonly StoredItem<bool> alarmSet = new StoredItem<bool>("alarmSet", true);
        public static readonly StoredItem<TimeSpan> alarmTime = new StoredItem<TimeSpan>("alarmTime", TimeSpan.Zero);
        #endregion

        // Constructor. Initialize the speech and clock logic. Also disable idle detection.
        public MainPage()
        {
            InitializeComponent();

            #region listpicker elements
            Difficulty = new List<CustomItem>()
            {
                new CustomItem(){Caption = "Easy", Description="Simple math problems"},
                new CustomItem(){Caption = "Medium", Description="More difficult math problems"},
                new CustomItem(){Caption = "Hard", Description="Hard math problems"}
            };

            Tones = new List<CustomItem>()
            {
                new CustomItem(){Caption = "alarm 1", Description=""},
                new CustomItem(){Caption = "alarm 2", Description=""},
                new CustomItem(){Caption = "alarm 3", Description=""},
                new CustomItem(){Caption = "alarm 4", Description=""},
                new CustomItem(){Caption = "alarm 5", Description=""},
                new CustomItem(){Caption = "alarm 6", Description=""}
            };
            #endregion

            //Grid grid = (App.RootFrame.Content as PhoneApplicationPage).Content as Grid;
            //this.mediaElement = new MediaElement();
            //grid.Children.Add(mediaElement);

            TouchPanel.EnabledGestures = GestureType.Flick;

            LayoutRoot.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(LayoutRoot_ManipulationCompleted);
            
            SoundEffects.Initialize();
            this.clock = new Clock(this);

            //this.slideAnimation = new RadResizeHeightAnimation();
            //this.slideAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(350));

            PhoneApplicationService.Current.ApplicationIdleDetectionMode =
                IdleDetectionMode.Disabled;

            //this.onLoadedEventHandler = new RoutedEventHandler(OnLoaded);
            //this.Loaded += onLoadedEventHandler;

            this.timer.Tick += Timer_Tick;

            // Initialize the alarm sound.
            this.alarmSound = SoundEffects.Alarm.CreateInstance();
            this.alarmSound.IsLooped = true;

            
            this.userAnswerTextBox = new PhoneTextBox();

            

            //this.windows.Add(window);

            


            alarmTime.Value = new TimeSpan(DateTime.Now.Hour + 1,
                                                    (DateTime.Now.Minute),
                                                    0
                                                    );

            alarmSet.Value = false;
            this.notificationSwitch.IsChecked = alarmSet.Value;
            timePicker.Value = new DateTime(1, 1, 1,
                                                  alarmTime.Value.Hours, // alarmTime.Value.Hours + 1
                                                  alarmTime.Value.Minutes,
                                                  0
                                                  );
            this.alarmTimeText.Text = "alarm off";

            // Update the alarm's time settings
            //Clock.alarm.BeginTime = DateTime.Now.TimeOfDay;

            bool dark = ((Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible);

            if (dark)
            {
                this.themeCheck.Text = "dark theme";
                this.themeCheck.Foreground = new System.Windows.Media.SolidColorBrush(Colors.White);
                this.LayoutRoot.Background = new System.Windows.Media.SolidColorBrush(Colors.Black);
                this.SettingsPane.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 44, 44, 44));
            }
            else if (dark == false)
            {
                this.themeCheck.Text = "light theme";
                this.themeCheck.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
                this.LayoutRoot.Background = new System.Windows.Media.SolidColorBrush(Colors.LightGray);
                this.SettingsPane.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 186, 186, 186));
                SystemTray.BackgroundColor = Colors.LightGray;
            }
            
        }



        public List<String> Alarms
        {
            get { return new List<String> { "alarm1.wma", "alarm2.wma", "alarm3.wma", "alarm4.wma", "alarm5.wma", "alarm6.wma" }; }
        }

        public class CustomItem
        {
            public string Caption { get; set; }
            public string Description { get; set; }
        }

        private double NotificationsDesiredHeight
        {
            get
            {
                double height = this.settingsPanel.DesiredSize.Height;
                foreach (UIElement child in this.settingsPanel.Children)
                {
                    height += child.DesiredSize.Height;
                }

                return height;
            }
        }

        

        #region initial load/navigate to/from
        /*public async void OnLoaded(object sender, EventArgs e)
        {

            this.timer.Stop();
            this.alarmSound.Stop();


            alarmSet.Value = false;
            this.notificationSwitch.IsChecked = alarmSet.Value;

            this.timePicker.Value = new DateTime(1, 1, 1,
                                                 alarmTime.Value.Hours + 1,
                                                 alarmTime.Value.Minutes,
                                                 0
                                                 );

            

            if (alarmSet.Value == true)
                this.alarmTimeText.Text = alarmTimeString;
            else if (alarmSet.Value == false)
                this.alarmTimeText.Text = "alarm off";

            //Update live tile

            ShellTile TileToFind = ShellTile.ActiveTiles.First();
            if (TileToFind != null)
            {
                IconicTileData NewTileData = new IconicTileData
                {
                    Title = "alarm set for " + alarmTimeString,
                };
                TileToFind.Update(NewTileData);
            }

            // Math message prompt
            
            

            
            
        }*/

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            PhoneApplicationService.Current.UserIdleDetectionMode =
                IdleDetectionMode.Enabled;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PhoneApplicationService.Current.UserIdleDetectionMode =
                IdleDetectionMode.Disabled;

            if (alarmSet.Value == false)
            {
                alarmSound.Stop();

                //alarmTime.Value = new TimeSpan(timePicker.Value.Value.Hour, timePicker.Value.Value.Minute, 0);

                //int hour = MainPage.alarmTime.Value.Hours;
                //int minute = MainPage.alarmTime.Value.Minutes;
                //DateTime alarmDateTime = new DateTime(1, 1, 1, hour, minute, 0);

                Clock.alarm.BeginTime = timePicker.Value.Value;

                notificationSwitch.IsChecked = MainPage.alarmSet.Value;
                alarmTimeText.Text = "alarm off";
            }
            else if (alarmSet.Value == true)
            {
                notificationSwitch.IsChecked = true;
                this.alarmTimeText.Text = alarmTimeString;
            }

            while (NavigationService.RemoveBackEntry() != null) ;
        }

        #endregion

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Vibrate for half of a second.
            VibrateController.Default.Start(TimeSpan.FromSeconds(.5));
            
        }

        #region settings pane open/close

        private bool _isSettingsOpen = false;

        void LayoutRoot_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                switch (gesture.GestureType)
                {
                    case GestureType.FreeDrag:
                        if (_isSettingsOpen)
                        {
                            VisualStateManager.GoToState(this, "SettingsClosedState", true);
                            _isSettingsOpen = false;
                        }
                        else
                        {
                            VisualStateManager.GoToState(this, "SettingsOpenState", true);
                            _isSettingsOpen = true;
                        }
                        break;
                }
            }
        }

        void PopUpPromptObjectCompleted(object sender, PopUpEventArgs<object, PopUpResult> e)
        {
            //resultBlock.Text = e.PopUpResult.ToString();
        }

        // Navigates to the alarm page.
        private void SetAlarmButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AboutPage.xaml",
             UriKind.Relative));
        }



        // Navigates to the settings page.
        private void Settings_OnClick(object sender, EventArgs e)
        {
            // this.NavigationService.Navigate(new Uri("/SettingsPage.xaml",
              //  UriKind.Relative));



            if (_isSettingsOpen)
            {
                VisualStateManager.GoToState(this, "SettingsClosedState", true);
                _isSettingsOpen = false;
            }
            else
            {
                VisualStateManager.GoToState(this, "SettingsOpenState", true);
                _isSettingsOpen = true;

                this.vibrationToggleSwitch.IsChecked = enableVibration.Value;
                
            }
        }

        #endregion

        #region alarm & settings

        /****************************
         * Alarm & Settings Page *
         * ***************************/


        // difficulty level value changed
        // 
        // if difficultylevel = easy
            // num1 = random1.Next(1, 100);
            // num2 = random1.Next(1, 100);
        // else if difficultylevel = medium
            // num1 = random1.Next(100, 200);
            // num2 = random1.Next(100, 200);
        // else if difficultylevel = hard
            // num1 = random1.Next(200, 500);
            // num2 = random1.Next(200, 500);

        public string alarmTimeString
        {
            get
            {
                int hour = alarmTime.Value.Hours;
                int minute = alarmTime.Value.Minutes;
                DateTime alarmDateTime = new DateTime(1, 1, 1, hour, minute, 0);

                return alarmDateTime.ToString("h:mmtt");
            }
        }

        //private void timePicker_ValueChanged(object sender, Telerik.Windows.Controls.ValueChangedEventArgs<object> args)
        private void timePicker_ValueChanged(object sender, EventArgs e)
        {
            
                // Update the alarm time
                alarmTime.Value = new TimeSpan(timePicker.Value.Value.Hour,
                                                        timePicker.Value.Value.Minute,
                                                        0
                                                        );

                // Update the alarm's time settings
                Clock.alarm.BeginTime = timePicker.Value.Value;

                this.alarmTimeText.Text = alarmTimeString;

                notificationSwitch.IsChecked = true;

                /*ToastPrompt toast = new ToastPrompt
                {
                    Title = "Alarm Set",
                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
                    Message = relativeAlarmTime,
                    ImageSource = new BitmapImage(new Uri("../../ApplicationIcon.png", UriKind.RelativeOrAbsolute))
                };

                toast.Show();*/


                ShellTile TileToFind = ShellTile.ActiveTiles.First();
                if (TileToFind != null)
                {
                    FlipTileData NewTileData = new FlipTileData
                    {
                        Title = "alarm set for " + alarmTimeString,
                    };
                    TileToFind.Update(NewTileData);
                }
            
        }

        private void vibrationToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            
                enableVibration.Value = true;
                Microsoft.Devices.VibrateController.Default.Start(TimeSpan.FromSeconds(0.1));
            
        }

        private void vibrationToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            enableVibration.Value = false;
        }

        private string relativeAlarmTime
        {
            get
            {
                TimeSpan timeToAlarm = new TimeSpan();

                DateTime now = DateTime.Now;
                timeToAlarm = alarmTime.Value - DateTime.Now.TimeOfDay;
                double delta = Math.Abs(timeToAlarm.TotalSeconds);

                /*if (delta < 60)
                {
                    return ts.Seconds == 1 ? "one second from now" : ts.Seconds + " seconds ago";
                }*/
                if (delta < 120)
                {
                    return "the alarm will ring in one minute.";
                }
                if (delta < 2700) // 45 * 60
                {
                    return "the alarm will ring in " + Math.Abs(timeToAlarm.Minutes) + " minutes.";
                }
                if (delta < 5400) // 90 * 60
                {
                    return "the alarm will ring in one hour and " + Math.Abs(timeToAlarm.Minutes) + " minutes.";
                }
                if (delta < 86400) // 24 * 60 * 60
                {
                    return "the alarm will ring in " + Math.Abs(timeToAlarm.Hours) + " hours and " + Math.Abs(timeToAlarm.Minutes) + " minutes.";
                }
                /*if (delta < 172800) // 48 * 60 * 60
                {
                    return "yesterday";
                }
                if (delta < 2592000) // 30 * 24 * 60 * 60
                {
                    return timeToAlarm.Days + " days ago";
                }
                if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
                {
                    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    return months <= 1 ? "one month ago" : months + " months ago";
                }*/
                return "error";
            }
        }


        private void difficultyLevel_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //textDefault.Text = "Selected Item is " + ((Items)this.difficultyLevel.SelectedItem).Name;

            //ListPickerItem lpi = (sender as ListPicker).SelectedItem as ListPickerItem;
            //MessageBox.Show("selected item is : " + lpi.Content);

          

                /*if (difficultyLevel.SelectedIndex == -1)
                {
                    return;
                }

                var selectedItem = (sender as ListPicker).SelectedItem;*/

                //MessageBox.Show((difficultyLevel.SelectedItem as CustomItem).Caption.ToString());

               // this.getDifficultyText.Text = ("difficulty selected is " + (difficultyLevel.SelectedItem as CustomItem).Caption.ToString());
            
        }

        private void alarmTone_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            /*if (hasFinishedLoading == true)
            {
                if (alarmTone.SelectedIndex == -1)
                {
                    return;
                }
                var selectedItem = (sender as ListPicker).SelectedItem;
                // MessageBox.Show((alarmTone.SelectedItem as CustomItem).Caption.ToString());

                this.getAlarmToneText.Text = ("tone selected is " + (alarmTone.SelectedItem as CustomItem).Caption.ToString());
            }*/
        }
        #endregion

        /*private async Task WriteToFile()
        {
            // Get the text data from the textbox. 
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(this.textBox1.Text.ToCharArray());

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync("DataFolder",
                CreationCollisionOption.OpenIfExists);

            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync("DataFile.txt",
            CreationCollisionOption.ReplaceExisting);

            // Write the data from the textbox.
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }
        } */

        private void tonePreview_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           /*e.Handled = true;
            var data = (sender as RoundButton).DataContext as string;
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(data + ".wma", FileMode.Open, FileAccess.Read))
                {
                    mediaElement.Stop();
                    mediaElement.SetSource(fileStream);
                    mediaElement.Position = TimeSpan.FromSeconds(0);
                    mediaElement.Volume = 20;
                    mediaElement.Play();
                    mediaElement.AutoPlay = true;
                }
            }*/ 
        }

 

        private void notificationSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
           
                //this.slideAnimation.StartHeight = this.settingsPanel.ActualHeight;
                //this.slideAnimation.EndHeight = 0;
                //RadAnimationManager.Play(this.settingsPanel, this.slideAnimation);

                settingsPanel.Height = 0;

                alarmSet.Value = false;

                this.alarmTimeText.Text = "alarm off";

                ShellTile TileToFind = ShellTile.ActiveTiles.First();
                if (TileToFind != null)
                {
                    FlipTileData NewTileData = new FlipTileData
                    {
                        Title = "no alarms set",
                    };
                    TileToFind.Update(NewTileData);
                }
            
        }

        private void notificationSwitch_Checked(object sender, RoutedEventArgs e)
        {
           
               // this.slideAnimation.StartHeight = 0;
                //this.slideAnimation.EndHeight = this.NotificationsDesiredHeight;
                //RadAnimationManager.Play(this.settingsPanel, this.slideAnimation);

                settingsPanel.Height = this.NotificationsDesiredHeight;

                alarmSet.Value = true;
                this.alarmTimeText.Text = alarmTimeString;

                ShellTile TileToFind = ShellTile.ActiveTiles.First();
                if (TileToFind != null)
                {
                    FlipTileData NewTileData = new FlipTileData
                    {
                        Title = "alarm set for " + alarmTimeString,

                    };
                    TileToFind.Update(NewTileData);
                }

            
        }

        private void alarmToneSelectClick(object sender, RoutedEventArgs e)
        {
            //this.window.PlacementTarget = null;
            //this.window.IsOpen = true;
        }

        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            //this.window.IsOpen = false;
        }

        private void tonePreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void alarmTone_Selected(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }



    }
}

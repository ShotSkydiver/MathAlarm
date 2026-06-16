using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MathClock
{
    public partial class AlarmRinging : PhoneApplicationPage
    {
        public int num1;
        public int num2;
        public int answer;
        public Random random1;
        public bool hasFinishedLoading;
        public int userSolve;

        private MainPage mainPage;

        public AlarmRinging()
        {
            InitializeComponent();

            this.random1 = new Random();

            this.mainPage = mainPage;

            this.hasFinishedLoading = false;

        }

        private void checkAnswer(int userAnswerInt)
        {

            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            num1 = random1.Next(1, 100);
            num2 = random1.Next(1, 100);
            answer = num1 + num2;

            equation.Text = ("What is " + num1 + " + " + num2 + "?");
            
            //displayAnswer.Text = ("answer: " + this.answer);
            

            
            
                //msgPrompt.OnCompleted(MessageBox.Show("Correct"));

            this.hasFinishedLoading = true;

        }

        private void Solve_Click(object sender, EventArgs e)
        {

            this.userSolve = Convert.ToInt32(answerInput.Text);
            if (userSolve != answer)
            {
                MessageBox.Show("Incorrect");
                //userAnswerInt = Convert.ToInt32(answerInput.Text);


            }

            else if (userSolve == answer)
            {
                MainPage.alarmSet.Value = false;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            
        
        }

        private void KeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.userSolve = Convert.ToInt32(answerInput.Text);
                if (userSolve != answer)
                {
                        MessageBox.Show("Incorrect");
                        //userAnswerInt = Convert.ToInt32(answerInput.Text);
                    
                    
                }

                else if (userSolve == answer)
                {

                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
        }
        
    }
}
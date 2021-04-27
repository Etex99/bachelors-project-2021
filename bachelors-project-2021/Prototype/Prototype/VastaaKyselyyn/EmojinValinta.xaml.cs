using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmojinValinta : ContentPage
    {
        public IList<Emoji> Emojis { get; set; }
        public IList<string> Images { get; set; }
        public string introMessage { get; set; }
        private int answer;

        public EmojinValinta()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            Emojis = SurveyManager.GetInstance().GetSurvey().emojis; //Change to survey provided by host
            introMessage = SurveyManager.GetInstance().GetSurvey().introMessage;
            Images = new List<string>();
            foreach(Emoji emoji in Emojis)
            {
                Images.Add(emoji.ImageSource);
            }

            BindingContext = this;
        }

        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            b0.Scale = 1;
            b1.Scale = 1;
            b2.Scale = 1;
            b3.Scale = 1;
            b4.Scale = 1;
            b5.Scale = 1;
            b6.Scale = 1;

            Button emoji = sender as Button;
            emoji.Scale = 1.75;
            answer = int.Parse(emoji.ClassId.ToString());
            Console.WriteLine(answer);
            Vastaus.IsEnabled = true;
        }

        private async void Vastaa_Clicked(object sender, EventArgs e)
        {
            await Main.GetInstance().client.SendResult(answer.ToString());
            await Navigation.PushAsync(new OdotetaanVastauksiaClient());
        }
    }
}
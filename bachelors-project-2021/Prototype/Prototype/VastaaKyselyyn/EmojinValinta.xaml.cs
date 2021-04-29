using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmojinValinta : ContentPage
    {
        public string introMessage { get; set; }
        private int answer;

        public EmojinValinta()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            introMessage = Main.GetInstance().client.intro;

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

            ImageButton emoji = sender as ImageButton;
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
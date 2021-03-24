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
        private string answer;

        public EmojinValinta()
        {
            InitializeComponent();

            Emojis = SurveyManager.GetInstance().GetSurvey().emojis; //Change to survey provided by host
            Images = new List<string>();
            foreach(Emoji emoji in Emojis)
            {
                Images.Add(emoji.ImageSource);
            }

            BindingContext = this;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button emoji = sender as Button;
            answer = emoji.ClassId.ToString();
            Console.WriteLine(answer);
        }

        private void Vastaa_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TabbedViewHost());
        }
    }
}
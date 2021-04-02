using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Prototype
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            BindingContext = Main.GetInstance();


        }

        void InfoClicked(object sender, EventArgs e)
        {
            InfoPopUp.IsVisible = true;
        }

        void OpenMojiClicked(object sender, EventArgs e)
        {
            Uri OpenMoji = new Uri("https://openmoji.org/");
            Launcher.OpenAsync(OpenMoji);
        }

        void LicenseClicked(object sender, EventArgs e)
        {
            Uri License = new Uri("https://creativecommons.org/licenses/by-sa/4.0/legalcode");
            Launcher.OpenAsync(License);
        }

        void InfoOKClicked(object sender, EventArgs e)
        {
            //commented out testing for ActivityVote vote1candidates
            /*
            Main.GetInstance().host.data.AddEmojiResults(0);
            Main.GetInstance().host.data.AddEmojiResults(1);
            Main.GetInstance().host.data.AddEmojiResults(1);
            Main.GetInstance().host.data.AddEmojiResults(1);
            Main.GetInstance().host.data.AddEmojiResults(2);
            Main.GetInstance().host.data.AddEmojiResults(2);
            Main.GetInstance().host.data.AddEmojiResults(3);
            Main.GetInstance().host.data.AddEmojiResults(3);
            Main.GetInstance().host.data.AddEmojiResults(3);

            Console.WriteLine(Main.GetInstance().host.data.ToString());

            Emoji emoji = new Emoji();
            Survey survey = new Survey();
            ActivityVote aVote = new ActivityVote();
            aVote.calcVote1Candidates(survey.emojis, Main.GetInstance().host.data.GetEmojiResults());
            Console.WriteLine(survey.ToString());
            Console.WriteLine(aVote.ToString());
            */

            //commented out testing for ActivityVote vote2candidates
            /*
            Main.GetInstance().host.data.AddVote1Results("foo");
            Main.GetInstance().host.data.AddVote1Results("bar");
            Main.GetInstance().host.data.AddVote1Results("bar");
            Main.GetInstance().host.data.AddVote1Results("this");
            Main.GetInstance().host.data.AddVote1Results("this");
            Main.GetInstance().host.data.AddVote1Results("this");
            Main.GetInstance().host.data.AddVote1Results("foo");
            Main.GetInstance().host.data.AddVote1Results("bar");
            Main.GetInstance().host.data.AddVote1Results("this");

            Console.WriteLine(Main.GetInstance().host.data.ToString());

            ActivityVote aVote = new ActivityVote();
            aVote.calcVote2Candidates(Main.GetInstance().host.data.vote1Results);
            Console.WriteLine(aVote.ToString());
            */

            InfoPopUp.IsVisible = false;
        }


        async void LuoUusiClicked(object sender, EventArgs e)
        {
            // siirrytään "luo uus kysely" sivulle
            await Navigation.PushAsync(new LuoKyselyJohdatus()); 
        }

        async void TallennetutKyselytClicked(object sender, EventArgs e)
        {
            // siirrytään "Tallenetut Kyselyt" sivulle
            await Navigation.PushAsync(new TallennetutKyselyt()); 
        }

       void LiityKyselyynClicked(object sender, EventArgs e)
        {
            // Kysytään kyselyn avainkoodi, placeholder(Ei ole mitenkään yhdistetty backendin kanssa)
           popupSelection.IsVisible = true;

            
            
        }

     

       void Peruuta_Clicked(object sender, EventArgs e)
        {
            popupSelection.IsVisible = false;
        }

        void Ok_Clicked(object sender, EventArgs e)
        {
            // siirrytään "Liity Kyselyyn" sivulle jos annettu koodi on ok

            if (entry.Text=="KOODI") { 
            Navigation.PushAsync(new EmojinValinta());


            popupSelection.IsVisible = false;
            }

            else DisplayAlert("Virheellinen avainkoodi", "Syöttämälläsi avainkoodilla ei löydy avointa kyselyä", "OK");
        }
    }
    }
    


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
        // Launcher.OpenAsync is provided by Xamarin.Essentials.
        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public MainPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            BindingContext = Main.GetInstance();
            BindingContext = this;



        }

        //Device back button navigation test to close the application from the back button 
        protected override bool OnBackButtonPressed()
        {
             Device.BeginInvokeOnMainThread(async () => 
         {
               var res = await this.DisplayAlert("Do you really want to exit the application?", "","Yes", "No").ConfigureAwait(false);

               if (res) System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        });           
        return true;

        }



        void InfoClicked(object sender, EventArgs e)
        {
            InfoPopUp.IsVisible = true;
        }



        void InfoOKClicked(object sender, EventArgs e)
        {
            //commented out testing for ActivityVote vote1candidates
            /*
            Main.GetInstance().host.data.AddEmojiResults(2);
            Main.GetInstance().host.data.AddEmojiResults(5);
            Main.GetInstance().host.data.AddEmojiResults(4);
            Main.GetInstance().host.data.AddEmojiResults(4);
            Main.GetInstance().host.data.AddEmojiResults(2);
            Main.GetInstance().host.data.AddEmojiResults(5);
            Main.GetInstance().host.data.AddEmojiResults(5);
            Main.GetInstance().host.data.AddEmojiResults(4);
            Main.GetInstance().host.data.AddEmojiResults(2);
            Main.GetInstance().host.data.AddEmojiResults(3);

            Console.WriteLine(Main.GetInstance().host.data.ToString());

            Survey survey = new Survey();
            ActivityVote aVote = new ActivityVote();
            List<Emoji> emojis = survey.emojis;
            aVote.calcVote1Candidates(emojis, Main.GetInstance().host.data.GetEmojiResults());
            Console.WriteLine(survey.ToString());
            Console.WriteLine(aVote.ToString());
            Console.WriteLine("Time to vote in the 1st vote: {0}", aVote.vote1Timer);
            */

            //commented out testing for ActivityVote vote2candidates

            /*
            Dictionary<int, string> dict1 = new Dictionary<int, string>();
            dict1.Add(0,"foo");
            dict1.Add(1, "bar");
            dict1.Add(2, "heh");
            dict1.Add(3, "this");
            Dictionary<int, string> dict2 = new Dictionary<int, string>();
            dict2.Add(0, "fii");
            dict2.Add(1, "bar");
            dict2.Add(2, "heh");
            dict2.Add(3, "that");
            Main.GetInstance().host.data.AddVote1Results(dict1);
            Main.GetInstance().host.data.AddVote1Results(dict2);
            

            Console.WriteLine(Main.GetInstance().host.data.ToString());

            ActivityVote aVote = new ActivityVote();
            aVote.calcVote2Candidates(Main.GetInstance().host.data.GetVote1Results());
            Console.WriteLine(aVote.ToString());
            
            Main.GetInstance().host.data.AddVote2Results("Tunti ulkona");
            Main.GetInstance().host.data.AddVote2Results("Tunti ulkona");
            Main.GetInstance().host.data.AddVote2Results("5 min tauko");
            Main.GetInstance().host.data.AddVote2Results("5 min tauko");
            Main.GetInstance().host.data.AddVote2Results("5 min tauko");
            Main.GetInstance().host.data.AddVote2Results("Tunti ulkona");
            Main.GetInstance().host.data.AddVote2Results("Tunti ulkona");

            Console.WriteLine(Main.GetInstance().host.data.ToString());

            ActivityVote aVote = new ActivityVote();
            aVote.calcFinalResult(Main.GetInstance().host.data.GetVote2Results());
            Console.WriteLine(aVote.ToString());
            */
            InfoPopUp.IsVisible = false;
        }


      

        //

        async void LuoUusiClicked(object sender, EventArgs e)
        {
            // siirrytään "luo uus kysely" sivulle
            Main.GetInstance().CreateNewSurvey();
            await Navigation.PushAsync(new LuoKyselyJohdatus()); 
        }

        async void TallennetutKyselytClicked(object sender, EventArgs e)
        {
            // siirrytään "Tallenetut Kyselyt" sivulle
            Main.GetInstance().BrowseSurveys();
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

        async void Ok_Clicked(object sender, EventArgs e)
        {
            // siirrytään "Liity Kyselyyn" sivulle jos annettu koodi on ok
            if (await Main.GetInstance().JoinSurvey(entry.Text)) { 
                await Navigation.PushAsync(new EmojinValinta());
                popupSelection.IsVisible = false;
            }
            else await DisplayAlert("Virheellinen avainkoodi", "Syöttämälläsi avainkoodilla ei löydy avointa kyselyä", "OK");
        }
    }
    }
    


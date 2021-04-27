using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OdotetaanVastauksia : ContentPage
    {
        public string roomCode { get; set; } = "Roomcode: ";


        public OdotetaanVastauksia()
        {
            InitializeComponent();
            roomCode += SurveyManager.GetInstance().GetSurvey().RoomCode;
            BindingContext = this; 

            //Ei enää mahdollista päästä takaisin kysleyn luontiin painamalla navigoinnin backbuttonia 
            NavigationPage.SetHasBackButton(this, false);

            //actually run the survey
            Host();
        }

        private async void Host()
		{
            if (!await Main.GetInstance().HostSurvey())
            {
                //host survey ended in a fatal unexpected error, aborting survey.
                //pop to root and display error
                await Navigation.PopToRootAsync();
                await DisplayAlert("Kysely suljettiin automaattisesti", "Tapahtui odottamaton virhe.", "OK");
            }
        }

        private async void JatkaTuloksiin(object sender, EventArgs e)
        {
			//Back to main and error, if nobody joined the survey!
			if (Main.GetInstance().host.clientCount == 0)
			{
                Main.GetInstance().host.DestroyHost();
                await Navigation.PopToRootAsync();
                await DisplayAlert("Kysely suljettiin automaattisesti", "Kyselyyn ei saatu yhtään vastausta", "OK");
                return;
            }

            await Main.GetInstance().host.CloseSurvey();
            await Navigation.PushAsync(new TabbedViewHost());
        }

        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }

        private async void Peruuta(object sender, EventArgs e)
        {

            //Varmistu kyselyn peruuttamisen yhteydessä

            var res = await DisplayAlert("Oletko varma että tahdot peruuttaa kyselyn?", "", "Kyllä", "Ei");

            if (res == true) {
                Main.GetInstance().host.DestroyHost();
                await Navigation.PopToRootAsync();
            }
            else return; 
           

        }
    }
}
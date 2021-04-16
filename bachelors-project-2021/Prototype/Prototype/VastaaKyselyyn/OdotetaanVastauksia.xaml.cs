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
        }

        private async void JatkaTuloksiin(object sender, EventArgs e)
        {
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
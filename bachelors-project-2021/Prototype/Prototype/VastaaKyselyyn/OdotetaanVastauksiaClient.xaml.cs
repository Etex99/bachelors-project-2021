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
    public partial class OdotetaanVastauksiaClient : ContentPage
    {
        public OdotetaanVastauksiaClient()
        {
            InitializeComponent();
            //Ei enää mahdollista päästä takaisin kysleyn luontiin painamalla navigoinnin backbuttonia 
            NavigationPage.SetHasBackButton(this, false);
            ReceiveSurveyData();
        }
        private async void ReceiveSurveyData() {
            bool success = await Main.GetInstance().client.ReceiveSurveyDataAsync();
			if (success)
			{
                await Navigation.PushAsync(new TabbedViewClient());
			} else {
                Main.GetInstance().client.DestroyClient();
                await Navigation.PopToRootAsync();
            }
        }        

        private async void Poistu(object sender, EventArgs e)
        {

            // Varmistus kyselystä poistumisen yhteydessä

             var res = await DisplayAlert("Oletko varma että tahdot poistua kyselystä?", "", "Kyllä", "Ei");

            if (res == true)
            {
                Main.GetInstance().client.DestroyClient();
                await Navigation.PopToRootAsync();
            }
            else return;
            

        }

        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }
    }
}
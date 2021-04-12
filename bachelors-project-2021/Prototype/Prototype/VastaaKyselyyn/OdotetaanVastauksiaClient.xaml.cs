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
            Task.Run(async () => {

                bool success = await Main.GetInstance().client.ReceiveSurveyDataAsync();
				if (success)
				{
                    //received correct summary, now we can show it :)
                    await Navigation.PushAsync(new TabbedViewClient());
                    return;
                }
                //received some actual garbage? cannot comprehend this... returning to main menu
                Main.GetInstance().client.DestroyClient();
                await Navigation.PopToRootAsync();
            });
        }
        
        private void Poistu(object sender, EventArgs e)
        {
            Main.GetInstance().client.DestroyClient();
            Navigation.PopToRootAsync();
        }


        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }
    }
}
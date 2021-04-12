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
        public OdotetaanVastauksia()
        {
            InitializeComponent();

            //Ei enää mahdollista päästä takaisin kysleyn luontiin painamalla navigoinnin backbuttonia 
            NavigationPage.SetHasBackButton(this, false);
        }

        private void JatkaTuloksiin(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TabbedViewHost());
            Main.GetInstance().host.CloseSurvey();
        }

        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }
    }
}
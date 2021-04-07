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
        }

        private void Poistu(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AktiviteettiäänestysToka : ContentPage
    {
        // pitää muuttaa siten, että drop down menu/popup pääsee käsiksi aktiviteetteihin
        //Tätä pitää muuttaa siten, että äänestyksessä mukana vain äänestyksen ensimmäisessä vaiheessa valitut aktiviteetit

        public IList<string> Items { get; set; }
        public string Selected { get; set; } = null;
        public AktiviteettiäänestysToka()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            //alustus
            Items = Main.GetInstance().client.voteCandidates2;

            BindingContext = this;
        }


        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }

        async void JatkaButtonClicked(object sender, EventArgs e)
        {


            //siirrytään "Äänestksen tulokset" sivulle 
            await Navigation.PushAsync(new AktiviteettiäänestysTulokset());
        }
    }
}
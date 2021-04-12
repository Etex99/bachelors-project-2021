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

            Vote2();
        }
        private async void Vote2() {
            await Task.Delay(Main.GetInstance().client.vote2Time * 1000);
            if (Selected != null)
            {
                await Main.GetInstance().client.SendVote2Result(Selected);
            }

            bool success = await Main.GetInstance().client.ReceiveVoteResult();
            if (success)
            {
                //received result changing view
                await Navigation.PushAsync(new AktiviteettiäänestysTulokset());
            }
        }


        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }
    }
}
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

        private int _countSeconds = 10;

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
            _countSeconds = Main.GetInstance().client.vote2Time;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                _countSeconds--;

                timer.Text = _countSeconds.ToString();


                if (_countSeconds == 0)
                {
                  
                        return false;
              


                }

                return Convert.ToBoolean(_countSeconds);
            });

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
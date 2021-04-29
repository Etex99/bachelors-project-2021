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
    public partial class TulostenOdotus : ContentPage
    {

        private int _countSeconds = 10;

        public TulostenOdotus()
        {
            InitializeComponent();

            //poistetaan turha navigointipalkki
            NavigationPage.SetHasNavigationBar(this, false);

            Main.GetInstance().host.StartActivityVote();

            //timer set to vote times, cooldowns, plus one extra
            _countSeconds = Main.GetInstance().host.voteCalc.vote1Timer + Main.GetInstance().host.voteCalc.vote2Timer + ( 3 * Main.GetInstance().host.voteCalc.coolDown);
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                _countSeconds--;

                 timer.Text = _countSeconds.ToString();

				if (Main.GetInstance().host.isVoteConcluded)
				{
                    Navigation.PushAsync(new AktiviteettiäänestysTulokset());
                    return false;
                }
                    

                 if (_countSeconds == 0) {
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        return false;
                    });

                  
                 }

                return Convert.ToBoolean(_countSeconds);
            });
        }

        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }




    }
}
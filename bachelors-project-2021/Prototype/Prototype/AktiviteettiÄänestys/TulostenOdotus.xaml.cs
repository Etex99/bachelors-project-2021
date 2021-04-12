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

            _countSeconds = Main.GetInstance().host.voteCalc.vote1Timer + Main.GetInstance().host.voteCalc.vote2Timer;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                _countSeconds--;

                 timer.Text = _countSeconds.ToString();
                

                 if (_countSeconds == 0) {
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        Navigation.PushAsync(new AktiviteettiäänestysTulokset());
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
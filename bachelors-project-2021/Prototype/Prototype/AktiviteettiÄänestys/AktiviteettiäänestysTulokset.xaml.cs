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
    public partial class AktiviteettiäänestysTulokset : ContentPage
    {
        public AktiviteettiäänestysTulokset()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            if (Main.GetInstance().state == Main.MainState.Participating)
            {
                result.Text = Main.GetInstance().client.voteResult;
            }
            else
            {
                result.Text = Main.GetInstance().host.data.voteResult;
            }
            
        }

        async void PoistuClicked(object sender, EventArgs e)
        {
			if (Main.GetInstance().state == Main.MainState.Participating)
			{
                Main.GetInstance().client.DestroyClient();
			} else {
                Main.GetInstance().host.DestroyHost();
			}
            await Navigation.PushAsync(new MainPage());
        }


        // Device back button navigation 
        protected override bool OnBackButtonPressed()
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("", "Poistutaanko tulsoten tarkastelusta ? ", "Kyllä", "Ei"))
                {
                    base.OnBackButtonPressed();

                    await Navigation.PushAsync(new MainPage());
                }
            });

            return true;


        
       
        }
    }
}
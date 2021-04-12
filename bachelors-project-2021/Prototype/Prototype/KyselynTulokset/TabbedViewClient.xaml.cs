using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedViewClient : TabbedPage
    {
        public TabbedViewClient()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);


            //Siirrytään aktiviteettin äänestykseen 10sec kuluttua. (timer testi) 
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                Navigation.PushAsync(new AktiviteettiäänestysEka());
                return false;
            });
        }

        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }
    }
}
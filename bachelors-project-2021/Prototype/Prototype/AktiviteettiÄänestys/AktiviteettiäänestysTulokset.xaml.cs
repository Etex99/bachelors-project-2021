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
        }

        async void PoistuClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}
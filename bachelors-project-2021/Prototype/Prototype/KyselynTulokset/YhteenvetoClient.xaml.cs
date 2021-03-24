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
    public partial class YhteenvetoClient : ContentPage
    {
        public YhteenvetoClient()
        {
            InitializeComponent();
        }
        async void PoistuClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }


        //Tämä painike on ihan vaan sitä varten että pääsee näkemään ja testaamaan äänestyksen näytöt
        async void TestaustaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AktiviteettiäänestysEka());
        }
    }
}
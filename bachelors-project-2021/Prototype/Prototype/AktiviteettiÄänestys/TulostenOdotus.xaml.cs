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
        public TulostenOdotus()
        {
            InitializeComponent();

            //poistetaan turha navigointipalkki
            NavigationPage.SetHasNavigationBar(this, false); 

            //Siirrytään aktiviteettin äänestykseen tuloksiin 10skuluttua. (timer testi) 
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {   
                Navigation.PushAsync(new AktiviteettiäänestysTulokset());
               
                return false;
            });
        }
    }
}
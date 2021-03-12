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
    public partial class LuoKyselyLopetus : ContentPage
    {
        public LuoKyselyLopetus()
        {
            InitializeComponent();
        }
        async void TallennaJaPoistuClicked(object sender, EventArgs e)
        {
            // siirrytään etusivulle 
            await Navigation.PushAsync(new MainPage()); ;
        }
    }
}
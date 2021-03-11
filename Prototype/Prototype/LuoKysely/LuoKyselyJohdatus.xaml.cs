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
    public partial class LuoKyselyJohdatus : ContentPage
    {
        public LuoKyselyJohdatus()
        {
            InitializeComponent();
        }

        async void JatkaButtonClicked(object sender, EventArgs e)
        {
            // siirrytään "luo uus kysely 2/3" sivulle 
            await Navigation.PushAsync(new LuoKyselyEmojit()); ;
        }

        async void PeruutaButtonClicked(object sender, EventArgs e)
        {
            // siirrytään etusivulle
            await Navigation.PushAsync(new MainPage()); ;
        }
    }

  
}
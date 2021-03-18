using System;
using System.IO;
using Xamarin.Forms;

namespace Prototype
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            BindingContext = Main.GetInstance();


        }



        async void LuoUusiClicked(object sender, EventArgs e)
        {
            // siirrytään "luo uus kysely" sivulle
            await Navigation.PushAsync(new LuoKyselyJohdatus()); 
        }

        async void TallennetutKyselytClicked(object sender, EventArgs e)
        {
            // siirrytään "Tallenetut Kyselyt" sivulle
            await Navigation.PushAsync(new TallennetutKyselyt()); 
        }
        async void LiityKyselyynClicked(object sender, EventArgs e)
        {
            // siirrytään "Liity Kyselyyn" sivulle
            await Navigation.PushAsync(new EmojinValinta());
        }


    }
}

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

       void LiityKyselyynClicked(object sender, EventArgs e)
        {
            // Kysytään kyselyn avainkoodi, placeholder(Ei ole mitenkään yhdistetty backendin kanssa)
           popupSelection.IsVisible = true;

            
            
        }

     

       void Peruuta_Clicked(object sender, EventArgs e)
        {
            popupSelection.IsVisible = false;
        }

        void Ok_Clicked(object sender, EventArgs e)
        {
            // siirrytään "Liity Kyselyyn" sivulle jos annettu koodi on ok

            if (entry.Text=="KOODI") { 
            Navigation.PushAsync(new EmojinValinta());


            popupSelection.IsVisible = false;
            }

            else DisplayAlert("Virheellinen avainkoodi", "Syöttämälläsi avainkoodilla ei löydy avointa kyselyä", "OK");
        }
    }
    }
    


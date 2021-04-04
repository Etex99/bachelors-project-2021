using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Essentials;
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

        void InfoClicked(object sender, EventArgs e)
        {
            InfoPopUp.IsVisible = true;
        }

        void OpenMojiClicked(object sender, EventArgs e)
        {
            Uri OpenMoji = new Uri("https://openmoji.org/");
            Launcher.OpenAsync(OpenMoji);
        }

        void LicenseClicked(object sender, EventArgs e)
        {
            Uri License = new Uri("https://creativecommons.org/licenses/by-sa/4.0/legalcode");
            Launcher.OpenAsync(License);
        }

        void InfoOKClicked(object sender, EventArgs e)
        {
            InfoPopUp.IsVisible = false;
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

        async void Ok_Clicked(object sender, EventArgs e)
        {
            // siirrytään "Liity Kyselyyn" sivulle jos annettu koodi on ok
            if (await Main.GetInstance().JoinSurvey(entry.Text)) { 
                await Navigation.PushAsync(new EmojinValinta());
                popupSelection.IsVisible = false;
            }
            else await DisplayAlert("Virheellinen avainkoodi", "Syöttämälläsi avainkoodilla ei löydy avointa kyselyä", "OK");
        }
    }
    }
    


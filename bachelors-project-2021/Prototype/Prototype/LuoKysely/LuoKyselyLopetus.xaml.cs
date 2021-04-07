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


            if (NameEditor.Text != null && KeyEditor.Text != null)
            {

                SurveyManager man = SurveyManager.GetInstance();
                //save survey code
                man.GetSurvey().RoomCode = KeyEditor.Text;
                //save survey
                man.SaveSurvey(NameEditor.Text + ".txt");

                // siirrytään etusivulle 
                await Navigation.PushAsync(new MainPage());

            }

            else await DisplayAlert("Nimi tai avainkoodi puuttuu", "Sinun on asetettava kyselylle nimi ja avainkoodi", "OK");

        }

        void JaaClicked(object sender, EventArgs e)
        {


            if(NameEditor.Text != null && KeyEditor.Text != null) {

                // Kysytään kyselyn tallentamisesta
                popupSelection.IsVisible = true; 
            }

            else DisplayAlert("Nimi tai avainkoodi puuttuu", "Sinun on asetettava kyselylle nimi ja avainkoodi", "OK");

        }

        void X_Clicked(object sender, EventArgs e)
        {

           // Suljetaan popup
            popupSelection.IsVisible = false;

        }

        async void Ei_Clicked(object sender, EventArgs e)
        {
            // siirrytään OdotettaanVastauksia, ei tallenneta kyselyä
            await Navigation.PushAsync(new OdotetaanVastauksia()); ;
            Main.GetInstance().HostSurvey();
        }

        async void Kyllä_Clicked(object sender, EventArgs e)
        {

            //kyselyn tallennus!

            SurveyManager man = SurveyManager.GetInstance();
            //save survey code
            man.GetSurvey().RoomCode = KeyEditor.Text;
            //save survey
            man.SaveSurvey(NameEditor.Text + ".txt");

            // siirrytään OdotettaanVastauksia sivulle 
            await Navigation.PushAsync(new OdotetaanVastauksia());
            Main.GetInstance().HostSurvey();


        }
    }
}
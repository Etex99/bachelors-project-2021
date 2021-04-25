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

            NameEditor.Text = SurveyManager.GetInstance().GetSurvey().Name;
            KeyEditor.Text = SurveyManager.GetInstance().GetSurvey().RoomCode;
        }
        async void TallennaJaPoistuClicked(object sender, EventArgs e)
        {
            if (NameEditor.Text != null && KeyEditor.Text != null)
            {

                SurveyManager man = SurveyManager.GetInstance();
                //save survey code and name
                man.GetSurvey().RoomCode = KeyEditor.Text;
                man.GetSurvey().Name = NameEditor.Text;
                //save survey
                man.SaveSurvey(NameEditor.Text + ".txt");

                // siirrytään etusivulle 
                await Navigation.PushAsync(new MainPage());

            }

            else await DisplayAlert("Nimi tai avainkoodi puuttuu", "Sinun on asetettava kyselylle nimi ja avainkoodi", "OK");

        }

        async void PeruutaClicked(object sender, EventArgs e)
        {

            var res = await DisplayAlert("Tahdotko varmasti keskeytää kyselyn luonnin?", "", "Kyllä", "Ei");

            if (res == true)
            {
                //survey resetoidaan
                SurveyManager.GetInstance().ResetSurvey();

                //Jos ollaan edit tilassa, niin siirrytään takaisin kyselyntarkastelu sivulle, muutoin main menuun
                if (Main.GetInstance().GetMainState() == Main.MainState.Editing)
                {
                    Main.GetInstance().BrowseSurveys();
                    await Navigation.PopAsync();
                }
                else
                {
                    // siirrytään etusivulle
                    await Navigation.PopToRootAsync();
                }

            }
            else return;
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
            man.GetSurvey().Name = NameEditor.Text;
            //save survey
            man.SaveSurvey(NameEditor.Text + ".txt");

            // siirrytään OdotettaanVastauksia sivulle 
            await Navigation.PushAsync(new OdotetaanVastauksia());
            Main.GetInstance().HostSurvey();


        }
    }
}
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
            SurveyManager man = SurveyManager.GetInstance();
            //save survey code
            man.GetSurvey().RoomCode = KeyEditor.Text;
            //save survey
            man.SaveSurvey(NameEditor.Text + ".txt");
            
            // siirrytään etusivulle 
            await Navigation.PushAsync(new MainPage()); ;
        }

        async void JaaClicked(object sender, EventArgs e)
        {
           

            // siirrytään Yhteenveto Host
            await Navigation.PushAsync(new YhteenVetoHost()); ;
        }
    }
}
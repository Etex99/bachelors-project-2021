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

        public IList<string> introMessage { get; set; }



        public LuoKyselyJohdatus()
        {
            InitializeComponent();

            introMessage = Const.intros;
           

       
            BindingContext = this;


 
        }

        private void btnPopupButton_Clicked(object sender, EventArgs e)
        {
           

            if (popupSelection.IsVisible == false)
            {

                popupSelection.IsVisible = true; 
            }

            else if (popupSelection.IsVisible == true)
            {

                popupSelection.IsVisible = false;
            }

        }

   


        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = e.CurrentSelection[0] as string;

            //Change the text of the button based on selected intro message
            
            if (selectedItem != null) { 
               JButton.Text = selectedItem;
                JatkaBtn.IsEnabled = true;
            }

            else
                JButton.Text = "Valitse johdatuslause";
        }



        async void JatkaButtonClicked(object sender, EventArgs e)
        {
            //kyselyn johdatuslause asetetaan.
            // SurveyManager.GetInstance().GetSurvey().introMessage = 
          

            //siirrytään "luo uus kysely 2/3" sivulle 
            await Navigation.PushAsync(new LuoKyselyEmojit());
        }

        async void PeruutaButtonClicked(object sender, EventArgs e)
        {
            //survey resetoidaan
            SurveyManager.GetInstance().ResetSurvey();

            // siirrytään etusivulle
            await Navigation.PopToRootAsync();
        }
    }

  
}
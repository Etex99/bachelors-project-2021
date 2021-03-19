﻿using System;
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


        //Testausta varten
        public IList<CollectionItem> Intros { get; private set; }

        public class CollectionItem
        {
            public string introMessage { get; set; }

            public override string ToString()
            {
                return introMessage;
            }
        }



      

        public LuoKyselyJohdatus()
        {
            InitializeComponent();

            //Testausta varten
            Intros = new List<CollectionItem>();

            Intros.Add(new CollectionItem
            {
                    introMessage="Testi intro1 "
            });

            Intros.Add(new CollectionItem
            {
                introMessage = "Testi intro2 "
            });

            Intros.Add(new CollectionItem
            {
                introMessage = "Testi intro3 "
            });

            BindingContext = this;


            //alustetaan näkymä avatun kyselyn introviestillä
            // = SurveyManager.GetInstance().GetSurvey().introMessage;
        }

        private void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            popupSelection.IsVisible = true;

        }

        private void Sulje_Clicked(object sender, EventArgs e)
        {
            popupSelection.IsVisible = false;
        }


        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionItem selectedItem = e.CurrentSelection[0] as CollectionItem;
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
            await Navigation.PushAsync(new MainPage());
        }
    }

  
}
﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    //joku taitava liittäkööt tämän backendin kanssa

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KyselynTarkastelu : ContentPage
    {

        public IList<CollectionItem> Emojit { get; private set; } = null;
        public static string surveyName;
        public string roomCode { get; set; } = "Roomcode: ";
        public string introMessage { get; set; } = "Intro: ";

        public static void SetSurveyName(string name)
        {
            surveyName = name;
        }

        public class CollectionItem
        {
            public Emoji Item { get; set; } = null;
            public IList<string> ActivityChoises { get; set; } = null;
            public string Color { get; set; } = null;
        }


        public KyselynTarkastelu()
        {
            InitializeComponent();

            //alustetaan emojit kyselyn emojeilla
            Emojit = new List<CollectionItem>();
            List<Emoji> temp = SurveyManager.GetInstance().GetSurvey().emojis;
            
            //Jeesusteippi ratkaisu, joka on kommentoitu poissa (IF ALL ELSE FAILS T: Discord nörtti)
            /*
            if (temp.Count > 7)
            {
                temp.RemoveRange(7, 7);
            }
            */

            //asetetaan otsikoksi kyselyn nimi
            title.Text = surveyName;

            //asetetaan kyselyn roomCode ja intro
            roomCode += SurveyManager.GetInstance().GetSurvey().RoomCode;
            introMessage += SurveyManager.GetInstance().GetSurvey().introMessage;
            

            //alustetaan radionappien valinnat
            //ei saa kyseenalaistaa tätä toteutusta, radionappeihin ei oikeastaan pääse käsiksi collection view layoutin sisältä
            foreach (var item in temp)
            {
                CollectionItem i = new CollectionItem();
                i.Item = item;
                i.ActivityChoises = item.activities;
                switch (item.Impact)
                {
                    case "positive":
                        i.Color = "Green";
                        break;
                    case "neutral":
                        i.Color = "Yellow";
                        break;
                    case "negative":
                        i.Color = "Red";
                        break;
                }
                Emojit.Add(i);
            }

            BindingContext = this;
        }

        //Device back button navigation 
        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new TallennetutKyselyt());
            return true;

        }


        async void JaaClicked(object sender, EventArgs e)
        {
            //siirrytään OdotetaanVastauksia sivulle
            await Navigation.PushAsync(new OdotetaanVastauksia());
            //Jaetaan kysely 
            Main.GetInstance().HostSurvey();
        }
        void MuokkaaClicked(object sender, EventArgs e)
        {

            // Siirrytään kyselyn muokkaukseen 
        }

        void PoistaClicked(object sender, EventArgs e)
        {
            popupSelection.IsVisible = true;

        }

        void X_Clicked(object sender, EventArgs e)
        {

            // Suljetaan popup
            popupSelection.IsVisible = false;

        }

        void Ei_Clicked(object sender, EventArgs e)
        {
            // siirrytään yhteenveto Host sivulle, ei tallenneta kyselyä
             popupSelection.IsVisible = false;
        }

        async void Kyllä_Clicked(object sender, EventArgs e)
        { 

            //kyselyn Poistaminen!
            SurveyManager.GetInstance().DeleteSurvey(surveyName);
            // siirrytään Tallenetut kyselyt sivulle 
           
             await Navigation.PushAsync(new TallennetutKyselyt());

        }


        void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            // :DDD
            if (sender is Button b && b.Parent is Grid g && g.Children[2] is Frame f)
            {
                if (f.IsVisible == false) { 
              
                    f.IsVisible = true;
                }

                else if (f.IsVisible == true)
                {

                    f.IsVisible = false;
                }

            }               
         } 
        
        }
    }

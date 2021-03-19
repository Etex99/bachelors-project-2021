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
    public partial class KyselynTarkastelu : ContentPage
    {


        public IList<CollectionItem> Emojit { get; private set; }

        public class CollectionItem
        {
            public Emoji Item { get; set; } = null;
            public bool IsNegative { get; set; } = false;
            public bool IsNeutral { get; set; } = false;
            public bool IsPositive { get; set; } = false;
        }


        public KyselynTarkastelu()
        {
            InitializeComponent();

            //alustetaan emojit kyselyn emojeilla
            Emojit = new List<CollectionItem>();
            List<Emoji> temp = SurveyManager.GetInstance().GetSurvey().emojis;

            //alustetaan radionappien valinnat
            //ei saa kyseenalaistaa tätä toteutusta, radionappeihin ei oikeastaan pääse käsiksi collection view layoutin sisältä
            foreach (var item in temp)
            {
                CollectionItem i = new CollectionItem();
                i.Item = item;
                switch (item.Impact)
                {
                    case "positive":
                        i.IsPositive = true;
                        break;
                    case "neutral":
                        i.IsNeutral = true;
                        break;
                    case "negative":
                        i.IsNegative = true;
                        break;
                }
                Emojit.Add(i);
            }

            BindingContext = this;
        }

         void JaaClicked(object sender, EventArgs e)
        {

            //Jaetaan kysely 
        }

      void MuokkaaClicked(object sender, EventArgs e)
        {

            // Siirrytään kyselyn muokkaukseen 
        }

       void PoistaClicked(object sender, EventArgs e)
        {
      
            // Poistetaan kysely 

        }
    }
}
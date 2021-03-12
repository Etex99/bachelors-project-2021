﻿using System;
using System.IO;
using Xamarin.Forms;

namespace Prototype
{
    public partial class MainPage : ContentPage
    {
        string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

        public MainPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();


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


    }
}

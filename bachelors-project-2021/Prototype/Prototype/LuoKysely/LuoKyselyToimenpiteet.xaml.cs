using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LuoKyselyToimenpiteet : ContentPage
    {
        public IList<CollectionItem> Items { get; set; }
        public class CollectionItem
        {
            public Emoji Emoji { get; set; }
            public IList<string> ActivityChoises { get; set; }
            public ObservableCollection<object> Selected { get; set; }

			public CollectionItem(Emoji emoji, IList<string> activities)
			{
				Emoji = emoji;
				ActivityChoises = activities;

                Selected = new ObservableCollection<object>();
				foreach (var item in emoji.activities)
				{
                    Selected.Add(ActivityChoises[ActivityChoises.IndexOf(item)]);
				}
			}
        }

        public LuoKyselyToimenpiteet()
        {
            InitializeComponent();

            //alustus
            Items = new List<CollectionItem>();   
			foreach (var item in SurveyManager.GetInstance().GetSurvey().emojis)
			{
                Items.Add(new CollectionItem(item, Const.activities[item.ID]));
			}

            BindingContext = this;
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

        void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            // :DDD
            if (sender is Button b && b.Parent is Grid g && g.Children[2] is Frame f)
			{
                if (f.IsVisible == false)
                {

                    f.IsVisible = true;
                }

                else if (f.IsVisible == true)
                {

                    f.IsVisible = false;
                }
            }
        }

    

        /*
        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if (sender is CollectionView cv && cv.SelectionChangedCommandParameter is CollectionItem item)
			{
                
			}
        }
        */

        async void JatkaButtonClicked(object sender, EventArgs e)
        {
            //asetetaan emojit survey olioon
            List<Emoji> tempEmojis = new List<Emoji>();
			foreach (var item in Items)
			{
                List<string> tempActivities = new List<string>();
				foreach (var selection in item.Selected)
				{
                    tempActivities.Add(selection as string);
				}
                item.Emoji.activities = tempActivities;
                tempEmojis.Add(item.Emoji);
			}
            SurveyManager.GetInstance().GetSurvey().emojis = tempEmojis;            

            // siirrytään "Luo kysely -lopetus" sivulle 
            await Navigation.PushAsync(new LuoKyselyLopetus()); ;
        }
    }
}
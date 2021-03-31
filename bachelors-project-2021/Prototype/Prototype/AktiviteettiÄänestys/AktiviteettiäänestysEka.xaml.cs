using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AktiviteettiäänestysEka : ContentPage
    {

        // Copy paste go brrrrrrrR
        //Tätä pitänee muuttaa siten, että äänestyksessä mukana vain kyselyn luonnin aikana valitut aktiviteetit

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

        public AktiviteettiäänestysEka()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            //alustus
            Items = new List<CollectionItem>();
            foreach (var item in SurveyManager.GetInstance().GetSurvey().emojis)
            {
                Items.Add(new CollectionItem(item, Const.activities[item.ID]));
            }

            BindingContext = this;
        }


        void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            // Edelleen :DDD
            if (sender is Button b && b.Parent is Grid g && g.Children[2] is Frame f)
            {
                f.IsVisible = true;
            }
        }

        private void Sulje_Clicked(object sender, EventArgs e)
        {
            //Edelleen hieno ratkasu :DDD
            if (sender is Button b && b.Parent.Parent.Parent is Frame f)
            {
                f.IsVisible = false;
            }
        }


        async void JatkaButtonClicked(object sender, EventArgs e)
        {
            //Copy and Paste
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

            // siirrytään Aktiviteetti äänestys 2/2
            await Navigation.PushAsync(new AktiviteettiäänestysToka()); ;
        }
    }
}
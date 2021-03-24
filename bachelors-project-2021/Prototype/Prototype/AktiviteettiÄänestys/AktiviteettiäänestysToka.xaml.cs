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
    public partial class AktiviteettiäänestysToka : ContentPage
    {
        // Copy paste go brrrrrrrR

        // pitää muuttaa siten, että drop down menu/popup pääsee käsiksi aktiviteetteihin
        //Tätä pitää muuttaa siten, että äänestyksessä mukana vain äänestyksen ensimmäisessä vaiheessa valitut aktiviteetit

        public IList<CollectionItem> Items { get; set; }
        public class CollectionItem
        {   
            //Täällä ei tarvita emojeja
           // public Emoji Emoji { get; set; }
            public IList<string> ActivityChoises { get; set; }
            public ObservableCollection<object> Selected { get; set; }

            public CollectionItem(Emoji emoji, IList<string> activities)
            {
            //    Emoji = emoji;
                ActivityChoises = activities;

                Selected = new ObservableCollection<object>();
                foreach (var item in emoji.activities)
                {
                    Selected.Add(ActivityChoises[ActivityChoises.IndexOf(item)]);
                }
            }
        }
        public AktiviteettiäänestysToka()
        {
            InitializeComponent();


            // Copy paste go brrrrrrrRRRRR

            //alustus
            Items = new List<CollectionItem>();
            foreach (var item in SurveyManager.GetInstance().GetSurvey().emojis)
            {
                Items.Add(new CollectionItem(item, Const.activities[item.ID]));
            }

            BindingContext = this;
        }



        private void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            popupSelection.IsVisible = true;


        }

        private void Sulje_Clicked(object sender, EventArgs e)
        {
            popupSelection.IsVisible = false;


        }


      

        async void JatkaButtonClicked(object sender, EventArgs e)
        {


            //siirrytään "Äänestksen tulokset" sivulle 
            await Navigation.PushAsync(new AktiviteettiäänestysTulokset());
        }
    }
}
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
    public partial class LuoKyselyToimenpiteet : ContentPage
    {
        //Testausta varten
        public IList<CollectionItemTwo> Activities { get; private set; }

        public class CollectionItemTwo
        {
            public string activity { get; set; }

            public override string ToString()
            {
                return activity;
            }
        }

        //emojit
        public IList<CollectionItem> Emojit { get; private set; }


        public class CollectionItem
        {
            public Emoji Item { get; set; } = null;
            public bool IsNegative { get; set; } = false;
            public bool IsNeutral { get; set; } = false;
            public bool IsPositive { get; set; } = false;
        }



        public LuoKyselyToimenpiteet()
        {
            InitializeComponent();

            //Popup testausta

           




            //Testausta varten
            Activities = new List<CollectionItemTwo>();

            Activities.Add(new CollectionItemTwo
            {
                activity = "Testi1 "
            });

            Activities.Add(new CollectionItemTwo
            {
                activity = "Testi2 "
            });

            Activities.Add(new CollectionItemTwo
            {
                activity = "Testi3 "
            });

       


            //Emojien alustus 

            Emojit = new List<CollectionItem>();
            List<Emoji> temp = SurveyManager.GetInstance().GetSurvey().emojis;

            foreach (var item in temp)
            {
                CollectionItem i = new CollectionItem();
                i.Item = item;
              
                Emojit.Add(i);
            }


            BindingContext = this;

        }
      

        void btnPopupButton_Clicked(object sender, EventArgs e)
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
            //asetetaan emojit survey olioon
            List<Emoji> temp = new List<Emoji>();
            SurveyManager.GetInstance().GetSurvey().emojis = temp;

            // siirrytään "Luo kysely -lopetus" sivulle 
            await Navigation.PushAsync(new LuoKyselyLopetus()); ;
        }
    }
}
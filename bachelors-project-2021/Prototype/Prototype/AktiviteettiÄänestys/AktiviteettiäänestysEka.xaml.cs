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
            public string Selected { get; set; } = null;

            public CollectionItem(Emoji emoji, IList<string> activities)
            {
                Emoji = emoji;
                ActivityChoises = activities;

                foreach (var item in emoji.activities)
                {
                    Console.WriteLine("Item: {0}", item);
                }
            }
        }

        public AktiviteettiäänestysEka()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            //alustus
            List<Emoji> Emojis = Main.GetInstance().client.survey.emojis;
            Items = new List<CollectionItem>();

            foreach (var item in Main.GetInstance().client.voteCandidates1)
            {
                Console.WriteLine("Key: {0}, Value: {1}", item.Key, Emojis[item.Key].activities);
                Items.Add(new CollectionItem(Emojis[item.Key], Emojis[item.Key].activities));
            }

            BindingContext = this;

            Task.Run(async () =>
            {
                bool success = await Main.GetInstance().client.ReceiveVote2Candidates();
				if (success)
				{
                    //received vote 2, sending vote 1 results as is
                    Jatka();
				}
            });
        }


        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }


        void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            // Edelleen :DDD
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

    


        async void Jatka()
        {
            //Copy and Paste
            //asetetaan emojit survey olioon
            Dictionary<int, string> answer = new Dictionary<int, string>();
            foreach (var item in Items)
            {
                if(item == null)
                {
                    break;
                }
                answer.Add(item.Emoji.ID, item.Selected);
            }
            await Main.GetInstance().client.SendVote1Result(answer);
            // siirrytään Aktiviteetti äänestys 2/2
            await Navigation.PushAsync(new AktiviteettiäänestysToka()); ;
        }
    }
}
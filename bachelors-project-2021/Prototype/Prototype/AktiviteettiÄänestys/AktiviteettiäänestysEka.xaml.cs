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

        private int _countSeconds = 10;
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

            Vote1();
        }
        
        private async void Vote1() {
         //   await Task.Delay(Main.GetInstance().client.vote1Time * 1000);

              _countSeconds = Main.GetInstance().client.vote1Time;
             Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                _countSeconds--;

                timer.Text = _countSeconds.ToString();


                if (_countSeconds == 0)
                {
                   
                        return false;
  
                }

                return Convert.ToBoolean(_countSeconds);
            });  


            FinishVote1();
            bool success = await Main.GetInstance().client.ReceiveVote2Candidates();
            if (success && _countSeconds == 0)
            {
                //received vote 2 changing view
                await Navigation.PushAsync(new AktiviteettiäänestysToka());
            } 
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

    


        async void FinishVote1()
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
        }
    }
}
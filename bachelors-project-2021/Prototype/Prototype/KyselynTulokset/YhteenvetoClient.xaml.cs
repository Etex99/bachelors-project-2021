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
    public partial class YhteenvetoClient : ContentPage
    {
        public IList<string> resultImages { get; set; }
        public IList<double> resultScale { get; set; }
        public YhteenvetoClient()
        {
            InitializeComponent();
            resultImages = new List<string>();
            resultScale = new List<double>();

            int count = 0;
            double calculateScale = 0.0;
            Dictionary<int, int> sorted = new Dictionary<int, int>();
            foreach (var item in Main.GetInstance().client.summary.GetEmojiResults().OrderByDescending(item => item.Value))
            {
                sorted.Add(item.Key, item.Value);
                count += item.Value;
            }

            foreach (int key in sorted.Keys)
            {
                resultImages.Add("emoji" + key.ToString() + ".png");
            }

            if (count == 0)
            {
                foreach (int value in sorted.Values)
                {
                    resultScale.Add(0);
                }
            }
            else
            {
                foreach (int value in sorted.Values)
                {
                    calculateScale = 3 * (double)value / count;
                    resultScale.Add(calculateScale);
                }
            }

            BindingContext = this;
        }
        async void PoistuClicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("Oletko varma että tahdot poistua kyselystä?", "", "Kyllä", "Ei");

            if (res == true)
            {
                Main.GetInstance().client.DestroyClient();
                await Navigation.PopToRootAsync();
            }
            else return;

        }
    }
}
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
            foreach (KeyValuePair<int, int> item in Main.GetInstance().client.summary.GetEmojiResults().OrderByDescending(key => key.Value))
            {
                sorted.Add(item.Key, item.Value);
                count += item.Value;
            }
            foreach (int key in sorted.Keys)
            {
                resultImages.Add(Main.GetInstance().client.survey.emojis[key].ImageSource);
            }
            foreach (int value in sorted.Values)
            {
                calculateScale = 3 * (double)value / count;
                resultScale.Add(calculateScale);
            }
            
            BindingContext = this;
        }
        async void PoistuClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
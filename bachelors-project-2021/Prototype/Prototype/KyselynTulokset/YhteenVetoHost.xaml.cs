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
    public partial class YhteenVetoHost : ContentPage
    {
        public IList<string> resultImages { get; set; }
        public IList<double> resultScale { get; set; }
        public YhteenVetoHost()
        {
            InitializeComponent();
            resultImages = new List<string>();
            resultScale = new List<double>();

            int count = 0;
            double calculateScale = 0.0;
            Dictionary<int, int> sorted = new Dictionary<int, int>();
			foreach (var item in Main.GetInstance().host.data.GetEmojiResults().OrderByDescending(item => item.Value))
			{
                sorted.Add(item.Key, item.Value);
			}
            count = sorted.Count;

            foreach (int key in sorted.Keys)
            {
                resultImages.Add(SurveyManager.GetInstance().GetSurvey().emojis[key].ImageSource);
            }

            if (count == 0)
            {
                foreach (int value in sorted.Values)
                {
                    resultScale.Add(0);
                }
            } else 
            {
                foreach (int value in sorted.Values)
                {
                    calculateScale = 3 * (double)value / count;
                    resultScale.Add(calculateScale);
                }
            }
            BindingContext = this;
        }
        async void LopetaClicked(object sender, EventArgs e)
        {
            //Sulkee kyselyn kaikilta osallisujilta (linjat poikki höhö XD)
            await Navigation.PopToRootAsync();
        }
        async void JatkaClicked(object sender, EventArgs e)
        {
            
            //Siirrytään odottamaan äänestyksen tuloksia (HOST)
            await Navigation.PushAsync(new TulostenOdotus());
        }
    }
}
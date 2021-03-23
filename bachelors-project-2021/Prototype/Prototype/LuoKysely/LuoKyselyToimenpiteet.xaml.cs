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

        public IList<Emoji> Emojit { get; set; }


        public LuoKyselyToimenpiteet()
        {
            InitializeComponent();

			//Emojien alustus 
			Emojit = SurveyManager.GetInstance().GetSurvey().emojis;

            BindingContext = this;
        }
      

        void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            // :DDD
            if (sender is Button b && b.Parent is Grid g && g.Children[2] is Frame f)
			{
                f.IsVisible = true;
			}
        }

        private void Sulje_Clicked(object sender, EventArgs e)
        {
            // :DDD
            if (sender is Button b && b.Parent.Parent.Parent is Frame f)
			{
                f.IsVisible = false;
			}
        }


        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if (sender is CollectionView cv && cv.SelectionChangedCommandParameter is IList<string> activities)
			{
                activities = e.CurrentSelection as IList<string>;
			}
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
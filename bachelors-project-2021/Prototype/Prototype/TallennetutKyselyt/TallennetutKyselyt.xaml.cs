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
    public partial class TallennetutKyselyt : ContentPage
    {
        public string SelectedSurvey { get; set; }
        public List<string> Surveys { get; set; }
        public TallennetutKyselyt()
        {
            InitializeComponent();
            Surveys = new List<String>();
            SurveyManager manager = SurveyManager.GetInstance();

            Surveys = manager.GetTemplates();

            BindingContext = this;
        }

        void OnListSelection(object sender, SelectionChangedEventArgs e)
        {
            SelectedSurvey = e.CurrentSelection[0] as string;

            //button enabled only when there is survey selected

            if (SelectedSurvey!= null )
                TButton.IsEnabled = true;

            else
                TButton.IsEnabled = false;
        }

        async void AvaaClicked(object sender, EventArgs e)
        {
            string surveyName = SelectedSurvey + ".txt";
            SurveyManager manager = SurveyManager.GetInstance();
            manager.LoadSurvey(surveyName);

            Console.WriteLine(surveyName);

            KyselynTarkastelu.SetSurveyName(surveyName);
            //navigoinnissa ei ole vielä mitenkään yhdistetty valittua kyselyä kyselyn tarkastelusivulle
            await Navigation.PushAsync(new KyselynTarkastelu());

        }
    }
}
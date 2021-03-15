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
        public string selectedSurvey;
        public IList<string> Surveys { get; set; }
        public TallennetutKyselyt()
        {
            InitializeComponent();
            SurveyManager manager = SurveyManager.GetInstance();

            Surveys = manager.GetTemplates();

            BindingContext = this;
        }

        void OnListSelection(object sender, SelectionChangedEventArgs e)
        {
            selectedSurvey = e.CurrentSelection[0] as string;
        }

        async void AvaaClicked(object sender, EventArgs e)
        {
            string surveyName = selectedSurvey + ".txt";
            SurveyManager manager = SurveyManager.GetInstance();
            manager.LoadSurvey(surveyName);

        }
    }
}
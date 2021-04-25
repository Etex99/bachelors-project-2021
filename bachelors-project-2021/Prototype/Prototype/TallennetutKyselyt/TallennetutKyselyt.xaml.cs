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

        public enum ToolbarItemPosition { Start, End }

 

        public string SelectedSurvey { get; set; }
        public List<string> Surveys { get; set; }
        public TallennetutKyselyt()
        {
            InitializeComponent();
            Surveys = new List<String>();
            SurveyManager manager = SurveyManager.GetInstance();

            NavigationPage.SetHasBackButton(this, false);


            Surveys = manager.GetTemplates();



            BindingContext = this;
        }

        //Device back button navigation 
        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());
            return true;

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

        async void OletusClicked(object sender, EventArgs e)
		{
            SurveyManager.GetInstance().SetDefaultSurvey();

            KyselynTarkastelu.canDelete = false;
            KyselynTarkastelu.canEdit = false;
            await Navigation.PushAsync(new KyselynTarkastelu());
        }

        async void AvaaClicked(object sender, EventArgs e)
        {
            string surveyName = SelectedSurvey + ".txt";
            SurveyManager manager = SurveyManager.GetInstance();
            manager.LoadSurvey(surveyName);

			Console.WriteLine(surveyName);

            KyselynTarkastelu.canDelete = true;
            KyselynTarkastelu.canEdit = true;
            await Navigation.PushAsync(new KyselynTarkastelu());

        }

        async void BackBtnClicked(object sender, EventArgs e)
        {

            await Navigation.PopToRootAsync();

        }
    }
}
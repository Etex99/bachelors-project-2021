using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Threading.Tasks;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedViewClient : TabbedPage
    {
        public TabbedViewClient()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);

            Task.Run(async () =>
            {

                bool success = await Main.GetInstance().client.ReceiveVote1Candidates();
				if (success)
				{
                    //Received vote candidates lets goooo
                    await Navigation.PushAsync(new AktiviteettiäänestysEka());
                    return;
				}
                //Did not get candidates, now it is safe to leave this survey, because nothing more is going to happen
                //TODO maybe indicate that the survey has concluded somehow?? e.g "Poistu" turns green
                return;
            });
        }

        //Device back button disabled
        protected override bool OnBackButtonPressed()
        {
            return true;

        }
    }
}
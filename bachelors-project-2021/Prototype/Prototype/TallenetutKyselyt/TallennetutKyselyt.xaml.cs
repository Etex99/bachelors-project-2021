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
        public TallennetutKyselyt()
        {
            InitializeComponent();
        }

        async void AvaaClicked(object sender, EventArgs e)
        {
            // siirrytään tallennettuun kyselyyn
          //  await Navigation.PushAsync(new ());
        }
    }
}
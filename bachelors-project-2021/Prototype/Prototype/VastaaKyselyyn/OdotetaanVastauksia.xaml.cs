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
    public partial class OdotetaanVastauksia : ContentPage
    {
        public OdotetaanVastauksia()
        {
            InitializeComponent();
        }

        private void JatkaTuloksiin(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TabbedViewHost());
        }
    }
}
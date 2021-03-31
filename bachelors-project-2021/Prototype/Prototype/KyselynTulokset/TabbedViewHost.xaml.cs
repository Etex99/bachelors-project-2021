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
    public partial class TabbedViewHost : TabbedPage
    {
        public TabbedViewHost()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}
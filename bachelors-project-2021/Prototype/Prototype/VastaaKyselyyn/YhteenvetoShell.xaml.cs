using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YhteenvetoShell : Shell
    {
        public YhteenvetoShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EmojiYhteenveto), typeof(EmojiYhteenveto));
        }
    }
}
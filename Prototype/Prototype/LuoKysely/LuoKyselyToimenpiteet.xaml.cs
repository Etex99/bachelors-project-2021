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
        public IList<EmojiToim> Emojit { get; private set; }

        public LuoKyselyToimenpiteet()
        {
            InitializeComponent();
            Emojit = new List<EmojiToim>();

            Emojit.Add(new EmojiToim
            {
                Buttons = "",
                Name = "Emojiiiii",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new EmojiToim
            {
                Buttons = "",
                Name = "Toinen Emojiiiii",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new EmojiToim
            {
                Buttons = "",
                Name = "Kolmas Emojiiiii",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new EmojiToim
            {
                Buttons = "",
                Name = "Neljäs Emojiiiii",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new EmojiToim
            {
                Buttons = "",
                Name = "Viides Emojiiiii",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new EmojiToim
            {
                Buttons = "",
                Name = "Kuudes Emojiiiii",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new EmojiToim
            {
                Buttons = "",
                Name = "Seitsemäs Emojiiiii",
                ImageSource = "Smiley.png"
            });


            BindingContext = this;


        }
        async void JatkaButtonClicked(object sender, EventArgs e)
        {
            // siirrytään "Luo kysely -lopetus" sivulle 
            await Navigation.PushAsync(new LuoKyselyLopetus()); ;
        }
    }
}
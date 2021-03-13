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
    public partial class LuoKyselyEmojit : ContentPage
    {

        public IList<Emoji> Emojit { get; private set; }

        public LuoKyselyEmojit()
        {
            InitializeComponent();

        

            Emojit = new List<Emoji>();

            Emojit.Add(new Emoji
            {
                
                Name = "",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new Emoji
            {
               
                Name = "",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new Emoji
            {
              
                Name = "",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new Emoji
            {
                
                Name = "",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new Emoji
            {
                
                Name = "",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new Emoji
            {
               
                Name = "",
                ImageSource = "Smiley.png"
            });

            Emojit.Add(new Emoji
            {
                
                Name = "",
                ImageSource = "Smiley.png"
            });


            BindingContext = this;
        }
    
        

    async void JatkaButtonClicked(object sender, EventArgs e)
        {
            // siirrytään "luo uus kysely 3/3" sivulle 
            await Navigation.PushAsync(new LuoKyselyToimenpiteet()); ;
        }
    }
}
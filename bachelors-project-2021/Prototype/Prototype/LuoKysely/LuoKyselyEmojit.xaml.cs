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

           // Kuva näkyy sovelluksessa JOS laittaa kuvan lähteen(linkin). Jokin ongelma drawable kansion kanssa(ei näytä sinne ladattua kuvaa)
           //Kaikki emojit tässä ovat vain placeholdereita

          Emojit = new List<Emoji>();

            Emojit.Add(new Emoji
            {
              
                Name = "",
                ImageSource = "https://cdn.icon-icons.com/icons2/1648/PNG/512/10024thinkingface_110034.png"
            });

            Emojit.Add(new Emoji
            {
               
                Name = "",
                ImageSource = "https://www.papertraildesign.com/wp-content/uploads/2017/06/emoji-nerd-glasses.png"
            });

            Emojit.Add(new Emoji
            {
              
                Name = "",
                ImageSource = "https://i.pinimg.com/originals/5f/62/a6/5f62a6ccc44d1d972587666f2c46ef94.png"
            });

            Emojit.Add(new Emoji
            {
                
                Name = "",
                ImageSource = "https://cdn.pixabay.com/photo/2019/11/14/03/22/party-4625237_960_720.png"
            });

            Emojit.Add(new Emoji
            {
                
                Name = "",
                ImageSource = "https://toppng.com/uploads/preview/angry-and-sad-emoji-115495132213jxf0zx4az.png"
            });

            Emojit.Add(new Emoji
            {
               
                Name = "",
                ImageSource = "https://picnicenglish.com/wp-content/uploads/2018/01/Sunglasses-Emoji-Picnic.png"
            });

            Emojit.Add(new Emoji
            {
                
                Name = "",
                ImageSource = "https://p.kindpng.com/picc/s/179-1799634_cute-cat-emoji-kitten-android-android-black-cat.png"
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
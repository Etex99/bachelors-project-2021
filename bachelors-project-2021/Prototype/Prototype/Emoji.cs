using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
    public class Emoji
    {
        
        public string Buttons { get; set; }
        public string Name { get; set; }
        public string ImageSource { get; set; }

        public override string ToString()
        {
            return Buttons;
        }
    }
}
 

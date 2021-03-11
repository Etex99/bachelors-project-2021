using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
    public class Survey
    {
        //private string introMessage;
        //private List<int, int, List<string>> emojis;
        //private string roomCode;

        public void setIntroMessage(string)
        {
            Console.WriteLine("$DEBUG: setting an intro message for the survey");
            //introMessage = Console.ReadLine();
        }

        public string getIntroMessage()
        {
            Console.WriteLine("$DEBUG: getting an intro message from the survey");
            //return this.introMessage;
        }

        public void setEmoji(int index, int id)
        {
            Console.WriteLine("$DEBUG: setting an emoji for the survey");
        }

        public void setEmojiImpact(int index, int value)
        {
            Console.WriteLine("$DEBUG: setting an impact for emoji");
        }

        public void addActivity(int index, string value)
        {
            Console.WriteLine("$DEBUG: adding activity for emoji");
        }

        public void removeActivity(int index, string)
        {
            Console.WriteLine("$DEBUG: removing activity from certain emoji in the survey");
        }

        public List<int, int, List<string>> getEmojis()
        {
            Console.WriteLine("$DEBUG: setting emojis (emoji, impact and activities) from the survey");
            //return this.emojis;
        }
    }
}

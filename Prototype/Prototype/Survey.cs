using System.Collections.Generic;

namespace Prototype
{
    public class Survey
    {
        public string introMessage = "default";

        public class Emoji {
			public int ID = 0;
            public int Impact = 0;
			public List<string> activities = null;

            public Emoji() {
                ID = 0;
                Impact = 0;
                activities = new List<string>() { "default", "foo", "bar" };
			}

            public Emoji(int ID, int impact, List<string> activities) {
				this.ID = ID;
				this.Impact = impact;
                this.activities = activities;
			}
		}

        public List<Emoji> emojis;
        public string RoomCode = "default";

        public Survey() {

        }
    }
}

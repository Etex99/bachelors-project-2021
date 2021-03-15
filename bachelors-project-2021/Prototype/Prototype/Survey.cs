using System.Collections.Generic;

namespace Prototype
{
	public class Survey
	{
		public string introMessage = "default";
		public List<Emoji> emojis;
		public string RoomCode = "default";
		
		//default survey consists of first intro entry, 7 emojis with various impact each with 3 first entries of activities.
		public Survey() {
			introMessage = Const.intros[0];
			emojis = new List<Emoji>();

			List<string> activities;

			Const.activities.TryGetValue(0, out activities);
			activities = activities.GetRange(0, 3);
			emojis.Add(new Emoji(0, "Mood 0", "neutral", activities, "emoji0.png"));

			Const.activities.TryGetValue(1, out activities);
			activities = activities.GetRange(0, 3);
			emojis.Add(new Emoji(1, "Mood 1", "neutral", activities, "emoji1.png"));

			Const.activities.TryGetValue(2, out activities);
			activities = activities.GetRange(0, 3);
			emojis.Add(new Emoji(2, "Mood 2", "neutral", activities, "emoji2.png"));

			Const.activities.TryGetValue(3, out activities);
			activities = activities.GetRange(0, 3);
			emojis.Add(new Emoji(3, "Mood 3", "neutral", activities, "emoji3.png"));

			Const.activities.TryGetValue(4, out activities);
			activities = activities.GetRange(0, 3);
			emojis.Add(new Emoji(4, "Mood 4", "neutral", activities, "emoji4.png"));

			Const.activities.TryGetValue(5, out activities);
			activities = activities.GetRange(0, 3);
			emojis.Add(new Emoji(5, "Mood 5", "neutral", activities, "emoji5.png"));

			Const.activities.TryGetValue(6, out activities);
			activities = activities.GetRange(0, 3);
			emojis.Add(new Emoji(6, "Mood 6", "neutral", activities, "emoji6.png"));

			RoomCode = null;
		}

		public override string ToString() {
			string value = "";

			value += $"Intro: {introMessage}\n";

			foreach (var item in emojis)
			{
				value += item.ToString();
				value += "\n";
			}

			value += $"RoomCode: {RoomCode}";

			return value;
		}
	}
}

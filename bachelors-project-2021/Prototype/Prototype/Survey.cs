using System.Collections.Generic;
using System;

namespace Prototype
{
	public class Survey
	{
		public string introMessage { get; set; } = "default";
		public List<Emoji> emojis { get; set; }
		public string RoomCode { get; set; } = "default";
		
		//empty constructor creates blank survey
		public Survey() {
			introMessage = null;
			emojis = new List<Emoji>();

			emojis.Add(new Emoji(0, "Iloinen", "neutral", new List<string>(), "emoji0.png"));
			emojis.Add(new Emoji(1, "Hämmästynyt", "neutral", new List<string>(), "emoji1.png"));
			emojis.Add(new Emoji(2, "Neutraali", "neutral", new List<string>(), "emoji2.png"));
			emojis.Add(new Emoji(3, "Vihainen", "neutral", new List<string>(), "emoji3.png"));
			emojis.Add(new Emoji(4, "Väsynyt", "neutral", new List<string>(), "emoji4.png"));
			emojis.Add(new Emoji(5, "Miettivä", "neutral", new List<string>(), "emoji5.png"));
			emojis.Add(new Emoji(6, "Itkunauru", "neutral", new List<string>(), "emoji6.png"));

			RoomCode = null;
		}
		public Survey(string introMessage, List<Emoji> emojis, string RoomCode)
		{
			this.introMessage = introMessage;
			this.emojis = emojis;
			this.RoomCode = RoomCode;
		}

		//default survey consists of first intro entry, 7 emojis with various impact each with 3 first entries of activities and a random roomcode
		public Survey GetDefaultSurvey()
		{
			string tempIntro = Const.intros[0];
			List<Emoji> tempEmojis = new List<Emoji>();

			List<string> activities;

			Const.activities.TryGetValue(0, out activities);
			activities = activities.GetRange(0, 2);
			tempEmojis.Add(new Emoji(0, "Iloinen", "positive", activities, "emoji0.png"));

			Const.activities.TryGetValue(1, out activities);
			activities = activities.GetRange(0, 2);
			tempEmojis.Add(new Emoji(1, "Hämmästynyt", "negative", activities, "emoji1.png"));

			Const.activities.TryGetValue(2, out activities);
			activities = activities.GetRange(0, 3);
			tempEmojis.Add(new Emoji(2, "Neutraali", "neutral", activities, "emoji2.png"));

			Const.activities.TryGetValue(3, out activities);
			activities = activities.GetRange(0, 2);
			tempEmojis.Add(new Emoji(3, "Vihainen", "negative", activities, "emoji3.png"));

			Const.activities.TryGetValue(4, out activities);
			activities = activities.GetRange(0, 2);
			tempEmojis.Add(new Emoji(4, "Väsynyt", "neutral", activities, "emoji4.png"));

			Const.activities.TryGetValue(5, out activities);
			activities = activities.GetRange(0, 3);
			tempEmojis.Add(new Emoji(5, "Miettivä", "neutral", activities, "emoji5.png"));

			Const.activities.TryGetValue(6, out activities);
			activities = activities.GetRange(0, 3);
			tempEmojis.Add(new Emoji(6, "Itkunauru", "positive", activities, "emoji6.png"));

			string TempRoomCode = GenerateRandomCode();

			return new Survey(tempIntro, tempEmojis, TempRoomCode);
		}

		//generates random room code of 5 numbers
		private string GenerateRandomCode()
		{
			Random r = new Random();
			string temp = "";

			for (int i = 0; i < 5; i++)
			{
				temp += r.Next(10).ToString();
			}

			return temp;
		}
		//toString method for getting the info of the survey in a string for data purposes
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

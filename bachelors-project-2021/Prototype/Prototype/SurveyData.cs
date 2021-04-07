using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	class SurveyData
	{
		private Dictionary<int, int> emojiResults;
		private Dictionary<(int, string), int> vote1Results;
		public int totalEmojis = 0;

		public SurveyData() {
			emojiResults = new Dictionary<int, int>();
			vote1Results = new Dictionary<(int, string), int>();
			emojiResults.Add(0, 0);
			emojiResults.Add(1, 0);
			emojiResults.Add(2, 0);
			emojiResults.Add(3, 0);
			emojiResults.Add(4, 0);
			emojiResults.Add(5, 0);
			emojiResults.Add(6, 0);
		}
		
		//Adds a single emoji answer to results
		public void AddEmojiResults(int emoji) {
			int count;
			if (emojiResults.TryGetValue(emoji, out count)) {
				count++;
				emojiResults[emoji] = count;
				totalEmojis++;
				return;
			}
		}
		public void AddVote1Results(Dictionary<int, string> activities)
        {
			foreach (var activity in activities)
			{
				int count;
				if (vote1Results.TryGetValue((activity.Key, activity.Value), out count))
				{
					Console.WriteLine("In Loop:: EmojiID: {0}, Activity: {1}", activity.Key, activity.Value);
					count++;
					vote1Results[(activity.Key, activity.Value)] = count;
				}
				Console.WriteLine("EmojiID: {0}, Activity: {1}", activity.Key, activity.Value);
				
				if (vote1Results.ContainsKey((activity.Key, activity.Value)) == false)
                {
					vote1Results.Add((activity.Key, activity.Value), 1);
				}
			}
        }
		public Dictionary<int, int> GetEmojiResults() {
			return emojiResults;
		}
		public Dictionary<(int, string), int> GetVote1Results()
        {
			return vote1Results;
        }
		public override string ToString() {
			/*string s = "{emojiResults: ";
			foreach (var item in emojiResults)
			{
				s += "[";
				s += item.Key;
				s += ": ";
				s += item.Value;
				s += "],";
			}
			s = s.Substring(0, s.Length - 1);
			s += "}";*/
			string s = "{vote1Results: ";
			foreach (var item in vote1Results)
			{
				s += "[";
				s += item.Key;
				s += ": ";
				s += item.Value;
				s += "],";
			}
			s = s.Substring(0, s.Length - 1);
			s += "}";

			return s;
		}
	}
}

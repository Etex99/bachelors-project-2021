using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	class SurveyData
	{
		private Dictionary<int, int> emojiResults;
		private Dictionary<string, int> vote1Results;
		public int totalEmojis = 0;

		public SurveyData() {
			emojiResults = new Dictionary<int, int>();
			vote1Results = new Dictionary<string, int>();
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
		public void AddVote1Results(string activity)
        {
			int count;
			if(vote1Results.TryGetValue(activity, out count))
            {
				count++;
				vote1Results[activity] = count;
				return;
            }
			vote1Results.Add(activity, 1);
        }
		public Dictionary<int, int> GetEmojiResults() {
			return emojiResults;
		}
		public Dictionary<string, int> GetVote1Results()
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	class SurveyData
	{
		private Dictionary<int, int> emojiResults;
		private Dictionary<string, int> vote1Results;

		public SurveyData() {
			emojiResults = new Dictionary<int, int>();
			vote1Results = new Dictionary<string, int>();
		}
		
		//Adds a single emoji answer to results
		public void AddEmojiResults(int emoji) {
			int count;
			if (emojiResults.TryGetValue(emoji, out count)) {
				count++;
				emojiResults[emoji] = count;
				return;
			}
			//emoji did not exist yet
			emojiResults.Add(emoji, 1);
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
			foreach (var item in v)
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	public class SurveyData
	{
		public Dictionary<int, int> emojiResults { get; private set; }
		public Dictionary<(int, string), int> vote1Results { get; private set; }
		public Dictionary<string, int> vote2Results { get; private set; }

		public string voteResult { get; set; }
		public int totalEmojis = 0;

		public SurveyData() {
			emojiResults = new Dictionary<int, int>();
			vote1Results = new Dictionary<(int, string), int>();
			vote2Results = new Dictionary<string, int>();
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
			//for each voted activity from user, check if that activity has been voted already add 1 to value, if not add it to the list with a value of 1
			foreach (var activity in activities)
			{
				int count;

				//check if activity already has a value
				if (vote1Results.TryGetValue((activity.Key, activity.Value), out count))
				{
					Console.WriteLine("In Loop:: EmojiID: {0}, Activity: {1}", activity.Key, activity.Value);
					count++;
					vote1Results[(activity.Key, activity.Value)] = count;
				}
				Console.WriteLine("EmojiID: {0}, Activity: {1}", activity.Key, activity.Value);
				
				//check if vote exists at all
				if (vote1Results.ContainsKey((activity.Key, activity.Value)) == false)
                {
					vote1Results.Add((activity.Key, activity.Value), 1);
				}
			}
        }

		public void AddVote2Results(string activity)
        {
			int count;
			
			//Check if activity has been voted already
			if (vote2Results.TryGetValue(activity, out count))
			{
				Console.WriteLine("In Loop:: Activity: {0}", activity);
				count++;
				vote2Results[activity] = count;
			}
			Console.WriteLine("Activity: {0}", activity);

			//check if activity exists at all
			if (vote2Results.ContainsKey(activity) == false)
			{
				vote2Results.Add(activity, 1);
			}
		}

		//get emojiResults
		public Dictionary<int, int> GetEmojiResults() {
			return emojiResults;
		}

		//get Vote1Results
		public Dictionary<(int, string), int> GetVote1Results()
        {
			return vote1Results;
        }

		//get Vote2Results
		public Dictionary<string, int> GetVote2Results()
        {
			return vote2Results;
        }
		public override string ToString() {
			string s = "{emojiResults: ";
			foreach (var item in emojiResults)
			{
				s += "[";
				s += item.Key;
				s += ": ";
				s += item.Value;
				s += "],";
			}
			s = s.Substring(0, s.Length - 1);
			s += "}";
			s += "{vote1Results: ";
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
			
			s += "{vote2Results: ";
			foreach (var item in vote2Results)
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	class SurveyData
	{
		private Dictionary<int, int> emojiResults;
		
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
		public Dictionary<int, int> GetEmojiResults() {
			return emojiResults;
		}
	}
}

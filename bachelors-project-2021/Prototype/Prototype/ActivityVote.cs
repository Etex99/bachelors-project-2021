using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype
{
    class ActivityVote
    {
        private Dictionary<int, IList<string>> vote1Candidates;
        private List<string> vote2Candidates;

        public ActivityVote ()
        {
            vote1Candidates = new Dictionary<int, IList<string>>();
            vote2Candidates = new List<string>();
        }

        public Dictionary<int, IList<string>> calcVote1Candidates(List<Emoji> emojis, Dictionary<int, int> emojiResults)
        {
            //for getting a sorted list out of emojiResults
            Dictionary<int, int> sorted = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> item in emojiResults.OrderByDescending(key => key.Value))
            {
                Console.WriteLine("key: {0}, value: {1}", item.Key, item.Value);
                sorted.Add(item.Key, item.Value);
            }

            foreach (int key in sorted.Keys)
            {
                vote1Candidates.Add(key, emojis[key].activities);
                Console.WriteLine(emojis[key].activities.ToString());
            }
            return vote1Candidates;
        }

        public List<string> calcVote2Candidates(Dictionary<string, int> vote1Results)
        {
            return vote2Candidates;
        }

        public override string ToString()
        {
            string value = "";

            foreach (var item in vote1Candidates)
            {
                value += $"ID: {item.Key.ToString()}, ";
                value += "Activities: [";
                foreach (var activity in item.Value)
                {
                    value += $"{activity} ";
                }
                value += "]";
                value += "\n";
            }
            return value;
        }
    }
}

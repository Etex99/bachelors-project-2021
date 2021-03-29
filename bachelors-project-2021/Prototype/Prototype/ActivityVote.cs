using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
    class ActivityVote
    {
        private Dictionary<int, string[]> vote1Cadidates;
        private List<string> vote2Cadidates;

        public ActivityVote (List<Emoji> emojis, Dictionary<int, int> emojiResults)
        {
            
        }

        public Dictionary<int, string[]> calcVote1Candidates(List<Emoji> emojis, Dictionary<int, int> emojiResults)
        {
            return vote1Cadidates;
        }

        public List<string> calcVote2Candidates(Dictionary<string, int> vote1Results)
        {
            return vote2Cadidates;
        }
    }
}

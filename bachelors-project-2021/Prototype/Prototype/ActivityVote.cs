using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
    class ActivityVote
    {
        private Dictionary<int, string[]> vote1Candidates;
        private List<string> vote2Candidates;

        public ActivityVote ()
        {
            vote1Candidates = new Dictionary<int, string[]>();
            vote2Candidates = new List<string>();
        }

        public Dictionary<int, string[]> calcVote1Candidates(List<Emoji> emojis, Dictionary<int, int> emojiResults)
        {
            return vote1Candidates;
        }

        public List<string> calcVote2Candidates(Dictionary<string, int> vote1Results)
        {
            return vote2Candidates;
        }
    }
}

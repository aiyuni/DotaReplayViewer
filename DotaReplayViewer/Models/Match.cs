using System.Collections.Generic;

namespace DotaReplayViewer.Models
{
    public class Match
    {
        public long match_id { get; set; }
        public int duration { get; set; }
        public List<Player> players { get; set; }
        public string replay_url { get; set; }
    }
}

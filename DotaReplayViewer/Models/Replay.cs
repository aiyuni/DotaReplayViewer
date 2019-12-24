using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotaReplayViewer.Models
{
    public class Replay
    {
        public long Match_id { get; set; }
        public long Cluster { get; set; }
        public long Replay_salt { get; set; }

        public Replay(int id, int cluster, int replaySalt)
        {
            this.Match_id = id;
            this.Cluster = cluster;
            this.Replay_salt = replaySalt;
        }
    }
}

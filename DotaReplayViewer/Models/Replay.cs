namespace DotaReplayViewer.Models
{
    public class Replay
    {
        public long match_id { get; set; }
        public int cluster { get; set; }
        public int replay_salt { get; set; }
    }
}

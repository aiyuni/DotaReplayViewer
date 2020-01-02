using DotaReplayViewer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotaReplayViewer.Helpers
{
    public class OpenDotaHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private static List<Hero> heroesList;

        static OpenDotaHelper()
        {
            using (StreamReader r = new StreamReader(Constants.HeroesJson))
            {
                string json = r.ReadToEnd();
                heroesList = JsonConvert.DeserializeObject<List<Hero>>(json);
            }
        }

        public static async Task<Replay> GetReplay(long matchId)
        {
            var response = await client.GetAsync($"{Constants.OpenDotaUrl}/replays?match_id={matchId}");
            var json = await response.Content.ReadAsStringAsync();
            JArray replaysJson = JArray.Parse(json);
            Replay replay = JsonConvert.DeserializeObject<Replay>(replaysJson[0].ToString());
            return replay;
        }

        public static async Task<Match> GetMatch(long matchId)
        {
            var response = await client.GetAsync($"{Constants.OpenDotaUrl}/matches/{matchId}");
            var json = await response.Content.ReadAsStringAsync();
            Match match = JsonConvert.DeserializeObject<Match>(json);
            return match;
        }

        public static JObject GetMatchDetails(Match match)
        {
            JObject matchDetails = new JObject();
            JArray players = new JArray();

            foreach (var p in match.players)
            {
                JObject player = new JObject();
                Hero hero = GetHeroFromId(p.hero_id);
                player.Add("player_slot", p.player_slot);
                player.Add("hero", JObject.FromObject(hero));
                players.Add(player);
            }

            matchDetails.Add("players", players);

            return matchDetails;
        }

        public static Hero GetHeroFromId(int heroId)
        {
            foreach (var h in heroesList)
                if (h.id == heroId) return h;
            return null;
        }

        public static string BuildReplayUrl(long matchId, int cluster, int replaySalt)
        {
            string url = cluster == 236 ? ".wmsj.cn/570/" : ".valve.net/570/";
            return $"http://replay{cluster}{url}{matchId}_{replaySalt}.dem.bz2";
        }
    }
}

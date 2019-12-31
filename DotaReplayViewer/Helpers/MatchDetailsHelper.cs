using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotaReplayViewer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotaReplayViewer.Helpers
{
    public class MatchDetailsHelper
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> getReplayUrl(long replayId)
        {
            var response = await client.GetAsync("https://api.opendota.com/api/replays?match_id=" + replayId);
            var jsonString = await response.Content.ReadAsStringAsync();
            List<Replay> jsonResponse = JsonConvert.DeserializeObject<List<Replay>>(jsonString);

            System.Diagnostics.Debug.WriteLine(jsonResponse[0].Match_id + " , " + jsonResponse[0].Cluster + ", " + jsonResponse[0].Replay_salt);

            long matchID = replayId;
            long cluster = jsonResponse[0].Cluster;
            long replaySalt = jsonResponse[0].Replay_salt;

            string replayUrl = null;

            if (jsonResponse[0].Cluster == 236)  //for chinese replays
            {
                replayUrl = "http://replay" + cluster + ".wmsj.cn/570/" + matchID + "_" + replaySalt + ".dem.bz2";
            }
            else
            {
                replayUrl = "http://replay" + cluster + ".valve.net/570/" + matchID + "_" + replaySalt + ".dem.bz2";

            }

            return replayUrl;
        }

        public static async Task<List<PlayerDetail>> RequestMatchDetails(long matchID)
        {
            //Get replay stats from opendota api
            var response = await client.GetAsync("https://api.opendota.com/api/matches/" + matchID);
            var jsonString = await response.Content.ReadAsStringAsync();

            //parse json string to obj
            JObject obj = JObject.Parse(jsonString); //dynamic obj doesn't work here, see: https://stackoverflow.com/questions/39468096/how-can-i-parse-json-string-from-httpclient

            List<PlayerDetail> playersDetails = new List<PlayerDetail>();
            //store player info in map
            Dictionary<string, int> heroesMap = new Dictionary<string, int>();
            foreach (var player in obj["players"])
            {
                string heroName = GetHeroNameFromId(Convert.ToInt32(player["hero_id"]));
                // System.Diagnostics.Debug.WriteLine("player has slot: " + player["player_slot"] + " and is playing: " + heroName);
                //heroesMap.Add(heroName, Convert.ToInt32(player["player_slot"]));

                PlayerDetail playerDetail = new PlayerDetail();
                playerDetail.heroName = heroName;
                playerDetail.playerSlot = Convert.ToInt32(player["player_slot"]);
                playersDetails.Add(playerDetail);
            }

            return playersDetails;
        }

        private static string GetHeroNameFromId(int id)
        {
            JArray heroesList;
            using (StreamReader r = System.IO.File.OpenText("hero_names.json"))
            {
                using (JsonTextReader reader = new JsonTextReader(r))
                {
                    heroesList = (JArray)JToken.ReadFrom(reader);
                }
            }

            foreach (var hero in heroesList)
            {
                if ((Convert.ToInt32(hero["id"])) == id)
                {
                    return hero["localized_name"].ToString();
                }
            }
            //System.Diagnostics.Debug.Write("Hero name is: " + heroesList[0]["localized_name"]);

            return "error";
        }

        public static async Task<int> GetMatchLength(long matchID)
        {
            //Get replay stats from opendota api
            var response = await client.GetAsync("https://api.opendota.com/api/matches/" + matchID);
            var jsonString = await response.Content.ReadAsStringAsync();

            //parse json string to obj
            JObject obj = JObject.Parse(jsonString);
            Debug.WriteLine("duration is: " + obj["duration"].ToString());

            return (obj["duration"].ToObject<Int32>() + 100); //add 100 seconds to take care of the setup time 

        }
    }
}

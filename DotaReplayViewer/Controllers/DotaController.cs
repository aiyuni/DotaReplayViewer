using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DotaReplayViewer.Helpers;
using DotaReplayViewer.Models;
using ICSharpCode.SharpZipLib.BZip2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotaReplayViewer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DotaController : ControllerBase
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);
        public static long matchID = 5164275078; //5164022682; //5163131785 for 13:46 long //5164286766 for 1v1
        public static string replayFolder = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\dota 2 beta\\game\\dota\\replays";
        //public static string ReplayFolder = ConfigurationManager.AppSettings["ReplayFolder"];
        private static readonly HttpClient client = new HttpClient();

        [HttpGet("GetMatchDetails/{matchId}")]
        public async Task<IActionResult> GetMatchDetails(long matchId)
        {
            //await Response.WriteAsync("inside getMatchDetails");
            matchID = matchId;
            FileHelper.InitializeWatcher();
            //Get replay URL from opendota api
            var replayUrl = await MatchDetailsHelper.getReplayUrl(matchId);

            //Download and unzip replay file
            FileHelper.DownloadAndUnzipReplay(replayUrl, matchID);

            //Get match details and store hero/player_slot info in map
            List<PlayerDetail> playersDetails = await MatchDetailsHelper.RequestMatchDetails(matchId);
            int matchDurationSeconds = await MatchDetailsHelper.GetMatchLength(matchId);

            return Ok(JsonConvert.SerializeObject((playersDetails)));
        }

        [HttpGet("startReplay/{matchID}/{playerSlot}")]
        public async Task<IActionResult> StartReplay (long matchID, int playerSlot)
        {
            bool success = await DotaClientHelper.LaunchDota();
            Debug.Write("success when starting dota is: " + success);

            await DotaClientHelper.StartReplay(playerSlot, matchID);

            return Ok(200);
        }



    }
}
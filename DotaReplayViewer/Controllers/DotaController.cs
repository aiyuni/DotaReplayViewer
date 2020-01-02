﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DotaReplayViewer.Helpers;
using DotaReplayViewer.Models;
using Microsoft.AspNetCore.Mvc;
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
        private static readonly HttpClient client = new HttpClient();

        [HttpGet("GetMatchDetails/{matchId}")]
        public async Task<IActionResult> GetMatchDetails(long matchId)
        {
            Match match = await OpenDotaHelper.GetMatch(matchId);
            JObject matchDetails = OpenDotaHelper.GetMatchDetails(match);
            return Ok(JsonConvert.SerializeObject((matchDetails)));
        }

        [HttpGet("StartReplay/{matchId}/{playerSlot}")]
        public async Task<IActionResult> StartReplay(long matchId, int playerSlot)
        {
            Match match = await OpenDotaHelper.GetMatch(matchId);
            Replay replay = await OpenDotaHelper.GetReplay(matchId);
            string replayUrl = OpenDotaHelper.BuildReplayUrl(replay.match_id, replay.cluster, replay.replay_salt);

            FileHelper.InitializeWatcher();
            FileHelper.DownloadAndUnzipReplay(replayUrl, matchId);

            Debug.WriteLine("Match id is: " + matchId);
            bool success = await DotaClientHelper.LaunchDota();
            Debug.Write("success when starting dota is: " + success);

            DotaClientHelper.StartReplay(playerSlot, matchId);

            await ObsStudioHelper.StartObs();
            Debug.Write("watching for ths many seconds:  " + match.duration);
            await ObsStudioHelper.WatchObs(match.duration);
            ObsStudioHelper.StopObs();

            return Ok(200);
        }
    }
}

using System;
using System.IO;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DotaReplayViewer.Helpers;
using DotaReplayViewer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace DotaReplayViewer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DotaController : ControllerBase
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        private readonly IHostingEnvironment hostingEnvironment;
        private static readonly HttpClient client = new HttpClient();

        public DotaController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("GetMatchDetails/{matchId}")]
        public async Task<IActionResult> GetMatchDetails(long matchId)
        {
            Match match = await OpenDotaHelper.GetMatch(matchId);

            if (match == null)
                return new NotFoundObjectResult(new { message = "404 Not Found" });

            JObject matchDetails = OpenDotaHelper.GetMatchDetails(match);
            return Ok(JsonConvert.SerializeObject((matchDetails)));
        }

        [HttpGet("GetHeroImage/{heroId}")]
        public IActionResult GetHeroImage(int heroId)
        {
            Hero hero = OpenDotaHelper.GetHeroFromId(heroId);
            string contentRootPath = hostingEnvironment.ContentRootPath;
            string file = Path.GetFullPath(Path.Combine(contentRootPath, Constants.HeroImagesPath, hero.img));
            Console.WriteLine(file);
            Byte[] bytes = System.IO.File.ReadAllBytes(file);
            return File(bytes, "image/png");
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

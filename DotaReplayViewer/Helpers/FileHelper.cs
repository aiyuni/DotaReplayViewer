using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.BZip2;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace DotaReplayViewer.Helpers
{
    public class FileHelper
    {
        private readonly IConfiguration config;
        public static string ReplayFolder = Constants.ReplayFolderAbsolutePath;
        private static readonly FileSystemWatcher watcher = new FileSystemWatcher();

        public FileHelper(IConfiguration config)
        {
            this.config = config;
        }

        public static void InitializeWatcher()
        {
            watcher.Path = ReplayFolder;
            watcher.Created += new FileSystemEventHandler(OnAdd);
            watcher.EnableRaisingEvents = true;
        }

        private static void OnAdd(object sender, FileSystemEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("detected file change");
        }

        public static void DownloadAndUnzipReplay(string replayUrl, long matchID)
        {
            string compressedFilePath = ReplayFolder + "\\" + matchID + ".dem.bz2";
            string uncompressedFilePath = ReplayFolder + "\\" + matchID + ".dem";
            string[] filePaths = Directory.GetFiles(ReplayFolder);

            foreach (string filePath in filePaths)
            {
                if (filePath.Equals(uncompressedFilePath))
                {
                    System.Diagnostics.Debug.WriteLine("Deleting file...");
                    System.IO.File.Delete(filePath);
                }
            }

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(replayUrl, compressedFilePath);

                using (FileStream fs = new FileInfo(compressedFilePath).OpenRead())
                {
                    try
                    {
                        Debug.Write("starting to decompress");
                        BZip2.Decompress(fs, System.IO.File.Create(uncompressedFilePath), true);
                    }
                    catch (Exception e)
                    {
                        Debug.Write("something went wrong: " + e.Message);
                    }
                }

                Debug.Write("deleting compressed file...");
                System.IO.File.Delete(Directory.GetFiles(ReplayFolder).Where(x => x.Equals(compressedFilePath)).FirstOrDefault());
            }
        }
    }
}

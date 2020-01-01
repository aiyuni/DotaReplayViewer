using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DotaReplayViewer.Helpers
{
    public class ObsStudioHelper
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);
        public static Process obs;

        public static async Task StartObs()
        {
            System.Diagnostics.Debug.WriteLine("inside StartObs");
            var obsStream = Process.GetProcessesByName("obs64");
            if (obsStream.Length == 0)
            {
                Process.Start(Constants.ObsStudioAbsolutePath);
            }

            obs = Process.GetProcessesByName("obs64").FirstOrDefault();

            IntPtr h = obs.MainWindowHandle;
            SetForegroundWindow(h);

            await Task.Delay(8000);
            System.Diagnostics.Debug.WriteLine("sending OBS inputs...");
            Debug.WriteLine("Starting stream..at: " + DateTime.Now);
            var ahk = new AutoHotkey.Interop.AutoHotkeyEngine();
            ahk.LoadFile(Constants.AhkRelativeFilePath);
            ahk.ExecFunction("StartStream");


        }

        public static async Task WatchObs(int durationInSeconds)
        {
            await Task.Delay(durationInSeconds * 1000);
        }

        public static async Task StopObs()
        {
            Debug.WriteLine("Stopping stream..at: " + DateTime.Now);
            obs = Process.GetProcessesByName("obs64").FirstOrDefault();

            IntPtr h = obs.MainWindowHandle;
            SetForegroundWindow(h);
            var ahk = new AutoHotkey.Interop.AutoHotkeyEngine();
            ahk.LoadFile(Constants.AhkRelativeFilePath);
            ahk.ExecFunction("StopStream");
        }
    }
}

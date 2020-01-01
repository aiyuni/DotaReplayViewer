using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DotaReplayViewer.Helpers
{
    public class DotaClientHelper
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);
        public static Process Dota;

        public static async Task<Boolean> LaunchDota()
        {
            var dota = Process.GetProcessesByName("dota2");
            if (dota.Length == 0)
            {
                Process.Start(Constants.DotaAbsolutePath);
                await Task.Delay(10000);  //wait for Dota to finish loading...
            }

            Dota = Process.GetProcessesByName("dota2").FirstOrDefault();

            return (Dota != null);

        }

        public static async Task StartReplay(int playerSlot, long matchID)
        {
            IntPtr h = Dota.MainWindowHandle;
            SetForegroundWindow(h);
            //Thread.Sleep(10000);
            Console.WriteLine("sending input..");
            System.Diagnostics.Debug.WriteLine("sending inputs...");

            var ahk = new AutoHotkey.Interop.AutoHotkeyEngine();
            ahk.LoadFile(Constants.AhkRelativeFilePath);
            ahk.ExecFunction("TestSend");
            ahk.ExecFunction("WatchThisReplay", matchID.ToString(), playerSlot.ToString());
        }

    }
}

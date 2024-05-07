namespace HybridFileXfer.Net.Utilities
{
    internal class Growl
    {
        public static void Info(string msg)
        {
            Log.Info($"Growl.Info {msg}");
            HandyControl.Controls.Growl.Info(new HandyControl.Data.GrowlInfo() { Message = msg, WaitTime = 3, ShowDateTime = false });
        }
        public static void Warning(string msg)
        {
            Log.Warn($"Growl.Warning {msg}");
            HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo() { Message = msg, WaitTime = 5, ShowDateTime = false });
        }
        public static void Success(string msg)
        {
            Log.Info($"Growl.Success {msg}");
            HandyControl.Controls.Growl.Success(new HandyControl.Data.GrowlInfo() { Message = msg, WaitTime = 3, ShowDateTime = true });
        }
        public static void Fatal(string msg)
        {
            Log.Info($"Growl.Fatal {msg}");
            HandyControl.Controls.Growl.Fatal(new HandyControl.Data.GrowlInfo() { Message = msg, WaitTime = 3, ShowDateTime = true });
        }
    }
}

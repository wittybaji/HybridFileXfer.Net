using Serilog;
using System;
using System.IO;

namespace HybridFileXfer.Net.Utilities
{
    internal class Log
    {
        static Log()
        {
            string logFolder = Path.Combine(Environment.CurrentDirectory, "Log");
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }
            string LogFile = Path.Combine(logFolder, "HybridFileXfer.log");
            Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.File(LogFile, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        public static void Debug(string content)
        {
            Serilog.Log.Debug(content);
        }
        public static void Info(string content)
        {
            Serilog.Log.Information(content);
        }
        public static void Warn(string content)
        {
            Serilog.Log.Warning(content);
        }
        public static void Error(string content)
        {
            Serilog.Log.Error(content);
        }
        public static void Fatal(string content)
        {
            Serilog.Log.Fatal(content);
        }
        public static void Ex(Exception ex, string content)
        {
            Serilog.Log.Warning($"{content} {ex.Message}{Environment.NewLine}{ex.StackTrace}");
        }
    }
}

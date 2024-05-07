using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using HybridFileXfer.Net.Models;
using HybridFileXfer.Net.Utilities;
using System;
using System.IO;
using System.Windows;

namespace HybridFileXfer.Net.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private Visibility loading = Visibility.Visible;

        public Visibility Loading
        {
            get => loading;
            set { loading = value; OnPropertyChanged(nameof(Loading)); }
        }


        public MainWindowViewModel()
        {
            Log.Info("程序启动");
        }

        /// <summary>
        /// 初始化ADB服务
        /// </summary>
        public void InitAdbServer()
        {
            if (AdbServer.Instance.GetStatus().IsRunning)
            {
                Loading = Visibility.Collapsed;
                return;
            }
            string adbPath = Path.Combine(Environment.CurrentDirectory, "Resources", "PlatformTools", "adb.exe");
            if (!File.Exists(adbPath))
            {
                Growl.Fatal("ABD程序缺失！");
                Loading = Visibility.Collapsed;
                return;
            }
            AdbServer server = new AdbServer();
            StartServerResult result = server.StartServer(adbPath, false);
            if (result != StartServerResult.Started)
            {
                Growl.Warning("打开ABD服务失败！");
            }
            Loading = Visibility.Collapsed;
        }

        public void StopAdbServer()
        {
            AdbServer.Instance.StopServer();
        }
    }
}

using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using HybridFileXfer.Net.Models;
using HybridFileXfer.Net.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HybridFileXfer.Net.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        #region 加载进度条是否可见
        private Visibility loading = Visibility.Visible;

        public Visibility Loading
        {
            get => loading;
            set { loading = value; OnPropertyChanged(nameof(Loading)); }
        }
        #endregion
        /// <summary>
        /// 设备列表
        /// </summary>
        public ObservableCollection<string> DeviceList { get; set; }

        /// <summary>
        /// 网络列表
        /// </summary>
        public ObservableCollection<NetworkInfo> NetworkList { get; set; }

        /// <summary>
        /// 刷新设备按钮
        /// </summary>
        public ICommand RefreshDeviceCommand { get; set; }

        /// <summary>
        /// 刷新网络按钮
        /// </summary>
        public ICommand RefreshNetworkCommand { get; set; }

        /// <summary>
        /// 开启服务
        /// </summary>
        public ICommand StartServiceCommand { get; set; }

        public MainWindowViewModel()
        {
            Log.Info("程序启动");
            DeviceList = new ObservableCollection<string>();
            NetworkList = new ObservableCollection<NetworkInfo>();
            RefreshDeviceCommand = new RelayCommand(GetAllClient);
            RefreshNetworkCommand = new RelayCommand(GetNetworkList);
            StartServiceCommand = new RelayCommand(() => { });
        }

        /// <summary>
        /// 初始化ADB服务
        /// </summary>
        public void InitAdbServer()
        {
            Task.Run(() =>
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

                if (AdbServer.Instance.StartServer(adbPath, false) != StartServerResult.Started)
                {
                    Growl.Fatal("打开ABD服务失败！");
                    Loading = Visibility.Collapsed;
                    return;
                }
                GetAllClient();
                GetNetworkList();
                Loading = Visibility.Collapsed;
            });
        }

        /// <summary>
        /// 停止ADB
        /// </summary>
        public void StopAdbServer()
        {
            AdbServer.Instance.StopServer();
        }

        /// <summary>
        /// 获取所有ADB设备
        /// </summary>
        private void GetAllClient()
        {
            try
            {
                var adbClient = new AdbClient();
                adbClient.Connect("127.0.0.1:62001");
                var deviceList = adbClient.GetDevices().Select(x => x.Name);
                Application.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    DeviceList.Clear();
                    foreach (var device in deviceList)
                    {
                        DeviceList.Add(device);
                    }
                }));

            }
            catch (Exception ex)
            {
                Log.Ex(ex, "获取所有ADB设备异常");
            }
        }

        /// <summary>
        /// 获取网卡
        /// </summary>
        private void GetNetworkList()
        {
            try
            {
                List<NetworkInfo> networkInfos = new List<NetworkInfo>();
                var nics = NetworkInterface.GetAllNetworkInterfaces().Where(x => x.NetworkInterfaceType == NetworkInterfaceType.Ethernet || x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211);
                foreach (var nic in nics)
                {
                    foreach (var address in nic.GetIPProperties().UnicastAddresses.Where(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        networkInfos.Add(new NetworkInfo() { NicName = nic.Name, IPAddress = address.Address.ToString() });
                    }
                }
                Application.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    NetworkList.Clear();
                    foreach (NetworkInfo network in networkInfos)
                    {
                        NetworkList.Add(network);
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.Ex(ex, "IP获取异常");
            }
        }
    }
}

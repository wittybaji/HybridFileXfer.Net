using HybridFileXfer.Net.ViewModels;
using System;
using System.Windows;

namespace HybridFileXfer.Net
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowViewModel _viewModel = new MainWindowViewModel();
        public MainWindow()
        {
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _viewModel.InitAdbServer();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _viewModel.StopAdbServer();
        }
    }
}

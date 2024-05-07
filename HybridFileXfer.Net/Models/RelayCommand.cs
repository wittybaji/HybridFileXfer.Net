using HybridFileXfer.Net.Utilities;
using System;
using System.Windows.Input;

namespace HybridFileXfer.Net.Models
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Action _excute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action excute, Func<bool> canExecute = null)
        {
            _excute = excute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 执行
        /// </summary>
        public void Execute(object parameter)
        {
            SafelyInvoke(_excute);
        }

        /// <summary>
        /// 是否可以执行
        /// </summary>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        /// <summary>
        /// 安全执行
        /// </summary>
        /// <param name="action"></param>
        public void SafelyInvoke(Action action)
        {
            try { action.Invoke(); }
            catch (Exception ex) { Log.Ex(ex, "RelayCommandException"); }
        }

    }
}

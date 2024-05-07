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

        private readonly Action<object> _excute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> excute, Func<object, bool> canExecute = null)
        {
            _excute = excute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            SafelyInvoke(_excute, parameter);
        }

        /// <summary>
        /// 是否可以执行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// 安全执行
        /// </summary>
        /// <param name="action"></param>
        public void SafelyInvoke(Action<object> action, object parameter)
        {
            try { action.Invoke(parameter); }
            catch (Exception ex) { Log.Ex(ex, "RelayCommandException"); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace QinSoft.WPF.Core
{
    /// <summary>
    /// 中继命令
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action _execute;
        readonly Func<bool> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// RelayCommand的构造函数
        /// </summary>
        /// <param name="execute">执行方法的委托</param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// RelayCommand的构造函数
        /// </summary>
        /// <param name="execute">执行方法的委托</param>
        /// <param name="canExecute">执行状态的委托</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members


        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute.Invoke();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        #endregion
    }

    /// <summary>
    /// 中继命令-泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        readonly Action<T> _execute;
        readonly Func<T, bool> _canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// RelayCommand的构造函数
        /// </summary>
        /// <param name="execute">执行方法的委托</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// RelayCommand的构造函数
        /// </summary>
        /// <param name="execute">执行方法的委托</param>
        /// <param name="canExecute">执行状态的委托</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members


        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            if (parameter == null && typeof(T).IsValueType)
                return _canExecute.Invoke(default(T));
            return _canExecute.Invoke((T)parameter);

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion
    }


    public static class CommandTools
    {
        public static void ActiveExecute(this ICommand command, object parameter = null)
        {
            if (command.CanExecute(parameter)) command.Execute(parameter);
        }
    }
}

using System;
using System.Windows.Input;

namespace FileSystemWatcher.Commands
{
    /// <summary>
    /// Basic command definition - implementation od ICommand
    /// </summary>
    public class Command : ICommand
    {
        #region Fields

        public event EventHandler CanExecuteChanged;
        protected Func<object, bool> canExecute;
        protected Action<object> execute;

        #endregion

        #region Constructors

        /// <summary>
        /// Default command, that can be disabled, with defined <see cref="Action"/> to run.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Simple command that is always enabled.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public Command(Action<object> execute) : this(execute, param => true) { }

        public Command(Action execute) : this(param => execute()) { }

        #endregion

        #region Methods

        public bool CanExecute(object parameter) => canExecute(parameter);

        public void Execute(object parameter) => execute(parameter);

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}

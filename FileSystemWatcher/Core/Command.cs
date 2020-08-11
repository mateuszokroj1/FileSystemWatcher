using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace FileSystemWatcher
{
    public class Command : ICommand
    {
        #region Fields

        public event EventHandler CanExecuteChanged;
        private Action<object> toExecute;
        private Func<object, bool> canExecute;

        #endregion

        #region Constructors

        public Command(Action toExecute) : this(param => toExecute(), param => true) { }

        public Command(Action<object> toExecute) : this(toExecute, param => true) { }

        public Command(Action toExecute, Func<object, bool> canExecute) : this(param => toExecute(), canExecute) { }

        public Command(Action<object> toExecute, Func<object, bool> canExecute)
        {
            this.toExecute = toExecute ?? throw new ArgumentNullException(nameof(toExecute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        #endregion

        #region Methods

        public bool CanExecute(object parameter) => this.canExecute?.Invoke(parameter) ?? false;

        public void Execute(object parameter) => this.toExecute(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #endregion
    }
}

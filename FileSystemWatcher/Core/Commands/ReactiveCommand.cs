using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace FileSystemWatcher.Commands
{
    /// <summary>
    /// Reactive version of command, that checks can 
    /// </summary>
    public class ReactiveCommand : Command
    {
        #region Fields

        private readonly ReactiveCommandObserver observer;
        private readonly IDisposable unsubscriber;
        private bool currentValueCanExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates reactive command, that can check canExecute value every time.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public ReactiveCommand(Action<object> execute, IObservable<bool> canExecute) : base(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));

            this.observer = new ReactiveCommandObserver(OnChange);
            this.unsubscriber = canExecute.Subscribe(this.observer);

            this.canExecute = param => this.currentValueCanExecute;
        }

        /// <summary>
        /// Creates reactive command, that can check canExecute value every time.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public ReactiveCommand(Action execute, IObservable<bool> canExecute) : this(param => execute(), canExecute) { }

        #endregion

        #region Methods

        private void OnChange(bool newValue)
        {
            if (this.currentValueCanExecute == newValue)
                return;

            this.currentValueCanExecute = newValue;
            RaiseCanExecuteChanged();
        }

        #endregion
    }
}

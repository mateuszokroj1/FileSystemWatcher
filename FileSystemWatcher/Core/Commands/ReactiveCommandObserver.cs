using System;
using System.Collections.Generic;
using System.Text;

namespace FileSystemWatcher.Commands
{
    internal class ReactiveCommandObserver : IObserver<bool>
    {
        #region Fields

        private readonly Action<bool> onChangeValue;

        #endregion

        #region Constructor

        public ReactiveCommandObserver(Action<bool> onChangeValue)
        {
            this.onChangeValue = onChangeValue ?? throw new ArgumentNullException(nameof(onChangeValue));
        }

        #endregion

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(bool value) => this.onChangeValue.Invoke(value);
    }
}

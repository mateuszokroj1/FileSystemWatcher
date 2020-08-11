using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace FileSystemWatcher.Models
{
    public class WatchingLocation : IDisposable
    {
        #region Fields

        private readonly System.IO.FileSystemWatcher watcher;
        private readonly IDisposable[] unsubscribers = new IDisposable[4];
        private readonly ICollection<HistoryItem> historyItems;
        public event ErrorEventHandler Error;

        #endregion

        #region Constructors

        public WatchingLocation(string path, bool includeSubdirectories, ICollection<HistoryItem> historyItems)
        {
            if (path == null)
                throw new ArgumentNullException();

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException("Directory not found: " + path);

            Path = path;

            if (historyItems == null)
                throw new ArgumentNullException(nameof(historyItems));

            this.watcher = new System.IO.FileSystemWatcher(path)
            {
                IncludeSubdirectories = includeSubdirectories,
                EnableRaisingEvents = true
            };

            this.watcher.Error += Error;

            InitSubscribers();
        }

        #endregion

        #region Properties

        public string Path { get; }

        #endregion

        #region Methods

        private void InitSubscribers()
        {
            this.unsubscribers[0] = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                handler => this.watcher.Created += handler,
                handler => this.watcher.Created -= handler
            ).ObserveOn(SynchronizationContext.Current)
            .Subscribe(args => AddHistoryItem(args.EventArgs.FullPath, Actions.Created));

            this.unsubscribers[1] = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                handler => this.watcher.Changed += handler,
                handler => this.watcher.Changed -= handler
            ).ObserveOn(SynchronizationContext.Current)
            .Subscribe(args => AddHistoryItem(args.EventArgs.FullPath, Actions.Modified));

            this.unsubscribers[2] = Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                handler => this.watcher.Renamed += handler,
                handler => this.watcher.Renamed -= handler
            ).ObserveOn(SynchronizationContext.Current)
            .Subscribe(args => AddHistoryItem($"{args.EventArgs.OldFullPath} -> {args.EventArgs.FullPath}", Actions.Renamed));

            this.unsubscribers[3] = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                handler => this.watcher.Deleted += handler,
                handler => this.watcher.Deleted -= handler
            ).ObserveOn(SynchronizationContext.Current)
            .Subscribe(args => AddHistoryItem(args.EventArgs.FullPath, Actions.Deleted));
        }

        private void AddHistoryItem(string path, Actions action)
        {
            if (this.historyItems.Count == int.MaxValue)
                this.historyItems.Clear();

            this.historyItems.Add(new HistoryItem
            {
                Path = path,
                Time = DateTime.UtcNow,
                Action = action
            });
        }

        public override string ToString() => Path;

        public void Dispose()
        {
            foreach (var unsubscriber in this.unsubscribers)
                unsubscriber?.Dispose();

            this.watcher.Dispose();
        }

        ~WatchingLocation() => Dispose();

        #endregion
    }
}

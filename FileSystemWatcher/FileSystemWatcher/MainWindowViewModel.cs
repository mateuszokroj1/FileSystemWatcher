using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

using FileSystemWatcher.Models;

namespace FileSystemWatcher
{
    public class MainWindowViewModel
    {
        private readonly System.IO.FileSystemWatcher watcher;

        public MainWindowViewModel()
        {
            watcher = new System.IO.FileSystemWatcher
            {
                Path = @"C:\",
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };
            Actions = new ObservableCollection<FileSystemAction>();

            Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                handler => watcher.Created += handler,
                handler => watcher.Created -= handler
            )
            .ObserveOn(SynchronizationContext.Current).Subscribe(args =>
            {
                
            });

            Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                handler => watcher.Changed += handler,
                handler => watcher.Changed -= handler
            )
            .ObserveOn(SynchronizationContext.Current).Subscribe(args =>
            {

            });

            Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                handler => watcher.Renamed += handler,
                handler => watcher.Renamed -= handler
            )
            .ObserveOn(SynchronizationContext.Current).Subscribe(args =>
            {

            });

            Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                handler => watcher.Deleted += handler,
                handler => watcher.Deleted -= handler
            )
            .ObserveOn(SynchronizationContext.Current).Subscribe(args =>
            {

            });
        }

        public ObservableCollection<FileSystemAction> Actions { get; }
    }
}

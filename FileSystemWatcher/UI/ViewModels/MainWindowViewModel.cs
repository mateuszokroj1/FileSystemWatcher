using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using FileSystemWatcher.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

namespace FileSystemWatcher.UI.ViewModels
{
    public class MainWindowViewModel
    {
        #region Constructors

        public MainWindowViewModel()
        {
            ClearHistoryCommand = new Command(() => History.Clear());
            SelectLocationCommand = new Command(() => SelectLocation());

            LocationToAdd = new ReactiveProperty<string>();
            IncludeSubdirectories = new ReactiveProperty<bool>(false);
            SelectedLocation = new ReactiveProperty<WatchingLocation>();

            AddLocationCommand = new ReactiveCommand();
            RemoveLocationCommand = new ReactiveCommand();
        }

        #endregion

        #region Properties

        public ObservableCollection<WatchingLocation> Locations { get; } = new ObservableCollection<WatchingLocation>();

        public ObservableCollection<HistoryItem> History { get; } = new ObservableCollection<HistoryItem>();

        public ReactiveProperty<string> LocationToAdd { get; set; }

        public ReactiveProperty<bool> IncludeSubdirectories { get; set; }

        public ReactiveProperty<WatchingLocation> SelectedLocation { get; set; }

        public ReactiveCommand AddLocationCommand { get; }

        public ReactiveCommand RemoveLocationCommand { get; }

        public Command SelectLocationCommand { get; }

        public Command ClearHistoryCommand { get; }

        #endregion

        #region Methods

        public void AddLocation()
        {
            if (!LocationCanBeAdd(LocationToAdd.Value))
                return;

            var location = new WatchingLocation(LocationToAdd.Value, IncludeSubdirectories.Value, History);
            location.Error += (sender, e) => OnWatcherError(sender as WatchingLocation, e.GetException());

            Locations.Add(location);

            LocationToAdd.Value = null;
            IncludeSubdirectories.Value = false;
        }

        public void SelectLocation()
        {
            var dialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                AllowNonFileSystemItems = false,
                AllowPropertyEditing = false,
                DefaultDirectoryShellContainer = ShellFileSystemFolder.FromFolderPath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)),
                EnsurePathExists = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Multiselect = false,
                NavigateToShortcut = true,
                ShowPlacesList = true,
                Title = "Select path to watch"
            };

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            LocationToAdd.Value = dialog.FileName;
        }

        public void OnWatcherError(WatchingLocation location, Exception e)
        {
            var message = new StringBuilder();

            message.AppendLine(location != null ? $"Cannot continue watching on location: '{location.Path}'." : "Cannot continue watching.");

            if (e != null)
            {
                message.AppendLine(e.GetType().FullName);
                message.Append(e.StackTrace);
            }

            MessageBox.Show(message.ToString(), "File System Watcher", MessageBoxButton.OK, MessageBoxImage.Warning);

            if (location != null && Locations.Contains(location))
                Locations.Remove(location);
        }

        public void RemoveLocation()
        {
            if (SelectedLocation == null)
                return;

            Locations.Remove(SelectedLocation);
        }

        private bool LocationIsAdded(string location) =>
            Locations
            .Where(watcherLocation => watcherLocation.Path == location)
            .Count() > 0;

        private bool LocationCanBeAdd(string location) =>
            !LocationIsAdded(location) && Directory.Exists(location);

        #endregion
    }
}

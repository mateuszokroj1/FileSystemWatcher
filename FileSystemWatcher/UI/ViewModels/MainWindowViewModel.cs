using FileSystemWatcher.Commands;
using FileSystemWatcher.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemWatcher.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;
        private string locationToAdd;
        private WatchingLocation selectedLocation;

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            Locations = new ObservableCollection<WatchingLocation>();
            History = new ObservableCollection<HistoryItem>();

            ClearHistoryCommand = new Command(() => History.Clear());

            AddLocationCommand = new ReactiveCommand()
        }

        #endregion

        #region Properties

        public ObservableCollection<WatchingLocation> Locations { get; }

        public string LocationToAdd
        {
            get => this.locationToAdd;
            set
            {
                if(this.locationToAdd != value)
                {
                    this.locationToAdd = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LocationToAdd)));
                }
            }
        }

        public WatchingLocation SelectedLocation
        {
            get => this.selectedLocation;
            set
            {
                if(this.selectedLocation != value)
                {
                    this.selectedLocation = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedLocation)));
                }
            }
        }

        public ObservableCollection<HistoryItem> History { get; }

        public ReactiveCommand AddLocationCommand { get; }

        public ReactiveCommand RemoveLocationCommand { get; }

        public Command ClearHistoryCommand { get; }

        #endregion

        #region Methods

        public void AddLocation()
        {

        }

        public void RemoveLocation()
        {
            if (SelectedLocation == null)
                return;
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

using System;

namespace FileSystemWatcher.Models
{
    public class HistoryItem
    {
        public string Path { get; set; }
        public Actions Action { get; set; }
        public DateTime Time { get; set; }
    }
}

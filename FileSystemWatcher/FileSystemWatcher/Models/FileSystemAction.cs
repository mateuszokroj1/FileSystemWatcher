using System;
using System.Collections.Generic;
using System.Text;

namespace FileSystemWatcher.Models
{
    public class FileSystemAction
    {
        public string Filename { get; set; }
        public Actions Action { get; set; }
        public DateTime ActionTime { get; set; }
    }
}

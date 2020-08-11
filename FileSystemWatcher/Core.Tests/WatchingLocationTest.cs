using System;
using Xunit;

using FileSystemWatcher.Models;

namespace FileSystemWatcher.Tests
{
    public class WatchingLocationTest
    {
        [Fact]
        public void Constructor_WhenPathIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new WatchingLocation(null, false, null));
        }

        [Fact]
        public void Constructor_WhenHistoryItemsIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new WatchingLocation("C:\\", false, null));
        }


    }
}

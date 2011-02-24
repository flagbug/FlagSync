using System;
using System.Collections;
using System.Collections.Generic;

namespace FlagSync.Core.Test.VirtualFileSystem
{
    class VirtualFileCollection : ICollection<VirtualFileInfo>
    {
        private VirtualDirectoryInfo ownerDirectory;
        private List<VirtualFileInfo> internList;

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.UnauthorizedAccessException">The exception that is thrown when the owner directory is locked</exception>
        public void Add(VirtualFileInfo item)
        {
            if (this.ownerDirectory.IsLocked)
                throw new UnauthorizedAccessException("The directory is locked!");

            this.internList.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(VirtualFileInfo item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(VirtualFileInfo[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(VirtualFileInfo item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<VirtualFileInfo> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public VirtualFileCollection(VirtualDirectoryInfo ownerDirectory)
        {
            if (ownerDirectory == null)
                throw new ArgumentNullException("ownerDirectory");

            this.ownerDirectory = ownerDirectory;
            this.internList = new List<VirtualFileInfo>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlagSync2.Core
{
    public class SyncInfoCreator : FolderInfoCreator
    {
        #region Private fields
        private DirectoryInfo directoryA;
        private DirectoryInfo directoryB;
        #endregion

        #region Public properties
        public DirectoryInfo DirectoryA
        {
            get
            {
                return this.directoryA;
            }

            set
            {
                this.directoryA = value;
            }
        }

        public DirectoryInfo DirectoryB
        {
            get
            {
                return this.directoryB;
            }

            set
            {
                this.directoryB = value;
            }
        }
        #endregion

        #region Constructor
        public SyncInfoCreator()
        {

        }

        public SyncInfoCreator(DirectoryInfo directoryA, DirectoryInfo directoryB)
        {
            this.directoryA = directoryA;
            this.directoryB = directoryB;
        }
        #endregion

        #region Public methods
        public override IEnumerable<SyncInfo> CreateSyncInfos()
        {
            List<SyncInfo> syncInfos = new List<SyncInfo>();

            syncInfos.AddRange(this.CreateBackupList(this.directoryA, this.directoryB));
            syncInfos.AddRange(this.CreateBackupList(this.directoryB, this.directoryA));

            return syncInfos;
        }
        #endregion
    }
}

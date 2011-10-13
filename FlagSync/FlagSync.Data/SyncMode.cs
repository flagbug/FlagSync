namespace FlagSync.Data
{
    /// <summary>
    /// Specifies the sync mode of a job. Used by the <see cref="FlagSync.Data.JobSetting"/> class.
    /// </summary>
    public enum SyncMode
    {
        /// <summary>
        /// Backup on the local file system
        /// </summary>
        LocalBackup,

        /// <summary>
        /// Synchronization on the local file System
        /// </summary>
        LocalSynchronization,

        /// <summary>
        /// Backup on a FTP server
        /// </summary>
        FtpBackup,

        /// <summary>
        /// Synchronization on a FTP server
        /// </summary>
        FtpSynchronization,

        /// <summary>
        /// Synchronization of an extern media device with iTunes
        /// </summary>
        ITunes
    }
}
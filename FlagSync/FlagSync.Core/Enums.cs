namespace FlagSync.Core
{
    /// <summary>
    /// Sync mode of a job
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

    /// <summary>
    /// The direction of the FTP backup
    /// </summary>
    public enum FtpBackupDirection
    {
        /// <summary>
        /// Backup from the FTP server to the local computer
        /// </summary>
        LocalToRemote,

        /// <summary>
        /// Backup from the local computer to the FTP server
        /// </summary>
        RemoteToLocal
    }
}
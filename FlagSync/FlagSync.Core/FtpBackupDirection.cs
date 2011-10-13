namespace FlagSync.Core
{
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
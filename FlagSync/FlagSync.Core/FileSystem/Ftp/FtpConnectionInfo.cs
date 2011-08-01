using System;
using System.Net;

namespace FlagSync.Core.FileSystem.Ftp
{
    internal class FtpConnectionInfo
    {
        /// <summary>
        /// Gets or sets the FTP server address.
        /// </summary>
        /// <value>The FTP server address.</value>
        public string FtpAddress { get; private set; }

        /// <summary>
        /// Gets or sets the FTP server credential.
        /// </summary>
        /// <value>The FTP server credential.</value>
        public NetworkCredential FtpCredential { get; private set; }

        /// <summary>
        /// Gets or sets the address of the proxy server.
        /// </summary>
        /// <value>The address of the proxy server.</value>
        public string ProxyAddress { get; private set; }

        /// <summary>
        /// Gets or sets the proxy credential.
        /// </summary>
        /// <value>The proxy credential.</value>
        public NetworkCredential ProxyCredential { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the proxy server should be used.
        /// </summary>
        /// <value><c>true</c> if the proxy server should be used; otherwise, <c>false</c>.</value>
        public bool UseProxy { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpConnectionInfo"/> class.
        /// </summary>
        /// <param name="ftpAddress">The FTP address.</param>
        /// <param name="ftpCredential">The FTP credential.</param>
        /// <param name="proxyAddress">The proxy address.</param>
        /// <param name="proxyCredential">The proxy credential.</param>
        /// <param name="useProxy">if set to <c>true</c> [use proxy].</param>
        public FtpConnectionInfo(string ftpAddress, NetworkCredential ftpCredential,
            string proxyAddress, NetworkCredential proxyCredential, bool useProxy)
        {
            if (ftpAddress == null)
                throw new ArgumentNullException("ftpAddress");

            if (ftpCredential == null)
                throw new ArgumentNullException("ftpCredential");

            if (proxyAddress == null)
                throw new ArgumentNullException("proxyAddress");

            if (proxyCredential == null)
                throw new ArgumentNullException("proxyCredential");

            this.FtpAddress = ftpAddress;
            this.FtpCredential = ftpCredential;
            this.ProxyAddress = proxyAddress;
            this.ProxyCredential = proxyCredential;
            this.UseProxy = useProxy;
        }
    }
}
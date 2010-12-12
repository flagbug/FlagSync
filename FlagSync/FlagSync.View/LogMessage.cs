namespace FlagSync.View
{
    public class LogMessage
    {
        /// <summary>
        /// Gets or sets the type (file or directory).
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the path of file A.
        /// </summary>
        /// <value>The path of file A.</value>
        public string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the path of file B.
        /// </summary>
        /// <value>The path of file B.</value>
        public string TargetPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"></see> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action">The action.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="targetPath">The target path.</param>
        public LogMessage(string type, string action, string sourcePath, string targetPath)
        {
            this.Type = type;
            this.Action = action;
            this.SourcePath = sourcePath;
            this.TargetPath = targetPath;
        }
    }
}
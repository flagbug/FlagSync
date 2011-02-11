using System.Reflection;

namespace FlagSync.View
{
    public class AboutBoxViewModel
    {
        /// <summary>
        /// Gets the application version.
        /// </summary>
        /// <value>The application version.</value>
        public string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        /// <summary>
        /// Gets the author of the application
        /// </summary>
        /// <value>The author of the application.</value>
        public string Author
        {
            get { return "Dennis Daume"; }
        }

        /// <summary>
        /// Gets the title of the application.
        /// </summary>
        /// <value>The title of the application.</value>
        public string Title
        {
            get { return "FlagSync"; }
        }
    }
}
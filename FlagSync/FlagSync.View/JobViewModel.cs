using FlagSync.Core;

namespace FlagSync.View
{
    internal class JobViewModel
    {
        private Job model;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public JobViewModel(Job model)
        {
            this.model = model;
        }

        /// <summary>
        /// Gets the directory A.
        /// </summary>
        public string DirectoryA
        {
            get { return this.model.DirectoryA.FullName; }
        }

        /// <summary>
        /// Gets the directory B.
        /// </summary>
        public string DirectoryB
        {
            get { return this.model.DirectoryB.FullName; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return this.model.Name; }
        }
    }
}
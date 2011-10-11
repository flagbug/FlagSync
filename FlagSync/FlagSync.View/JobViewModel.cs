using FlagSync.Core;

namespace FlagSync.View
{
    internal class JobViewModel
    {
        private Job model;

        public JobViewModel(Job model)
        {
            this.model = model;
        }

        public string DirectoryA
        {
            get { return this.model.DirectoryA.FullName; }
        }

        public string DirectoryB
        {
            get { return this.model.DirectoryB.FullName; }
        }

        public string Name
        {
            get { return this.model.Name; }
        }
    }
}
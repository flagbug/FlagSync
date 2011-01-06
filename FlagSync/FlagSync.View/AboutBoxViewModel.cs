using System.Reflection;

namespace FlagSync.View
{
    public class AboutBoxViewModel
    {
        public string Version { get; private set; }
        public string Author { get; private set; }
        public string Title { get; private set; }

        public AboutBoxViewModel()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            this.Version = assembly.GetName().Version.ToString();
            this.Author = "Dennis Daume";
            this.Title = "FlagSync";
        }
    }
}
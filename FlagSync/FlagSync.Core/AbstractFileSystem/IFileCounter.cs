namespace FlagSync.Core.AbstractFileSystem
{
    internal interface IFileCounter
    {
        FileCounterResults CountJobFiles(JobSetting settings);
    }
}
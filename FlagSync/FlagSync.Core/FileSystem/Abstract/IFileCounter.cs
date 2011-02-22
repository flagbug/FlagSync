namespace FlagSync.Core.FileSystem.Abstract
{
    internal interface IFileCounter
    {
        FileCounterResults CountJobFiles(JobSetting settings);
    }
}
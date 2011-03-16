using FlagLib.Patterns;

namespace FlagSync.View
{
    class ITunesJobSettingsViewModel : ViewModelBase<ITunesJobSettingsViewModel>
    {
        private JobSettingViewModel jobSetting;

        /// <summary>
        /// Gets or sets the job setting.
        /// </summary>
        /// <value>The job setting.</value>
        public JobSettingViewModel JobSetting
        {
            get { return this.jobSetting; }
            set
            {
                if (this.JobSetting != value)
                {
                    this.jobSetting = value;
                    this.OnPropertyChanged(vm => vm.JobSetting);
                }
            }
        }
    }
}
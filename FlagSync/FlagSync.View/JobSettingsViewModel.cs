using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FlagLib.Patterns;
using FlagSync.Core;

namespace FlagSync.View
{
    public class JobSettingsViewModel : ViewModelBase<JobSettingsViewModel>
    {
        private ObservableCollection<JobSettingViewModel> jobSettings = new ObservableCollection<JobSettingViewModel>();
        private JobSettingViewModel selectedJobSetting;

        /// <summary>
        /// Gets the job settings.
        /// </summary>
        /// <value>The job settings.</value>
        public ObservableCollection<JobSettingViewModel> JobSettings
        {
            get
            {
                return this.jobSettings;
            }
        }

        /// <summary>
        /// Gets the intern job settings.
        /// </summary>
        /// <value>The intern job settings.</value>
        public IEnumerable<JobSetting> InternJobSettings
        {
            get
            {
                return this.jobSettings.Select(setting => setting.InternJobSetting);
            }
        }

        /// <summary>
        /// Gets the included intern job settings.
        /// </summary>
        /// <value>The included intern job settings.</value>
        public IEnumerable<JobSetting> IncludedInternJobSettings
        {
            get
            {
                return this.InternJobSettings.Where(setting => setting.IsIncluded);
            }
        }

        /// <summary>
        /// Gets or sets the selected job setting.
        /// </summary>
        /// <value>The selected job setting.</value>
        public JobSettingViewModel SelectedJobSetting
        {
            get
            {
                return this.selectedJobSetting;
            }

            set
            {
                if (this.SelectedJobSetting != value)
                {
                    this.selectedJobSetting = value;
                    this.OnPropertyChanged(view => view.SelectedJobSetting);
                }
            }
        }

        /// <summary>
        /// Adds a new job setting.
        /// </summary>
        public void AddNewJobSetting()
        {
            this.JobSettings.Add(new JobSettingViewModel("New Job " + (this.JobSettings.Count + 1)));
        }

        /// <summary>
        /// Deletes the selected job setting.
        /// </summary>
        public void DeleteSelectedJobSetting()
        {
            this.jobSettings.Remove(this.SelectedJobSetting);

            if (this.JobSettings.Count == 0)
            {
                this.AddNewJobSetting();
            }

            this.SelectedJobSetting = this.JobSettings[this.JobSettings.Count - 1];
        }

        /// <summary>
        /// Saves the job settings.
        /// </summary>
        /// <param name="path">The path.</param>
        public void SaveJobSettings(string path)
        {
            JobSettingSerializer.Save(this.InternJobSettings.ToList(), path);
        }

        /// <summary>
        /// Loads the job settings.
        /// </summary>
        /// <param name="path">The path.</param>
        public void LoadJobSettings(string path)
        {
            this.JobSettings.Clear();

            IEnumerable<JobSetting> settings = JobSettingSerializer.Read(path);

            foreach (JobSetting setting in settings)
            {
                this.JobSettings.Add(new JobSettingViewModel(setting));
            }
        }
    }
}
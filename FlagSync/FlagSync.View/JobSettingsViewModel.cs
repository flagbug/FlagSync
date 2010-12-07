using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FlagSync.Core;

namespace FlagSync.View
{
    public class JobSettingsViewModel : INotifyPropertyChanged
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
                    this.OnPropertyChanged("SelectedJobSetting");
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
            JobSettingSerializer.Save(this.InternJobSettings, path);
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

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property has been changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
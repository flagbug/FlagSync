using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlagLib.Patterns;
using FlagLib.Serialization;
using FlagSync.Core;

namespace FlagSync.View
{
    public class JobSettingsViewModel : ViewModelBase<JobSettingsViewModel>
    {
        private JobSettingViewModel selectedJobSetting;

        /// <summary>
        /// Gets the job settings.
        /// </summary>
        /// <value>The job settings.</value>
        public ObservableCollection<JobSettingViewModel> JobSettings { get; private set; }

        /// <summary>
        /// Gets the current job settings panel.
        /// </summary>
        /// <value>The current job settings panel.</value>
        public ObservableCollection<UserControl> CurrentJobSettingsPanel { get; private set; }

        /// <summary>
        /// Gets the intern job settings.
        /// </summary>
        /// <value>The intern job settings.</value>
        public IEnumerable<JobSetting> InternJobSettings
        {
            get { return this.JobSettings.Select(setting => setting.InternJobSetting); }
        }

        /// <summary>
        /// Gets the included intern job settings.
        /// </summary>
        /// <value>The included intern job settings.</value>
        public IEnumerable<JobSetting> IncludedInternJobSettings
        {
            get { return this.InternJobSettings.Where(setting => setting.IsIncluded); }
        }

        /// <summary>
        /// Gets or sets the selected job setting.
        /// </summary>
        /// <value>The selected job setting.</value>
        public JobSettingViewModel SelectedJobSetting
        {
            get { return this.selectedJobSetting; }
            set
            {
                if (this.SelectedJobSetting != value)
                {
                    this.selectedJobSetting = value;
                    this.OnPropertyChanged(vm => vm.SelectedJobSetting);

                    this.CurrentJobSettingsPanel.Clear();

                    switch (value.SyncMode)
                    {
                        case SyncMode.LocalBackup:
                        case SyncMode.LocalSynchronization:
                            this.CurrentJobSettingsPanel.Add(new LocalJobSettingsPanel(value));
                            break;

                        case SyncMode.FtpBackup:
                        case SyncMode.FtpSynchronization:
                            this.CurrentJobSettingsPanel.Add(new FtpJobSettingsPanel(value));
                            break;

                        case SyncMode.ITunes:
                            this.CurrentJobSettingsPanel.Add(new ITunesJobSettingsPanel(value));
                            break;
                    }
                }
            }
        }

        public ICommand DeleteSelectedJobSettingCommand
        {
            get
            {
                return new RelayCommand
                (
                    arg =>
                    {
                        this.JobSettings.Remove(this.SelectedJobSetting);

                        if (this.JobSettings.Count == 0)
                        {
                            this.AddNewJobSetting(SyncMode.LocalBackup);
                        }

                        this.SelectedJobSetting = this.JobSettings.Last();
                    }
                );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSettingsViewModel"/> class.
        /// </summary>
        public JobSettingsViewModel()
        {
            this.JobSettings = new ObservableCollection<JobSettingViewModel>();
            this.CurrentJobSettingsPanel = new ObservableCollection<UserControl>();
            this.AddNewJobSetting(SyncMode.LocalBackup);
            this.SelectedJobSetting = this.JobSettings[0];
        }

        /// <summary>
        /// Adds a new job setting with the specified mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        public void AddNewJobSetting(SyncMode mode)
        {
            JobSettingViewModel setting = new JobSettingViewModel(Properties.Resources.NewJobString + " " + (this.JobSettings.Count + 1));
            setting.SyncMode = mode;

            this.JobSettings.Add(setting);
        }

        /// <summary>
        /// Saves the job settings.
        /// </summary>
        /// <param name="path">The path.</param>
        public void SaveJobSettings(string path)
        {
            GenericXmlSerializer.SerializeCollection<JobSetting>(this.InternJobSettings.ToList(), path);
        }

        /// <summary>
        /// Loads the job settings.
        /// </summary>
        /// <param name="path">The path.</param>
        public bool TryLoadJobSettings(string path)
        {
            IEnumerable<JobSetting> settings;

            try
            {
                settings = GenericXmlSerializer.DeserializeCollection<JobSetting>(path);
            }

            catch (InvalidOperationException)
            {
                return false;
            }

            if (settings.Any(setting => setting.SyncMode == SyncMode.ITunes) && !MainViewModel.IsITunesOpened)
            {
                MessageBox.Show(
                    Properties.Resources.iTunesErrorMessageBoxText,
                    Properties.Resources.iTunesErrorMessageBoxCaption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }

            this.JobSettings.Clear();

            foreach (JobSetting setting in settings)
            {
                this.JobSettings.Add(new JobSettingViewModel(setting));
            }

            if (settings.Any())
            {
                this.SelectedJobSetting = this.JobSettings.First();
            }

            return true;
        }
    }
}
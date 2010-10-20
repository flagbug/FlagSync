using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using FlagSync.Core;
using System.ComponentModel;

namespace FlagSync.View
{
    public class JobListBoxViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<JobSetting> jobSettings = new ObservableCollection<JobSetting>();
        private JobSetting selectedJobSetting;

        public ObservableCollection<JobSetting> JobSettings
        {
            get
            {
                return this.jobSettings;
            }
        }

        public JobSetting SelectedJobSetting
        {
            get
            {
                return this.selectedJobSetting;
            }

            set
            {
                if(this.SelectedJobSetting != value)
                {
                    this.selectedJobSetting = value;
                    this.OnPropertyChanged("SelectedJobSetting");
                }
            }
        }

        public void AddNewJobSetting()
        {
            this.JobSettings.Add(new JobSetting("New Job " + (this.JobSettings.Count + 1)));
        }

        public void DeleteSelectedJobSetting()
        {
            this.jobSettings.Remove(this.SelectedJobSetting);

            if(this.JobSettings.Count == 0)
            {
                this.AddNewJobSetting();
            }

            this.SelectedJobSetting = this.JobSettings[this.JobSettings.Count - 1];
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
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

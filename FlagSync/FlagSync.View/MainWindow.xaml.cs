using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlagSync.Core;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the exitMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles the Click event of the newJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void newJobButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobSettingsViewModel.AddNewJobSetting();
        }

        /// <summary>
        /// Handles the Checked event of the jobSettingsBackupModeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void jobSettingsBackupModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobSettingsViewModel.SelectedJobSetting.SyncMode = JobWorker.SyncMode.Backup;
        }

        /// <summary>
        /// Handles the Checked event of the jobSettingsSyncModeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void jobSettingsSyncModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobSettingsViewModel.SelectedJobSetting.SyncMode = JobWorker.SyncMode.Synchronization;
        }

        /// <summary>
        /// Handles the SelectionChanged event of the jobListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void jobListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.mainViewModel.JobSettingsViewModel.SelectedJobSetting == null)
                return;

            switch (this.mainViewModel.JobSettingsViewModel.SelectedJobSetting.SyncMode)
            {
                case JobWorker.SyncMode.Backup:
                    this.jobSettingsBackupModeRadioButton.IsChecked = true;
                    break;

                case JobWorker.SyncMode.Synchronization:
                    this.jobSettingsSyncModeRadioButton.IsChecked = true;
                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the deleteJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void deleteJobButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobSettingsViewModel.DeleteSelectedJobSetting();
        }
    }
}

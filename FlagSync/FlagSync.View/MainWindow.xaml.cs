using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FlagLib.Collections;
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
            Properties.Resources.Culture = Properties.Settings.Default.Language;
            InitializeComponent();

            this.mainViewModel.JobSettingsViewModel.SelectedJobSetting = this.mainViewModel.JobSettingsViewModel.JobSettings[0];
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
            this.mainViewModel.JobSettingsViewModel.SelectedJobSetting.SyncMode = SyncMode.Backup;
        }

        /// <summary>
        /// Handles the Checked event of the jobSettingsSyncModeRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void jobSettingsSyncModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobSettingsViewModel.SelectedJobSetting.SyncMode = SyncMode.Synchronization;
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
                case SyncMode.Backup:
                    this.jobSettingsBackupModeRadioButton.IsChecked = true;
                    break;

                case SyncMode.Synchronization:
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

        /// <summary>
        /// Handles the Click event of the loadJobsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void loadJobsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = "|*.xml";
                dialog.InitialDirectory = this.mainViewModel.AppDataFolderPath;
                dialog.Multiselect = false;

                dialog.FileOk += (dialogSender, dialogEventArgs) =>
                {
                    this.mainViewModel.JobSettingsViewModel.LoadJobSettings(dialog.FileName);
                };

                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of the saveJobsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void saveJobsButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.SaveFileDialog())
            {
                dialog.AddExtension = true;
                dialog.DefaultExt = ".xml";
                dialog.FileName = "NewJobSettings";
                dialog.InitialDirectory = this.mainViewModel.AppDataFolderPath;

                dialog.FileOk += (dialogSender, dialogEventArgs) =>
                    {
                        this.mainViewModel.JobSettingsViewModel.SaveJobSettings(dialog.FileName);
                    };

                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of the directoryAButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void directoryAButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobSettingsViewModel.SelectedJobSetting.DirectoryA = this.ShowFolderDialog();
        }

        /// <summary>
        /// Handles the Click event of the directoryBButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void directoryBButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobSettingsViewModel.SelectedJobSetting.DirectoryB = this.ShowFolderDialog();
        }

        /// <summary>
        /// Shows a folder dialog.
        /// </summary>
        /// <returns>The selected folder</returns>
        private string ShowFolderDialog()
        {
            string selectedFolder = String.Empty;

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                selectedFolder = dialog.SelectedPath;
            }

            return selectedFolder;
        }

        /// <summary>
        /// Handles the Click event of the previewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void previewButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobWorkerViewModel.ResetJobWorker();

            this.mainViewModel.JobWorkerViewModel.StartJobWorker(this.mainViewModel.JobSettingsViewModel.IncludedInternJobSettings, true);
            this.mainTabControl.SelectedIndex = 1;
        }

        /// <summary>
        /// Handles the Click event of the startButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobWorkerViewModel.ResetJobWorker();

            this.mainViewModel.JobWorkerViewModel.StartJobWorker(this.mainViewModel.JobSettingsViewModel.IncludedInternJobSettings, false);
            this.mainTabControl.SelectedIndex = 1;
        }

        /// <summary>
        /// Handles the Click event of the pauseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            bool isJobWorkerPaused = this.mainViewModel.JobWorkerViewModel.IsPaused;

            if (isJobWorkerPaused)
            {
                this.mainViewModel.JobWorkerViewModel.ContinueJobWorker();
            }

            else
            {
                this.mainViewModel.JobWorkerViewModel.PauseJobWorker();
            }
        }

        /// <summary>
        /// Handles the Click event of the stopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobWorkerViewModel.StopJobWorker();
        }

        /// <summary>
        /// Handles the TextChanged event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).ScrollToEnd();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThreadSafeObservableCollection<LogMessage> messages = this.mainViewModel.JobWorkerViewModel.LogMessages;
            bool isPreview = this.mainViewModel.JobWorkerViewModel.IsPreview;
            bool isRunning = this.mainViewModel.JobWorkerViewModel.IsRunning;
            bool isPaused = this.mainViewModel.JobWorkerViewModel.IsPaused;

            if (!isPreview && messages.Count > 0 && isRunning && !isPaused)
            {
                ((ListView)sender).ScrollIntoView(messages.Last());
            }
        }

        /// <summary>
        /// Handles the Click event of the englishMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void englishMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = new CultureInfo("en-US");
            Restart();
        }

        /// <summary>
        /// Restarts this instance.
        /// </summary>
        private static void Restart()
        {
            App.Current.Shutdown();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
        }

        /// <summary>
        /// Handles the Click event of the germanMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void germanMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = new CultureInfo("de-DE");
            Restart();
        }

        /// <summary>
        /// Handles the Closing event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Handles the Click event of the aboutMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}
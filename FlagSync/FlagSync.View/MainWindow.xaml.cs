using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length == 2)
            {
                this.WindowState = WindowState.Minimized;

                var vm = this.mainViewModel.JobSettingsViewModel;

                if (!vm.TryLoadJobSettings(args[1]))
                {
                    this.WindowState = WindowState.Maximized;

                    MessageBox.Show(Properties.Resources.LoadSettingsErrorMessage,
                                    Properties.Resources.ErrorString,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);

                    Application.Current.Shutdown();
                }

                else
                {
                    this.StartJobWorker(false);
                }
            }
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
            Button newJobButton = (Button)sender;
            newJobButton.ContextMenu.IsEnabled = true;
            newJobButton.ContextMenu.PlacementTarget = newJobButton;
            newJobButton.ContextMenu.Placement = PlacementMode.Bottom;
            newJobButton.ContextMenu.IsOpen = true;
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
                        var vm = this.mainViewModel.JobSettingsViewModel;

                        if (!vm.TryLoadJobSettings(dialog.FileName))
                        {
                            MessageBox.Show(Properties.Resources.LoadSettingsErrorMessage,
                                            Properties.Resources.ErrorString,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                        }
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
        /// Handles the Click event of the previewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void previewButton_Click(object sender, RoutedEventArgs e)
        {
            this.StartJobWorker(true);
        }

        /// <summary>
        /// Handles the Click event of the startButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.StartJobWorker(false);
        }

        /// <summary>
        /// Starts the job worker.
        /// </summary>
        /// <param name="preview">if set to true a preview will be performed.</param>
        private void StartJobWorker(bool preview)
        {
            this.mainViewModel.JobWorkerViewModel.ResetJobWorker();

            this.mainViewModel.JobWorkerViewModel.StartJobWorker(this.mainViewModel.JobSettingsViewModel.IncludedInternJobSettings, preview);
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
            this.Restart();
        }

        /// <summary>
        /// Handles the Click event of the germanMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void germanMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Language = new CultureInfo("de-DE");
            this.Restart();
        }

        /// <summary>
        /// Restarts this instance.
        /// </summary>
        private void Restart()
        {
            MessageBoxResult result = MessageBox.Show(
                Properties.Resources.ApplicationRestartMessageBoxText,
                Properties.Resources.ApplicationRestartMessageBoxCaption,
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                App.Current.Shutdown();
                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            }
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

        /// <summary>
        /// Handles the Click event of the newLocalJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void newLocalJobButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobSettingsViewModel.AddNewJobSetting(SyncMode.LocalBackup);
            this.CloseNewJobButtonContextMenu();
        }

        /// <summary>
        /// Handles the Click event of the newFtpJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void newFtpJobButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.JobSettingsViewModel.AddNewJobSetting(SyncMode.FtpBackup);
            this.CloseNewJobButtonContextMenu();
        }

        /// <summary>
        /// Handles the Click event of the newITunesJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void newITunesJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainViewModel.IsITunesOpened)
            {
                this.mainViewModel.JobSettingsViewModel.AddNewJobSetting(SyncMode.ITunes);
                this.CloseNewJobButtonContextMenu();
            }

            else
            {
                MessageBox.Show(
                    Properties.Resources.iTunesErrorMessageBoxText,
                    Properties.Resources.iTunesErrorMessageBoxCaption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Closes the new job button's context menu.
        /// </summary>
        /// <param name="button">The button.</param>
        private void CloseNewJobButtonContextMenu()
        {
            this.newJobButton.ContextMenu.IsOpen = false;
        }

        /// <summary>
        /// Handles the RequestNavigate event of the Hyperlink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Navigation.RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
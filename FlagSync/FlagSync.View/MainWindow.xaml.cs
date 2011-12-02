using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FlagSync.Data;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window
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

                var result = this.mainWindowViewModel.LoadJobSettings(args[1]);

                switch (result)
                {
                    case JobSettingsLoadingResult.CorruptFile:
                        {
                            this.WindowState = WindowState.Maximized;

                            MessageBox.Show(Properties.Resources.LoadSettingsErrorMessage,
                                            Properties.Resources.ErrorString,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);

                            Application.Current.Shutdown();
                        }
                        break;

                    case JobSettingsLoadingResult.ITunesNotOpened:
                        {
                            this.WindowState = WindowState.Maximized;

                            ShowITunesErrorMessageBox();

                            Application.Current.Shutdown();
                        }
                        break;

                    case JobSettingsLoadingResult.Succeed:
                        {
                            this.mainWindowViewModel.StartJobWorkerCommand.Execute(false);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the newJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void newJobButton_Click(object sender, RoutedEventArgs e)
        {
            var newJobButton = (Button)sender;
            newJobButton.ContextMenu.IsEnabled = true;
            newJobButton.ContextMenu.PlacementTarget = newJobButton;
            newJobButton.ContextMenu.Placement = PlacementMode.Bottom;
            newJobButton.ContextMenu.IsOpen = true;
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
                dialog.InitialDirectory = DataController.AppDataFolderPath;
                dialog.Multiselect = false;

                dialog.FileOk += (dialogSender, dialogEventArgs) =>
                    {
                        var result = this.mainWindowViewModel.LoadJobSettings(dialog.FileName);

                        switch (result)
                        {
                            case JobSettingsLoadingResult.CorruptFile:
                                {
                                    MessageBox.Show(Properties.Resources.LoadSettingsErrorMessage,
                                                    Properties.Resources.ErrorString,
                                                    MessageBoxButton.OK,
                                                    MessageBoxImage.Error);
                                }
                                break;

                            case JobSettingsLoadingResult.ITunesNotOpened:
                                {
                                    ShowITunesErrorMessageBox();
                                }
                                break;
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
                dialog.InitialDirectory = DataController.AppDataFolderPath;

                dialog.FileOk += (dialogSender, dialogEventArgs) =>
                    this.mainWindowViewModel.SaveJobSettings(dialog.FileName);

                dialog.ShowDialog();
            }
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
            var messages = this.mainWindowViewModel.LogMessages;
            bool isPreview = this.mainWindowViewModel.IsPreview;
            bool isRunning = this.mainWindowViewModel.IsRunning;
            bool isPaused = this.mainWindowViewModel.IsPaused;

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
        /// Restarts this instance.
        /// </summary>
        private static void Restart()
        {
            var result = MessageBox.Show(
                Properties.Resources.ApplicationRestartMessageBoxText,
                Properties.Resources.ApplicationRestartMessageBoxCaption,
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
                Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
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
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the newLocalJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void newLocalJobButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindowViewModel.AddNewJobSetting(SyncMode.LocalBackup);
            this.CloseNewJobButtonContextMenu();
        }

        /// <summary>
        /// Handles the Click event of the newFtpJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void newFtpJobButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindowViewModel.AddNewJobSetting(SyncMode.FtpBackup);
            this.CloseNewJobButtonContextMenu();
        }

        /// <summary>
        /// Handles the Click event of the newITunesJobButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void newITunesJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataController.IsITunesOpened())
            {
                this.mainWindowViewModel.AddNewJobSetting(SyncMode.ITunes);
                this.CloseNewJobButtonContextMenu();
            }

            else
            {
                ShowITunesErrorMessageBox();
            }
        }

        /// <summary>
        /// Closes the new job button's context menu.
        /// </summary>
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

        /// <summary>
        /// Shows the Itunes error message box.
        /// </summary>
        private static void ShowITunesErrorMessageBox()
        {
            MessageBox.Show
            (
                Properties.Resources.iTunesErrorMessageBoxText,
                Properties.Resources.iTunesErrorMessageBoxCaption,
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}
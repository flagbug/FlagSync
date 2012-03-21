using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FlagSync.Data;
using FlagSync.View.Properties;
using FlagSync.View.Views;

namespace FlagSync.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            Properties.Resources.Culture = Settings.Default.Language;
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

                try
                {
                    this.mainWindowViewModel.LoadJobSettings(args[1]);
                }

                catch (CorruptSaveFileException)
                {
                    this.WindowState = WindowState.Maximized;

                    MessageBox.Show
                    (
                        Properties.Resources.LoadSettingsErrorMessage,
                        Properties.Resources.ErrorString,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );

                    Application.Current.Shutdown();
                }

                catch (ITunesNotOpenedException)
                {
                    this.WindowState = WindowState.Maximized;

                    ShowITunesErrorMessageBox();

                    Application.Current.Shutdown();
                }

                this.mainWindowViewModel.StartJobWorkerCommand.Execute(false);
            }
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
                        try
                        {
                            this.mainWindowViewModel.LoadJobSettings(dialog.FileName);
                        }
                        catch (CorruptSaveFileException)
                        {
                            MessageBox.Show
                            (
                                Properties.Resources.LoadSettingsErrorMessage,
                                Properties.Resources.ErrorString,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error
                            );
                        }

                        catch (ITunesNotOpenedException)
                        {
                            ShowITunesErrorMessageBox();
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

            if (!isPreview && messages.Any() && isRunning && !isPaused)
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
            Settings.Default.Language = new CultureInfo("en-US");
            Restart();
        }

        /// <summary>
        /// Handles the Click event of the germanMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void germanMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Language = new CultureInfo("de-DE");
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
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Settings.Default.Save();
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
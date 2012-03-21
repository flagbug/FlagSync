using FlagSync.View.ViewModels;

namespace FlagSync.View.Views
{
    /// <summary>
    /// Interaction logic for JobCompositionControl.xaml
    /// </summary>
    public partial class JobCompositionControl
    {
        public JobCompositionControl(JobCompositionViewModel viewModel)
        {
            InitializeComponent();

            this.DataContext = viewModel;
        }
    }
}
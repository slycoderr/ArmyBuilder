using System.Threading;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel.UiContext = SynchronizationContext.Current;
        }

        private void AddDetachmentButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //AddDetachmentMenu.IsOpen = true;
        }
    }
}
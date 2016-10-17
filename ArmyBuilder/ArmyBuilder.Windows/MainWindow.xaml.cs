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
    }
}
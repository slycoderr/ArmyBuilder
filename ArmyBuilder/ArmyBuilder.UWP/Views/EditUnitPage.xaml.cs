using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.UWP.Views
{
    public sealed partial class EditUnitPage
    {
        public EditUnitPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = ((MainViewModel) Application.Current.Resources["MainViewModel"]).CurrentArmyListViewModel.SelectedUnit;
        }
    }
}
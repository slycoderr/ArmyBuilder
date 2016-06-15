using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.ViewModels;
using ArmyBuilder.UWP.Views;

namespace ArmyBuilder.UWP
{
    public sealed partial class MainPage
    {
        private MainViewModel MainViewModel => DataContext != null && DataContext.GetType() == typeof (MainViewModel) ? (MainViewModel) DataContext : null;

        private static bool navigationLoaded;
        private static bool loadedDatabase;
        
        public MainPage()
        {
            InitializeComponent();
            if (!navigationLoaded)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

                SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
                {
                    if (Frame.CanGoBack)
                    {
                        e.Handled = true;
                        Frame.GoBack();
                    }
                };

                navigationLoaded = true;
                MainViewModel.UiContext = SynchronizationContext.Current;
            }
        }

        public async Task Initalize()
        {
            if (!loadedDatabase)
            {
                var folder = ApplicationData.Current.LocalFolder;

                foreach (var army in Army.Armies)
                {
                    var f = await Package.Current.InstalledLocation.GetFileAsync($"{army}.xml");
                    var data = await folder.TryGetItemAsync($"{ army}.xml");

                    if (data != null)
                    {
                        var currentDbProperties = await data.GetBasicPropertiesAsync();
                        var projectDbProperties = await f.GetBasicPropertiesAsync();

                        if (projectDbProperties.DateModified.DateTime.CompareTo(currentDbProperties.DateModified.DateTime) > 0)
                        {
                            await data.DeleteAsync();
                            await f.CopyAsync(folder);
                        }
                    }

                    else
                    {
                        await f.CopyAsync(folder);
                    }
                }

                await MainViewModel.Load(folder.Path, Army.Armies.Select(a => new FileStream(Path.Combine(folder.Path, $"{a}.xml"), FileMode.Open)).Cast<Stream>().ToList());

                //await MainViewModel.Load(folder.Path, new List<Stream>
                //{
                //    new FileStream(Path.Combine(folder.Path, "SpaceWolves.xml"), FileMode.Open),
                //    new FileStream(Path.Combine(folder.Path, "Necrons.xml"), FileMode.Open)
                //});

                loadedDatabase = true;
            }
        }

        private async void MainPage_OnLoading(FrameworkElement sender, object args)
        {
            ToggleLoading();
            await Initalize();
            ToggleLoading(false);
        }

        private void ToggleLoading(bool show = true)
        {
            //ProgressPanel.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            IsEnabled = !show;
        }

        private void UnitListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WindowSizeStates.CurrentState == WideScreen && UnitEditor != null)
            {
                UnitEditor.Visibility = UnitListView.SelectedItem != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void UnitListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (WindowSizeStates.CurrentState == SmallScreen)
            {
                MainViewModel.CurrentArmyListViewModel.SelectedUnit = (ArmyListData) e.ClickedItem;
                Frame.Navigate(typeof (EditUnitPage));
            }
        }
    }
}
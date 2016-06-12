using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.Utility;
using ArmyBuilder.Core.ViewModels;
using ArmyBuilder.Windows.Views;

namespace ArmyBuilder.Windows
{
    public partial class MainWindow 
    {
        private static bool loadedDatabase;

        private MainViewModel MainViewModel
            =>
                DataContext != null && DataContext.GetType() == typeof (MainViewModel)
                    ? (MainViewModel) DataContext
                    : null;

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel.UiContext = SynchronizationContext.Current;
        }

        private void ToggleLoading(bool show = true)
        {
            //ProgressPanel.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            IsEnabled = !show;
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ToggleLoading();
            await Initalize();
            ToggleLoading(false);
        }


        public async Task Initalize()
        {
            if (!loadedDatabase)
            {
                var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "ArmyBuilder");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

            List<string> dataFiles = new List<string>()
                {
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SpaceWolves.xml"),
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Necrons.xml")
                };

                foreach (var path in dataFiles)
                {
                    var fileName = Path.GetFileName(path);
                    File.Copy(path, Path.Combine(folder, fileName), true);
                }

                await MainViewModel.Load(folder, new List<Stream>
                {
                    new FileStream(Path.Combine(folder, "SpaceWolves.xml"), FileMode.Open),
                    new FileStream(Path.Combine(folder, "Necrons.xml"), FileMode.Open)
                });

                loadedDatabase = true;
            }
        }
    }
}
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.Windows
{
    public partial class App
    {
        public static readonly string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\ArmyBuilder\\");
        public static readonly string ArmyDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\ArmyBuilder\\Data\\");

        protected override void OnStartup(StartupEventArgs e)
        {
            MainViewModel.UiContext = SynchronizationContext.Current;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            if (!Directory.Exists(ArmyDataPath))
            {
                Directory.CreateDirectory(ArmyDataPath);
            }

            var files = Directory.EnumerateFiles(ArmyDataPath).Where(f => f.ToLower().Contains(".xml")).ToList();
            var streams = files.Select(f => new FileStream(f, FileMode.Open)).Cast<Stream>().ToList();

            ((MainViewModel)FindResource("MainViewModel")).LoadArmyData(streams);
            ((MainViewModel)FindResource("MainViewModel")).LoadDatabase(DataPath);

            base.OnStartup(e);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(((Exception) e.ExceptionObject).ToString());
            MessageBox.Show(Current.MainWindow, e.ExceptionObject.ToString(), "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
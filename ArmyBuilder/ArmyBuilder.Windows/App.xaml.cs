using System;
using System.IO;
using System.Windows;
using ArmyBuilder.Core.ViewModels;

namespace ArmyBuilder.Windows
{
    public partial class App
    {
        public static string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ArmyBuilder");

        protected override async void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            await ((MainViewModel) FindResource("MainViewModel")).Load(DataPath);
            base.OnStartup(e);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(((Exception) e.ExceptionObject).ToString());
            MessageBox.Show(Current.MainWindow, e.ExceptionObject.ToString(), "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
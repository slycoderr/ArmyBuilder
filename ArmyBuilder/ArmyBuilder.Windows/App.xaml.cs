using System;
using System.IO;
using System.Windows;
using ArmyBuilder.Core.ViewModels;
using MahApps.Metro;

namespace ArmyBuilder.Windows
{
    public partial class App
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            MahApps.Metro.Accent a = new Accent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            base.OnStartup(e);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(((Exception) e.ExceptionObject).ToString());
            MessageBox.Show(Current.MainWindow, e.ExceptionObject.ToString(), "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
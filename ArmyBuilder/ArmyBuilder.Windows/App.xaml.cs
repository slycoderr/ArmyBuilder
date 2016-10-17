using System;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace ArmyBuilder.Windows
{
    public partial class App
    {
        public static readonly string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\ArmyBuilder\\");

        protected override void OnStartup(StartupEventArgs e)
        {
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
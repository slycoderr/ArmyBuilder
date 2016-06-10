using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ArmyBuilder.Windows
{
    public partial class App : Application
    {
        public static readonly string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\ArmyBuilder\\");

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            base.OnStartup(e);
            ((CollectionViewSource)App.Current.FindResource("ArmyListDataCollection")).GroupDescriptions.Add(new PropertyGroupDescription("UnitData.ForceOrgId"));
            ((CollectionViewSource)App.Current.FindResource("ArmyListCollection")).GroupDescriptions.Add(new PropertyGroupDescription("Army"));
            //((CollectionViewSource)App.Current.FindResource("UnitEquipmentCollection")).GroupDescriptions.Add(new PropertyGroupDescription("UnitData.ForceOrgId"));
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //Logger.Log("CurrentDomainOnUnhandledException", LogType.Fatal, ((Exception)e.ExceptionObject));
            Console.WriteLine(((Exception)e.ExceptionObject).ToString());
            MessageBox.Show(Current.MainWindow, e.ExceptionObject.ToString(), "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

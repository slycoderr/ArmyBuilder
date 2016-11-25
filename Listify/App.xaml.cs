using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArmyBuilder.Core.ViewModels;
using Xamarin.Forms;

[assembly: Dependency(typeof(MainViewModel))]

namespace Listify
{
    public partial class App
    {
        public App(params string[] files)
        {
            InitializeComponent();

            MainPage = new MainPage();
            ((MainViewModel) Current.Resources["MainViewModel"]).LoadArmyData(files);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
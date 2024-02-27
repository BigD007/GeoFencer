using GeoFencer.Services;
using GeoFencer.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoFencer
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MapPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

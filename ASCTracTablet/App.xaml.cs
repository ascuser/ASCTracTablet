using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet
{
    public partial class App : Application
    {
        public static Data.RestManager myRestManager { get; private set; }

        public App()
        {
            InitializeComponent();
            var mydatabase = new Data.ASCTracDB();
            mydatabase.InitTables();
            myRestManager = new Data.RestManager(new Data.RestService());

            MainPage = new NavigationPage(new MainPage(mydatabase));
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

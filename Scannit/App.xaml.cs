using Scannit.Views;
using Xamarin.Forms;

namespace Scannit
{
    public partial class App : Application
    {
        public NavigationPage Navigator { get; private set; }

        public App()
        {
            InitializeComponent();

            Navigator = new NavigationPage(new MainPage());
            MainPage = Navigator;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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

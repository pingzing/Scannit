using System.Diagnostics;
using Scannit.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Scannit
{
    public partial class App : Application
    {
        public NavigationPage Navigator { get; private set; }

        public App()
        {
#if DEBUG
            Xamarin.Forms.Internals.Log.Listeners.Add(new DelegateLogListener((arg1, arg2) => Debug.WriteLine(arg2)));
#endif

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

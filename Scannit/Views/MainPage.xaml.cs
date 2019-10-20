using Scannit.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Scannit.Views
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService(typeof(MainViewModel));
        }

        private async void SettingsButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}

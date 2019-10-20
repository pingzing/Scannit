using Scannit.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Scannit.Views
{
    [DesignTimeVisible(true)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService(typeof(SettingsViewModel));
        }
    }
}
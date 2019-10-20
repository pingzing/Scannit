using Scannit.ViewModels;
using Xamarin.Forms;

namespace Scannit
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            DependencyService.Register<MainViewModel>();
        }

        public MainViewModel MainVM => DependencyService.Get<MainViewModel>();
    }
}

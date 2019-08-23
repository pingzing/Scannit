using System.Threading.Tasks;
using Xamarin.Forms;

namespace Scannit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        public async Task OnCardAdded(ISmartCard smartCard)
        {
            //var travelCard = await CardOperations.ReadTravelCard(smartCard);
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

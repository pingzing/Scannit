using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SmartCards;
using Windows.UI.Xaml;

namespace Scannit.UWP
{
    public sealed partial class MainPage
    {
        private Scannit.App _crossPlatApp;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            _crossPlatApp = new Scannit.App();
            LoadApplication(_crossPlatApp);
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            string selector = SmartCardReader.GetDeviceSelector();
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);

            foreach (DeviceInformation device in devices)
            {
                SmartCardReader reader = await SmartCardReader.FromIdAsync(device.Id);
                reader.CardAdded += Reader_CardAdded;
                reader.CardRemoved += Reader_CardRemoved;
                foreach (var foundCard in (await reader.FindAllCardsAsync()))
                {
                    await ReadCard(foundCard);
                }
            }
        }

        private async void Reader_CardAdded(SmartCardReader sender, CardAddedEventArgs args)
        {
            await ReadCard(args.SmartCard);
        }

        private async Task ReadCard(SmartCard card)
        {
            UwpSmartCard smartCard = new UwpSmartCard(card);
            var travelCard = await CardOperations.ReadTravelCard(smartCard);
            await _crossPlatApp.OnCardAdded(smartCard);
        }

        private void Reader_CardRemoved(SmartCardReader sender, CardRemovedEventArgs args)
        {
            // Update UI, etc.
        }
    }
}

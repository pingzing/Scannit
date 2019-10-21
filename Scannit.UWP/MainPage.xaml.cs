using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scannit.Messaging;
using Windows.Devices.Enumeration;
using Windows.Devices.SmartCards;
using Windows.UI.Xaml;
using Xamarin.Forms;

namespace Scannit.UWP
{
    public sealed partial class MainPage
    {
        private Scannit.App _crossPlatApp;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            Startup.Init(ConfigureServices);
            _crossPlatApp = new Scannit.App();
            LoadApplication(_crossPlatApp);
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Handle setting up any native services here.
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
                    ReadCard(foundCard);
                }
            }
        }

        private void Reader_CardAdded(SmartCardReader sender, CardAddedEventArgs args)
        {
            ReadCard(args.SmartCard);
        }

        private void ReadCard(SmartCard card)
        {
            UwpSmartCard smartCard = new UwpSmartCard(card);
            MessagingCenter.Send(new CardAddedMessage { Card = smartCard }, "");
        }

        private void Reader_CardRemoved(SmartCardReader sender, CardRemovedEventArgs args)
        {
            // Update UI, etc.
        }
    }
}

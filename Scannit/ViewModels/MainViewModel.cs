using Scannit.Messaging;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Scannit.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ScannitSharp.TravelCard _card;
        public ScannitSharp.TravelCard Card
        {
            get => _card;
            set => Set(ref _card, value);
        }

        public MainViewModel()
        {
            MessagingCenter.Subscribe<CardAddedMessage>(this, "", CardAdded);
        }

        private async void CardAdded(CardAddedMessage card)
        {
            if (card?.Card == null)
            {
                // TODO: Display message of sadness.
                return;
            }

            Card = await CardOperations.ReadTravelCard(card.Card);
        }
    }
}

using Scannit.Messaging;
using ScannitSharp;
using System;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;

namespace Scannit.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private TravelCardViewModel _card;
        public TravelCardViewModel Card
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

            var readCard = await CardOperations.ReadTravelCard(card.Card);
            if (readCard == null)
            {
                // TODO: Display more messages of sadness.
                return;
            }

            Card = new TravelCardViewModel(readCard);
        }
    }
}

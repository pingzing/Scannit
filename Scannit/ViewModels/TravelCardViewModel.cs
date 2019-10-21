using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Scannit.ViewModels
{
    public class TravelCardViewModel : ViewModelBase
    {
        private readonly ScannitSharp.TravelCard _backingCard;

        public TravelCardViewModel(ScannitSharp.TravelCard backingCard)
        {
            _backingCard = backingCard;
            SetExpiryString(backingCard);
        }

        // TODO: Break Season Pass stuff apart into SeasonPassViewModels, and just display both.

        private string _seasonPassExpiryString;
        public string SeasonPassExpiryString
        {
            get => _seasonPassExpiryString;
            set => Set(ref _seasonPassExpiryString, value);
        }

        public DateTimeOffset? LatestExpiryDate
        {
            get
            {
                var latest = _backingCard?.PeriodPass?.PeriodEndDate1;
                if (_backingCard?.PeriodPass?.PeriodEndDate2 > latest)
                {
                    latest = _backingCard?.PeriodPass?.PeriodEndDate2;
                }
                return latest;
            }
        }

        public string SeasonPassLoadDate => _backingCard?.PeriodPass?.LoadedPeriodDateTime.ToString("d");
        public string SeasonPassLastLoadNumberOfDays => _backingCard?.PeriodPass?.LoadedPeriodLength.ToString();
        public string SeasonPassLastLoadPrice => $"€{_backingCard?.PeriodPass?.LoadedPeriodPrice / 100}.";
        public string ValueString => $"€{_backingCard.StoredValueCents / 100m}";

        private void SetExpiryString(ScannitSharp.TravelCard card)
        {
            if (card == null)
            {
                return;
            }

            var now = DateTimeOffset.UtcNow;
            var latestEndDate = card.PeriodPass.PeriodEndDate1;
            if (card.PeriodPass.PeriodEndDate2 > latestEndDate)
            {
                latestEndDate = card.PeriodPass.PeriodEndDate2;
            }

            TimeSpan timeRemaining = latestEndDate - now;
            if (timeRemaining < TimeSpan.Zero)
            {
                timeRemaining = TimeSpan.Zero;
            }

            if (timeRemaining.Days >= 1)
            {
                SeasonPassExpiryString = String.Format(AppResources.SeasonPassDaysRemaining, timeRemaining.Days);
            }
            else
            {
                string sep = DateTimeFormatInfo.CurrentInfo.TimeSeparator;
                SeasonPassExpiryString = String.Format(AppResources.SeasonPassHoursRemaining, timeRemaining.Hours, timeRemaining.Minutes);
            }
        }
    }
}

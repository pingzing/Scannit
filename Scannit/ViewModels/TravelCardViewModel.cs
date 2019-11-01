using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ScannitSharp;

namespace Scannit.ViewModels
{
    public class TravelCardViewModel : ViewModelBase
    {
        private readonly ScannitSharp.TravelCard _backingCard;

        public TravelCardViewModel(TravelCard backingCard)
        {
            _backingCard = backingCard;
            SeasonPasses[0] = new SeasonPassViewModel(_backingCard.PeriodPass.ProductCode1,
                _backingCard.PeriodPass.PeriodStartDate1,
                _backingCard.PeriodPass.PeriodEndDate1,
                _backingCard.PeriodPass.ValidityArea1);
            SeasonPasses[1] = new SeasonPassViewModel(_backingCard.PeriodPass.ProductCode2,
                _backingCard.PeriodPass.PeriodStartDate2,
                _backingCard.PeriodPass.PeriodEndDate2,
                _backingCard.PeriodPass.ValidityArea2);
        }

        private SeasonPassViewModel[] _seasonPasses = new SeasonPassViewModel[2];
        public SeasonPassViewModel[] SeasonPasses
        {
            get => _seasonPasses;
            set => Set(ref _seasonPasses, value);
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
        public string SeasonPassLastLoadPrice
        {
            get
            {
                if (_backingCard?.PeriodPass != null)
                {
                    return "€" + (_backingCard.PeriodPass.LoadedPeriodPrice / 100).ToString("0.00");
                }
                else
                {
                    return "";
                }
            }
        }

        public string ValueString => "€" + (_backingCard.StoredValueCents / 100m).ToString("0.00");
        public DateTimeOffset LastValueTopUpDate => _backingCard.LastLoadDateTime;
        public string LastValueTopUpAmount => "€" + (_backingCard.LastLoadValue / 100m).ToString("0.00");
    }
}

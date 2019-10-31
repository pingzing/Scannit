using System;
using System.Globalization;
using OneOf;
using ScannitSharp.Models.ProductCodes;
using ScannitSharp.Models.ValidityAreas;

namespace Scannit.ViewModels
{
    public class SeasonPassViewModel : ViewModelBase
    {
        public SeasonPassViewModel(OneOf<FaresFor2010, FaresFor2014> productCode,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            OneOf<OldZone, NewZone, Vehicle> validityArea)
        {
            SetExpiryString(startDate, endDate);
            SetValidityAreaString(validityArea);
        }

        private string _seasonPassExpiryString;
        public string SeasonPassExpiryString
        {
            get => _seasonPassExpiryString;
            set => Set(ref _seasonPassExpiryString, value);
        }

        private string _validityAreaString;
        public string ValidityAreaString
        {
            get => _validityAreaString;
            set => Set(ref _validityAreaString, value);
        }

        private void SetExpiryString(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            TimeSpan timeRemaining = endDate - DateTimeOffset.UtcNow;
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

        private void SetValidityAreaString(OneOf<OldZone, NewZone, Vehicle> validityArea)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Linq;
using OneOf;
using Scannit.Extensions;
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
            _endDate = endDate;
            SetExpiryString(startDate, endDate);
            SetValidityAreaString(validityArea);
        }

        private string _expiryString;
        public string ExpiryString
        {
            get => _expiryString;
            set => Set(ref _expiryString, value);
        }

        private DateTimeOffset _endDate;
        public DateTimeOffset EndDate
        {
            get => _endDate;
            set => Set(ref _endDate, value);
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
                ExpiryString = String.Format(AppResources.SeasonPassDaysRemaining, timeRemaining.Days);
            }
            else
            {
                string sep = DateTimeFormatInfo.CurrentInfo.TimeSeparator;
                ExpiryString = String.Format(AppResources.SeasonPassHoursRemaining, timeRemaining.Hours, timeRemaining.Minutes);
            }
        }

        private void SetValidityAreaString(OneOf<OldZone, NewZone, Vehicle> validityArea)
        {
            string validityString = validityArea.Match(
                oldZone =>
                {
                    return $"{AppResources.OldZoneName} {oldZone.Value}";
                },
                newZone =>
                {
                    return newZone.Value
                        .Select(x => x.AsShortString())
                        .Aggregate((accumulated, newVal) => $"{accumulated}{newVal}");
                },
                vehicle =>
                {
                    return vehicle.Value.AsLocalizedVehicle();
                });
            ValidityAreaString = validityString;
        }
    }
}

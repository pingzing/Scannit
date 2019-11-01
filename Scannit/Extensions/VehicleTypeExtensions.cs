using System;
using ScannitSharp.Models;

namespace Scannit.Extensions
{
    public static class VehicleTypeExtensions
    {
        public static string AsLocalizedVehicle(this VehicleType vehicle)
        {
            switch (vehicle)
            {
                case VehicleType.Bus:
                    return AppResources.Vehicles_Bus;
                case VehicleType.Ferry:
                    return AppResources.Vehicles_Ferry;
                case VehicleType.Metro:
                    return AppResources.Vehicles_Metro;
                case VehicleType.Train:
                    return AppResources.Vehicles_Train;
                case VehicleType.Tram:
                    return AppResources.Vehicles_Tram;
                case VehicleType.ULine:
                    return AppResources.Vehicles_ULine;
                case VehicleType.Undefined:
                    return AppResources.Vehicles_Undefined;
                default:
                    throw new ArgumentException($"Unknown vehicle type: '{vehicle}'.");
            }
        }
    }
}

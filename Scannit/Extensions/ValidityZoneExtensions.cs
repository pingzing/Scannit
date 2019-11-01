using System;
using ScannitSharp.Models;

namespace Scannit.Extensions
{
    public static class ValidityZoneExtensions
    {
        public static string AsShortString(this ValidityZone zone)
        {
            switch (zone)
            {
                case ValidityZone.ZoneA:
                    return "A";
                case ValidityZone.ZoneB:
                    return "B";
                case ValidityZone.ZoneC:
                    return "C";
                case ValidityZone.ZoneD:
                    return "D";
                case ValidityZone.ZoneE:
                    return "E";
                case ValidityZone.ZoneF:
                    return "F";
                case ValidityZone.ZoneG:
                    return "G";
                case ValidityZone.ZoneH:
                    return "H";
                default:
                    throw new ArgumentException($"ValidityZone '{zone}' not supported. Did you forget to add it to {nameof(AsShortString)}?");
            }
        }
    }
}

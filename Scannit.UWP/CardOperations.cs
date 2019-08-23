using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ScannitSharp;

namespace Scannit
{
    public class CardOperations
    {
        public static async Task<TravelCard> ReadTravelCard(ISmartCard card)
        {
            using (ISmartCardConnection connection = await card.Connect())
            {
                try
                {
                    byte[] selection = await connection.Transcieve(Commands.SelectHslCommand);
                    if (selection != null
                        && selection.Length > 0
                        && selection.SequenceEqual(Commands.OkResponse))
                    {
                        // Travel card info bytes
                        byte[] appInfo = null;
                        byte[] controlInfo = null;
                        byte[] periodPass = null;
                        byte[] storedValue = null;
                        byte[] eTicket = null;
                        byte[] history = null;

                        // Temporary containers for history chunks
                        byte[] hist1 = new byte[2];
                        byte[] hist2 = new byte[2];

                        appInfo = await connection.Transcieve(Commands.ReadAppInfoCommand);
                        controlInfo = await connection.Transcieve(Commands.ReadControlInfoCommand);
                        periodPass = await connection.Transcieve(Commands.ReadPeriodPassCommand);
                        storedValue = await connection.Transcieve(Commands.ReadStoredValueCommand);
                        eTicket = await connection.Transcieve(Commands.ReadETicketCommand);
                        hist1 = await connection.Transcieve(Commands.ReadHistoryCommand);

                        // If we have more history, the last two bytes of the history array will contain the MORE_DATA bytes.
                        if (hist1.Skip(Math.Max(0, hist1.Length - 2)).ToArray() == Commands.MoreDataResponse)
                        {
                            hist2 = await connection.Transcieve(Commands.ReadNextCommand);
                        }

                        // Combine the two history chunks into a single array, minus their last two MORE_DATA bytes
                        history = hist1.Take(hist1.Length - 2)
                                         .Concat(hist2.Take(hist2.Length - 2))
                                         .ToArray();

                        return TravelCard.CreateTravelCard(appInfo, controlInfo, periodPass, storedValue, eTicket, history);
                    }
                    else
                    {
                        Debug.WriteLine($"Failed to read travel card. Did not received OK_RESPONSE when trying to select the HSL application.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to read card. Exception: {ex}");
                    return null;
                }
            }
        }
    }
}

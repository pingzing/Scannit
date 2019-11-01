using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;

namespace Scannit.UWP
{
    public class UwpSmartCard : ISmartCard
    {
        private readonly SmartCard _backingSmartCard;

        public UwpSmartCard(SmartCard backingSmartCard)
        {
            _backingSmartCard = backingSmartCard;
        }

        public async Task<ISmartCardConnection> Connect()
        {
            var connection = await _backingSmartCard.ConnectAsync();
            return new UwpSmartCardConnection(connection);
        }
    }

    public class UwpSmartCardConnection : ISmartCardConnection
    {
        private readonly SmartCardConnection _backingConnection;

        public UwpSmartCardConnection(SmartCardConnection backingConnection)
        {
            _backingConnection = backingConnection;
        }

        public async Task<byte[]> Transcieve(byte[] buffer)
        {
            return (await _backingConnection.TransmitAsync(buffer.AsBuffer())).ToArray();
        }

        protected virtual void Dispose(bool cleanUpManagedToo)
        {
            _backingConnection.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

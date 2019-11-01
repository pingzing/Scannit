using System;
using System.Threading.Tasks;
using Android.Nfc.Tech;

namespace Scannit.Droid
{
    public class AndroidSmartCard : ISmartCard
    {
        private readonly IsoDep _backingSmartCard;

        public AndroidSmartCard(IsoDep backingSmartCard)
        {
            _backingSmartCard = backingSmartCard;
        }

        public async Task<ISmartCardConnection> Connect()
        {
            await _backingSmartCard.ConnectAsync();
            return new AndroidSmartCardConnection(_backingSmartCard);
        }
    }

    public class AndroidSmartCardConnection : ISmartCardConnection
    {
        public readonly IsoDep _backingSmartCard;

        public AndroidSmartCardConnection(IsoDep backingSmartCard)
        {
            _backingSmartCard = backingSmartCard;
        }

        public Task<byte[]> Transcieve(byte[] buffer)
        {
            return _backingSmartCard.TransceiveAsync(buffer);
        }

        protected virtual void Dispose(bool cleanUpManagedToo)
        {
            _backingSmartCard.Close();
            _backingSmartCard.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
using System;
using System.Threading.Tasks;

namespace Scannit
{
    public interface ISmartCard
    {
        Task<ISmartCardConnection> Connect();
    }

    public interface ISmartCardConnection : IDisposable
    {
        Task<byte[]> Transcieve(byte[] buffer);
    }
}

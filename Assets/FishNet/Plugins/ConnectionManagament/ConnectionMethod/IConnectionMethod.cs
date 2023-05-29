using System.Threading.Tasks;

namespace FishNet.ConnectionManagement
{
    public interface IConnectionMethod
    {
        int NbReconnectAttempts { get; }
        Task SetupClientConnection();
        Task SetupServerConnection();
    }
}
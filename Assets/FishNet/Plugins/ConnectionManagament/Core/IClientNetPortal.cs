using System.Threading.Tasks;
using FishNet.Managing.Client;

namespace FishNet.ConnectionManagement
{
    public interface IClientNetPortal
    {
        ClientManager ClientManager { get; }
        Task StartConnection(IConnectionMethod connectionMethod);
        void RequestDisconnect();
    }
}
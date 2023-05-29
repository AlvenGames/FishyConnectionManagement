using System.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing.Server;

namespace FishNet.ConnectionManagement
{
    public interface IServerNetPortal
    {
        ServerManager ServerManager { get; }
        int MaxConnectedPlayers { get; set; }
        Task StartConnection(IConnectionMethod connectionMethod);
        void RequestDisconnect();
        void DisconnectWithReason(NetworkConnection connection, ClientConnectStatus connectStatus);
    }
}
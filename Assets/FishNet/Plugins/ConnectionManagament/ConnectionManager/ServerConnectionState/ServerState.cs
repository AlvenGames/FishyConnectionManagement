using System.Threading.Tasks;
using FishNet.Connection;

namespace FishNet.ConnectionManagement
{
    internal abstract class ServerState : IExitableState
    {
        public virtual void Exit() { }

        public virtual void OnUserRequestedShutdown() { }

        public virtual void OnServerStarted() { }

        public virtual void OnServerStopped() { }

        public virtual void OnRemoteClientConnected(NetworkConnection connection) { }

        public virtual void OnRemoteClientDisconnect(NetworkConnection connection) { }

        public virtual Task StartServer(IConnectionMethod connectionMethod) { return Task.CompletedTask; }

        public virtual void DisconnectWithReason(NetworkConnection connection, ClientConnectStatus connectStatus) { }
    }
}
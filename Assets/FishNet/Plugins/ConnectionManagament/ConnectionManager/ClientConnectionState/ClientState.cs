using System;
using System.Threading.Tasks;

namespace FishNet.ConnectionManagement
{
    [Serializable]
    internal abstract class ClientState : IExitableState
    {
        public virtual void Exit() { }

        internal virtual void OnClientConnected() { }

        internal virtual void OnClientDisconnect() { }

        internal virtual void OnUserRequestedShutdown() { }

        internal virtual void OnDisconnectReasonReceived(ClientConnectStatus connectStatus) { }

        internal virtual Task StartClient(IConnectionMethod connectionMethod) { return Task.CompletedTask; }
    }
}
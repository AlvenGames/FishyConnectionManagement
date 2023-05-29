using FishNet.Connection;

namespace FishNet.ConnectionManagement
{
    public readonly struct RemoteConnectStatus
    {
        public readonly NetworkConnection NetworkConnection;
        public readonly ClientConnectStatus ConnectStatus;

        public RemoteConnectStatus(NetworkConnection networkConnection, ClientConnectStatus connectStatus)
        {
            NetworkConnection = networkConnection;
            ConnectStatus = connectStatus;
        }
    }
}
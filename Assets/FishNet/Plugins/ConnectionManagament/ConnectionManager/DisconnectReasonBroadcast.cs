using FishNet.Broadcast;

namespace FishNet.ConnectionManagement
{
    public struct DisconnectReasonBroadcast : IBroadcast
    {
        public readonly ClientConnectStatus ConnectStatus;

        public DisconnectReasonBroadcast(ClientConnectStatus connectStatus)
        {
            ConnectStatus = connectStatus;
        }
    }
}
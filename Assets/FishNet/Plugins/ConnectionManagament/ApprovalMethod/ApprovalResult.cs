using FishNet.Connection;

namespace FishNet.ConnectionManagement
{
    public readonly struct ApprovalResult<TConnectionPayload>
    {
        public readonly NetworkConnection Connection;
        public readonly TConnectionPayload ConnectionPayload;
        public readonly bool AuthenticationResult;
        public readonly ClientConnectStatus ConnectStatus;

        public ApprovalResult(NetworkConnection connection, bool authenticationResult, ClientConnectStatus connectStatus, TConnectionPayload connectionPayload)
        {
            Connection = connection;
            AuthenticationResult = authenticationResult;
            ConnectStatus = connectStatus;
            ConnectionPayload = connectionPayload;
        }
    }
}
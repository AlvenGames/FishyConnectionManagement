using System;
using FishNet.Authenticating;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Transporting;

namespace FishNet.ConnectionManagement
{
    public abstract class ApprovalMethod<TConnectionPayload> : Authenticator, IApprovalMethod<TConnectionPayload> where TConnectionPayload : struct, IBroadcast
    {
        public abstract TConnectionPayload ConnectionPayload { get; set; }

        public override event Action<NetworkConnection, bool> OnAuthenticationResult;
        public event Action<ApprovalResult<TConnectionPayload>> OnConnectionPayloadApproval;

        public override void InitializeOnce(NetworkManager networkManager)
        {
            base.InitializeOnce(networkManager);
            
            networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
            networkManager.ServerManager.RegisterBroadcast<TConnectionPayload>(OnConnectionPayloadBroadcast, false);
        }

        protected abstract ClientConnectStatus ApprovalCheck(NetworkConnection connection, TConnectionPayload payload);

        private void OnConnectionPayloadBroadcast(NetworkConnection connection, TConnectionPayload payload)
        {
            ClientConnectStatus connectStatus = ApprovalCheck(connection, payload);
            
            bool authenticationResult = connectStatus == ClientConnectStatus.Success;
            OnAuthenticationResult?.Invoke(connection, authenticationResult);
            OnConnectionPayloadApproval?.Invoke(new ApprovalResult<TConnectionPayload>(connection, authenticationResult, connectStatus, payload));
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            if (args.ConnectionState != LocalConnectionState.Started) return;
            
            NetworkManager.ClientManager.Broadcast(ConnectionPayload);
        }
    }
}
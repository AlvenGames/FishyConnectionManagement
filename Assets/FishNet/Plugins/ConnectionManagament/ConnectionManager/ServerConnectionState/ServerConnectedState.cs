using FishNet.Connection;
using FishNet.Managing.Server;

namespace FishNet.ConnectionManagement
{
    internal class ServerConnectedState : ServerOnlineState, IState
    {
        private readonly IPublisher<RemoteConnectStatus> _remoteConnectStatusPub;

        public ServerConnectedState(StateMachine<ServerState> stateMachine, ServerManager serverManager, 
            IPublisher<ServerConnectStatus> connectStatusPub, IPublisher<RemoteConnectStatus> remoteConnectStatusPub) : base(stateMachine, serverManager, connectStatusPub)
        {
            _remoteConnectStatusPub = remoteConnectStatusPub;
        }

        public void Enter() { }

        public override void OnServerStopped()
        {
            _stateMachine.Enter<ServerOfflineState>();
        }

        public override void OnRemoteClientConnected(NetworkConnection connection)
        {
            _connectStatusPub.Publish(ServerConnectStatus.Success);
        }

        public override void OnRemoteClientDisconnect(NetworkConnection connection)
        {
            _remoteConnectStatusPub.Publish(new RemoteConnectStatus(connection, ClientConnectStatus.GenericDisconnect));
        }

        public override void DisconnectWithReason(NetworkConnection connection, ClientConnectStatus connectStatus)
        {
            _serverManager.Broadcast(connection, new DisconnectReasonBroadcast(connectStatus), false);
            connection.Disconnect(false);
        }

        public override void OnUserRequestedShutdown()
        {
            _serverManager.Broadcast(new DisconnectReasonBroadcast(ClientConnectStatus.ServerEndedSession));
            base.OnUserRequestedShutdown();
        }
    }
}

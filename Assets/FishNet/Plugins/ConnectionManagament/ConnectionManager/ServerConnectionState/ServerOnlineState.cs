using FishNet.Managing.Server;

namespace FishNet.ConnectionManagement
{
    internal abstract class ServerOnlineState : ServerState
    {
        protected readonly ServerManager _serverManager;
        protected readonly StateMachine<ServerState> _stateMachine;
        protected readonly IPublisher<ServerConnectStatus> _connectStatusPub;

        protected ServerOnlineState(StateMachine<ServerState> stateMachine, ServerManager serverManager, IPublisher<ServerConnectStatus> connectStatusPub)
        {
            _connectStatusPub = connectStatusPub;
            _stateMachine = stateMachine;
            _serverManager = serverManager;
        }

        public override void OnUserRequestedShutdown()
        {
            _connectStatusPub.Publish(ServerConnectStatus.UserRequestedDisconnect);
            _serverManager.StopConnection(false);
            _stateMachine.Enter<ServerOfflineState>();
        }
    }
}
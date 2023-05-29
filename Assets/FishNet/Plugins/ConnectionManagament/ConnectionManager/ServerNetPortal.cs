using System;
using System.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Transporting;

namespace FishNet.ConnectionManagement
{
    public sealed class ServerNetPortal : IServerNetPortal, IDisposable
    {
        public ServerManager ServerManager { get; }
        public int MaxConnectedPlayers { get; set; } = 999;

        private readonly StateMachine<ServerState> _stateMachine = new();
        private ServerState ActiveState => _stateMachine.ActiveState;

        public ServerNetPortal(ServerManager serverManager, IPublisher<ServerConnectStatus> connectStatusPub,
            IPublisher<RemoteConnectStatus> remoteConnectStatusPub)
        {
            ServerManager = serverManager;

            RegisterState(new ServerOfflineState(_stateMachine, ServerManager));
            RegisterState(new ServerConnectingState(_stateMachine, ServerManager, connectStatusPub));
            RegisterState(new ServerConnectedState(_stateMachine, ServerManager, connectStatusPub, remoteConnectStatusPub));
            
            _stateMachine.Enter<ServerOfflineState>();
            ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
            ServerManager.OnServerConnectionState += OnServerConnectionState;
        }

        public async Task StartConnection(IConnectionMethod connectionMethod)
        {
            await ActiveState.StartServer(connectionMethod);
        }

        public void RequestDisconnect()
        {
            ActiveState.OnUserRequestedShutdown();
        }

        public void DisconnectWithReason(NetworkConnection connection, ClientConnectStatus connectStatus)
        {
            ActiveState.DisconnectWithReason(connection, connectStatus);
        }

        private void OnRemoteConnectionState(NetworkConnection connection, RemoteConnectionStateArgs args)
        {
            switch (args.ConnectionState)
            {
                case RemoteConnectionState.Stopped:
                    ActiveState.OnRemoteClientDisconnect(connection);
                    break;
                case RemoteConnectionState.Started:
                    ActiveState.OnRemoteClientConnected(connection);
                    break;
            }
        }

        private void OnServerConnectionState(ServerConnectionStateArgs args)
        {
            switch (args.ConnectionState)
            {
                case LocalConnectionState.Stopped:
                    ActiveState.OnServerStopped();
                    break;
                case LocalConnectionState.Started:
                    ActiveState.OnServerStarted();
                    break;
            }
        }

        private void RegisterState<TState>(TState state) where TState : ServerState
        {
            _stateMachine.RegisterState(state);
        }

        public void Dispose()
        {
            ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
            ServerManager.OnServerConnectionState -= OnServerConnectionState;
        }
    }
}
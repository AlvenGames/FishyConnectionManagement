using System;
using System.Threading.Tasks;
using FishNet.Managing.Client;
using FishNet.Transporting;

namespace FishNet.ConnectionManagement
{
    public class ClientNetPortal : IClientNetPortal, IDisposable
    {
        public ClientManager ClientManager { get; }

        private readonly StateMachine<ClientState> _stateMachine = new();
        private ClientState ActiveState => _stateMachine.ActiveState;

        public ClientNetPortal(ClientManager clientManager, IPublisher<ClientConnectStatus> connectStatusPub, IPublisher<ReconnectMessage> reconnectMessagePub, ICoroutineRunner coroutineRunner)
        {
            ClientManager = clientManager;

            RegisterState(new ClientStartingState(_stateMachine, ClientManager, connectStatusPub));
            RegisterState(new ClientConnectedState(_stateMachine, ClientManager, connectStatusPub));
            RegisterState(new DisconnectingWithReasonState(_stateMachine, ClientManager, connectStatusPub));
            RegisterState(new ClientOfflineState(_stateMachine));
            RegisterState(new ClientReconnectingState(_stateMachine, ClientManager, connectStatusPub, 
                reconnectMessagePub, coroutineRunner));
            
            ClientManager.RegisterBroadcast<DisconnectReasonBroadcast>(OnDisconnectReasonBroadcast);
            ClientManager.OnClientConnectionState += OnClientConnectionState;
            
            _stateMachine.Enter<ClientOfflineState>();
        }

        public async Task StartConnection(IConnectionMethod connectionMethod)
        {
            await ActiveState.StartClient(connectionMethod);
        }

        public void RequestDisconnect()
        {
            ActiveState.OnUserRequestedShutdown();
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            switch (args.ConnectionState)
            {
                case LocalConnectionState.Stopped:
                    ActiveState.OnClientDisconnect();
                    break;
                case LocalConnectionState.Started:
                    ActiveState.OnClientConnected();
                    break;
            }
        }

        private void OnDisconnectReasonBroadcast(DisconnectReasonBroadcast disconnectReasonBroadcast)
        {
            ActiveState.OnDisconnectReasonReceived(disconnectReasonBroadcast.ConnectStatus);
        }

        private void RegisterState<TState>(TState state) where TState : ClientState
        {
            _stateMachine.RegisterState(state);
        }

        public void Dispose()
        {
            ClientManager.UnregisterBroadcast<DisconnectReasonBroadcast>(OnDisconnectReasonBroadcast);
            ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }
    }
}
using FishNet.Managing.Client;

namespace FishNet.ConnectionManagement
{
    /// <summary>
    /// Base class representing an online connection state.
    /// </summary>
    internal abstract class ClientOnlineState : ClientState
    {
        protected readonly IPublisher<ClientConnectStatus> _connectStatusPub;
        protected readonly ClientManager _clientManager;
        protected readonly StateMachine<ClientState> _stateMachine;

        protected ClientOnlineState(StateMachine<ClientState> stateMachine, ClientManager clientManager, IPublisher<ClientConnectStatus> connectStatusPub)
        {
            _stateMachine = stateMachine;
            _clientManager = clientManager;
            _connectStatusPub = connectStatusPub;
        }

        internal override void OnDisconnectReasonReceived(ClientConnectStatus disconnectReason)
        {
            _connectStatusPub.Publish(disconnectReason);
            _stateMachine.Enter<DisconnectingWithReasonState>();
        }

        internal override void OnUserRequestedShutdown()
        {
            // This behaviour will be the same for every online state
            _clientManager.StopConnection();
            _connectStatusPub.Publish(ClientConnectStatus.UserRequestedDisconnect);
            _stateMachine.Enter<ClientOfflineState>();
        }
    }
}

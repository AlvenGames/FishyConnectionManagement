using System;
using FishNet.Managing.Client;

namespace FishNet.ConnectionManagement
{
    [Serializable]
    internal class DisconnectingWithReasonState : ClientOnlineState, IState
    {
        public DisconnectingWithReasonState(StateMachine<ClientState> stateMachine, ClientManager clientManager, 
            IPublisher<ClientConnectStatus> connectStatusPub) : base(stateMachine, clientManager, connectStatusPub) { }

        public void Enter() { }

        internal override void OnClientDisconnect()
        {
            _stateMachine.Enter<ClientOfflineState>();
        }
    }
}
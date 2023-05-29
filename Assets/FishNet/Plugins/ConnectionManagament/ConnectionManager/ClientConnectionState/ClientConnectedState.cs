using System;
using FishNet.Managing.Client;
using UnityEngine;

namespace FishNet.ConnectionManagement
{
    [Serializable]
    internal class ClientConnectedState : ClientOnlineState, IPayloadedState<IConnectionMethod>
    {
        [SerializeReference] private IConnectionMethod _connectionMethod;

        public ClientConnectedState(StateMachine<ClientState> stateMachine, ClientManager clientManager,
            IPublisher<ClientConnectStatus> connectStatusPub) : base(stateMachine, clientManager, connectStatusPub) { }

        public void Enter(IConnectionMethod connectionMethod)
        {
            _connectionMethod = connectionMethod;
        }

        internal override void OnClientDisconnect()
        {
            _connectStatusPub.Publish(ClientConnectStatus.Reconnecting);
            _stateMachine.Enter<ClientReconnectingState, IConnectionMethod>(_connectionMethod);
        }
    }
}

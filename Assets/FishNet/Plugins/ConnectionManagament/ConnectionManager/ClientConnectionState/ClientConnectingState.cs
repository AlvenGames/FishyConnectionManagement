using System;
using System.Threading.Tasks;
using FishNet.Managing.Client;
using UnityEngine;

namespace FishNet.ConnectionManagement
{
    [Serializable]
    internal abstract class ClientConnectingState : ClientOnlineState
    {
        [SerializeReference] protected IConnectionMethod _connectionMethod;

        public ClientConnectingState(StateMachine<ClientState> stateMachine, ClientManager clientManager, 
            IPublisher<ClientConnectStatus> connectStatusPub) : base(stateMachine, clientManager, connectStatusPub) { }

        internal override void OnClientConnected()
        {
            _connectStatusPub.Publish(ClientConnectStatus.Success);
            _stateMachine.Enter<ClientConnectedState, IConnectionMethod>(_connectionMethod);
        }

        internal override void OnClientDisconnect()
        {
            StartingClientFailed();
        }

        internal async Task StartClientConnection()
        {
            try
            {
                await _connectionMethod.SetupClientConnection();
                _clientManager.StartConnection();
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting client, see following exception");
                Debug.LogException(e);
                StartingClientFailed();
                throw;
            }
        }

        private void StartingClientFailed()
        {
            _connectStatusPub.Publish(ClientConnectStatus.StartClientFailed);
            _stateMachine.Enter<ClientOfflineState>();
        }
    }
}

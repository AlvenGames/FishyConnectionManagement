using System;
using System.Collections;
using FishNet.Managing.Client;
using UnityEngine;

namespace FishNet.ConnectionManagement
{
    [Serializable]
    internal class ClientReconnectingState : ClientConnectingState, IPayloadedState<IConnectionMethod>
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IPublisher<ReconnectMessage> _reconnectPub;

        private Coroutine _reconnectCoroutine;
        [SerializeField] private int _nbAttempts;

        private const float TIME_BETWEEN_ATTEMPTS = 2;

        public ClientReconnectingState(StateMachine<ClientState> stateMachine, ClientManager clientManager, 
            IPublisher<ClientConnectStatus> connectStatusPub, IPublisher<ReconnectMessage> reconnectPub, ICoroutineRunner coroutineRunner) 
            : base(stateMachine, clientManager, connectStatusPub)
        {
            _reconnectPub = reconnectPub;
            _coroutineRunner = coroutineRunner;
        }

        public void Enter(IConnectionMethod connectionMethod)
        {
            _connectionMethod = connectionMethod;
            _nbAttempts = 0;
            if (_connectionMethod.NbReconnectAttempts > 0)
            {
                _reconnectCoroutine = _coroutineRunner.StartCoroutine(ReconnectCoroutine());
            }
        }

        public override void Exit()
        {
            if (_reconnectCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_reconnectCoroutine);
                _reconnectCoroutine = null;
            }

            int nbReconnectAttempts = _connectionMethod.NbReconnectAttempts;
            _reconnectPub.Publish(new ReconnectMessage(nbReconnectAttempts, nbReconnectAttempts));
        }

        internal override void OnClientDisconnect()
        {
            if (_nbAttempts < _connectionMethod.NbReconnectAttempts)
            {
                _reconnectCoroutine = _coroutineRunner.StartCoroutine(ReconnectCoroutine());
            }
            else
            {
                _connectStatusPub.Publish(ClientConnectStatus.GenericDisconnect);
                _stateMachine.Enter<ClientOfflineState>();
            }
        }

        internal override void OnDisconnectReasonReceived(ClientConnectStatus disconnectReason)
        {
            _connectStatusPub.Publish(disconnectReason);
            switch (disconnectReason)
            {
                case ClientConnectStatus.UserRequestedDisconnect:
                case ClientConnectStatus.ServerEndedSession:
                case ClientConnectStatus.ServerFull:
                    _stateMachine.Enter<DisconnectingWithReasonState>();
                    break;
            }
        }

        private IEnumerator ReconnectCoroutine()
        {
            if (_nbAttempts > 0)
            {
                yield return new WaitForSeconds(TIME_BETWEEN_ATTEMPTS);
            }
            
            Debug.Log("Lost connection to host, trying to reconnect...");
            
            _clientManager.StopConnection();
            
            // yield return new WaitWhile(() => _connectionManager.NetworkManager.ShutdownInProgress); // wait until NetworkManager completes shutting down
            Debug.Log($"Reconnecting attempt {_nbAttempts + 1}/{_connectionMethod.NbReconnectAttempts}...");
            _reconnectPub.Publish(new ReconnectMessage(_nbAttempts, _connectionMethod.NbReconnectAttempts));
            
            _nbAttempts++;
            
            yield return StartClientConnection();
        }
    }
}
using System;
using System.Threading.Tasks;
using FishNet.Managing.Server;
using UnityEngine;

namespace FishNet.ConnectionManagement
{
    [Serializable]
    internal class ServerConnectingState : ServerOnlineState, IPayloadedStateAsync<IConnectionMethod>
    {
        [SerializeReference] private IConnectionMethod _connectionMethod;

        public ServerConnectingState(StateMachine<ServerState> stateMachine, ServerManager serverManager, 
            IPublisher<ServerConnectStatus> connectStatusPub) : base(stateMachine, serverManager, connectStatusPub) { }

        public async Task Enter(IConnectionMethod connectionMethod)
        {
            _connectionMethod = connectionMethod;

            try
            {
                await _connectionMethod.SetupServerConnection();
                bool connected = _serverManager.StartConnection();
                if (!connected)
                {
                    throw new Exception();
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                StartServerFailed();
            }
        }

        public override void OnServerStarted()
        {
            _connectStatusPub.Publish(ServerConnectStatus.Success);
            _stateMachine.Enter<ServerConnectedState>();
        }

        private void StartServerFailed()
        {
            _connectStatusPub.Publish(ServerConnectStatus.StartServerFailed);
            _stateMachine.Enter<ServerOfflineState>();
        }
    }
}
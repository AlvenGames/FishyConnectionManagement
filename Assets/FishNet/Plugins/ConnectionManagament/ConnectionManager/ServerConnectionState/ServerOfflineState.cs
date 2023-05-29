using System.Threading.Tasks;
using FishNet.Managing.Server;

namespace FishNet.ConnectionManagement
{
    internal class ServerOfflineState : ServerState, IState
    {
        private readonly ServerManager _serverManager;
        private readonly StateMachine<ServerState> _stateMachine;

        public ServerOfflineState(StateMachine<ServerState> stateMachine, ServerManager serverManager)
        {
            _stateMachine = stateMachine;
            _serverManager = serverManager;
        }

        public void Enter()
        {
            _serverManager.StopConnection(false);
        }

        public override async Task StartServer(IConnectionMethod connectionMethod)
        {
            await _stateMachine.EnterAsync<ServerConnectingState, IConnectionMethod>(connectionMethod);
        }
    }
}

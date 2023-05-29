using System.Threading.Tasks;

namespace FishNet.ConnectionManagement
{
    internal class ClientOfflineState : ClientState, IState
    {
        private readonly StateMachine<ClientState> _stateMachine;

        public ClientOfflineState(StateMachine<ClientState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter() { }

        internal override async Task StartClient(IConnectionMethod connectionMethod)
        {
            await _stateMachine.EnterAsync<ClientStartingState, IConnectionMethod>(connectionMethod);
        }
    }
}
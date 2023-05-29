using System.Threading.Tasks;
using FishNet.Managing.Client;

namespace FishNet.ConnectionManagement
{
    internal sealed class ClientStartingState : ClientConnectingState, IPayloadedStateAsync<IConnectionMethod>
    {
        public ClientStartingState(StateMachine<ClientState> stateMachine, ClientManager clientManager, 
            IPublisher<ClientConnectStatus> connectStatusPub) : base(stateMachine, clientManager, connectStatusPub) { }

        public async Task Enter(IConnectionMethod connectionMethod)
        {
            _connectionMethod = connectionMethod;
            await StartClientConnection();
        }
    }
}
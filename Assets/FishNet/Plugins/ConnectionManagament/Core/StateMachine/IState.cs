using System.Threading.Tasks;

namespace FishNet.ConnectionManagement
{
    public interface IExitableState
    {
        public void Exit();
    }

    public interface IState : IExitableState
    {
        public void Enter();
    }

    public interface IPayloadedState<in TPayload> : IExitableState
    {
        public void Enter(TPayload payload);
    }
    
    public interface IStateAsync : IExitableState
    {
        public Task Enter();
    }
    
    public interface IPayloadedStateAsync<in TPayload> : IExitableState
    {
        public Task Enter(TPayload payload);
    }
}
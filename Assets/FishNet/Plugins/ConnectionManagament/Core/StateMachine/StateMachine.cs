using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FishNet.ConnectionManagement
{
    [Serializable]
    public class StateMachine<T> where T : IExitableState
    {
        [SerializeField] private T _activeState;
        public T ActiveState => _activeState;

        private readonly Dictionary<Type,T> _states = new();

        public void RegisterState<TState>(TState state) where TState : class, T
        {
            _states[typeof(TState)] = state;
        }

        public TState Enter<TState>() where TState : class, T, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
            return state;
        }
        
        public TPayloadedState Enter<TPayloadedState, TPayload>(TPayload payload) where TPayloadedState : class, T, IPayloadedState<TPayload>
        {
            var state = ChangeState<TPayloadedState>();
            state.Enter(payload);
            return state;
        }

        public async Task<TPayloadedStateAsync> EnterAsync<TPayloadedStateAsync, TPayload>(TPayload payload) where TPayloadedStateAsync : class, T, IPayloadedStateAsync<TPayload>
        {
            var state = ChangeState<TPayloadedStateAsync>();
            await state.Enter(payload);
            return state;
        }
        
        private TState ChangeState<TState>() where TState : class, T
        {
            _activeState?.Exit();
            
            var state = GetState<TState>();
            _activeState = state;
            
            return state;
        }
        
        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}
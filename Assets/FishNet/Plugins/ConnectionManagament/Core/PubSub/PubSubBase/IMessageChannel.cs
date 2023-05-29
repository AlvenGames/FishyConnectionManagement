using System;

namespace FishNet.ConnectionManagement
{
    public interface IPublisher<in T>
    {
        void Publish(T broadcast);
    }
    
    public interface ISubscriber<out T>
    {
        IDisposable Subscribe(Action<T> handler);
        void Unsubscribe(Action<T> handler);
    }

    public interface IMessageChannel<T> : IPublisher<T>, ISubscriber<T>, IDisposable
    {
        bool IsDisposed { get; }
    }
}

using FishNet.Connection;

namespace FishNet.ConnectionManagement
{
    public interface IPersistentPlayer
    {
        bool IsConnected { get; }
        NetworkPlayerId NetworkPlayerId { get; }
        NetworkConnection NetworkConnection { get; }
    }
}
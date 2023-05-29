using FishNet.Managing;

namespace FishNet.ConnectionManagement
{
    public interface IConnectionManager
    {
        IClientNetPortal ClientNetPortal { get; }
        IServerNetPortal ServerNetPortal { get; }
        NetworkManager NetworkManager { get; }
    }
}
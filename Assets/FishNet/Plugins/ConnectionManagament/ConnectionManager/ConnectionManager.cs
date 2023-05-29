using FishNet.Managing;
using UnityEngine;

namespace FishNet.ConnectionManagement
{
    public class ConnectionManager : MonoBehaviour, IConnectionManager
    {
        [SerializeField] private NetworkManager _networkManager;
        public NetworkManager NetworkManager => _networkManager;

        public IClientNetPortal ClientNetPortal { get; private set; }
        public IServerNetPortal ServerNetPortal { get; private set; }

        public void Construct(IClientNetPortal clientNetPortal, IServerNetPortal serverNetPortal)
        {
            ClientNetPortal = clientNetPortal;
            ServerNetPortal = serverNetPortal;
        }
    }
}
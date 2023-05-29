using System.Threading.Tasks;
using UnityEngine;

namespace FishNet.ConnectionManagement
{
    public abstract class ConnectionMethod : MonoBehaviour, IConnectionMethod
    {
        public abstract int NbReconnectAttempts { get; }
        public abstract Task SetupClientConnection();
        public abstract Task SetupServerConnection();
    }
}
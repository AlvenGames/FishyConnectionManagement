using System.Threading.Tasks;

namespace FishNet.ConnectionManagement
{
    public class ConnectionMethodDefault : ConnectionMethod
    {
        public override int NbReconnectAttempts => 0;

        public override Task SetupClientConnection()
        {
            return Task.CompletedTask;
        }

        public override Task SetupServerConnection()
        {
            return Task.CompletedTask;
        }
    }
}
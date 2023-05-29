using System;

namespace FishNet.ConnectionManagement
{
    [Serializable]
    public struct NetworkPlayerId
    {
        public int Id;

        public NetworkPlayerId(string playerId)
        {
            Id = playerId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is NetworkPlayerId other && Equals(other);
        }

        public bool Equals(NetworkPlayerId other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public static bool operator ==(NetworkPlayerId a, NetworkPlayerId b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NetworkPlayerId a, NetworkPlayerId b)
        {
            return !(a == b);
        }
    }
}
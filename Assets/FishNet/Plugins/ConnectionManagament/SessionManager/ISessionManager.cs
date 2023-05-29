using System.Collections.Generic;
using FishNet.Connection;

namespace FishNet.ConnectionManagement
{
    public interface ISessionManager
    {
        /// <summary>
        /// Marks the current session as started, so from now on we keep the data of disconnected players.
        /// </summary>
        void StartSession();

        /// <summary>
        /// Reinitializes session data from connected players, and clears data from disconnected players, so that if they reconnect in the next game, they will be treated as new players
        /// </summary>
        void EndSession();

        bool IsPlayerConnected(NetworkPlayerId networkPlayerId);
    }

    public interface ISessionManager<out TPersistentPlayer> : ISessionManager
    {
        IReadOnlyCollection<TPersistentPlayer> Players { get; }
        TPersistentPlayer LocalPlayer { get; }
        
        event NetworkAction<TPersistentPlayer> OnPlayerConnected;
        event NetworkAction<TPersistentPlayer> OnPlayerDisconnected;
        event NetworkAction<TPersistentPlayer> OnPlayerReconnected;
        
        TPersistentPlayer GetPersistentPlayer(NetworkPlayerId networkPlayerId);
        
        TPersistentPlayer GetPersistentPlayer(NetworkConnection connection);
    }
}
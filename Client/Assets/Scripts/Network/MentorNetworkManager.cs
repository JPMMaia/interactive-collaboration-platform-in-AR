using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class MentorNetworkManager : NetworkManager
    {
        #region Events
        public event EventHandler OnPlayerConnected;
        public event EventHandler OnPlayerDisconnected;
        #endregion

        #region Properties

        public static MentorNetworkManager Instance
        {
            get
            {
                return FindObjectOfType(typeof(MentorNetworkManager)) as MentorNetworkManager;
            }
        }

        public int PlayerCount
        {
            get { return numPlayers; }
        }
        public bool IsAppreticeConnected
        {
            get { return _connections.Count == 2; }
        }

        #endregion

        private readonly Dictionary<int, NetworkConnection> _connections = new Dictionary<int, NetworkConnection>();

        public override void OnServerConnect(NetworkConnection connection)
        {
            base.OnServerConnect(connection);

            if (_connections.ContainsKey(connection.connectionId))
                return;

            _connections.Add(connection.connectionId, connection);
            if (OnPlayerConnected != null)
                OnPlayerConnected(this, EventArgs.Empty);
        }
        public override void OnServerDisconnect(NetworkConnection connection)
        {
            base.OnServerDisconnect(connection);

            if (!_connections.ContainsKey(connection.connectionId))
                return;

            _connections.Remove(connection.connectionId);
            if (OnPlayerDisconnected != null)
                OnPlayerDisconnected(this, EventArgs.Empty);
        }
    }
}

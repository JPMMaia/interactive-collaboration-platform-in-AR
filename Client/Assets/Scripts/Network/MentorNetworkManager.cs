using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class MentorNetworkManager : NetworkManager
    {
        #region Events
        public event EventHandler OnUserConnected;
        public event EventHandler OnUserDisconnected;
        public event EventHandler OnNeedMoreInstructions;
        public event EventHandler OnStepCompleted;
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
            if (OnUserConnected != null)
                OnUserConnected(this, EventArgs.Empty);

            if (IsAppreticeConnected)
            {
                client.RegisterHandler(NetworkHandles.NeedMoreInstructions, NeedMoreInstructions);
                client.RegisterHandler(NetworkHandles.StepCompleted, StepCompleted);
            }
        }
        public override void OnServerDisconnect(NetworkConnection connection)
        {
            base.OnServerDisconnect(connection);

            if (!_connections.ContainsKey(connection.connectionId))
                return;

            _connections.Remove(connection.connectionId);
            if (OnUserDisconnected != null)
                OnUserDisconnected(this, EventArgs.Empty);
        }
        private void NeedMoreInstructions(NetworkMessage networkMessage)
        {
            if(OnNeedMoreInstructions != null)
                OnNeedMoreInstructions(this, EventArgs.Empty);
        }
        private void StepCompleted(NetworkMessage networkMessage)
        {
            if (OnStepCompleted != null)
                OnStepCompleted(this, EventArgs.Empty);
        }
    }
}

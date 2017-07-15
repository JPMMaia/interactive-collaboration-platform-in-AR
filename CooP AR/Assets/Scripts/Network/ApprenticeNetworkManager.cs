using System;
using UnityEngine.Networking;

namespace CollaborationEngine.Network
{
    public class ApprenticeNetworkManager : NetworkManager
    {
        #region Events
        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;
        #endregion

        public override void OnClientConnect(NetworkConnection networkConnection)
        {
            if(OnConnected != null)
                OnConnected(this, EventArgs.Empty);
        }

        public override void OnClientDisconnect(NetworkConnection networkConnection)
        {
            if (OnDisconnected != null)
                OnDisconnected(this, EventArgs.Empty);
        }
    }
}

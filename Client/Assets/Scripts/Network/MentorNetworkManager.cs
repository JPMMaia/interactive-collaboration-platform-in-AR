using System;
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
        public byte PlayerCount { get; set; }
        public bool IsAppreticeConnected
        {
            get { return PlayerCount == 2; }
        }

        #endregion

        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);

            ++PlayerCount;

            if(OnPlayerConnected != null)
                OnPlayerConnected(this, EventArgs.Empty);
        }
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);

            --PlayerCount;

            if (OnPlayerDisconnected != null)
                OnPlayerDisconnected(this, EventArgs.Empty);
        }
    }
}

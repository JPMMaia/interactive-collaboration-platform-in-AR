using CollaborationEngine.Base;
using CollaborationEngine.Network;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.ApprenticeBox
{
    public class ApprenticeNetworkStateView : Entity
    {
        public Text Text;
        public Color OfflineColor;
        public Color OnlineColor;

        public void Start()
        {
            var networkManager = MentorNetworkManager.Instance;

            networkManager.OnPlayerConnected += NetworkManager_OnConnectionsChanged;
            networkManager.OnPlayerDisconnected += NetworkManager_OnConnectionsChanged;
        }

        private void NetworkManager_OnConnectionsChanged(object sender, System.EventArgs e)
        {
            var networkManager = MentorNetworkManager.Instance;

            if (networkManager.IsAppreticeConnected)
            {
                Text.text = "The apprentice is online.";
                Text.color = OnlineColor;
            }
            else
            {
                Text.text = "The apprentice is offline.";
                Text.color = OfflineColor;
            }
        }
    }
}

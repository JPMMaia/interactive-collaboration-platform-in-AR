using System;
using CollaborationEngine.Network;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Console
{
    public class ConsoleController : MonoBehaviour
    {
        #region Unity Editor

        public ScrollRect Scroll;
        public RectTransform ContentPanel;
        public Text TextPrefab;

        #endregion

        public void Start()
        {
            var networkController = NetworkController.Instance;
            networkController.OnPlayerConnected += NetworkController_OnPlayerConnected;
            networkController.OnPlayerDisconnected += NetworkController_OnPlayerDisconnected;

            if (NetworkController.Instance.PlayerCount == 2)
                AddText("Apprentice is connected!");
        }
        public void OnDestroy()
        {
            var networkController = NetworkController.Instance;
            if (networkController != null)
            {
                networkController.OnPlayerDisconnected -= NetworkController_OnPlayerDisconnected;
                networkController.OnPlayerConnected -= NetworkController_OnPlayerConnected;
            }
        }

        public void AddText(String text)
        {
            var textObject = Instantiate(TextPrefab);
            textObject.text = text;
            textObject.transform.SetParent(ContentPanel.transform);

            // Push the scroll down:
            Scroll.normalizedPosition = new Vector2(0, 0);
        }

        #region Event Handlers
        private void NetworkController_OnPlayerConnected(object sender, EventArgs e)
        {
            if (NetworkController.Instance.PlayerCount == 2)
                AddText("Apprentice is connected!");
        }
        private void NetworkController_OnPlayerDisconnected(object sender, EventArgs e)
        {
            if (NetworkController.Instance.PlayerCount == 1)
                AddText("Apprentice is disconnected!");
        }
        #endregion
    }
}

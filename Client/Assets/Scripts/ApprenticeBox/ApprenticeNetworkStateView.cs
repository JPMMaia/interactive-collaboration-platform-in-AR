using System;
using CollaborationEngine.Base;
using CollaborationEngine.Network;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CollaborationEngine.ApprenticeBox
{
    public class ApprenticeNetworkStateView : Entity
    {
        public Image Background;
        public Text Text;
        public Color NormalBackgroundColor;
        public Color HighlightBackgroundColor;
        public Color NormalTextColor;
        public Color HighlightTextColor;

        private bool NewNotification
        {
            get { return _newNotification; }
            set
            {
                _newNotification = value;
                if(_newNotification)
                    _notificationTime = DateTime.Now;
            }
        }

        private DateTime _notificationTime;
        private bool _newNotification;

        public void Start()
        {
            var networkManager = MentorNetworkManager.Instance;

            networkManager.OnPlayerConnected += NetworkManager_OnConnectionsChanged;
            networkManager.OnPlayerDisconnected += NetworkManager_OnConnectionsChanged;

            networkManager.client.RegisterHandler(NetworkHandles.NeedMoreInstructions, OnNeedMoreInstructions);
            networkManager.client.RegisterHandler(NetworkHandles.StepCompleted, OnStepCompleted);
        }

        public void Update()
        {
            if (NewNotification)
            {
                var amount = (float) (DateTime.Now - _notificationTime).Ticks / TimeSpan.FromSeconds(5.0f).Ticks;

                Background.color = InterpolateColor(HighlightBackgroundColor, NormalBackgroundColor, amount);
                Text.color = InterpolateColor(HighlightTextColor, NormalTextColor, amount);

                if (amount >= 1.0f)
                    NewNotification = false;
            }
        }

        private void NetworkManager_OnConnectionsChanged(object sender, System.EventArgs e)
        {
            var networkManager = MentorNetworkManager.Instance;

            if (networkManager.IsAppreticeConnected)
            {
                Text.text = "The apprentice is online.";
                NewNotification = true;
            }
            else
            {
                Text.text = "The apprentice is offline.";
                NewNotification = true;
            }
        }

        private void OnNeedMoreInstructions(NetworkMessage networkMessage)
        {
            Text.text = "The apprentice is requesting more instructions.";
            NewNotification = true;
        }
        private void OnStepCompleted(NetworkMessage networkMessage)
        {
            Text.text = "The apprentice completed step.";
            NewNotification = true;
        }

        private Color InterpolateColor(Color color1, Color color2, float amount)
        {
            var red = Mathf.Lerp(color1.r, color2.r, amount);
            var green = Mathf.Lerp(color1.g, color2.g, amount);
            var blue = Mathf.Lerp(color1.b, color2.b, amount);
            var alpha = Mathf.Lerp(color1.a, color2.a, amount);

            return new Color(red, green, blue, alpha);
        }
    }
}

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
        public NotificationIcon IconPrefab;
        public AudioClip NotificationAudioClip;
        public Image Background;
        public Text Text;

        private NotificationIcon _icon;

        public void Start()
        {
            var networkManager = MentorNetworkManager.Instance;

            networkManager.OnPlayerConnected += NetworkManager_OnConnectionsChanged;
            networkManager.OnPlayerDisconnected += NetworkManager_OnConnectionsChanged;

            networkManager.client.RegisterHandler(NetworkHandles.NeedMoreInstructions, OnNeedMoreInstructions);
            networkManager.client.RegisterHandler(NetworkHandles.StepCompleted, OnStepCompleted);
        }

        private void NetworkManager_OnConnectionsChanged(object sender, EventArgs e)
        {
            var networkManager = MentorNetworkManager.Instance;

            Text.text = networkManager.IsAppreticeConnected ? "The apprentice is online." : "The apprentice is offline.";

            // Remove icon:
            if(_icon)
                Destroy(_icon.gameObject);

            PlayNotificationAudioClip();
        }

        private void ChangeIcon(NotificationType notificationType)
        {
            if (!_icon)
            {
                _icon = Instantiate(IconPrefab, transform);
                _icon.transform.SetAsFirstSibling();
            }
                
            _icon.NotificationType = notificationType;
        }
        private void PlayNotificationAudioClip()
        {
            if(NotificationAudioClip)
                AudioSource.PlayClipAtPoint(NotificationAudioClip, transform.position);
        }

        private void OnNeedMoreInstructions(NetworkMessage networkMessage)
        {
            Text.text = "The apprentice is requesting more instructions.";
            ChangeIcon(NotificationType.Help);
            PlayNotificationAudioClip();
        }
        private void OnStepCompleted(NetworkMessage networkMessage)
        {
            Text.text = "The apprentice completed step.";
            ChangeIcon(NotificationType.StepCompleted);
            PlayNotificationAudioClip();
        }
    }
}

using System;
using CollaborationEngine.Base;
using CollaborationEngine.Network;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.ApprenticeBox
{
    [RequireComponent(typeof(AudioSource))]
    public class ApprenticeNetworkStateView : Entity
    {
        public NotificationIcon IconPrefab;
        public Image Background;
        public Text Text;

        private NotificationIcon _icon;
        private DateTime _notificationTime;
        private bool _notifification;

        public void Start()
        {
            var networkManager = MentorNetworkManager.Instance;

            networkManager.OnPlayerConnected += NetworkManager_OnConnectionsChanged;
            networkManager.OnPlayerDisconnected += NetworkManager_OnConnectionsChanged;
            networkManager.OnNeedMoreInstructions += OnNeedMoreInstructions;
            networkManager.OnStepCompleted += OnStepCompleted;
        }

        public void Update()
        {
            if (_notifification && DateTime.Now - _notificationTime > TimeSpan.FromSeconds(10.0f))
            {
                SetNetworkStateMessage();
            }
        }

        public void Reset()
        {
            SetNetworkStateMessage();
        }

        private void NetworkManager_OnConnectionsChanged(object sender, EventArgs e)
        {
            SetNetworkStateMessage();
        }
        private void SetNetworkStateMessage()
        {
            var networkManager = MentorNetworkManager.Instance;

            Text.text = networkManager.IsAppreticeConnected ? "The apprentice is online." : "The apprentice is offline.";
            _notificationTime = DateTime.Now;
            _notifification = false;

            // Remove icon:
            if (_icon)
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
            var audioSource = GetComponent<AudioSource>();
            if(audioSource)
                audioSource.Play();
        }

        private void OnNeedMoreInstructions(object sender, EventArgs eventArgs)
        {
            Text.text = "The apprentice is requesting more instructions.";
            _notificationTime = DateTime.Now;
            _notifification = true;
            ChangeIcon(NotificationType.Help);
            PlayNotificationAudioClip();
        }
        private void OnStepCompleted(object sender, EventArgs eventArgs)
        {
            Text.text = "The apprentice completed step.";
            _notificationTime = DateTime.Now;
            _notifification = true;
            ChangeIcon(NotificationType.StepCompleted);
            PlayNotificationAudioClip();
        }
    }
}

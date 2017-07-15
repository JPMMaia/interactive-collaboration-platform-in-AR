using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.ApprenticeBox
{
    public enum NotificationType
    {
        Help,
        StepCompleted
    }

    [Serializable]
    public struct NotificationTypeTexturePair
    {
        public NotificationType Type;
        public Texture Texture;
    }

    [RequireComponent(typeof(RawImage))]
    public class NotificationIcon : MonoBehaviour
    {
        public NotificationTypeTexturePair[] TypeTextures;

        public NotificationType NotificationType
        {
            get { return _notificationType; }
            set
            {
                _notificationType = value;

                _rawImage.texture = (from pair in TypeTextures
                    where pair.Type == value
                    select pair.Texture).First();
            }
        }

        private RawImage _rawImage;
        private NotificationType _notificationType;

        public void Awake()
        {
            _rawImage = GetComponent<RawImage>();
        }
    }
}

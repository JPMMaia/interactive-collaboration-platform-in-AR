using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollaborationEngine.UserInterface
{
    [RequireComponent(typeof(EventTrigger))]
    public class ChangeCursorButtonBehaviour : MonoBehaviour
    {
        public Texture2D CursorTexture;

        public void Awake()
        {
            
            var eventTrigger = GetComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();

            // Register pointer enter:
            {
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                entry.callback.AddListener(OnPointerEnter);

                eventTrigger.triggers.Add(entry);
            }

            // Register pointer exit:
            {
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerExit
                };
                entry.callback.AddListener(OnPointerExit);

                eventTrigger.triggers.Add(entry);
            }
        }

        public void OnPointerEnter(BaseEventData eventData)
        {
            Cursor.SetCursor(CursorTexture, new Vector2(9.0f, 0.0f), CursorMode.Auto);
        }
        public void OnPointerExit(BaseEventData eventData)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}

using System;
using UnityEngine;

namespace CollaborationEngine.Objects.Collision
{
    public abstract class AbstractCollider : MonoBehaviour
    {
        public delegate void InputEvent(object sender, EventArgs args);

        public event InputEvent OnMouseDownEvent;
        public event InputEvent OnPressedEvent;
        public event InputEvent OnDraggedEvent;

        protected void NotifyMouseDown(object sender, EventArgs args)
        {
            if (OnMouseDownEvent != null)
                OnMouseDownEvent(sender, args);
        }

        protected void NotifyPressed(object sender, EventArgs args)
        {
            if(OnPressedEvent != null)
                OnPressedEvent(sender, args);
        }

        protected void NotifyDragged(object sender, EventArgs args)
        {
            if (OnDraggedEvent != null)
                OnDraggedEvent(sender, args);
        }
    }
}
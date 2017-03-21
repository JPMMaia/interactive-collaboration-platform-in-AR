using System;
using UnityEngine;

namespace CollaborationEngine.Objects.Collision
{
    public abstract class AbstractCollider : MonoBehaviour
    {
        public delegate void InputEvent(object sender, EventArgs args);
        public event InputEvent OnPressed;

        protected void Press(object sender, EventArgs args)
        {
            if(OnPressed != null)
                OnPressed.Invoke(sender, args);
        }
    }
}
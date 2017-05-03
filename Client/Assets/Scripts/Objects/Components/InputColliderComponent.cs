using System;
using CollaborationEngine.Objects.Collision;

namespace CollaborationEngine.Objects.Components
{
    public class InputColliderComponent<TGameObjectType> : IComponent where TGameObjectType : SceneObject
    {
        public delegate void InputEvent(InputColliderComponent<TGameObjectType> sender, EventArgs eventArgs);
        public event InputEvent OnPressed;

        public TGameObjectType GameObject { get; private set; }
        public InputCollider Collider { get; private set; }

        public InputColliderComponent(TGameObjectType gameObject)
        {
            GameObject = gameObject;
            GameObject.AddComponent(this);
        }

        public void Instantiate()
        {
            // Susbscribe to collider events:
            Collider = GameObject.GetComponent<InputCollider>();
            Collider.OnPressedEvent += ColliderOnPressedEvent;
        }

        public void Destroy()
        {
            // Unsubscrive to collider events:
            if (Collider)
            {
                Collider.OnPressedEvent -= ColliderOnPressedEvent;
                Collider = null;
            }
        }

        public void Update()
        {
        }

        private void ColliderOnPressedEvent(object sender, EventArgs args)
        {
            if (OnPressed != null)
                OnPressed.Invoke(this, args);
        }
    }
}
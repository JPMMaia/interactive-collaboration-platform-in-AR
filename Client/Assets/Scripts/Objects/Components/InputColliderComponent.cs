using System;
using CollaborationEngine.Objects.Collision;

namespace CollaborationEngine.Objects.Components
{
    public class InputColliderComponent<TGameObjectType> : IComponent where TGameObjectType : SceneObject2
    {
        public delegate void InputEvent(InputColliderComponent<TGameObjectType> sender, EventArgs args);
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
            Collider.OnPressed += Collider_OnPressed;
        }

        public void Destroy()
        {
            // Unsubscrive to collider events:
            if (Collider)
            {
                Collider.OnPressed -= Collider_OnPressed;
                Collider = null;
            }
        }

        public void Update()
        {
        }

        private void Collider_OnPressed(object sender, EventArgs args)
        {
            if (OnPressed != null)
                OnPressed.Invoke(this, args);
        }
    }
}